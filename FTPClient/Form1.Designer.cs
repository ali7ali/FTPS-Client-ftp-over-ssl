namespace FTPClient
{
    partial class FTPSClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTPSClient));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Waiting..");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("ClientFileExplorer");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setClientDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.sendCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtserver = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtusername = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtpassword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.txtport = new System.Windows.Forms.ToolStripTextBox();
            this.btn_Sconnect = new System.Windows.Forms.ToolStripButton();
            this.btn_connect = new System.Windows.Forms.ToolStripButton();
            this.btn_desconnect = new System.Windows.Forms.ToolStripButton();
            this.richTextLog = new System.Windows.Forms.RichTextBox();
            this.ServerView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ClientView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1UploadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2UploadDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(924, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setClientDirectory,
            this.sendCommandToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // setClientDirectory
            // 
            this.setClientDirectory.Name = "setClientDirectory";
            this.setClientDirectory.Size = new System.Drawing.Size(175, 22);
            this.setClientDirectory.Text = "Set Client Directory";
            this.setClientDirectory.Click += new System.EventHandler(this.setClientDirectoryToolStripMenuItem_Click);
            // 
            // sendCommandToolStripMenuItem
            // 
            this.sendCommandToolStripMenuItem.Name = "sendCommandToolStripMenuItem";
            this.sendCommandToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sendCommandToolStripMenuItem.Text = "Send Command";
            this.sendCommandToolStripMenuItem.Click += new System.EventHandler(this.sendCommandToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtserver,
            this.toolStripLabel2,
            this.txtusername,
            this.toolStripLabel3,
            this.txtpassword,
            this.toolStripLabel4,
            this.txtport,
            this.btn_Sconnect,
            this.btn_connect,
            this.btn_desconnect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(924, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "Server:";
            // 
            // txtserver
            // 
            this.txtserver.Name = "txtserver";
            this.txtserver.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel2.Text = "UserName:";
            // 
            // txtusername
            // 
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel3.Text = "Password:";
            // 
            // txtpassword
            // 
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel4.Text = "Port:";
            // 
            // txtport
            // 
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(100, 25);
            this.txtport.Text = "21";
            // 
            // btn_Sconnect
            // 
            this.btn_Sconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Sconnect.Image = ((System.Drawing.Image)(resources.GetObject("btn_Sconnect.Image")));
            this.btn_Sconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Sconnect.Name = "btn_Sconnect";
            this.btn_Sconnect.Size = new System.Drawing.Size(23, 22);
            this.btn_Sconnect.Text = "Connect Secure";
            this.btn_Sconnect.Click += new System.EventHandler(this.btn_Sconnect_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_connect.Image = ((System.Drawing.Image)(resources.GetObject("btn_connect.Image")));
            this.btn_connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(23, 22);
            this.btn_connect.Text = "Connect FTP";
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_desconnect
            // 
            this.btn_desconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_desconnect.Image = ((System.Drawing.Image)(resources.GetObject("btn_desconnect.Image")));
            this.btn_desconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_desconnect.Name = "btn_desconnect";
            this.btn_desconnect.Size = new System.Drawing.Size(23, 22);
            this.btn_desconnect.Text = "btn_desconnect";
            this.btn_desconnect.Click += new System.EventHandler(this.btn_desconnect_Click);
            // 
            // richTextLog
            // 
            this.richTextLog.Location = new System.Drawing.Point(0, 52);
            this.richTextLog.Name = "richTextLog";
            this.richTextLog.Size = new System.Drawing.Size(924, 179);
            this.richTextLog.TabIndex = 2;
            this.richTextLog.Text = "";
            this.richTextLog.TextChanged += new System.EventHandler(this.richTextLog_TextChanged);
            // 
            // ServerView
            // 
            this.ServerView.ContextMenuStrip = this.contextMenuStrip1;
            this.ServerView.ImageIndex = 0;
            this.ServerView.ImageList = this.imageList1;
            this.ServerView.Location = new System.Drawing.Point(468, 237);
            this.ServerView.Name = "ServerView";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Waiting..";
            this.ServerView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.ServerView.SelectedImageIndex = 0;
            this.ServerView.ShowNodeToolTips = true;
            this.ServerView.Size = new System.Drawing.Size(456, 150);
            this.ServerView.TabIndex = 6;
            this.ServerView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ServerView_AfterLabelEdit);
            this.ServerView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ServerView_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDirectoryToolStripMenuItem,
            this.removeDirectoryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 48);
            // 
            // newDirectoryToolStripMenuItem
            // 
            this.newDirectoryToolStripMenuItem.Name = "newDirectoryToolStripMenuItem";
            this.newDirectoryToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.newDirectoryToolStripMenuItem.Text = "New Folder";
            this.newDirectoryToolStripMenuItem.Click += new System.EventHandler(this.newDirectoryToolStripMenuItem_Click);
            // 
            // removeDirectoryToolStripMenuItem
            // 
            this.removeDirectoryToolStripMenuItem.Name = "removeDirectoryToolStripMenuItem";
            this.removeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.removeDirectoryToolStripMenuItem.Text = "Remove Folder";
            this.removeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.removeDirectoryToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.ico");
            this.imageList1.Images.SetKeyName(1, "folder.ico");
            this.imageList1.Images.SetKeyName(2, "folder.ico");
            this.imageList1.Images.SetKeyName(3, "netfol.ico");
            // 
            // ClientView
            // 
            this.ClientView.ContextMenuStrip = this.contextMenuStrip2;
            this.ClientView.ImageIndex = 0;
            this.ClientView.ImageList = this.imageList1;
            this.ClientView.Location = new System.Drawing.Point(0, 237);
            this.ClientView.Name = "ClientView";
            treeNode2.Name = "Node0";
            treeNode2.Text = "ClientFileExplorer";
            this.ClientView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.ClientView.SelectedImageIndex = 0;
            this.ClientView.Size = new System.Drawing.Size(462, 317);
            this.ClientView.TabIndex = 4;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1UploadFile,
            this.toolStripMenuItem2UploadDirectory});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(164, 48);
            // 
            // toolStripMenuItem1UploadFile
            // 
            this.toolStripMenuItem1UploadFile.Name = "toolStripMenuItem1UploadFile";
            this.toolStripMenuItem1UploadFile.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItem1UploadFile.Text = "Upload File";
            this.toolStripMenuItem1UploadFile.ToolTipText = "Upload File";
            this.toolStripMenuItem1UploadFile.Click += new System.EventHandler(this.toolStripMenuItem1UploadFile_Click);
            // 
            // toolStripMenuItem2UploadDirectory
            // 
            this.toolStripMenuItem2UploadDirectory.Name = "toolStripMenuItem2UploadDirectory";
            this.toolStripMenuItem2UploadDirectory.Size = new System.Drawing.Size(163, 22);
            this.toolStripMenuItem2UploadDirectory.Text = "Upload Directory";
            this.toolStripMenuItem2UploadDirectory.ToolTipText = "Upload Directory";
            this.toolStripMenuItem2UploadDirectory.Click += new System.EventHandler(this.toolStripMenuItem2UploadDirectory_Click);
            // 
            // ServerFiles
            // 
            this.ServerFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.ServerFiles.ContextMenuStrip = this.contextMenuStrip3;
            this.ServerFiles.LabelEdit = true;
            this.ServerFiles.Location = new System.Drawing.Point(468, 393);
            this.ServerFiles.Name = "ServerFiles";
            this.ServerFiles.Size = new System.Drawing.Size(456, 202);
            this.ServerFiles.TabIndex = 5;
            this.ServerFiles.UseCompatibleStateImageBehavior = false;
            this.ServerFiles.View = System.Windows.Forms.View.Details;
            this.ServerFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.ServerFiles_AfterLabelEdit);
            this.ServerFiles.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.ServerFiles_BeforeLabelEdit);
            this.ServerFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ServerFiles_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 168;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "File Type";
            this.columnHeader3.Width = 109;
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameFileToolStripMenuItem,
            this.removeFileToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(139, 48);
            // 
            // renameFileToolStripMenuItem
            // 
            this.renameFileToolStripMenuItem.Name = "renameFileToolStripMenuItem";
            this.renameFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.renameFileToolStripMenuItem.Text = "Rename File";
            this.renameFileToolStripMenuItem.Click += new System.EventHandler(this.renameFileToolStripMenuItem_Click);
            // 
            // removeFileToolStripMenuItem
            // 
            this.removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            this.removeFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.removeFileToolStripMenuItem.Text = "Delete File";
            this.removeFileToolStripMenuItem.Click += new System.EventHandler(this.removeFileToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 560);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(450, 34);
            this.progressBar1.TabIndex = 7;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Dpwork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Progresschanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(209, 576);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 8;
            // 
            // FTPSClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(924, 598);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ServerFiles);
            this.Controls.Add(this.ClientView);
            this.Controls.Add(this.ServerView);
            this.Controls.Add(this.richTextLog);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FTPSClient";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTPSClient";
            this.Load += new System.EventHandler(this.FTPSClient_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendCommandToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtserver;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtusername;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox txtpassword;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox txtport;
        private System.Windows.Forms.ToolStripButton btn_connect;
        private System.Windows.Forms.ToolStripButton btn_desconnect;
        private System.Windows.Forms.RichTextBox richTextLog;
        private System.Windows.Forms.TreeView ServerView;
        private System.Windows.Forms.TreeView ClientView;
        private System.Windows.Forms.ListView ServerFiles;
        private System.Windows.Forms.ContextMenuStrip ContextForTreeViewServer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1UploadFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2UploadDirectory;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripButton btn_Sconnect;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDirectoryToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem renameFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setClientDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
    }
}

