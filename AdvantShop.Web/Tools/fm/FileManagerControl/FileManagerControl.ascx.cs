using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ICSharpCode.SharpZipLib.Zip;

namespace Tools.fm.FileManagerControl
{ //--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
    public partial class FileManagerControl : System.Web.UI.UserControl
    {

        private string _rootDir;
        public string RootDir
        {
            get
            {
                return _rootDir;
            }
            set
            {
                _rootDir = value;
            }
        }

        private string RequestedPath;
        private DirectoryInfo[] CurrentDirectoryDirectories;
        private FileInfo[] CurrentDirectoryFiles;

        private List<string> _selectedDirectories;
        private List<string> _selectedFiles;

        public string[] SelectedDirectories
        {
            get
            {
                if (_selectedDirectories == null)
                {
                    return (string[])Array.CreateInstance(typeof(string), 0);
                }
                else
                {
                    return _selectedDirectories.ToArray();
                }
            }
        }

        public string[] SelectedFiles
        {
            get
            {
                if (_selectedFiles == null)
                {
                    return (string[])Array.CreateInstance(typeof(string), 0);
                }
                else
                {
                    return _selectedFiles.ToArray();
                }
            }
        }


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.ClientScript.IsStartupScriptRegistered("dummy"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "dummy", "AddDummyQueryString();", true);
            }

            //RootDir = Request.PhysicalPath.Substring(0, Request.PhysicalPath.LastIndexOf("\")) & "\Files\"
            RootDir = Server.MapPath("~\\");

            if (!string.IsNullOrEmpty((string)ViewState["FileManagerPath"]))
            {
                RequestedPath = (string)ViewState["FileManagerPath"];
            }
            else
            {
                RequestedPath = "";
            }

            LoadSelectedDirectories();
            LoadSelectedFiles();

            if (!string.IsNullOrEmpty(Request.QueryString["path"]))
            {
                var tmpUri = Request.QueryString["path"];
                string req = RootDir + tmpUri;
                FileInfo requestedFileInfo = new FileInfo(req);
                if (requestedFileInfo.FullName.StartsWith(RootDir) == false)
                {
                    FileManagerFooterBlock.InnerText = "Запрошенный путь недоступен";
                    return;
                }
                if (File.Exists(req))
                {
                    DownloadFile(req);
                    return;
                }
            }

            ScriptManager.GetCurrent(this.Page).Navigate += new EventHandler<HistoryEventArgs>(FileManagerControl_FileManagerControl_Navigate);





            UpdateDirectoryContentPanel();
            UpdateFileSystemTree(FilemanagerTreeControl);
            UpdateHeader();

        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            SaveSelectedDirectories();
            SaveSelectedFiles();

            UpdateFileSystemTree(FilemanagerTreeControl);
            UpdateDirectoryContentPanel();
            UpdateHeader();
            ExpandAndSelectTreeNode(RequestedPath);
            DataBind();
        }

        public void FileManagerControl_FileManagerControl_Navigate(object sender, HistoryEventArgs e)
        {
            NavigateTo(e.State["path"]);
        }

        public void FileManagerFilemanagerTreeControl_SelectedNodeChanged_Handler(object sender, EventArgs e)
        {
            TreeNode selectedNode = FilemanagerTreeControl.SelectedNode;
            var path = GetNodePath(selectedNode);
            NavigateTo(path);
            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void FileManagerHeaderLink_Command_Handler(object sender, CommandEventArgs e)
        {
            NavigateTo((string)e.CommandArgument);
            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void DirectoryContentLink_Click_Handler(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(RequestedPath))
            {
                NavigateTo(RequestedPath + "\\" + linkButton.Text);
            }
            else
            {
                NavigateTo(linkButton.Text);
            }

            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void FileManagerUpdateButton_ClickHandler(object sender, EventArgs e)
        {
            UpdateDirectoryContentPanel();
            UpdateFileSystemTree(FilemanagerTreeControl);
            UpdateHeader();
            ActionPanel.SetActiveView(FolderContentView);
        }

        public void FileManagerUploadButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(UploadView);
        }

        public void FileManagerDeleteButton_ClickHandler(object sender, EventArgs e)
        {
            foreach (string fileName in SelectedFiles)
            {
                File.Delete(RootDir + RequestedPath + "\\" + fileName);
            }

            foreach (string directoryName in SelectedDirectories)
            {
                Directory.Delete(RootDir + RequestedPath + "\\" + directoryName, true);
            }
            Cache.Remove(RootDir);

            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void FileManagerCopyButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(CopyView);
            copyTree.Nodes = GetFileSystemTree(RootDir, true);
        }

        public void FileManagerMoveButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(MoveView);
            moveTree.Nodes = GetFileSystemTree(RootDir, true);
        }

        public void FileManagerCreateDirectoryButton_ClockHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(CreateDirectoryView);
        }

        public void CancelUploadButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
        }

        public void FileManagerAddFoldersToZipButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(AddToZipView);
        }

        public void UploadButton_ClickHandler(object sender, EventArgs e)
        {
            if (FileManagerUpload1.HasFile)
            {
                FileManagerUpload1.SaveAs(RootDir + RequestedPath + "\\" + FileManagerUpload1.FileName);
            }

            if (FileManagerUpload2.HasFile)
            {
                FileManagerUpload2.SaveAs(RootDir + RequestedPath + "\\" + FileManagerUpload2.FileName);
            }

            if (FileManagerUpload3.HasFile)
            {
                FileManagerUpload3.SaveAs(RootDir + RequestedPath + "\\" + FileManagerUpload3.FileName);
            }

            if (FileManagerUpload4.HasFile)
            {
                FileManagerUpload4.SaveAs(RootDir + RequestedPath + "\\" + FileManagerUpload4.FileName);
            }

            ActionPanel.SetActiveView(FolderContentView);

        }

        public void MoveButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);

            var path = RootDir + moveTree.SelectedNode + "\\";
            foreach (string fileName in SelectedFiles)
            {
                File.Move(RootDir + RequestedPath + "\\" + fileName, path + fileName);
            }
            foreach (string directoryName in SelectedDirectories)
            {
                var directory = new DirectoryInfo(RootDir + RequestedPath + "\\" + directoryName);
                directory.Copy(RootDir + RequestedPath + "\\" + directoryName, path + directoryName);
                directory.Delete(true);

            }
            Cache.Remove(RootDir);

            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void CancelMoveButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
        }

        public void CopyButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);

            var path = RootDir + copyTree.SelectedNode + "\\";
            foreach (string fileName in SelectedFiles)
            {
                File.Copy(RootDir + RequestedPath + "\\" + fileName, path + fileName);
            }
            foreach (string directoryName in SelectedDirectories)
            {
                var directory = new DirectoryInfo(RootDir + RequestedPath + "\\" + directoryName);
                directory.Copy(RootDir + RequestedPath + "\\" + directoryName, path + directoryName);
            }
            Cache.Remove(RootDir);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void CancelCopyButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
        }

        public void CreateDirectoryButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
            Cache.Remove(RootDir);
            Directory.CreateDirectory(RootDir + RequestedPath + "\\" + FileManagerDirectoryNameTextBox.Text);
        }

        public void CancelCreateDirectoryButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
        }

        public void FileManagerExtractZipArchiveButton_ClickHandler(object sender, EventArgs e)
        {
            foreach (string fileName in SelectedFiles)
            {
                if (fileName.EndsWith(".zip"))
                {

                    FastZip zip = new FastZip();
                    zip.ExtractZip(RootDir + RequestedPath + "\\" + fileName, RootDir + RequestedPath, null);
                }
            }
            ClearSelectedFiles();
            Cache.Remove(RootDir);
        }

        public void AddToZipButton_ClickHandler(object sender, EventArgs e)
        {
            string directoryFilter = string.Join("|", SelectedDirectories);
            if (!string.IsNullOrEmpty(directoryFilter))
            {
                directoryFilter = "(" + directoryFilter + ")";
            }
            else
            {
                directoryFilter = "^(.*)";
            }

            string fileFilter = string.Join("|", SelectedFiles);
            if (string.IsNullOrEmpty(fileFilter))
            {
                fileFilter = "^(" + FileManagerZipNameTextBox.Text + ")";
            }
            if (SelectedDirectories.Count() != 0)
            {
                fileFilter = "(" + fileFilter + ")|((" + directoryFilter + ")\\\\.*)";
            }
            else
            {
                fileFilter = "(" + fileFilter + ")";
            }


            FastZip zip = new FastZip();
            zip.CreateZip(RootDir + RequestedPath + "\\" + FileManagerZipNameTextBox.Text, RootDir + RequestedPath, true, fileFilter, directoryFilter);
            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void CancelAddToZipButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void FileManagerRenameButton_ClickHandler(object sender, EventArgs e)
        {
            if (SelectedFiles.Length != 0)
            {
                FileManagerOldNameTextBox.Text = SelectedFiles[0];
                ActionPanel.SetActiveView(RenameView);
            }
        }

        public void RenameButton_ClickHandler(object sender, EventArgs e)
        {
            if (SelectedFiles.Count() > 0)
            {
                File.Move(RootDir + RequestedPath + "\\" + SelectedFiles[0], RootDir + RequestedPath + "\\" + FileManagerNewNameTextBox.Text);
            }

            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        public void CancelRenameButton_ClickHandler(object sender, EventArgs e)
        {
            ActionPanel.SetActiveView(FolderContentView);
            ClearSelectedDirectories();
            ClearSelectedFiles();
        }

        protected void DeleteButton_ClickHandler(object sender, CommandEventArgs e)
        {
            string path = RootDir + RequestedPath + "\\" + e.CommandName;

            if (File.Exists(path))
            {
                File.Delete(path);
                return;
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Cache.Remove(RootDir);

            ClearSelectedDirectories();
            ClearSelectedFiles();

        }


        private void NavigateTo(string path)
        {
            RequestedPath = path;
            if (RequestedPath.EndsWith(".."))
            {
                RequestedPath = RequestedPath.Substring(0, RequestedPath.Length - 3);
                if ((RequestedPath.LastIndexOf("\\") != -1))
                {
                    RequestedPath = RequestedPath.Substring(0, RequestedPath.LastIndexOf("\\"));
                }
                else
                {
                    RequestedPath = "";
                }
            }
            ExpandAndSelectTreeNode(RequestedPath);
            ViewState["FileManagerPath"] = RequestedPath;
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            if (scriptManager.IsInAsyncPostBack)
            {
                scriptManager.AddHistoryPoint("path", path);
            }

        }

        private void DownloadFile(string path)
        {
            Response.ContentType = "application/download";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + path.Substring(path.LastIndexOf("\\") + 1));
            Response.TransmitFile(path);
            Response.End();
        }

        private void ExpandAndSelectTreeNode(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            TreeNode node = FilemanagerTreeControl.FindNode(path);
            node.Select();

            while (node != null)
            {
                node.Expand();
                node = node.Parent;
            }
        }

        private bool IsValidPathString(string path)
        {
            return !path.Contains(new string(System.IO.Path.GetInvalidPathChars()));
        }

        private TreeNodeCollection GetFileSystemTree(string path, bool singleRoot)
        {
            TreeNodeCollection baseData = GetFileSystemTree(path);
            if (singleRoot)
            {
                TreeNode rootNode = new TreeNode();
                foreach (TreeNode node in baseData)
                {
                    rootNode.ChildNodes.Add(node);
                }
                rootNode.Text = "root";
                rootNode.Value = "\\\\";
                var result = new TreeNodeCollection();
                result.Add(rootNode);
                return result;
            }
            else
            {
                return baseData;
            }
        }

        private TreeNodeCollection GetFileSystemTree(string path)
        {

            TreeNodeCollection result = new TreeNodeCollection();
            DirectoryInfo rootDirectory = new DirectoryInfo(path);
            DirectoryInfo[] directories;
            directories = rootDirectory.GetDirectories();

            foreach (DirectoryInfo dir in directories)
            {
                TreeNode treeNode = new TreeNode(dir.Name);
                foreach (TreeNode tn in GetFileSystemTree(dir.FullName))
                {
                    treeNode.ChildNodes.Add(tn);
                }
                result.Add(treeNode);
            }
            return result;
        }


        private string GetNodePath(TreeNode node)
        {
            string result = node.Text;
            if (node.Parent != null)
            {
                result = GetNodePath(node.Parent) + "\\" + result;
            }
            return result;
        }

        private void UpdateFileSystemTree(TreeView control)
        {
            UpdateFileSystemTree(control, false, null);
        }

        private void UpdateFileSystemTree(TreeView control, bool singleRoot, string rootName)
        {
            string path = RootDir;
            if (Directory.Exists(path))
            {
                control.Nodes.Clear();
                TreeNodeCollection rootNodes;
                if (singleRoot)
                {
                    if (Cache[path + rootName] != null)
                    {
                        rootNodes = (TreeNodeCollection)Cache[path + rootName];
                    }
                    else
                    {
                        rootNodes = GetFileSystemTree(path);
                        Cache.Insert(path + rootName, rootNodes, null, DateTime.MaxValue, TimeSpan.FromMinutes(10));
                    }
                    TreeNode root = new TreeNode(rootName);
                    foreach (TreeNode tn in rootNodes)
                    {
                        root.ChildNodes.Add(tn);
                    }
                    control.Nodes.Add(root);
                }
                else
                {
                    if (Cache[path] != null)
                    {
                        rootNodes = (TreeNodeCollection)Cache[path];
                    }
                    else
                    {
                        rootNodes = GetFileSystemTree(path);
                        Cache.Insert(path, rootNodes, null, DateTime.MaxValue, TimeSpan.FromMinutes(10));
                    }
                    foreach (TreeNode tn in rootNodes)
                    {
                        control.Nodes.Add(tn);
                    }
                }
                control.CollapseAll();
            }
            else
            {
                FileManagerFooterBlock.InnerText = "Путь " + RequestedPath + " не существует";
            }
        }

        private void UpdateDirectoryContentPanel()
        {
            FileManagerUpdatePanel.Triggers.Clear();
            DirectoryInfo directory = new DirectoryInfo(RootDir + RequestedPath);
            CurrentDirectoryDirectories = directory.GetDirectories();
            CurrentDirectoryFiles = directory.GetFiles();
            HtmlTableRow header = (HtmlTableRow)fileManagerFolderContentTable.Controls[0];
            fileManagerFolderContentTable.Controls.Clear();
            fileManagerFolderContentTable.Controls.Add(header);
            if (!string.IsNullOrEmpty(RequestedPath))
            {
                HtmlTableRow tr = new HtmlTableRow();
                tr.Attributes.Add("onmouseover", "onTableRowMouseOver(this)");
                tr.Attributes.Add("onmouseout", "onTableRowMouseLeft(this)");

                HtmlTableCell checkCell = new HtmlTableCell();

                checkCell.Attributes.Add("class", "fileManagerFolderContentTableSelectCell");
                tr.Controls.Add(checkCell);

                HtmlTableCell nameCell = new HtmlTableCell();
                LinkButton nameLinkButton = new LinkButton();
                nameLinkButton.Text = "..";
                nameLinkButton.ID = "..";
                if (nameLinkButton.Text.Length > 80)
                {
                    nameLinkButton.Text = nameLinkButton.Text.Substring(0, 80) + "...";
                }
                nameLinkButton.Click += new System.EventHandler(DirectoryContentLink_Click_Handler);
                nameCell.Controls.Add(nameLinkButton);
                nameCell.Attributes.Add("class", "fileManagerFolderContentTableNameCell");
                tr.Controls.Add(nameCell);

                HtmlTableCell lengthCell = new HtmlTableCell();
                lengthCell.InnerText = "Папка";
                lengthCell.Attributes.Add("class", "fileManagerFolderContentTableLengthCell");
                tr.Controls.Add(lengthCell);

                HtmlTableCell lastChangeCell = new HtmlTableCell();
                Literal LastChangeLiteral = new Literal();

                lastChangeCell.Controls.Add(LastChangeLiteral);
                lastChangeCell.Attributes.Add("class", "fileManagerFolderContentTableLastModificationCell");
                tr.Controls.Add(lastChangeCell);

                HtmlTableCell downloadCell = new HtmlTableCell();
                downloadCell.Attributes.Add("class", "fileManagerFolderContentTableDownloadCell");
                tr.Controls.Add(downloadCell);

                HtmlTableCell deleteCell = new HtmlTableCell();
                deleteCell.Attributes.Add("class", "fileManagerFolderContentTableDeleteCell");
                tr.Controls.Add(deleteCell);

                fileManagerFolderContentTable.Controls.Add(tr);
            }
            foreach (DirectoryInfo dir in CurrentDirectoryDirectories)
            {
                HtmlTableRow tr = new HtmlTableRow();
                tr.Attributes.Add("onmouseover", "onTableRowMouseOver(this)");
                tr.Attributes.Add("onmouseout", "onTableRowMouseLeft(this)");
                tr.Attributes.Add("onclick", "onTableRowClick(this, event);");

                HtmlTableCell checkCell = new HtmlTableCell();
                CheckBox check = new CheckBox();
                check.ID = dir.Name.GetHashCode() + "$Directory";
                checkCell.Controls.Add(check);
                if (SelectedDirectories.Contains(dir.Name))
                {
                    check.Checked = true;
                }
                check.Attributes.Add("onclick", "stopBubble(event);");
                checkCell.Attributes.Add("class", "fileManagerFolderContentTableSelectCell");
                tr.Controls.Add(checkCell);

                HtmlTableCell nameCell = new HtmlTableCell();
                LinkButton nameLinkButton = new LinkButton();
                nameLinkButton.Text = dir.Name;
                nameLinkButton.ID = dir.Name.GetHashCode().ToString();
                if (nameLinkButton.Text.Length > 80)
                {
                    nameLinkButton.Text = nameLinkButton.Text.Substring(0, 80) + "...";
                }
                nameLinkButton.Click += new System.EventHandler(DirectoryContentLink_Click_Handler);
                nameCell.Controls.Add(nameLinkButton);
                nameCell.Attributes.Add("class", "fileManagerFolderContentTableNameCell");
                tr.Controls.Add(nameCell);

                HtmlTableCell lengthCell = new HtmlTableCell();
                lengthCell.InnerText = "Папка";
                lengthCell.Attributes.Add("class", "fileManagerFolderContentTableLengthCell");
                tr.Controls.Add(lengthCell);

                HtmlTableCell lastChangeCell = new HtmlTableCell();
                Literal LastChangeLiteral = new Literal();
                LastChangeLiteral.Text = dir.LastWriteTime.ToString("dd MMM yyyy H:MM", CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                lastChangeCell.Controls.Add(LastChangeLiteral);
                lastChangeCell.Attributes.Add("class", "fileManagerFolderContentTableLastModificationCell");
                tr.Controls.Add(lastChangeCell);


                HtmlTableCell downloadCell = new HtmlTableCell();
                downloadCell.Attributes.Add("class", "fileManagerFolderContentTableDownloadCell");

                tr.Controls.Add(downloadCell);

                HtmlTableCell deleteCell = new HtmlTableCell();
                deleteCell.Attributes.Add("class", "fileManagerFolderContentTableDeleteCell");
                ImageButton deleteButton = new ImageButton();
                deleteButton.ImageUrl = "images/delete.png";
                deleteButton.ToolTip = "Удалить";
                deleteButton.AlternateText = "Удалить";
                deleteButton.ID = dir.Name.GetHashCode() + "_deleteButton";
                deleteButton.CommandName = dir.Name;
                deleteButton.Command += new System.Web.UI.WebControls.CommandEventHandler(DeleteButton_ClickHandler);

                ConfirmButtonExtender es = new ConfirmButtonExtender();
                es.ID = dir.Name.GetHashCode().ToString() + "_DeleteConfirmer";
                es.TargetControlID = deleteButton.ID;
                es.ConfirmText = "Вы дейстивтельно хотите удалить эту папку?";

                deleteCell.Controls.Add(es);

                deleteCell.Controls.Add(deleteButton);

                tr.Controls.Add(deleteCell);


                fileManagerFolderContentTable.Controls.Add(tr);
            }

            foreach (FileInfo file in CurrentDirectoryFiles)
            {
                HtmlTableRow tr = new HtmlTableRow();
                tr.Attributes.Add("onmouseover", "onTableRowMouseOver(this)");
                tr.Attributes.Add("onmouseout", "onTableRowMouseLeft(this)");
                tr.Attributes.Add("onclick", "onTableRowClick(this, event);");

                HtmlTableCell checkCell = new HtmlTableCell();
                CheckBox check = new CheckBox();
                check.Attributes.Add("onclick", "stopBubble(event);");
                check.ID = file.Name.GetHashCode() + "$File";
                if (SelectedFiles.Contains(file.Name))
                {
                    check.Checked = true;
                }
                checkCell.Controls.Add(check);
                checkCell.Attributes.Add("class", "fileManagerFolderContentTableSelectCell");
                tr.Controls.Add(checkCell);

                HtmlTableCell nameCell = new HtmlTableCell();
                HtmlAnchor nameLink = new HtmlAnchor();
                nameLink.ID = file.Name.GetHashCode() + "Link";
                nameLink.HRef = Request.AppRelativeCurrentExecutionFilePath + "?path=" + RequestedPath + "\\" + file.Name;
                nameLink.InnerText = file.Name;
                if (nameLink.InnerText.Length > 80)
                {
                    nameLink.InnerText = nameLink.InnerText.Substring(0, 80) + "...";
                }
                var nameLinkContainer = new HtmlGenericControl("div");
                nameLinkContainer.Controls.Add(nameLink);
                nameCell.Controls.Add(nameLinkContainer);
                nameCell.Attributes.Add("class", "fileManagerFolderContentTableNameCell");
                tr.Controls.Add(nameCell);


                HtmlTableCell lengthCell = new HtmlTableCell();
                Literal lengthLiteral = new Literal();
                long length = 0;
                if (Cache[file.FullName + "_Length"] != null)
                {
                    length = (long)Cache[file.FullName + "_Length"];
                }
                else
                {
                    length = file.Length;
                    CacheDependency cd = new CacheDependency(file.FullName);
                    Cache.Insert(file.FullName + "_Length", length, cd, DateTime.MaxValue, TimeSpan.FromMinutes(5));
                }
                lengthLiteral.Text = Math.Ceiling((double)length / 1024).ToString() + " КБ";
                lengthCell.Controls.Add(lengthLiteral);
                lengthCell.Attributes.Add("class", "fileManagerFolderContentTableLengthCell");
                tr.Controls.Add(lengthCell);

                HtmlTableCell lastChangeCell = new HtmlTableCell();
                Literal LastChangeLiteral = new Literal();
                DateTime lastChange;
                if (Cache[file.FullName + "_LastWriteTime"] != null)
                {
                    lastChange = (DateTime)Cache[file.FullName + "_LastWriteTime"];
                }
                else
                {
                    lastChange = file.LastWriteTime;
                    Cache.Insert(file.FullName + "_LastWriteTime", lastChange, null, DateTime.MaxValue, TimeSpan.FromMinutes(5));
                }
                LastChangeLiteral.Text = lastChange.ToString("dd MMM yyyy H:MM", CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                lastChangeCell.Controls.Add(LastChangeLiteral);
                lastChangeCell.Attributes.Add("class", "fileManagerFolderContentTableLastModificationCell");
                tr.Controls.Add(lastChangeCell);

                HtmlTableCell downloadCell = new HtmlTableCell();
                downloadCell.Attributes.Add("class", "fileManagerFolderContentTableDownloadCell");
                ImageButton downButton = new ImageButton();
                downButton.ImageUrl = "images/download.png";
                downButton.AlternateText = "Скачать";
                downButton.ToolTip = "Скачать";
                downButton.ID = file.Name + "_downlodButton";
                downButton.Attributes.Add("onclick", "downloadFile(event, \'" + RequestedPath + file.Name + "\');");
                downloadCell.Controls.Add(downButton);

                tr.Controls.Add(downloadCell);

                HtmlTableCell deleteCell = new HtmlTableCell();
                deleteCell.Attributes.Add("class", "fileManagerFolderContentTableDeleteCell");
                ImageButton deleteButton = new ImageButton();
                deleteButton.ImageUrl = "images/delete.png";
                deleteButton.ToolTip = "Удалить";
                deleteButton.AlternateText = "Удалить";
                deleteButton.ID = file.Name.GetHashCode() + "_deleteButton";
                deleteButton.CommandName = file.Name;
                deleteButton.Command += new System.Web.UI.WebControls.CommandEventHandler(DeleteButton_ClickHandler);
                deleteCell.Controls.Add(deleteButton);

                ConfirmButtonExtender es = new ConfirmButtonExtender();
                es.ID = file.Name.GetHashCode() + "_DeleteConfirmer";
                es.TargetControlID = deleteButton.ID;
                es.ConfirmText = "Вы дейстивтельно хотите удалить этот файл?";

                deleteCell.Controls.Add(es);

                deleteCell.Controls.Add(deleteButton);

                tr.Controls.Add(deleteCell);

                fileManagerFolderContentTable.Controls.Add(tr);

            }
        }

        private void UpdateHeader()
        {
            FileManagerControlHeader.Controls.Clear();
            LinkButton rootLinkButton = new LinkButton();
            rootLinkButton.Text = "root";
            rootLinkButton.ID = "HeaderRootLinkButton";
            rootLinkButton.CommandName = "Click";
            rootLinkButton.CommandArgument = "";
            rootLinkButton.Command += new System.Web.UI.WebControls.CommandEventHandler(FileManagerHeaderLink_Command_Handler);
            FileManagerControlHeader.Controls.Add(rootLinkButton);
            if (!string.IsNullOrEmpty(RequestedPath))
            {
                string[] pathParts = RequestedPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length > 0)
                {
                    for (var i = 0; i <= pathParts.Length - 1; i++)
                    {
                        string part = pathParts[i];
                        Literal separator = new Literal();
                        separator.Text = " / ";
                        FileManagerControlHeader.Controls.Add(separator);

                        string commandArgument = string.Join("\\", pathParts, 0, i + 1);
                        LinkButton partLink = new LinkButton();
                        partLink.Text = part;
                        partLink.CommandName = "Click";
                        partLink.CommandArgument = commandArgument;
                        partLink.ID = part + "_headerLink";
                        partLink.Command += new System.Web.UI.WebControls.CommandEventHandler(FileManagerHeaderLink_Command_Handler);
                        FileManagerControlHeader.Controls.Add(partLink);
                    }
                }
            }
        }

        private void SaveSelectedDirectories()
        {
            if (SelectedDirectories.Length != 0)
            {
                ViewState["FileManager_SelectedDirectories"] = _selectedDirectories;
            }
        }

        private void LoadSelectedDirectories()
        {
            if (ViewState["FileManager_SelectedDirectories"] != null)
            {
                _selectedDirectories = (List<string>)ViewState["FileManager_SelectedDirectories"];
            }
            if (_selectedDirectories == null)
            {
                _selectedDirectories = new List<string>();
            }

            Dictionary<int, string> directoryDic = new Dictionary<int, string>();

            string[] dics = Directory.GetDirectories(RootDir + RequestedPath);
            for (int i = 0; i < dics.Length; i++)
            {
                dics[i] = dics[i].Substring(dics[i].LastIndexOf("\\") + 1);
                directoryDic.Add(dics[i].GetHashCode(), dics[i]);
            }

            foreach (string req in Request.Form.AllKeys)
            {
                if (req != null)
                {
                    string[] idParts = req.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (idParts[idParts.Length - 1] == "Directory")
                    {
                        if (directoryDic.ContainsKey(int.Parse(idParts[idParts.Length - 2])))
                        {
                            try
                            {
                                _selectedDirectories.Add(directoryDic[int.Parse(idParts[idParts.Length - 2])]);
                            }
                            catch (Exception ex)
                            {
                                AdvantShop.Diagnostics.Debug.Log.Error(ex);
                            }

                        }
                    }
                }
            }
        }

        private void ClearSelectedDirectories()
        {
            _selectedDirectories = null;
            if (ViewState["FileManager_SelectedDirectories"] != null)
            {
                ViewState.Remove("FileManager_SelectedDirectories");
            }
        }

        private void SaveSelectedFiles()
        {
            if (SelectedFiles.Length != 0)
            {
                ViewState["FileManager_SelectedFiles"] = _selectedFiles;
            }
        }

        private void LoadSelectedFiles()
        {
            if (ViewState["FileManager_SelectedFiles"] != null)
            {
                _selectedFiles = (List<string>)ViewState["FileManager_SelectedFiles"];
            }
            if (_selectedFiles == null)
            {
                _selectedFiles = new List<string>();
            }

            Dictionary<int, string> fileDic = new Dictionary<int, string>();
            string[] files = Directory.GetFiles(RootDir + RequestedPath);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                fileDic.Add(files[i].GetHashCode(), files[i]);
            }
            foreach (string req in Request.Form.AllKeys)
            {
                if (req != null)
                {
                    string[] idParts = req.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (idParts[idParts.Length - 1] == "File")
                    {
                        if (fileDic.ContainsKey(int.Parse(idParts[idParts.Length - 2])))
                        {
                            try
                            {
                                _selectedFiles.Add(fileDic[int.Parse(idParts[idParts.Length - 2])]);
                            }
                            catch (Exception ex)
                            {
                                AdvantShop.Diagnostics.Debug.Log.Error(ex);
                            }
                        }
                    }
                }
            }
        }

        private void ClearSelectedFiles()
        {
            _selectedFiles = null;
            if (ViewState["FileManager_SelectedFiles"] != null)
            {
                ViewState.Remove("FileManager_SelectedFiles");
            }
        }

        public bool Contains(string @this, char[] chars)
        {
            bool result;
            result = true;
            foreach (char ch in chars)
            {
                result &= @this.Contains(ch);
            }
            return result;
        }

    }

    static class DirectoryExtensions
    {
        public static void Copy(this System.IO.DirectoryInfo @this, string source, string destination)
        {
            DirectoryInfo dir = new DirectoryInfo(source);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw (new DirectoryNotFoundException("Source directory does not exist or could not be found: " + source));
            }

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destination, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (var subdir in dirs)
            {
                string temppath = Path.Combine(destination, subdir.Name);
                @this.Copy(subdir.FullName, temppath);
            }

        }

        public static void Move(this System.IO.DirectoryInfo @this, string source, string destination)
        {
            DirectoryInfo dir = new DirectoryInfo(source);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw (new DirectoryNotFoundException("Source directory does not exist or could not be found: " + source));
            }

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destination, file.Name);
                file.MoveTo(temppath);
            }

            foreach (var subdir in dirs)
            {
                string temppath = Path.Combine(destination, subdir.Name);
                @this.Move(subdir.FullName, temppath);
            }

        }
    }
}