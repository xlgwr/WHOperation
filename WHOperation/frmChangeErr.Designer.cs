namespace WHOperation
{
    partial class frmChangeErr
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
            this.tfrirno = new System.Windows.Forms.TextBox();
            this.tfpartno = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tfmfgpart = new System.Windows.Forms.TextBox();
            this.tfdnqty = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt0PrintedQty = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lbl_msg = new System.Windows.Forms.Label();
            this.btn2Print = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tfrirno
            // 
            this.tfrirno.Location = new System.Drawing.Point(138, 26);
            this.tfrirno.Name = "tfrirno";
            this.tfrirno.ReadOnly = true;
            this.tfrirno.Size = new System.Drawing.Size(123, 21);
            this.tfrirno.TabIndex = 1;
            // 
            // tfpartno
            // 
            this.tfpartno.Location = new System.Drawing.Point(138, 54);
            this.tfpartno.Name = "tfpartno";
            this.tfpartno.ReadOnly = true;
            this.tfpartno.Size = new System.Drawing.Size(123, 21);
            this.tfpartno.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "RIR Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "Part Number";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "PO QPL Part No.";
            // 
            // tfmfgpart
            // 
            this.tfmfgpart.Location = new System.Drawing.Point(138, 81);
            this.tfmfgpart.Name = "tfmfgpart";
            this.tfmfgpart.ReadOnly = true;
            this.tfmfgpart.Size = new System.Drawing.Size(123, 21);
            this.tfmfgpart.TabIndex = 2;
            // 
            // tfdnqty
            // 
            this.tfdnqty.Enabled = false;
            this.tfdnqty.Location = new System.Drawing.Point(138, 108);
            this.tfdnqty.Name = "tfdnqty";
            this.tfdnqty.ReadOnly = true;
            this.tfdnqty.Size = new System.Drawing.Size(123, 21);
            this.tfdnqty.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(93, 112);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 38;
            this.label13.Text = "DN Qty";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 38;
            this.label1.Text = "Printed Qty";
            // 
            // txt0PrintedQty
            // 
            this.txt0PrintedQty.Location = new System.Drawing.Point(138, 135);
            this.txt0PrintedQty.Name = "txt0PrintedQty";
            this.txt0PrintedQty.Size = new System.Drawing.Size(123, 21);
            this.txt0PrintedQty.TabIndex = 0;
            this.txt0PrintedQty.TextChanged += new System.EventHandler(this.txt0PrintedQty_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txt0PrintedQty);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tfpartno);
            this.groupBox1.Controls.Add(this.tfdnqty);
            this.groupBox1.Controls.Add(this.tfrirno);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.tfmfgpart);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(21, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 172);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fix Printed Qty";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(86, 190);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 36);
            this.btnUpdate.TabIndex = 40;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lbl_msg
            // 
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.ForeColor = System.Drawing.Color.Red;
            this.lbl_msg.Location = new System.Drawing.Point(19, 237);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(0, 12);
            this.lbl_msg.TabIndex = 41;
            // 
            // btn2Print
            // 
            this.btn2Print.Location = new System.Drawing.Point(189, 190);
            this.btn2Print.Name = "btn2Print";
            this.btn2Print.Size = new System.Drawing.Size(144, 36);
            this.btn2Print.TabIndex = 40;
            this.btn2Print.Text = "Retry Print(本地)";
            this.btn2Print.UseVisualStyleBackColor = true;
            this.btn2Print.Click += new System.EventHandler(this.btn2Print_Click);
            // 
            // frmChangeErr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 260);
            this.Controls.Add(this.lbl_msg);
            this.Controls.Add(this.btn2Print);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmChangeErr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChangeErr";
            this.Load += new System.EventHandler(this.frmChangeErr_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tfrirno;
        private System.Windows.Forms.TextBox tfpartno;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tfmfgpart;
        private System.Windows.Forms.TextBox tfdnqty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt0PrintedQty;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lbl_msg;
        private System.Windows.Forms.Button btn2Print;
    }
}