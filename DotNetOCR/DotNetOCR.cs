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

                string tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

                // Ensure tessdata and spa.traineddata are available in the output directory
                string errorMsg;
                if (!EnsureTessdataAvailable(out tessdataPath, out errorMsg)) {
                    MessageBox.Show(errorMsg, "Tesseract data not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    using (var engine = new TesseractEngine(tessdataPath, "spa", EngineMode.Default)) {

                        using (var image = Pix.LoadFromFile(txtImagePath.Text)) {

                            using (var page = engine.Process(image)) {

                                string extractedText = page.GetText();
                                txtExtractedText.Text = extractedText;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error initializing Tesseract engine:\n{ex.Message}", "Tesseract error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            string tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

                            string errorMsg;
                            if (!EnsureTessdataAvailable(out tessdataPath, out errorMsg)) {
                                MessageBox.Show(errorMsg, "Tesseract data not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            try {
                                using (var engine = new TesseractEngine(tessdataPath, "spa", EngineMode.Default))
                                {
                                    using (var page = engine.Process(pix))
                                    {
                                        string extractedText = page.GetText();
                                        txtExtractedText.Text = extractedText;
                                    }
                                }
                            }
                            catch (Exception ex) {
                                MessageBox.Show($"Error initializing Tesseract engine:\n{ex.Message}", "Tesseract error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Tries to ensure tessdata is present in the application's base directory.
        // If not present, searches parent directories for a 'tessdata' folder containing 'spa.traineddata'
        // and copies it into the application's output directory.
        private bool EnsureTessdataAvailable(out string tessdataPath, out string errorMessage)
        {
            tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            string spaFile = Path.Combine(tessdataPath, "spa.traineddata");
            if (Directory.Exists(tessdataPath) && File.Exists(spaFile)) {
                errorMessage = null;
                return true;
            }

            // Search upwards for a tessdata folder (project folder, solution root, user folder, etc.)
            string dir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            for (int i = 0; i < 6; i++) {
                string candidate = Path.Combine(dir, "tessdata");
                string candidateSpa = Path.Combine(candidate, "spa.traineddata");
                if (Directory.Exists(candidate) && File.Exists(candidateSpa)) {
                    try {
                        CopyDirectory(candidate, tessdataPath);
                        if (File.Exists(Path.Combine(tessdataPath, "spa.traineddata"))) {
                            errorMessage = null;
                            return true;
                        }
                        else {
                            errorMessage = $"Copied tessdata from '{candidate}' but '{spaFile}' is still missing after copy.";
                            return false;
                        }
                    }
                    catch (Exception ex) {
                        errorMessage = $"Found tessdata at '{candidate}' but failed to copy to output folder: {ex.Message}";
                        return false;
                    }
                }

                dir = Path.GetDirectoryName(dir);
                if (string.IsNullOrEmpty(dir)) break;
            }

            errorMessage = "Could not find 'tessdata' with 'spa.traineddata' in the application folder or parent folders.\n" +
                           "Make sure the 'tessdata' folder (containing spa.traineddata) is copied to the application's output directory or set the TESSDATA_PREFIX environment variable.";
            return false;
        }

        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(destinationDir)) Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                var destSubDir = Path.Combine(destinationDir, Path.GetFileName(directory));
                CopyDirectory(directory, destSubDir);
            }
        }
    }
}
