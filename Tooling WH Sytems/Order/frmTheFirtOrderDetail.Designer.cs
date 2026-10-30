namespace Tooling_WH_Sytems.Order
{
    partial class frmTheFirtOrderDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTheFirtOrderDetail));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbFactory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.plStyle = new System.Windows.Forms.Panel();
            this.cbStyleC = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtOrderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbPartNomal = new System.Windows.Forms.CheckBox();
            this.chkSpecPart = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.plStyle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.chkSpecPart);
            this.panel1.Controls.Add(this.ckbPartNomal);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.plStyle);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 251);
            this.panel1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.Location = new System.Drawing.Point(273, 205);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbFactory);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(11, 119);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(665, 38);
            this.panel3.TabIndex = 4;
            // 
            // cbFactory
            // 
            this.cbFactory.FormattingEnabled = true;
            this.cbFactory.Location = new System.Drawing.Point(108, 6);
            this.cbFactory.Name = "cbFactory";
            this.cbFactory.Size = new System.Drawing.Size(550, 27);
            this.cbFactory.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nhà máy :";
            // 
            // plStyle
            // 
            this.plStyle.Controls.Add(this.cbStyleC);
            this.plStyle.Controls.Add(this.label2);
            this.plStyle.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plStyle.Location = new System.Drawing.Point(11, 75);
            this.plStyle.Name = "plStyle";
            this.plStyle.Size = new System.Drawing.Size(665, 38);
            this.plStyle.TabIndex = 2;
            // 
            // cbStyleC
            // 
            this.cbStyleC.FormattingEnabled = true;
            this.cbStyleC.Location = new System.Drawing.Point(108, 6);
            this.cbStyleC.Name = "cbStyleC";
            this.cbStyleC.Size = new System.Drawing.Size(550, 27);
            this.cbStyleC.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Loại phiếu :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtOrderName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(11, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(665, 38);
            this.panel2.TabIndex = 0;
            // 
            // txtOrderName
            // 
            this.txtOrderName.Location = new System.Drawing.Point(108, 3);
            this.txtOrderName.Name = "txtOrderName";
            this.txtOrderName.Size = new System.Drawing.Size(550, 26);
            this.txtOrderName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên phiếu :";
            // 
            // ckbPartNomal
            // 
            this.ckbPartNomal.AutoSize = true;
            this.ckbPartNomal.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckbPartNomal.Location = new System.Drawing.Point(119, 163);
            this.ckbPartNomal.Name = "ckbPartNomal";
            this.ckbPartNomal.Size = new System.Drawing.Size(166, 23);
            this.ckbPartNomal.TabIndex = 7;
            this.ckbPartNomal.Text = "Linh kiện thông thường";
            this.ckbPartNomal.UseVisualStyleBackColor = true;
            this.ckbPartNomal.CheckedChanged += new System.EventHandler(this.ckbPartNomal_CheckedChanged);
            // 
            // chkSpecPart
            // 
            this.chkSpecPart.AutoSize = true;
            this.chkSpecPart.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSpecPart.Location = new System.Drawing.Point(404, 163);
            this.chkSpecPart.Name = "chkSpecPart";
            this.chkSpecPart.Size = new System.Drawing.Size(135, 23);
            this.chkSpecPart.TabIndex = 7;
            this.chkSpecPart.Text = "Linh kiện đặc biệt";
            this.chkSpecPart.UseVisualStyleBackColor = true;
            this.chkSpecPart.CheckedChanged += new System.EventHandler(this.chkSpecPart_CheckedChanged);
            // 
            // frmTheFirtOrderDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 255);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTheFirtOrderDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "THÊM LINH KIỆN MUỐN XUẤT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTheFirtOrderDetail_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.plStyle.ResumeLayout(false);
            this.plStyle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtOrderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel plStyle;
        private System.Windows.Forms.ComboBox cbStyleC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbFactory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSpecPart;
        private System.Windows.Forms.CheckBox ckbPartNomal;
    }
}