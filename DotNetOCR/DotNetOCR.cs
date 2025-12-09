using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using PdfiumViewer;

namespace DotNetOCR
{
    public partial class DotNetOCR : Form
    {
        public DotNetOCR()
        {
            InitializeComponent();
        }

        private async void btnSelectPdf_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string filePath = openFileDialog1.FileName;
            rtbOutput.Clear();

            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;

                // Determine tessdata path
                string tessdataPath = Environment.GetEnvironmentVariable("TESSDATA_PREFIX") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(tessdataPath) || !Directory.Exists(tessdataPath))
                {
                    // Ask user to select tessdata folder if not set
                    using var fbd = new FolderBrowserDialog();
                    fbd.Description = "Selecciona la carpeta que contiene los archivos tessdata (por ejemplo spa.traineddata).";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        tessdataPath = fbd.SelectedPath;
                    }
                }

                if (string.IsNullOrWhiteSpace(tessdataPath) || !Directory.Exists(tessdataPath))
                {
                    MessageBox.Show("No se ha encontrado la carpeta tessdata. Por favor, configure la variable de entorno TESSDATA_PREFIX o seleccione la carpeta manualmente.", "Tessdata no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    progressBar.Visible = false;
                    return;
                }

                // Default language: spanish if available, otherwise english
                string lang = File.Exists(Path.Combine(tessdataPath, "spa.traineddata")) ? "spa" : "eng";

                string ext = Path.GetExtension(filePath).ToLowerInvariant();

                // Handle PDF and image files in background to keep UI responsive
                await Task.Run(() =>
                {
                    try
                    {
                        using var engine = new TesseractEngine(tessdataPath, lang, EngineMode.Default);

                        if (ext == ".pdf")
                        {
                            // PDF flow (existing behavior)
                            using var document = PdfDocument.Load(filePath);
                            int pageCount = document.PageCount;
                            progressBar.Invoke(new Action(() => progressBar.Maximum = Math.Max(1, pageCount)));

                            for (int i = 0; i < pageCount; i++)
                            {
                                // Render page to bitmap at 150 DPI
                                using var image = document.Render(i, 150, 150, PdfRenderFlags.CorrectFromDpi);

                                // Convert Bitmap to Pix via memory stream
                                using var ms = new MemoryStream();
                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var bytes = ms.ToArray();

                                using var pix = Pix.LoadFromMemory(bytes);
                                using var page = engine.Process(pix);

                                string text = page.GetText();
                                float conf = page.GetMeanConfidence();

                                // Update UI with the result for this page
                                Invoke(new Action(() =>
                                {
                                    AppendPageResult(i + 1, text, conf);
                                    progressBar.Value = i + 1;
                                }));
                            }
                        }
                        else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                        {
                            // Single-image flow
                            // Load image bytes (avoid locking the file) and process as one page
                            byte[] bytes = File.ReadAllBytes(filePath);

                            using var ms = new MemoryStream(bytes);
                            using var image = new Bitmap(ms);

                            // Convert Bitmap to Pix via memory stream (PNG)
                            using var ms2 = new MemoryStream();
                            image.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
                            var imgBytes = ms2.ToArray();

                            using var pix = Pix.LoadFromMemory(imgBytes);
                            using var page = engine.Process(pix);

                            string text = page.GetText();
                            float conf = page.GetMeanConfidence();

                            progressBar.Invoke(new Action(() =>
                            {
                                progressBar.Maximum = 1;
                                progressBar.Value = 1;
                                AppendPageResult(1, text, conf);
                            }));
                        }
                        else
                        {
                            // Unsupported type (shouldn't happen due to filter), report to UI
                            Invoke(new Action(() => MessageBox.Show($"Tipo de archivo no soportado: {ext}", "Tipo no soportado", MessageBoxButtons.OK, MessageBoxIcon.Warning)));
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => MessageBox.Show($"Error al ejecutar OCR: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error procesando el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private void AppendPageResult(int pageNumber, string text, float confidence)
        {
            if (rtbOutput.InvokeRequired)
            {
                rtbOutput.Invoke(new Action(() => AppendPageResult(pageNumber, text, confidence)));
                return;
            }

            // Simple RTF enriched formatting: page header bold and confidence in color
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.SelectionFont = new Font(rtbOutput.Font, FontStyle.Bold);
            rtbOutput.AppendText($"--- Página {pageNumber} (Confianza: {confidence:P1}) ---\n");

            rtbOutput.SelectionFont = new Font(rtbOutput.Font, FontStyle.Regular);
            rtbOutput.AppendText(text + "\n\n");

            // Scroll to end
            rtbOutput.SelectionStart = rtbOutput.TextLength;
            rtbOutput.ScrollToCaret();
        }

        private void DotNetOCR_Load(object sender, EventArgs e)
        {

        }
    }
}
