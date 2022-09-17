using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace RunAsClient
{
    public partial class FileTree : Form
    {
        private ClientForm topForm;
        private string? currParent;
        private string? before;
        public string? dragDropFile;
        private List<string> sequence = new();
        private int currIndex = -1;
        private Hashtable icons = new();
        private Hashtable orders = new();
        private List<string> selNames = new();

        public FileTree(string tree, ClientForm _topForm)
        {
            topForm = _topForm;
            currIndex++;
            sequence.Add("");
            InitializeComponent();
            UpdateData(tree);
        }

        public void UpdateData(string tree)
        {
            string[] rows = tree.Split('\0')[0].Split("\r\n");

            if (rows.Length == 0)
                return;
            if (rows.Length == 1 && string.IsNullOrWhiteSpace(rows[0]))
                return;

            list.Items.Clear();

            foreach (string row in rows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;

                string[] data = row.Split(";");
                ListViewItem i = list.Items.Add(string.IsNullOrWhiteSpace(data[1]) ? data[0] : data[1], data[0], data[2] == "1" ? "folder.png" : "file.png");
                i.Tag = data[2];
                i.SubItems.Add(data[3]);
                i.SubItems.Add(data[4]);
                if (selNames.Contains(i.Name))
                    i.Selected = true;

                if (string.IsNullOrEmpty(currParent))
                {
                    list.Columns[1].Text = "Total";
                    list.Columns[2].Text = "Free";
                    i.ImageKey = "disk.png";
                    i.UseItemStyleForSubItems = false;
                    i.SubItems[2].ForeColor = Color.Green;
                }
                else
                {
                    list.Columns[1].Text = "Size";
                    list.Columns[2].Text = "Last modified";
                    i.UseItemStyleForSubItems = true;
                }

                if ((string)i.Tag == "1")
                    i.Font = new(i.Font, FontStyle.Bold);
                else
                {
                    string? ext = Path.GetExtension(i.Name);
                    if (!icons.ContainsKey(ext))
                    {
                        try
                        {
                            Bitmap? ico = Icons.IconFromExtensionShell(ext, Icons.SystemIconSize.Small);
                            if (ico != null)
                            {
                                list.SmallImageList.Images.Add(ico);
                                i.ImageIndex = list.SmallImageList.Images.Count - 1;
                                icons.Add(ext, i.ImageIndex);
                            }
                        }
                        catch
                        {
                            i.ImageKey = "file.png";
                        };
                    }
                    else
                        i.ImageIndex = (int)icons[ext];
                };
            }

            List<ListViewItem> l = String.IsNullOrEmpty(currParent) ?
                    list.Items.Cast<ListViewItem>().OrderBy(x => x.Name).ToList() :
                    list.Items.Cast<ListViewItem>().OrderByDescending(x => x.Tag).ToList();
            for (int j = 0; j < list.Columns.Count; j++)
                if (orders.ContainsKey(j))
                {
                    if ((bool)orders[j])
                        l = list.Items.Cast<ListViewItem>().OrderBy(x => x.SubItems[j].Text).ToList();
                    else
                        l = list.Items.Cast<ListViewItem>().OrderByDescending(x => x.SubItems[j].Text).ToList();
                    break;
                };

            l = l.Where(x => x.Text != ".").ToList();
            if (!string.IsNullOrEmpty(search.Text))
                l = l.Where(x => x.Text.ToLower().Contains(search.Text.ToLower())).ToList();

            list.Items.Clear();

            ListViewItem top = list.Items.Add("", "\\", "folder.png");
            top.Tag = "1";
            top.Font = new(top.Font, FontStyle.Bold);
            top.SubItems.Add("");
            top.SubItems.Add("");

            list.Items.AddRange(l.ToArray());

            if (!string.IsNullOrWhiteSpace(before))
            {
                ListViewItem[] items = list.Items.Find(before, false);
                if (items.Length > 0)
                    items[0].BeginEdit();
            }

            parentBox.Text = currParent;
        }

        private void FileTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            topForm.treeForm = null;
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            if (list.Items.Count == 0 || list.SelectedItems.Count == 0)
                return;
            if ((string)list.SelectedItems[0].Tag == "1")
            {
                currParent = list.SelectedItems[0].Name;
                topForm.SendMessage("#filetree#" + currParent, "View file system");
                sequence.Add(currParent);
                forward.Enabled = false;
                back.Enabled = true;
                currIndex++;
            }
            else
                topForm.SendMessage(list.SelectedItems[0].Name, "");
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.View = View.SmallIcon;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.View = View.List;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Items.Count == 0 || list.SelectedItems.Count == 0)
                    return;

                string folder = Directory.CreateDirectory(Path.GetTempPath() + Guid.NewGuid().ToString()).FullName;

                foreach (ListViewItem i in list.SelectedItems)
                {
                    if (i.Name == "" || i.Text.StartsWith(".") || i.ImageKey == "disk.png")
                        continue;
                    if ((string)i.Tag == "0")
                    {
                        topForm.SendMessage("#download#" + i.Name, "", true, true);
                        File.Move(dragDropFile, folder + "\\" + Path.GetFileName(dragDropFile));
                    }
                    else
                    {
                        string newFolder = Directory.CreateDirectory(folder + "\\" + i.Text).FullName;
                        ProcessPathsInRemoteFolder(i.Name, i.Text, ref newFolder);
                    }
                }

                if (Directory.EnumerateFileSystemEntries(folder).Any())
                {
                    StringCollection dropList = new();
                    foreach (string file in Directory.GetFiles(folder))
                        dropList.Add(file);
                    foreach (string dir in Directory.GetDirectories(folder))
                        dropList.Add(dir);
                    Clipboard.SetFileDropList(dropList);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StringCollection dropList = Clipboard.GetFileDropList(); 
                if (dropList.Count > 0 && !string.IsNullOrWhiteSpace(currParent))
                {
                    foreach (string? file in dropList)
                    {
                        if (file == null)
                            continue;
                        if (Directory.Exists(file))
                            ProcessPathsInFolder(file, currParent);
                        else
                            topForm.SendMessage("#uploadnoexec#" + currParent + (currParent[currParent.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(file) + ";" + Convert.ToBase64String(File.ReadAllBytes(file)), "", true);
                    }
                    topForm.SendMessage("#filetree#" + currParent, "View file system");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (list.Items.Count == 0 || list.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem i in list.SelectedItems)
            {
                if (i.Name == "" || i.Text.StartsWith(".") || i.ImageKey == "disk.png")
                    continue;
                topForm.SendMessage("#deletefile#" + i.Name, "", true);
            }
            topForm.SendMessage("#filetree#" + currParent, "View file system");
        }

        private void list_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            before = list.SelectedItems[0].Name;
        }

        private void list_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currParent))
                return;

            topForm.SendMessage("#rename#" + before + ";" + currParent + (currParent[currParent.Length - 1] == '\\' ? "" : "\\") + e.Label, "", true);
            topForm.SendMessage("#filetree#" + currParent, "View file system");

            before = null;
        }

        private void createDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currParent))
                return;

            before = currParent + (currParent[currParent.Length - 1] == '\\' ? "" : "\\") + "New folder";
            topForm.SendMessage("#createdir#" + before, "", true);
            topForm.SendMessage("#filetree#" + currParent, "View file system");
        }

        private void list_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] dropList = (string[])e.Data.GetData(DataFormats.FileDrop);
                        if (dropList.Length > 0 && !string.IsNullOrWhiteSpace(currParent))
                        {
                            ListViewHitTestInfo h = list.HitTest(list.PointToClient(new Point(e.X, e.Y)));
                            foreach (string? file in dropList)
                            {
                                if (file == null)
                                    continue;
                                if (h.Item == null || (string)h.Item.Tag == "0")
                                    if (Directory.Exists(file))
                                        ProcessPathsInFolder(file, currParent);
                                    else
                                        topForm.SendMessage("#uploadnoexec#" + currParent + (currParent[currParent.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(file) + ";" + Convert.ToBase64String(File.ReadAllBytes(file)), "", true);
                                else if (h.Item != null)
                                    if (Directory.Exists(file))
                                        ProcessPathsInFolder(file, h.Item.Name);
                                    else
                                        topForm.SendMessage("#uploadnoexec#" + h.Item.Name + (h.Item.Name[h.Item.Name.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(file) + ";" + Convert.ToBase64String(File.ReadAllBytes(file)), "", true);
                            }
                            topForm.SendMessage("#filetree#" + currParent, "View file system");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessPathsInFolder(string folder, string parent)
        {
            string newParent = parent + (parent[parent.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(folder);
            topForm.SendMessage("#createdir#" + newParent, "", true);
            string[] files = Directory.GetFiles(folder);
            foreach (string file in files)
                topForm.SendMessage("#uploadnoexec#" + newParent + (newParent[newParent.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(file) + ";" + Convert.ToBase64String(File.ReadAllBytes(file)), "", true);
            string[] dirs = Directory.GetDirectories(folder);
            foreach (string dir in dirs)
            {
                newParent = parent + (parent[parent.Length - 1] == '\\' ? "" : "\\") + Path.GetFileName(dir);
                topForm.SendMessage("#createdir#" + newParent, "", true);
                ProcessPathsInFolder(dir, newParent);
            }
        }

        private void ProcessPathsInRemoteFolder(string parent, string name, ref string folder)
        {
            if (string.IsNullOrEmpty(folder))
                folder = Directory.CreateDirectory(Path.GetTempPath() + Guid.NewGuid().ToString() + "\\" + name).FullName;

            using FileTree tree = new("", topForm);
            topForm.treeForm = tree;
            topForm.SendMessage("#filetree#" + parent, "View file system");

            foreach (ListViewItem i in tree.list.Items)
                if ((string)i.Tag == "1" && !string.IsNullOrWhiteSpace(i.Name) && !i.Text.StartsWith("."))
                {
                    string newFolder = Directory.CreateDirectory(folder + "\\" + i.Text).FullName;
                    ProcessPathsInRemoteFolder(i.Name, i.Text, ref newFolder);
                }
                else if ((string)i.Tag == "0" && !string.IsNullOrWhiteSpace(i.Name) && !i.Text.StartsWith("."))
                {
                    topForm.SendMessage("#download#" + i.Name, "", true, true);
                    File.WriteAllBytes(folder + "\\" + Path.GetFileName(tree.dragDropFile), File.ReadAllBytes(tree.dragDropFile));
                };
            
            topForm.treeForm = this;
        }

        private void list_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
        }

        private void list_DragOver(object sender, DragEventArgs e)
        {
        }

        private void parentBox_Leave(object sender, EventArgs e)
        {
            currParent = parentBox.Text;
            topForm.SendMessage("#filetree#" + currParent, "View file system");
            sequence.Add(currParent);
            forward.Enabled = false;
            back.Enabled = true;
            currIndex++;
        }

        private void parentBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                list.Focus();
        }

        private void back_Click(object sender, EventArgs e)
        {
            currIndex--;
            currParent = sequence[currIndex];
            parentBox.Text = currParent;
            topForm.SendMessage("#filetree#" + currParent, "View file system");
            forward.Enabled = true;
            if (currIndex == 0)
                back.Enabled = false;
        }

        private void forward_Click(object sender, EventArgs e)
        {
            currIndex++;
            currParent = sequence[currIndex];
            parentBox.Text = currParent;
            topForm.SendMessage("#filetree#" + currParent, "View file system");
            back.Enabled = true;
            if (currIndex == sequence.Count - 1)
                forward.Enabled = false;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.View = View.Details;
        }

        private void list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo h = list.HitTest(e.X, e.Y);
                if (list.SelectedItems.Count == 0)
                {
                    contextMenuStrip1.Items[1].Visible = false;
                    contextMenuStrip1.Items[3].Visible = false;
                    contextMenuStrip1.Items[5].Visible = false;
                }
                else if (h.Item != null)
                {
                    contextMenuStrip1.Items[1].Visible = h.Item.ImageKey != "disk.png" && !h.Item.Text.StartsWith(".") && h.Item.Name != "";
                    contextMenuStrip1.Items[3].Visible = h.Item.ImageKey != "disk.png" && !h.Item.Text.StartsWith(".") && h.Item.Name != "";
                    contextMenuStrip1.Items[5].Visible = h.Item.ImageKey != "disk.png" && !h.Item.Text.StartsWith(".") && h.Item.Name != "";
                }
                contextMenuStrip1.Items[2].Visible = !string.IsNullOrWhiteSpace(currParent);
                contextMenuStrip1.Items[2].Enabled = Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop);
                contextMenuStrip1.Items[4].Visible = !string.IsNullOrWhiteSpace(currParent);
            }
        }

        private void list_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                contextMenuStrip1.Items[1].Visible = e.Item.ImageKey != "disk.png" && !e.Item.Text.StartsWith(".") && e.Item.Name != "";
                contextMenuStrip1.Items[3].Visible = e.Item.ImageKey != "disk.png" && !e.Item.Text.StartsWith(".") && e.Item.Name != "";
                contextMenuStrip1.Items[5].Visible = e.Item.ImageKey != "disk.png" && !e.Item.Text.StartsWith(".") && e.Item.Name != "";
                if (!selNames.Contains(e.Item.Name))
                    selNames.Add(e.Item.Name);
            }
            else
            {
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                if (selNames.Contains(e.Item.Name))
                    selNames.Remove(e.Item.Name);
            }
            contextMenuStrip1.Items[2].Visible = !string.IsNullOrWhiteSpace(currParent);
            contextMenuStrip1.Items[4].Visible = !string.IsNullOrWhiteSpace(currParent);
        }

        private void list_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            foreach (ColumnHeader columnHeader in list.Columns)
            {
                if (columnHeader.Index != e.Column)
                {
                    columnHeader.ImageIndex = -1;
                    columnHeader.TextAlign = columnHeader.TextAlign;
                    if (orders.ContainsKey(columnHeader.Index))
                        orders.Remove(columnHeader.Index);
                };
            }
            
            List<ListViewItem> l;
            if (orders.ContainsKey(e.Column))
            {
                if ((bool)orders[e.Column])
                {
                    l = list.Items.Cast<ListViewItem>().OrderByDescending(x => x.SubItems[e.Column].Text).ToList();
                    list.Columns[e.Column].ImageKey = "down.png";
                    orders[e.Column] = false;
                }
                else 
                {
                    l = list.Items.Cast<ListViewItem>().OrderBy(x => x.SubItems[e.Column].Text).ToList();
                    list.Columns[e.Column].ImageKey = "up.png";
                    orders[e.Column] = true;
                };
            }
            else
            {
                l = list.Items.Cast<ListViewItem>().OrderBy(x => x.SubItems[e.Column].Text).ToList();
                list.Columns[e.Column].ImageKey = "up.png";
                orders.Add(e.Column, true);
            };

            list.Items.Clear();
            list.Items.AddRange(l.ToArray());
        }

        private void search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                list.Focus();
        }

        private void search_Leave(object sender, EventArgs e)
        {
            topForm.SendMessage("#filetree#" + currParent, "View file system");
        }

        private void defaultOrderingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            orders.Clear();
            foreach (ColumnHeader columnHeader in list.Columns)
            {
                columnHeader.ImageIndex = - 1;
                columnHeader.TextAlign = columnHeader.TextAlign;
            };
            topForm.SendMessage("#filetree#" + currParent, "View file system");
        }

        private void list_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void list_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                string folder = Directory.CreateDirectory(Path.GetTempPath() + Guid.NewGuid().ToString()).FullName;

                foreach (ListViewItem i in list.SelectedItems)
                {
                    if (i.Name == "" || i.Text.StartsWith(".") || i.ImageKey == "disk.png")
                        return;

                    if ((string)i.Tag == "0")
                    {
                        topForm.SendMessage("#download#" + i.Name, "", true, true);
                        File.Move(dragDropFile, folder + "\\" + Path.GetFileName(dragDropFile));
                    }
                    else
                    {
                        string newFolder = Directory.CreateDirectory(folder + "\\" + i.Text).FullName;
                        ProcessPathsInRemoteFolder(i.Name, i.Text, ref newFolder);
                    }
                };

                if (Directory.EnumerateFileSystemEntries(folder).Any())
                {
                    List<string> dropList = new();
                    foreach (string file in Directory.GetFiles(folder))
                        dropList.Add(file);
                    foreach (string dir in Directory.GetDirectories(folder))
                        dropList.Add(dir);
                    list.DoDragDrop(new DataObject(DataFormats.FileDrop, dropList.ToArray()), DragDropEffects.Copy);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [DllImport("shell32.dll", SetLastError = true)]
        static extern int SHMultiFileProperties(IDataObject pdtobj, int flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ILCreateFromPath(string path);

        [DllImport("shell32.dll", CharSet = CharSet.None)]
        public static extern void ILFree(IntPtr pidl);

        [DllImport("shell32.dll", CharSet = CharSet.None)]
        public static extern int ILGetSize(IntPtr pidl);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;

        private MemoryStream CreateShellIDList(StringCollection filenames)
        {
            int pos = 0;
            byte[][] pidls = new byte[filenames.Count][];
            foreach (var filename in filenames)
            {
                IntPtr pidl = ILCreateFromPath(filename);
                int pidlSize = ILGetSize(pidl);
                pidls[pos] = new byte[pidlSize];
                Marshal.Copy(pidl, pidls[pos++], 0, pidlSize);
                ILFree(pidl);
            }

            int pidlOffset = 4 * (filenames.Count + 2);
            MemoryStream memStream = new();
            BinaryWriter sw = new(memStream);
            sw.Write(filenames.Count);
            sw.Write(pidlOffset);
            pidlOffset += 4; 
            foreach (var pidl in pidls)
            {
                sw.Write(pidlOffset);
                pidlOffset += pidl.Length;
            }

            sw.Write(0);
            foreach (var pidl in pidls) sw.Write(pidl);
            return memStream;
        }

        private void ShowProperties(IEnumerable<string> Filenames)
        {
            StringCollection Files = new StringCollection();
            foreach (string s in Filenames)
                Files.Add(s);
            DataObject data = new();
            data.SetFileDropList(Files);
            data.SetData("Shell IDList Array", true, CreateShellIDList(Files));
            _ = SHMultiFileProperties(data, 0);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Items.Count == 0 || list.SelectedItems.Count == 0)
                    return;

                string folder = Directory.CreateDirectory(Path.GetTempPath() + Guid.NewGuid().ToString()).FullName;

                if (list.SelectedItems.Count == 1)
                {
                    ListViewItem i = list.SelectedItems[0];
                    if (i.Name == "" || i.Text.StartsWith(".") || i.ImageKey == "disk.png")
                        return;

                    SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
                    info.cbSize = Marshal.SizeOf(info);
                    info.lpVerb = "properties";
                    info.nShow = SW_SHOW;
                    info.fMask = SEE_MASK_INVOKEIDLIST;

                    if ((string)i.Tag == "0")
                    {
                        topForm.SendMessage("#download#" + i.Name, "", true, true);
                        string fileName = folder + "\\" + Path.GetFileName(dragDropFile);
                        File.Move(dragDropFile, fileName);
                        info.lpFile = fileName;
                    }
                    else
                    {
                        string newFolder = Directory.CreateDirectory(folder + "\\" + i.Text).FullName;
                        ProcessPathsInRemoteFolder(i.Name, i.Text, ref newFolder);
                        info.lpFile = newFolder;
                    }

                    ShellExecuteEx(ref info);
                }
                else
                {
                    foreach (ListViewItem i in list.SelectedItems)
                    {
                        if (i.Name == "" || i.Text.StartsWith(".") || i.ImageKey == "disk.png")
                            return;

                        if ((string)i.Tag == "0")
                        {
                            topForm.SendMessage("#download#" + i.Name, "", true, true);
                            File.Move(dragDropFile, folder + "\\" + Path.GetFileName(dragDropFile));
                        }
                        else
                        {
                            string newFolder = Directory.CreateDirectory(folder + "\\" + i.Text).FullName;
                            ProcessPathsInRemoteFolder(i.Name, i.Text, ref newFolder);
                        }
                    };

                    List<string> dropList = new();
                    if (Directory.EnumerateFileSystemEntries(folder).Any())
                    {
                        foreach (string file in Directory.GetFiles(folder))
                            dropList.Add(file);
                        foreach (string dir in Directory.GetDirectories(folder))
                            dropList.Add(dir);
                        list.DoDragDrop(new DataObject(DataFormats.FileDrop, dropList.ToArray()), DragDropEffects.Copy);
                    };
                    if (dropList.Count > 0)
                        ShowProperties(dropList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run As Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class Icons
    {
        public static object Colors { get; private set; }

        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [Flags]
        enum FileInfoFlags : int
        {
            SHGFI_ICON = 0x000000100,
            SHGFI_USEFILEATTRIBUTES = 0x000000010
        }

        [DllImport("Shell32", CharSet = CharSet.Auto)]
        extern static IntPtr SHGetFileInfo(
            string pszPath,
            int dwFileAttributes,
            out SHFILEINFO psfi,
            int cbFileInfo,
            FileInfoFlags uFlags);

        public enum SystemIconSize : int
        {
            Large = 0x000000000,
            Small = 0x000000001
        }

        public static Bitmap IconFromExtensionShell(string extension, SystemIconSize size)
        {
            if (extension[0] != '.') extension = '.' + extension;
            SHFILEINFO fileInfo = new();

            SHGetFileInfo(
                extension,
                0,
                out fileInfo,
                Marshal.SizeOf(fileInfo),
                FileInfoFlags.SHGFI_ICON | FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (FileInfoFlags)size);

            Bitmap bmp = Icon.FromHandle(fileInfo.hIcon).ToBitmap();
            bmp.MakeTransparent(Color.Black);

            return bmp;
        }
    }
}
