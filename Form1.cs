using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using Tesseract;
using PdfiumViewer;
using System.Drawing;

namespace AgroCoordenadas
{
    public partial class Form1 : Form
    {
        private string tempFilePath = "";
        private static List<string> texts = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsSelectablePdf(string tempFilePath)
        {
            bool isSelectablePdf = false;

            using (var reader = new PdfReader(tempFilePath))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var currentText = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    if (!string.IsNullOrWhiteSpace(currentText))
                    {
                        isSelectablePdf = true;
                        break;
                    }
                }
            }

            return isSelectablePdf;
        }

        private List<string> PdfText(string tempFilePath)
        {
            using (var reader = new PdfReader(tempFilePath))
            {

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var currentText = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    texts.Add(currentText);
                }


            }
            return texts;
        }

        private List<string> PdfImg(string tempFilePath)
        {
            string outputFolder = @"Tempory/";
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }
            Directory.CreateDirectory(outputFolder);


            static Bitmap ConvertToGrayscale(Bitmap original)
            {
                Bitmap grayscaleBitmap = new Bitmap(original.Width, original.Height);

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        int luminance = (int)(originalColor.R * 0.3 + originalColor.G * 0.59 + originalColor.B * 0.11);
                        Color grayscaleColor = Color.FromArgb(luminance, luminance, luminance);
                        grayscaleBitmap.SetPixel(x, y, grayscaleColor);
                    }
                }

                return grayscaleBitmap;
            }


            //converte pdf para imagens salva na pasta criada logo acima
            using (PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(tempFilePath))
            {
                for (int pageNumber = 0; pageNumber < pdfDocument.PageCount; pageNumber++)
                {
                    using (Image image = pdfDocument.Render(pageNumber, 2200, 2200, 200, 200, PdfRenderFlags.Annotations))
                    {
                        using (Bitmap bitmap = new Bitmap(image))
                        {
                            using (Bitmap grayscaleBitmap = ConvertToGrayscale(bitmap))
                            {
                                string outputFile = System.IO.Path.Combine(outputFolder, $"ToImage-{pageNumber + 1}.png");
                                grayscaleBitmap.Save(outputFile, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }

                    }
                }
            }


            File.Delete(tempFilePath);


            //lê as imagens com tecnologia OCR usando o tesseract
            Environment.SetEnvironmentVariable("TESSDATA_PREFIX", @"./tessdata/");
            using (var engine = new TesseractEngine(@"./tessdata/", "eng", EngineMode.Default))
            {
                string[] imageFiles = Directory.GetFiles(outputFolder, "*.png");
                foreach (var imageFile in imageFiles)
                {
                    using (var img = Pix.LoadFromFile(imageFile))
                    {
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            text = text.Replace("\n", " ");
                            text = text.Replace("\r", " ");
                            text = text.Replace("\t", " ");
                            text = text.Replace("º", "°");
                            texts.Add(text);
                        }
                    }
                }
            }
            Directory.Delete(outputFolder, true);
            return texts;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);
                string nomeDoArquivoNormalizado = fileName.Normalize(NormalizationForm.FormD);
                string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), nomeDoArquivoNormalizado);
                File.Copy(filePath, tempFilePath, true);
                richTextBox1.Text = fileName;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tempFilePath))
            {
                MessageBox.Show("Selecione um arquivo PDF antes de continuar.");
            }
            else
            {

            }
        }
    }
}