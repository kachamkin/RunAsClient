using System.Net.Sockets;
using System.Resources;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;

namespace RunAsClient
{
    public partial class ClientForm : Form
    {
        private int portNum = 0;
        public ProcessForm? procForm;
        public FileTree? treeForm;
        public WMIForm? wmi;
        public SecureString? password;
        private AesCng? aes;

        public ClientForm()
        {
            InitializeComponent();
            //port.DataBindings.Add("Text", portNum, "");
            action.Text = "Get processes list";
        }

        private void port_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(port.Text, out int newValue) && newValue > 0)
                portNum = newValue;
            else
                port.Text = portNum.ToString();
        }

        private void send_Click(object sender, EventArgs e)
        {
            if (portNum == 0)
            {
                MessageBox.Show("Port is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (string.IsNullOrEmpty(host.Text))
            {
                MessageBox.Show("Host is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (string.IsNullOrEmpty(process.Text))
            {
                MessageBox.Show("Process is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (process.Text.Contains("#") || process.Text.Contains(";"))
            {
                MessageBox.Show("Invalid symbols in process!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };


            try
            {
                SendMessage(process.Text + (string.IsNullOrWhiteSpace(args.Text) ? "" : "; " + args.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't send message!\r\n" + ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] RSAEncrypt(byte[] dataToEncrypt, string hashAlg)
        {
            if (!int.TryParse(hashAlg[3..], out int hashSize))
                throw new Exception("Unknown hash algorithm!");

            using X509Certificate2? cert = new((byte[])new ResourceManager("RunAsClient.app", typeof(ClientForm).Assembly)?.GetObject("cert"), password);

            using RSA? rsa = cert.GetRSAPublicKey();
            if (rsa == null)
                throw new Exception("No RSA public key found!");
            if (rsa.KeySize < 2048 && hashAlg == "SHA512")
                throw new Exception("OAEP SHA512 padding is not applicable when key size is less than 2048!");

            //dataToEncrypt += "\0";

            byte[] data = new byte[rsa.KeySize / 8 - 2 * hashSize / 8 - 2];
            int keySizeInBytes = rsa.KeySize / 8;

            using MemoryStream fs = new(dataToEncrypt);
            using MemoryStream ms = new();

            long read = 0, total = 0;
            do
            {
                read = fs.Read(data, 0, (int)(fs.Length - total > data.Length ? data.Length : fs.Length - total));
                if (read > 0)
                    ms.Write(rsa.Encrypt(data, RSAEncryptionPadding.CreateOaep(new HashAlgorithmName(hashAlg))), 0, keySizeInBytes);
                total += read;
            }
            while (read > 0);

            fs.Close();
            ms.Close();

            return ms.ToArray();
        }

        //private string RSADecrypt(string dataToDecrypt, string hashAlg)
        //{
        //    if (!int.TryParse(hashAlg[3..], out int hashSize))
        //        throw new Exception("Unknown hash algorithm!");

        //    using X509Certificate2? cert = new((byte[])new ResourceManager("RunAsClient.app", typeof(ClientForm).Assembly)?.GetObject("cert"), password);

        //    using RSA? rsa = cert.GetRSAPrivateKey();
        //    if (rsa == null)
        //        throw new Exception("No RSA public key found!");
        //    if (rsa.KeySize < 2048 && hashAlg == "SHA512")
        //        throw new Exception("OAEP SHA512 padding is not applicable when key size is less than 2048!");

        //    int keySizeInBytes = rsa.KeySize / 8;
        //    int decrLen = keySizeInBytes - 2 * hashSize / 8 - 2;
        //    byte[] data = new byte[keySizeInBytes];

        //    using MemoryStream fs = new(Convert.FromBase64String(dataToDecrypt));
        //    using MemoryStream ms = new();

        //    bar.Step = 1;
        //    long read = 0, total = 0;
        //    do
        //    {
        //        read = fs.Read(data, 0, data.Length);
        //        ms.Write(rsa.Decrypt(data, RSAEncryptionPadding.CreateOaep(new HashAlgorithmName(hashAlg))), 0, decrLen);
        //        total += read;
        //        bar.Value = fs.Length > 0 ? (int)(100 * total / fs.Length) : 0;
        //    }
        //    while (read > 0);

        //    bar.Value = 0;

        //    fs.Close();
        //    string ret = Encoding.Unicode.GetString(ms.ToArray());
        //    ms.Close();

        //    return ret;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Multiselect = false
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                file.Text = ofd.FileName;
        }

        private void butUpload_Click(object sender, EventArgs e)
        {
            if (portNum == 0)
            {
                MessageBox.Show("Port is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (string.IsNullOrEmpty(host.Text))
            {
                MessageBox.Show("Host is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (string.IsNullOrEmpty(file.Text) || !File.Exists(file.Text))
            {
                MessageBox.Show("File is empty or does not exist!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

            try
            {
                string extension = Path.GetExtension(file.Text);
                if (!string.IsNullOrEmpty(extension))
                    extension = extension.Substring(1);
                string textToSend = "#upload#" + (string.IsNullOrEmpty(extension) ? "" : extension + ";") + (string.IsNullOrWhiteSpace(uploadArgs.Text) ? "" : uploadArgs.Text + "#") + Convert.ToBase64String(File.ReadAllBytes(file.Text));

                SendMessage(textToSend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't send message!\r\n" + ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void action_TextChanged(object sender, EventArgs e)
        {
            user.Visible = action.Text == "Logoff user" || action.Text == "Send message to user";
            if (!user.Visible)
                user.Text = "";
            processToKill.Visible = action.Text == "Kill process";
            if (!processToKill.Visible)
                processToKill.Text = "";
            message.Visible = action.Text.StartsWith("Send message");
            if (!message.Visible)
                message.Text = "";
        }

        private byte[] AESEncrypt(string dataToEncrypt)
        {
            dataToEncrypt += '\0';
            byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
            
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            
            cs.Write(data, 0, data.Length);

            cs.Close();
            ms.Close();

            return ms.ToArray();
        }

        private string AESDecrypt(string dataToDecrypt)
        {
            byte[] data = Convert.FromBase64String(dataToDecrypt);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);

            cs.Close();
            ms.Close();

            return Encoding.Unicode.GetString(ms.ToArray());
        }

        private string Read(NetworkStream stream)
        {
            using BinaryReader reader = new(stream);

            byte[] buffer = new byte[1048576];
            using MemoryStream ms = new();
            int read = 0;
            do
            {
                read = reader.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, read);
            }
            while (read > 0);
            reader.Close();
            ms.Close();

            return AESDecrypt(Encoding.Unicode.GetString(ms.ToArray()).Replace("\0", "")).Split('\0')[0];
        }

        public void SendMessage(string textToSend, string actionText = "", bool noMB = false, bool dragDrop = false)
        {
            CheckForIllegalCrossThreadCalls = false;
            
            if (String.IsNullOrWhiteSpace(textToSend))
                return;

            try
            {
                aes?.Dispose();
                aes = new()
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 256,
                    IV = new byte[16]
                };

                byte[] aesKey = RSAEncrypt(aes.Key, "SHA256");
                byte[] strData = AESEncrypt(textToSend);
                byte[] resToEncypt = new byte[aesKey.Length + strData.Length];
                aesKey.CopyTo(resToEncypt, 0);
                strData.CopyTo(resToEncypt, aesKey.Length);

                using TcpClient client = new();
                client.Connect(host.Text, portNum);
                using NetworkStream stream = client.GetStream();

                using BinaryWriter writer = new(stream, Encoding.UTF8, true);
                writer.Write(Encoding.UTF8.GetBytes(Convert.ToBase64String(resToEncypt)));
                writer.Close();
                stream.Socket.Shutdown(SocketShutdown.Send);

                string res = Read(stream);

                if (textToSend.StartsWith("#proclist#") && (actionText == "Get processes list" || action.Text == "Get processes list"))
                {
                    if (procForm == null)
                    {
                        procForm = new(res, this);
                        procForm.Show();
                    }
                    else
                        procForm.UpdateData(res);
                }
                else if (textToSend.StartsWith("#download#"))
                {
                    if (res == "Failed")
                        MessageBox.Show(res, "Run As Client", MessageBoxButtons.OK, res.StartsWith("S") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                    else
                    {
                        string fileName = Path.GetTempPath() + textToSend.Substring(textToSend.LastIndexOf('\\') + 1);
                        File.WriteAllBytes(fileName, Convert.FromBase64String(res));
                        if (dragDrop && treeForm != null)
                            treeForm.dragDropFile = fileName;
                        else if (!dragDrop)
                        {
                            StringCollection dropList = new()
                            {
                                fileName
                            };
                            Thread t = new(() => SetClipboard(dropList));
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                        }
                    }
                }
                else if (textToSend.StartsWith("#memusage#") || textToSend.StartsWith("#sysinfo#"))
                {
                    new ModulesForm(res).Show();
                }
                else if (textToSend.StartsWith("#filetree#"))
                {
                    if (treeForm == null)
                    {
                        treeForm = new(res, this);
                        treeForm.Show();
                    }
                    else
                        treeForm.UpdateData(res);
                }
                else if (textToSend.StartsWith("#devices#"))
                {
                    if (wmi == null)
                    {
                        wmi = new(this, res);
                        wmi.Show();
                    }
                    else
                        wmi.UpdateData(res);
                }
                else if (textToSend.StartsWith("#modules#") || textToSend.StartsWith("#memorydetails#") || textToSend.StartsWith("#threads#"))
                    new ModulesForm(res, textToSend.StartsWith("#modules#")).ShowDialog(procForm);
                else if (!noMB)
                    MessageBox.Show(res, "Run As Client", MessageBoxButtons.OK, res.StartsWith("S") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void SetClipboard(StringCollection dropList)
        {
            Clipboard.SetFileDropList(dropList);
        }

        private void butAction_Click(object sender, EventArgs e)
        {
            if (portNum == 0)
            {
                MessageBox.Show("Port is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (string.IsNullOrEmpty(host.Text))
            {
                MessageBox.Show("Host is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (action.Text == "Logoff user" && string.IsNullOrEmpty(user.Text))
            {
                MessageBox.Show("User is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (action.Text == "Kill process" && string.IsNullOrEmpty(processToKill.Text))
            {
                MessageBox.Show("Process is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (action.Text.StartsWith("Send message") && string.IsNullOrEmpty(message.Text))
            {
                MessageBox.Show("Message is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };
            if (action.Text == "Send message to user" && string.IsNullOrEmpty(user.Text))
            {
                MessageBox.Show("User is empty!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

            try
            {
                string textToSend = "";
                switch (action.Text)
                {
                    case "Reboot":
                        textToSend = "#reboot#";
                        break;
                    case "Shutdown":
                        textToSend = "#shutdown#";
                        break;
                    case "Logoff":
                        textToSend = "#logoff#";
                        break;
                    case "Logoff user":
                        textToSend = "#logoff#" + user.Text;
                        break;
                    case "Kill process":
                        textToSend = "#kill#" + processToKill.Text;
                        break;
                    case "Send message to all":
                        textToSend = "#message#" + message.Text;
                        break;
                    case "Send message to user":
                        textToSend = "#message#" + user.Text + "#" + message.Text;
                        break;
                    case "Get processes list":
                        textToSend = "#proclist#";
                        break;
                    case "Get memory usage":
                        textToSend = "#memusage#";
                        break;
                    case "Get system info":
                        textToSend = "#sysinfo#";
                        break;
                    case "View file system":
                        textToSend = "#filetree#";
                        break;
                    case "WMI":
                        textToSend = "#devices#";
                        break;
                    case "RDP":
                        Process.Start("mstsc", "/v:" + host.Text);
                        return;
                    default:
                        break;
                };

                SendMessage(textToSend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't send message!\r\n" + ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            password = null;
            new PassForm(this).ShowDialog();
            if (password != null)
            {
                X509Certificate2? cert = null;
                try
                {
                    cert = new((byte[])new ResourceManager("RunAsClient.app", typeof(ClientForm).Assembly)?.GetObject("cert"), password);
                }
                catch { };
                if (cert == null)
                {
                    MessageBox.Show("Invalid password!", "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                else
                    cert.Dispose();
            }
            else
                Close();
        }
    }
}