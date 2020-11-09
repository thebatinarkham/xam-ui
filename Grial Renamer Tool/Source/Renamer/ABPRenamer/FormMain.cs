using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ABPRenamer
{
    public class FormMain : Form
    {
        public class MyEventArgs : EventArgs
        {
            public bool IsCompleted
            {
                get;
                set;
            } = true;

        }

        private IContainer components;

        private Label lbOriginalProjectName;

        private Label lbProjectPath;

        private TextBox txtOldCompanyName;

        private TextBox txtOldProjectName;

        private Button btnStart;

        private TextBox txtNewProjectName;

        private TextBox txtNewCompanyName;

        private Label lbNewProjectName;

        private Label lbArrow2nd;

        private Button btnSelect;

        private BackgroundWorker backgroundWorker;

        private Button btnClose;

        private ProgressBar progressBar1;

        private Label lbTitle;

        private Panel panel1;

        private Label label9;

        public const int WM_SYSCOMMAND = 274;

        public const int SC_MOVE = 61456;

        public const int HTCAPTION = 2;

        private TextBox Console;

        private Label lbReset;

        private Button btnReset;

        private TextBox txtFilter;

        private TextBox txtRootDir;

        public FormMain()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Renombrar")
            {
                StartMethod();
            }
            else
            {
                StopMethod();
            }
        }

        private void StartMethod()
        {
            Arguments arguments = new Arguments
            {
                OldCompanyName = txtOldCompanyName.Text.Trim(),
                OldProjectName = txtOldProjectName.Text.Trim(),
                NewCompanyName = txtNewCompanyName.Text.Trim(),
            };
            arguments.NewProjectName = txtNewProjectName.Text.Trim();
            if (string.IsNullOrEmpty(arguments.NewProjectName))
            {
                MessageBox.Show("Please select the project path!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txtNewProjectName.Focus();
                return;
            }
            arguments.RootDir = txtRootDir.Text.Trim();
            if (string.IsNullOrWhiteSpace(arguments.RootDir))
            {
                if (DialogResult.Yes == MessageBox.Show("Please select the project path!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Question))
                {
                    BtnSelect_Click(null, null);
                }
            }
            else if (!Directory.Exists(arguments.RootDir))
            {
                MessageBox.Show("Please select the correct project path!");
            }
            else
            {
                progressBar1.Visible = true;
                backgroundWorker.RunWorkerAsync(arguments);
            }
        }

        private void StopMethod()
        {
            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show("Cancelling..");
                backgroundWorker.CancelAsync();
            }
        }

        private void Log(string value)
        {
            if (Console.InvokeRequired)
            {
                Action<string> method = delegate (string text)
                {
                    Console.AppendText(text);
                };
                Console.Invoke(method, value);
            }
            else
            {
                Console.AppendText(value);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Arguments arguments = e.Argument as Arguments;
            string rootDir = arguments.RootDir;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            RenameAllDir(worker, e, arguments);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Log($"================= Directory renaming completed =================time spent: {elapsedMilliseconds}(s)\r\n");
            stopwatch.Reset();
            stopwatch.Start();
            arguments.RootDir = rootDir;
            RenameAllFileNameAndContent(worker, e, arguments);
            stopwatch.Stop();
            Log($"================= File name and content renaming completed =================time spent: {stopwatch.ElapsedMilliseconds}(s)\r\n");
            Log($"================= Completed =================Time-spent catalog:{elapsedMilliseconds}s File time spent: {stopwatch.ElapsedMilliseconds}s\r\n");
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Log(e.UserState.ToString());
            progressBar1.PerformStep();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Text = "Renombrar";
            if (e.Cancelled)
            {
                MessageBox.Show("Task terminated");
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show("Internal error", e.Error.Message);
                throw e.Error;
            }
            if (DialogResult.Yes == MessageBox.Show("Processing completed successfully. Terminate App？", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                BtnClose_Click(null, new MyEventArgs());
            }
        }

        private void RenameAllDir(BackgroundWorker worker, DoWorkEventArgs e, Arguments arguments)
        {
            string[] directories = Directory.GetDirectories(arguments.RootDir);
            int percentProgress = 0;
            string[] array = directories;
            int num = 0;
            while (true)
            {
                if (num >= array.Length)
                {
                    return;
                }
                string text = array[num];
                if (worker.CancellationPending)
                {
                    break;
                }
                arguments.RootDir = text;
                RenameAllDir(worker, e, arguments);
                DirectoryInfo directoryInfo = new DirectoryInfo(text);
                if (directoryInfo.Name.Contains(arguments.OldCompanyName) || directoryInfo.Name.Contains(arguments.OldProjectName))
                {
                    string text2 = directoryInfo.Name;
                    if (!string.IsNullOrEmpty(arguments.OldCompanyName))
                    {
                        text2 = text2.Replace(arguments.OldCompanyName, arguments.NewCompanyName);
                    }
                    text2 = text2.Replace(arguments.OldProjectName, arguments.NewProjectName);
                    string text3 = Path.Combine(directoryInfo.Parent.FullName, text2);
                    if (directoryInfo.FullName != text3)
                    {
                        worker.ReportProgress(percentProgress, directoryInfo.FullName + "\r\n=>\r\n" + text3 + "\r\n\r\n");
                        directoryInfo.MoveTo(text3);
                    }
                }
                num++;
            }
            e.Cancel = true;
        }

        private void RenameAllFileNameAndContent(BackgroundWorker worker, DoWorkEventArgs e, Arguments arguments)
        {
            List<FileInfo> list = (from m in new DirectoryInfo(arguments.RootDir).GetFiles()
                                   where arguments.filter.Contains(m.Extension)
                                   select m).ToList();
            int percentProgress = 0;
            foreach (FileInfo item in list)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                string text = File.ReadAllText(item.FullName, Encoding.UTF8);
                if (!string.IsNullOrEmpty(arguments.OldCompanyName))
                {
                    text = text.Replace(arguments.OldCompanyName, arguments.NewCompanyName);
                }
                text = text.Replace(arguments.OldProjectName, arguments.NewProjectName);
                if (item.Name.Contains(arguments.OldCompanyName) || item.Name.Contains(arguments.OldProjectName))
                {
                    string text2 = item.Name;
                    if (!string.IsNullOrEmpty(arguments.OldCompanyName))
                    {
                        text2 = text2.Replace(arguments.OldCompanyName, arguments.NewCompanyName);
                    }
                    text2 = text2.Replace(arguments.OldProjectName, arguments.NewProjectName);
                    string text3 = Path.Combine(item.DirectoryName, text2);
                    if (text3 != item.FullName)
                    {
                        worker.ReportProgress(percentProgress, "\r\n" + item.FullName + "\r\n=>\r\n" + text3 + "\r\n\r\n");
                        File.Delete(item.FullName);
                    }
                    File.WriteAllText(text3, text, Encoding.UTF8);
                }
                else
                {
                    File.WriteAllText(item.FullName, text, Encoding.UTF8);
                }
                worker.ReportProgress(percentProgress, item.Name + "=>Complete\r\n");
            }
            string[] directories = Directory.GetDirectories(arguments.RootDir);
            int num = 0;
            while (true)
            {
                if (num < directories.Length)
                {
                    string rootDir = directories[num];
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                    arguments.RootDir = rootDir;
                    RenameAllFileNameAndContent(worker, e, arguments);
                    num++;
                    continue;
                }
                return;
            }
            e.Cancel = true;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Please select the folder where the ABP project is located.(aspnet-zero-core-7.0)"
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    MessageBox.Show(this, "Folder path cannot be empty", "Prompt");
                }
                else
                {
                    txtRootDir.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                Settings.Default.setFilter = txtFilter.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtOldCompanyName.Text))
            {
                Settings.Default.setOldCompanyName = txtOldCompanyName.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtOldProjectName.Text))
            {
                Settings.Default.setOldProjectName = txtOldProjectName.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(txtRootDir.Text))
            {
                Settings.Default.setRootDir = txtRootDir.Text.Trim();
            }
            Settings.Default.setNewCompanyName = txtNewCompanyName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtNewProjectName.Text))
            {
                Settings.Default.setNewProjectName = txtNewProjectName.Text.Trim();
            }
            if (e is MyEventArgs)
            {
                Settings.Default.setOldCompanyName = txtNewCompanyName.Text.Trim();
                Settings.Default.setOldProjectName = txtNewProjectName.Text.Trim();
            }
            Settings.Default.Save();
            Environment.Exit(0);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.setFilter))
            {
                txtFilter.Text = Settings.Default.setFilter.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Settings.Default.setOldCompanyName))
            {
                txtOldCompanyName.Text = Settings.Default.setOldCompanyName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Settings.Default.setOldProjectName))
            {
                txtOldProjectName.Text = Settings.Default.setOldProjectName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Settings.Default.setRootDir))
            {
                txtRootDir.Text = Settings.Default.setRootDir.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Settings.Default.setNewCompanyName))
            {
                txtNewCompanyName.Text = Settings.Default.setNewCompanyName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Settings.Default.setNewProjectName))
            {
                txtNewProjectName.Text = Settings.Default.setNewProjectName.Trim();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtFilter.Text = ".cs,.cshtml,.js,.ts,.csproj,.sln,.xml,.config,.DotSettings,.json,.xaml,.txt,.html,.gitignore,.ps1,.md,.plist";
        }

        private void lbOriginalName_Click(object sender, EventArgs e)
        {
            txtOldCompanyName.Text = "MyCompanyName";
        }

        private void lbOriginalProjectName_Click(object sender, EventArgs e)
        {
            txtOldProjectName.Text = "AbpZeroTemplate";
        }

        private void lbProjectPath_Click(object sender, EventArgs e)
        {
            txtRootDir.Text = "";
        }

        private void lbNewCompanyName_Click(object sender, EventArgs e)
        {
            txtNewCompanyName.Text = "";
        }

        private void lbNewProjectName_Click(object sender, EventArgs e)
        {
            txtNewProjectName.Text = "";
        }

        private void lbOriginalAreaName_Click(object sender, EventArgs e)
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbOriginalProjectName = new System.Windows.Forms.Label();
            this.lbProjectPath = new System.Windows.Forms.Label();
            this.txtOldCompanyName = new System.Windows.Forms.TextBox();
            this.txtOldProjectName = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtNewProjectName = new System.Windows.Forms.TextBox();
            this.txtNewCompanyName = new System.Windows.Forms.TextBox();
            this.lbNewProjectName = new System.Windows.Forms.Label();
            this.lbArrow2nd = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.Console = new System.Windows.Forms.TextBox();
            this.lbReset = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.txtRootDir = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbOriginalProjectName
            // 
            this.lbOriginalProjectName.AutoSize = true;
            this.lbOriginalProjectName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbOriginalProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbOriginalProjectName.Location = new System.Drawing.Point(12, 600);
            this.lbOriginalProjectName.Name = "lbOriginalProjectName";
            this.lbOriginalProjectName.Size = new System.Drawing.Size(73, 13);
            this.lbOriginalProjectName.TabIndex = 1;
            this.lbOriginalProjectName.Text = "Original Name";
            this.lbOriginalProjectName.Click += new System.EventHandler(this.lbOriginalProjectName_Click);
            // 
            // lbProjectPath
            // 
            this.lbProjectPath.AutoSize = true;
            this.lbProjectPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbProjectPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbProjectPath.Location = new System.Drawing.Point(12, 669);
            this.lbProjectPath.Name = "lbProjectPath";
            this.lbProjectPath.Size = new System.Drawing.Size(74, 13);
            this.lbProjectPath.TabIndex = 2;
            this.lbProjectPath.Text = "Solution folder";
            this.lbProjectPath.Click += new System.EventHandler(this.lbProjectPath_Click);
            // 
            // txtOldCompanyName
            // 
            this.txtOldCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtOldCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldCompanyName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtOldCompanyName.Location = new System.Drawing.Point(138, 572);
            this.txtOldCompanyName.Name = "txtOldCompanyName";
            this.txtOldCompanyName.Size = new System.Drawing.Size(224, 20);
            this.txtOldCompanyName.TabIndex = 3;
            this.txtOldCompanyName.Text = "MyCompanyName";
            this.txtOldCompanyName.Visible = false;
            // 
            // txtOldProjectName
            // 
            this.txtOldProjectName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtOldProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtOldProjectName.Location = new System.Drawing.Point(138, 598);
            this.txtOldProjectName.Name = "txtOldProjectName";
            this.txtOldProjectName.Size = new System.Drawing.Size(224, 20);
            this.txtOldProjectName.TabIndex = 4;
            this.txtOldProjectName.Text = "AppName";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(187)))), ((int)(((byte)(119)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(539, 650);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(194, 48);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Rename";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // txtNewProjectName
            // 
            this.txtNewProjectName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtNewProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtNewProjectName.Location = new System.Drawing.Point(509, 598);
            this.txtNewProjectName.Name = "txtNewProjectName";
            this.txtNewProjectName.Size = new System.Drawing.Size(224, 20);
            this.txtNewProjectName.TabIndex = 11;
            // 
            // txtNewCompanyName
            // 
            this.txtNewCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtNewCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewCompanyName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtNewCompanyName.Location = new System.Drawing.Point(509, 572);
            this.txtNewCompanyName.Name = "txtNewCompanyName";
            this.txtNewCompanyName.Size = new System.Drawing.Size(224, 20);
            this.txtNewCompanyName.TabIndex = 10;
            this.txtNewCompanyName.Visible = false;
            // 
            // lbNewProjectName
            // 
            this.lbNewProjectName.AutoSize = true;
            this.lbNewProjectName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbNewProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbNewProjectName.Location = new System.Drawing.Point(398, 600);
            this.lbNewProjectName.Name = "lbNewProjectName";
            this.lbNewProjectName.Size = new System.Drawing.Size(97, 13);
            this.lbNewProjectName.TabIndex = 9;
            this.lbNewProjectName.Text = "New Desired name";
            this.lbNewProjectName.Click += new System.EventHandler(this.lbNewProjectName_Click);
            // 
            // lbArrow2nd
            // 
            this.lbArrow2nd.AutoSize = true;
            this.lbArrow2nd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbArrow2nd.Location = new System.Drawing.Point(368, 600);
            this.lbArrow2nd.Name = "lbArrow2nd";
            this.lbArrow2nd.Size = new System.Drawing.Size(19, 13);
            this.lbArrow2nd.TabIndex = 12;
            this.lbArrow2nd.Text = "=>";
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.FlatAppearance.BorderSize = 0;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.Location = new System.Drawing.Point(458, 663);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 25);
            this.btnSelect.TabIndex = 14;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar1.Location = new System.Drawing.Point(0, 707);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar1.Maximum = 2009;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(739, 11);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 15;
            this.progressBar1.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(187)))), ((int)(((byte)(119)))));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(691, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 30);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbTitle.Location = new System.Drawing.Point(135, 10);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(531, 20);
            this.lbTitle.TabIndex = 19;
            this.lbTitle.Text = "Project Renamer - change the \"AppName\" for another you want automatic";
            this.lbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 34);
            this.panel1.TabIndex = 20;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.label9.Location = new System.Drawing.Point(352, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "=>";
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Console.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.Console.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Console.Location = new System.Drawing.Point(6, 40);
            this.Console.Multiline = true;
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Console.Size = new System.Drawing.Size(727, 496);
            this.Console.TabIndex = 22;
            this.Console.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            // 
            // lbReset
            // 
            this.lbReset.AutoSize = true;
            this.lbReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(173)))), ((int)(((byte)(173)))));
            this.lbReset.Location = new System.Drawing.Point(12, 548);
            this.lbReset.Name = "lbReset";
            this.lbReset.Size = new System.Drawing.Size(118, 13);
            this.lbReset.TabIndex = 23;
            this.lbReset.Text = "look and replace in files";
            this.lbReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(652, 542);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(81, 25);
            this.btnReset.TabIndex = 25;
            this.btnReset.Text = "Default";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtFilter.Location = new System.Drawing.Point(138, 546);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(508, 20);
            this.txtFilter.TabIndex = 24;
            this.txtFilter.Text = ".cs,.cshtml,.js,.ts,.csproj,.sln,.xml,.config,.DotSettings,.json,.xaml,.txt,.html" +
    ",.gitignore,.ps1,.md,.plist";
            // 
            // txtRootDir
            // 
            this.txtRootDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.txtRootDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRootDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            this.txtRootDir.Location = new System.Drawing.Point(82, 667);
            this.txtRootDir.Name = "txtRootDir";
            this.txtRootDir.Size = new System.Drawing.Size(370, 20);
            this.txtRootDir.TabIndex = 26;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(739, 718);
            this.Controls.Add(this.txtRootDir);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.lbReset);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lbArrow2nd);
            this.Controls.Add(this.txtNewProjectName);
            this.Controls.Add(this.txtNewCompanyName);
            this.Controls.Add(this.lbNewProjectName);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtOldProjectName);
            this.Controls.Add(this.txtOldCompanyName);
            this.Controls.Add(this.lbProjectPath);
            this.Controls.Add(this.lbOriginalProjectName);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Creador de arquetipos";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(base.Handle, 274, 61458, 0);
        }
    }
}
