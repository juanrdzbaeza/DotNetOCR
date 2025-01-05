using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace DotNetOCR
{
    public partial class DotNetOCR : Form {
        public DotNetOCR() {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, EventArgs e) {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                

                string selectedImagePath = openFileDialog.FileName;
                
                txtImagePath.Text = selectedImagePath;

                if (string.IsNullOrEmpty(txtImagePath.Text)) {
                    MessageBox.Show("Please select an image first.");
                    return;
                }

                string tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata\\");
                using (var engine = new TesseractEngine(tessdataPath, "spa", EngineMode.Default)) {

                    using (var image = Pix.LoadFromFile(txtImagePath.Text)) {

                        using (var page = engine.Process(image)) {

                            string extractedText = page.GetText();
                            txtExtractedText.Text = extractedText;
                        }
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                // Specify the URL you want to open
                string url = "https://github.com/juanrdzbaeza/DotNetOCR";

                // Use the default browser to open the URL
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex) {
                MessageBox.Show($"An error occurred while trying to open the link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                using (var image = Clipboard.GetImage())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStream.Position = 0;
                        using (var pix = Pix.LoadFromMemory(memoryStream.ToArray()))
                        {
                            string tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata\\");
                            using (var engine = new TesseractEngine(tessdataPath, "spa", EngineMode.Default))
                            {
                                using (var page = engine.Process(pix))
                                {
                                    string extractedText = page.GetText();
                                    txtExtractedText.Text = extractedText;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No image found in the clipboard.");
            }
        }
    }
}
