using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RunAsWinSvc
{
    internal class Listener
    {
        private readonly TcpListener listener;
        private EventLog? _logger;

        private delegate void DataReceivedEvent(string message, IPAddress ip);
        private event DataReceivedEvent? OnDataReceived;

        public Listener(int port)
        {
            OnDataReceived += OnDataReceivedHandler;
            listener = new TcpListener(IPAddress.Any, port);
        }

        async public void Start(EventLog logger)
        {
            _logger = logger;

            listener.Start();
            await Task.Run(() => Accept());
        }

        public void Stop()
        {
            listener.Stop();
            GC.Collect();
        }

        async private void Accept()
        {
            try
            {
                using TcpClient client = await listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();
                using BinaryReader reader = new(stream);

                OnDataReceived?.Invoke(await Task.Run(() => reader.ReadString()), ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                reader.Close();
                stream.Close();
                client.Close();
                GC.Collect();
            }
            catch (Exception ex)
            {
                _logger?.WriteEntry("Error in accept method: " + ex.Message, EventLogEntryType.Error);
            };
            Accept();
        }

        private void OnDataReceivedHandler(string message, IPAddress ip)
        {
            _logger?.WriteEntry("Received message from " + ip + ":\r\n" + message, EventLogEntryType.Information);
            
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    throw new Exception("Process name is empty!");

                message = RSADecrypt(message, "SHA256");
                
                string[] args = message.Split(';');
                if (args.Length == 0)
                    throw new Exception("Process name is empty!");

                if (args.Length == 1)
                    Process.Start(args[0].Trim());
                else
                    Process.Start(args[0].Trim(), args[1].Trim());
                _logger?.WriteEntry("Process started successfully: " + args[0]);
            }
            catch (Exception ex)
            {
                _logger?.WriteEntry("Couldn't start process!\r\n" + ex.Message, EventLogEntryType.Error);
            }
        }

        ~Listener()
        {
            listener.Stop();
            GC.Collect();
        }

        private static string RSADecrypt(string dataToDecrypt, string hashAlg, string password = "")
        {
            if (!int.TryParse(hashAlg.Substring(3), out int hashSize))
                throw new Exception("Unknown hash algorithm!");

            using X509Certificate2? cert = new((byte[])new ResourceManager("RunAsWinSvc.app", typeof(RunAsSvc).Assembly)?.GetObject("cert"), string.IsNullOrWhiteSpace(password) ? null : password);

            if (!cert.HasPrivateKey)
                throw new Exception("Specified certificate has no private key!");

            using RSA? rsa = cert.GetRSAPrivateKey();
            if (rsa == null)
                throw new Exception("No RSA private key found!");
            if (rsa.KeySize < 2048 && hashAlg == "SHA512")
                throw new Exception("OAEP SHA512 padding is not applicable when key size is less than 2048!");

            int blockSize = rsa.KeySize / 8 - 2 * hashSize / 8 - 2;
            byte[] data = new byte[rsa.KeySize / 8];

            using MemoryStream fs = new(Convert.FromBase64String(dataToDecrypt));
            using MemoryStream ms = new();

            while (fs.Read(data, 0, data.Length) > 0)
                ms.Write(rsa.Decrypt(data, RSAEncryptionPadding.CreateOaep(new HashAlgorithmName(hashAlg))), 0, blockSize);

            fs.Close();
            string ret = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();

            return ret;

        }


    }
}
