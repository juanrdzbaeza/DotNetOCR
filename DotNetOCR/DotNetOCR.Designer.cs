namespace DotNetOCR
{
    partial class DotNetOCR
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnSelectPdf;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.LinkLabel linkLabel1;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DotNetOCR));
            btnSelectPdf = new Button();
            btnPaste = new Button();
            rtbOutput = new RichTextBox();
            progressBar = new ProgressBar();
            openFileDialog1 = new OpenFileDialog();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // btnSelectPdf
            // 
            btnSelectPdf.Location = new Point(12, 12);
            btnSelectPdf.Name = "btnSelectPdf";
            btnSelectPdf.Size = new Size(140, 77);
            btnSelectPdf.TabIndex = 0;
            btnSelectPdf.Text = "Seleccionar PDF/Imagen";
            btnSelectPdf.UseVisualStyleBackColor = true;
            btnSelectPdf.Click += btnSelectPdf_Click;
            // 
            // btnPaste
            // 
            btnPaste.Image = (Image)resources.GetObject("btnPaste.Image");
            btnPaste.Location = new Point(158, 12);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(80, 77);
            btnPaste.TabIndex = 1;
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // rtbOutput
            // 
            rtbOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbOutput.Location = new Point(12, 95);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(1118, 583);
            rtbOutput.TabIndex = 2;
            rtbOutput.Text = "";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(244, 17);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(816, 20);
            progressBar.TabIndex = 3;
            progressBar.Visible = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|Image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog1.Title = "Seleccionar archivo PDF o imagen";
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(913, 681);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(217, 25);
            linkLabel1.TabIndex = 2;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "juanrdzbaeza/DotNetOCR";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // DotNetOCR
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 715);
            Controls.Add(rtbOutput);
            Controls.Add(progressBar);
            Controls.Add(btnPaste);
            Controls.Add(btnSelectPdf);
            Controls.Add(linkLabel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(820, 500);
            Name = "DotNetOCR";
            Text = "DotNetOCR - OCR de PDF";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
