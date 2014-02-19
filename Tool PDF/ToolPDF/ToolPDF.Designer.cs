namespace ToolPDF
{
    partial class ToolPDF
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbInputHTML = new System.Windows.Forms.TextBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenHTML = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenPDF = new System.Windows.Forms.Button();
            this.tbInputPDF = new System.Windows.Forms.TextBox();
            this.btnGeneratePDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input HTML File";
            // 
            // tbInputHTML
            // 
            this.tbInputHTML.Location = new System.Drawing.Point(105, 20);
            this.tbInputHTML.Name = "tbInputHTML";
            this.tbInputHTML.Size = new System.Drawing.Size(182, 20);
            this.tbInputHTML.TabIndex = 1;
            // 
            // btnOpenHTML
            // 
            this.btnOpenHTML.Location = new System.Drawing.Point(293, 20);
            this.btnOpenHTML.Name = "btnOpenHTML";
            this.btnOpenHTML.Size = new System.Drawing.Size(38, 23);
            this.btnOpenHTML.TabIndex = 2;
            this.btnOpenHTML.Text = "...";
            this.btnOpenHTML.UseVisualStyleBackColor = true;
            this.btnOpenHTML.Click += new System.EventHandler(this.btnOpenHTML_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Input PDF File";
            // 
            // btnOpenPDF
            // 
            this.btnOpenPDF.Location = new System.Drawing.Point(293, 54);
            this.btnOpenPDF.Name = "btnOpenPDF";
            this.btnOpenPDF.Size = new System.Drawing.Size(38, 23);
            this.btnOpenPDF.TabIndex = 4;
            this.btnOpenPDF.Text = "...";
            this.btnOpenPDF.UseVisualStyleBackColor = true;
            this.btnOpenPDF.Click += new System.EventHandler(this.btnOpenPDF_Click);
            // 
            // tbInputPDF
            // 
            this.tbInputPDF.Location = new System.Drawing.Point(105, 55);
            this.tbInputPDF.Name = "tbInputPDF";
            this.tbInputPDF.Size = new System.Drawing.Size(182, 20);
            this.tbInputPDF.TabIndex = 5;
            // 
            // btnGeneratePDF
            // 
            this.btnGeneratePDF.Location = new System.Drawing.Point(105, 95);
            this.btnGeneratePDF.Name = "btnGeneratePDF";
            this.btnGeneratePDF.Size = new System.Drawing.Size(182, 23);
            this.btnGeneratePDF.TabIndex = 6;
            this.btnGeneratePDF.Text = "Generate PDF";
            this.btnGeneratePDF.UseVisualStyleBackColor = true;
            this.btnGeneratePDF.Click += new System.EventHandler(this.btnGeneratePDF_Click);
            // 
            // ToolPDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 142);
            this.Controls.Add(this.btnGeneratePDF);
            this.Controls.Add(this.tbInputPDF);
            this.Controls.Add(this.btnOpenPDF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOpenHTML);
            this.Controls.Add(this.tbInputHTML);
            this.Controls.Add(this.label1);
            this.Name = "ToolPDF";
            this.Text = "ToolPDF";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInputHTML;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button btnOpenHTML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenPDF;
        private System.Windows.Forms.TextBox tbInputPDF;
        private System.Windows.Forms.Button btnGeneratePDF;
    }
}

