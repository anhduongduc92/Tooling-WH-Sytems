namespace Tooling_WH_Sytems.Backup
{
    partial class frmBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackup));
            this.btnOpen = new DevExpress.XtraEditors.SimpleButton();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnBackup = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnOpen.Location = new System.Drawing.Point(596, 131);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(189, 133);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(401, 21);
            this.txtUrl.TabIndex = 1;
            // 
            // btnBackup
            // 
            this.btnBackup.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.btnBackup.Location = new System.Drawing.Point(344, 190);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(97, 49);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "Backup";
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // frmBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 371);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.btnOpen);
            this.Name = "frmBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBackup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOpen;
        private System.Windows.Forms.TextBox txtUrl;
        private DevExpress.XtraEditors.SimpleButton btnBackup;
    }
}