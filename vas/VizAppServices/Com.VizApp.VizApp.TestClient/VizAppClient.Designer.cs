namespace Com.VizApp.VizApp.TestClient
{
    partial class VizAppClient
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
            this.btSaveFBDatas = new System.Windows.Forms.Button();
            this.cmbURL = new System.Windows.Forms.ComboBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.btSaveSettings = new System.Windows.Forms.Button();
            this.btUpdateSettings = new System.Windows.Forms.Button();
            this.btUpdateLocation = new System.Windows.Forms.Button();
            this.btUpdateFriends = new System.Windows.Forms.Button();
            this.btFriends = new System.Windows.Forms.Button();
            this.btLogout = new System.Windows.Forms.Button();
            this.btRegister = new System.Windows.Forms.Button();
            this.btLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btSaveFBDatas
            // 
            this.btSaveFBDatas.Location = new System.Drawing.Point(12, 58);
            this.btSaveFBDatas.Name = "btSaveFBDatas";
            this.btSaveFBDatas.Size = new System.Drawing.Size(107, 23);
            this.btSaveFBDatas.TabIndex = 0;
            this.btSaveFBDatas.Text = "Save FB Datas";
            this.btSaveFBDatas.UseVisualStyleBackColor = true;
            this.btSaveFBDatas.Click += new System.EventHandler(this.btSaveFBDatas_Click);
            // 
            // cmbURL
            // 
            this.cmbURL.FormattingEnabled = true;
            this.cmbURL.Location = new System.Drawing.Point(13, 22);
            this.cmbURL.Name = "cmbURL";
            this.cmbURL.Size = new System.Drawing.Size(457, 21);
            this.cmbURL.TabIndex = 1;
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(13, 222);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(457, 115);
            this.txtResponse.TabIndex = 2;
            // 
            // btSaveSettings
            // 
            this.btSaveSettings.Location = new System.Drawing.Point(125, 58);
            this.btSaveSettings.Name = "btSaveSettings";
            this.btSaveSettings.Size = new System.Drawing.Size(107, 23);
            this.btSaveSettings.TabIndex = 0;
            this.btSaveSettings.Text = "SaveSettings";
            this.btSaveSettings.UseVisualStyleBackColor = true;
            this.btSaveSettings.Click += new System.EventHandler(this.btGetSettings_Click);
            // 
            // btUpdateSettings
            // 
            this.btUpdateSettings.Location = new System.Drawing.Point(238, 58);
            this.btUpdateSettings.Name = "btUpdateSettings";
            this.btUpdateSettings.Size = new System.Drawing.Size(107, 23);
            this.btUpdateSettings.TabIndex = 0;
            this.btUpdateSettings.Text = "Update Settings";
            this.btUpdateSettings.UseVisualStyleBackColor = true;
            this.btUpdateSettings.Click += new System.EventHandler(this.btUpdateSettings_Click);
            // 
            // btUpdateLocation
            // 
            this.btUpdateLocation.Location = new System.Drawing.Point(351, 58);
            this.btUpdateLocation.Name = "btUpdateLocation";
            this.btUpdateLocation.Size = new System.Drawing.Size(107, 23);
            this.btUpdateLocation.TabIndex = 0;
            this.btUpdateLocation.Text = "Update Location";
            this.btUpdateLocation.UseVisualStyleBackColor = true;
            this.btUpdateLocation.Click += new System.EventHandler(this.btUpdateLocation_Click);
            // 
            // btUpdateFriends
            // 
            this.btUpdateFriends.Location = new System.Drawing.Point(125, 87);
            this.btUpdateFriends.Name = "btUpdateFriends";
            this.btUpdateFriends.Size = new System.Drawing.Size(107, 23);
            this.btUpdateFriends.TabIndex = 0;
            this.btUpdateFriends.Text = "Update Friends";
            this.btUpdateFriends.UseVisualStyleBackColor = true;
            this.btUpdateFriends.Click += new System.EventHandler(this.btUpdateFriends_Click);
            // 
            // btFriends
            // 
            this.btFriends.Location = new System.Drawing.Point(13, 87);
            this.btFriends.Name = "btFriends";
            this.btFriends.Size = new System.Drawing.Size(107, 23);
            this.btFriends.TabIndex = 0;
            this.btFriends.Text = "Get Friends";
            this.btFriends.UseVisualStyleBackColor = true;
            this.btFriends.Click += new System.EventHandler(this.btGetFriends_Click);
            // 
            // btLogout
            // 
            this.btLogout.Location = new System.Drawing.Point(13, 116);
            this.btLogout.Name = "btLogout";
            this.btLogout.Size = new System.Drawing.Size(107, 23);
            this.btLogout.TabIndex = 0;
            this.btLogout.Text = "Logout";
            this.btLogout.UseVisualStyleBackColor = true;
            this.btLogout.Click += new System.EventHandler(this.btLogout_Click);
            // 
            // btRegister
            // 
            this.btRegister.Location = new System.Drawing.Point(238, 87);
            this.btRegister.Name = "btRegister";
            this.btRegister.Size = new System.Drawing.Size(107, 23);
            this.btRegister.TabIndex = 0;
            this.btRegister.Text = "Register";
            this.btRegister.UseVisualStyleBackColor = true;
            this.btRegister.Click += new System.EventHandler(this.btRegister_Click);
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(351, 87);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(107, 23);
            this.btLogin.TabIndex = 0;
            this.btLogin.Text = "Login";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // VizAppClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 349);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.cmbURL);
            this.Controls.Add(this.btRegister);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.btLogout);
            this.Controls.Add(this.btFriends);
            this.Controls.Add(this.btUpdateFriends);
            this.Controls.Add(this.btUpdateLocation);
            this.Controls.Add(this.btUpdateSettings);
            this.Controls.Add(this.btSaveSettings);
            this.Controls.Add(this.btSaveFBDatas);
            this.Name = "VizAppClient";
            this.Text = "VizAppClient";
            this.Load += new System.EventHandler(this.VizAppClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSaveFBDatas;
        private System.Windows.Forms.ComboBox cmbURL;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btSaveSettings;
        private System.Windows.Forms.Button btUpdateSettings;
        private System.Windows.Forms.Button btUpdateLocation;
        private System.Windows.Forms.Button btUpdateFriends;
        private System.Windows.Forms.Button btFriends;
        private System.Windows.Forms.Button btLogout;
        private System.Windows.Forms.Button btRegister;
        private System.Windows.Forms.Button btLogin;
    }
}