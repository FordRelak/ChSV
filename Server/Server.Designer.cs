namespace Server
{
    partial class Server
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
            this.StatusBox = new System.Windows.Forms.TextBox();
            this.ServerM_GB = new System.Windows.Forms.GroupBox();
            this.CloseCon = new System.Windows.Forms.Button();
            this.Chat_GB = new System.Windows.Forms.GroupBox();
            this.STB = new System.Windows.Forms.TextBox();
            this.SendM = new System.Windows.Forms.Button();
            this.ChatTB = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.серверToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запускToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.чатToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчиститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пользователиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.разорватьВсеСоединенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UserActiv_GB = new System.Windows.Forms.GroupBox();
            this.User_LB = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ServerParam_GB = new System.Windows.Forms.GroupBox();
            this.SavePS_TB = new System.Windows.Forms.Button();
            this.IpServer_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PortServer_TB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WcServer_CB = new System.Windows.Forms.ComboBox();
            this.ServerM_GB.SuspendLayout();
            this.Chat_GB.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.UserActiv_GB.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ServerParam_GB.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBox
            // 
            this.StatusBox.Location = new System.Drawing.Point(6, 19);
            this.StatusBox.Multiline = true;
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.ReadOnly = true;
            this.StatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusBox.Size = new System.Drawing.Size(545, 200);
            this.StatusBox.TabIndex = 3;
            // 
            // ServerM_GB
            // 
            this.ServerM_GB.Controls.Add(this.StatusBox);
            this.ServerM_GB.Location = new System.Drawing.Point(162, 230);
            this.ServerM_GB.Name = "ServerM_GB";
            this.ServerM_GB.Size = new System.Drawing.Size(557, 225);
            this.ServerM_GB.TabIndex = 6;
            this.ServerM_GB.TabStop = false;
            this.ServerM_GB.Text = "Сообщения сервера";
            // 
            // CloseCon
            // 
            this.CloseCon.Location = new System.Drawing.Point(6, 397);
            this.CloseCon.Name = "CloseCon";
            this.CloseCon.Size = new System.Drawing.Size(137, 23);
            this.CloseCon.TabIndex = 6;
            this.CloseCon.Text = "Закрыть соединение";
            this.CloseCon.UseVisualStyleBackColor = true;
            this.CloseCon.Click += new System.EventHandler(this.CloseCon_Click);
            // 
            // Chat_GB
            // 
            this.Chat_GB.Controls.Add(this.STB);
            this.Chat_GB.Controls.Add(this.SendM);
            this.Chat_GB.Controls.Add(this.ChatTB);
            this.Chat_GB.Location = new System.Drawing.Point(162, 27);
            this.Chat_GB.Name = "Chat_GB";
            this.Chat_GB.Size = new System.Drawing.Size(557, 197);
            this.Chat_GB.TabIndex = 7;
            this.Chat_GB.TabStop = false;
            this.Chat_GB.Text = "Чат";
            // 
            // STB
            // 
            this.STB.Location = new System.Drawing.Point(6, 170);
            this.STB.Name = "STB";
            this.STB.Size = new System.Drawing.Size(464, 20);
            this.STB.TabIndex = 5;
            // 
            // SendM
            // 
            this.SendM.Location = new System.Drawing.Point(476, 170);
            this.SendM.Name = "SendM";
            this.SendM.Size = new System.Drawing.Size(75, 23);
            this.SendM.TabIndex = 4;
            this.SendM.Text = "Отправить";
            this.SendM.UseVisualStyleBackColor = true;
            this.SendM.Click += new System.EventHandler(this.SendM_Click);
            // 
            // ChatTB
            // 
            this.ChatTB.Location = new System.Drawing.Point(6, 19);
            this.ChatTB.Multiline = true;
            this.ChatTB.Name = "ChatTB";
            this.ChatTB.ReadOnly = true;
            this.ChatTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatTB.Size = new System.Drawing.Size(545, 145);
            this.ChatTB.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.серверToolStripMenuItem,
            this.чатToolStripMenuItem,
            this.пользователиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(968, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // серверToolStripMenuItem
            // 
            this.серверToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запускToolStripMenuItem,
            this.остановкаToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.серверToolStripMenuItem.Name = "серверToolStripMenuItem";
            this.серверToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.серверToolStripMenuItem.Text = "Сервер";
            // 
            // запускToolStripMenuItem
            // 
            this.запускToolStripMenuItem.Name = "запускToolStripMenuItem";
            this.запускToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.запускToolStripMenuItem.Text = "Запуск";
            this.запускToolStripMenuItem.Click += new System.EventHandler(this.запускToolStripMenuItem_Click);
            // 
            // остановкаToolStripMenuItem
            // 
            this.остановкаToolStripMenuItem.Name = "остановкаToolStripMenuItem";
            this.остановкаToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.остановкаToolStripMenuItem.Text = "Остановка";
            this.остановкаToolStripMenuItem.Click += new System.EventHandler(this.остановкаToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // чатToolStripMenuItem
            // 
            this.чатToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отчиститьToolStripMenuItem});
            this.чатToolStripMenuItem.Name = "чатToolStripMenuItem";
            this.чатToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.чатToolStripMenuItem.Text = "Чат";
            // 
            // отчиститьToolStripMenuItem
            // 
            this.отчиститьToolStripMenuItem.Name = "отчиститьToolStripMenuItem";
            this.отчиститьToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.отчиститьToolStripMenuItem.Text = "Отчистить";
            // 
            // пользователиToolStripMenuItem
            // 
            this.пользователиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.разорватьВсеСоединенияToolStripMenuItem});
            this.пользователиToolStripMenuItem.Name = "пользователиToolStripMenuItem";
            this.пользователиToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.пользователиToolStripMenuItem.Text = "Пользователи";
            // 
            // разорватьВсеСоединенияToolStripMenuItem
            // 
            this.разорватьВсеСоединенияToolStripMenuItem.Name = "разорватьВсеСоединенияToolStripMenuItem";
            this.разорватьВсеСоединенияToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.разорватьВсеСоединенияToolStripMenuItem.Text = "Разорвать все соединения";
            // 
            // UserActiv_GB
            // 
            this.UserActiv_GB.Controls.Add(this.User_LB);
            this.UserActiv_GB.Controls.Add(this.CloseCon);
            this.UserActiv_GB.Location = new System.Drawing.Point(5, 27);
            this.UserActiv_GB.Name = "UserActiv_GB";
            this.UserActiv_GB.Size = new System.Drawing.Size(151, 428);
            this.UserActiv_GB.TabIndex = 9;
            this.UserActiv_GB.TabStop = false;
            this.UserActiv_GB.Text = "Активные подключения";
            // 
            // User_LB
            // 
            this.User_LB.FormattingEnabled = true;
            this.User_LB.Location = new System.Drawing.Point(6, 19);
            this.User_LB.Name = "User_LB";
            this.User_LB.Size = new System.Drawing.Size(137, 368);
            this.User_LB.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ServerParam_GB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.WcServer_CB);
            this.groupBox1.Location = new System.Drawing.Point(726, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 428);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры запуска сервера";
            // 
            // ServerParam_GB
            // 
            this.ServerParam_GB.Controls.Add(this.SavePS_TB);
            this.ServerParam_GB.Controls.Add(this.IpServer_TB);
            this.ServerParam_GB.Controls.Add(this.label3);
            this.ServerParam_GB.Controls.Add(this.PortServer_TB);
            this.ServerParam_GB.Controls.Add(this.label2);
            this.ServerParam_GB.Enabled = false;
            this.ServerParam_GB.Location = new System.Drawing.Point(9, 46);
            this.ServerParam_GB.Name = "ServerParam_GB";
            this.ServerParam_GB.Size = new System.Drawing.Size(214, 376);
            this.ServerParam_GB.TabIndex = 2;
            this.ServerParam_GB.TabStop = false;
            this.ServerParam_GB.Text = "Настройки";
            // 
            // SavePS_TB
            // 
            this.SavePS_TB.Location = new System.Drawing.Point(7, 347);
            this.SavePS_TB.Name = "SavePS_TB";
            this.SavePS_TB.Size = new System.Drawing.Size(199, 23);
            this.SavePS_TB.TabIndex = 11;
            this.SavePS_TB.Text = "Сохранить настройки";
            this.SavePS_TB.UseVisualStyleBackColor = true;
            this.SavePS_TB.Click += new System.EventHandler(this.SavePS_TB_Click);
            // 
            // IpServer_TB
            // 
            this.IpServer_TB.Location = new System.Drawing.Point(75, 19);
            this.IpServer_TB.Name = "IpServer_TB";
            this.IpServer_TB.Size = new System.Drawing.Size(131, 20);
            this.IpServer_TB.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Порт Сервера";
            // 
            // PortServer_TB
            // 
            this.PortServer_TB.Location = new System.Drawing.Point(90, 45);
            this.PortServer_TB.Name = "PortServer_TB";
            this.PortServer_TB.Size = new System.Drawing.Size(116, 20);
            this.PortServer_TB.TabIndex = 8;
            this.PortServer_TB.Text = "8009";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "IP Сервера";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Режим работы";
            // 
            // WcServer_CB
            // 
            this.WcServer_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WcServer_CB.FormattingEnabled = true;
            this.WcServer_CB.Items.AddRange(new object[] {
            "Локальный",
            "Глобальный"});
            this.WcServer_CB.Location = new System.Drawing.Point(94, 19);
            this.WcServer_CB.Name = "WcServer_CB";
            this.WcServer_CB.Size = new System.Drawing.Size(121, 21);
            this.WcServer_CB.TabIndex = 0;
            this.WcServer_CB.SelectedIndexChanged += new System.EventHandler(this.WcServer_CB_SelectedIndexChanged);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 459);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.UserActiv_GB);
            this.Controls.Add(this.Chat_GB);
            this.Controls.Add(this.ServerM_GB);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.ServerM_GB.ResumeLayout(false);
            this.ServerM_GB.PerformLayout();
            this.Chat_GB.ResumeLayout(false);
            this.Chat_GB.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.UserActiv_GB.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ServerParam_GB.ResumeLayout(false);
            this.ServerParam_GB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox StatusBox;
        private System.Windows.Forms.GroupBox ServerM_GB;
        private System.Windows.Forms.GroupBox Chat_GB;
        public System.Windows.Forms.TextBox ChatTB;
        private System.Windows.Forms.TextBox STB;
        private System.Windows.Forms.Button SendM;
        private System.Windows.Forms.Button CloseCon;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem серверToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запускToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem остановкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem чатToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчиститьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пользователиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem разорватьВсеСоединенияToolStripMenuItem;
        private System.Windows.Forms.GroupBox UserActiv_GB;
        private System.Windows.Forms.ListBox User_LB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox ServerParam_GB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox WcServer_CB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PortServer_TB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IpServer_TB;
        private System.Windows.Forms.Button SavePS_TB;
    }
}