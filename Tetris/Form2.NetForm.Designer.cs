namespace Tetris
{
    partial class Form2
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
        
        private void InitializeComponent()
        {
            this.rad_host = new System.Windows.Forms.RadioButton();
            this.rad_client = new System.Windows.Forms.RadioButton();
            this.mtb_remoteIp = new System.Windows.Forms.MaskedTextBox();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lbl_tip = new System.Windows.Forms.Label();
            this.cbo_ip = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();

            // 
            // netform
            // 
            this.ClientSize = new System.Drawing.Size(239, 181);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(52, 52);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "netform";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            //this.State = Tetris.GameState.Single;
            this.Text = "NetPlay Config";
            this.Visible = false;
            this.Load += new System.EventHandler(this.Form2_Load);
            // 
            // rad_host
            // 
            this.rad_host.AutoSize = true;
            this.rad_host.Checked = true;
            this.rad_host.Location = new System.Drawing.Point(34, 57);
            this.rad_host.Name = "rad_host";
            this.rad_host.Size = new System.Drawing.Size(47, 16);
            this.rad_host.TabIndex = 0;
            this.rad_host.TabStop = true;
            this.rad_host.Text = "主机";
            this.rad_host.UseVisualStyleBackColor = true;
            this.rad_host.CheckedChanged += new System.EventHandler(this.rad_host_CheckedChanged);
            // 
            // rad_client
            // 
            this.rad_client.AutoSize = true;
            this.rad_client.Location = new System.Drawing.Point(34, 94);
            this.rad_client.Name = "rad_client";
            this.rad_client.Size = new System.Drawing.Size(47, 16);
            this.rad_client.TabIndex = 1;
            this.rad_client.Text = "副机";
            this.rad_client.UseVisualStyleBackColor = true;
            this.rad_client.CheckedChanged += new System.EventHandler(this.rad_client_CheckedChanged);
            // 
            // mtb_remoteIp
            // 
            this.mtb_remoteIp.Enabled = false;
            this.mtb_remoteIp.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.mtb_remoteIp.Location = new System.Drawing.Point(106, 92);
            this.mtb_remoteIp.Mask = "000.000.000.000";
            this.mtb_remoteIp.Name = "mtb_remoteIp";
            this.mtb_remoteIp.Size = new System.Drawing.Size(101, 21);
            this.mtb_remoteIp.TabIndex = 2;
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(32, 133);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 20);
            this.btn_connect.TabIndex = 3;
            this.btn_connect.Text = "连接(&o)";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(132, 133);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 20);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "取消(&c)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_tip
            // 
            this.lbl_tip.AutoSize = true;
            this.lbl_tip.Location = new System.Drawing.Point(34, 22);
            this.lbl_tip.Name = "lbl_tip";
            this.lbl_tip.Size = new System.Drawing.Size(89, 12);
            this.lbl_tip.TabIndex = 5;
            this.lbl_tip.Text = "选择主机或副机";
            // 
            // cbo_ip
            // 
            this.cbo_ip.FormattingEnabled = true;
            this.cbo_ip.IntegralHeight = false;
            this.cbo_ip.Location = new System.Drawing.Point(106, 55);
            this.cbo_ip.Name = "cbo_ip";
            this.cbo_ip.Size = new System.Drawing.Size(101, 20);
            this.cbo_ip.TabIndex = 6;
            this.cbo_ip.SelectedIndexChanged += new System.EventHandler(this.cbo_ip_SelectedIndexChanged);
            // 
            // NetForm
            // 
            this.AcceptButton = this.btn_connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(239, 181);
            this.Controls.Add(this.cbo_ip);
            this.Controls.Add(this.lbl_tip);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.mtb_remoteIp);
            this.Controls.Add(this.rad_client);
            this.Controls.Add(this.rad_host);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NetPlay Config";
            this.Load += new System.EventHandler(this.form_net_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_net_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // "Host" 单选按钮
        private System.Windows.Forms.RadioButton rad_host;
        // "Client" 单选按钮
        private System.Windows.Forms.RadioButton rad_client;
        // ip输入框
        private System.Windows.Forms.MaskedTextBox mtb_remoteIp;
        // "Connect" 和 "Start Host" 按钮
        private System.Windows.Forms.Button btn_connect;
        // "Cancel" 按钮
        private System.Windows.Forms.Button btn_cancel;
        // 显示提示信息
        private System.Windows.Forms.Label lbl_tip;
        // 显示ip列表
        private System.Windows.Forms.ComboBox cbo_ip;
    }
}