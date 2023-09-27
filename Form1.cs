using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using Tesseract;
using PdfiumViewer;
using System.Drawing;
using AgroCoordenadas.Interface;
using AgroCoordenadas.Service;
using System.Windows.Forms;

namespace AgroCoordenadas
{
    public partial class Form1 : Form
    {
        private string tempFilePath = "";
        private static List<string> texts = new List<string>();
        private readonly IFilter _filter;
        private VScrollBar vScrollBar1;

        public Form1()
        {
            InitializeComponent();
            _filter = new FilterService();


            richTextBox4.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox5.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox6.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox7.ScrollBars = RichTextBoxScrollBars.None;
            panel3.AutoScroll = true;
            vScrollBar1 = new VScrollBar();
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
            panel3.Controls.Add(richTextBox5);
            panel3.Controls.Add(richTextBox6);
            panel3.Controls.Add(richTextBox7);
            panel3.Controls.Add(richTextBox4);

            this.Controls.Add(panel3);
            this.Controls.Add(vScrollBar1);



        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

            panel3.VerticalScroll.Value = e.NewValue;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            texts.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);
                string nomeDoArquivoNormalizado = fileName.Normalize(NormalizationForm.FormD);
                tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), nomeDoArquivoNormalizado);
                File.Copy(filePath, tempFilePath, true);
                richTextBox1.Text = fileName;
            }

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
            string outputFolder = "Tempory/";
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

            Environment.SetEnvironmentVariable("TESSDATA_PREFIX", "./tessdata");
            using (TesseractEngine engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
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


        private FilterService.FilteredData ApplyFilter(List<string> texts)
        {
            FilterService.FilteredData results = new FilterService.FilteredData();

            foreach (var text in texts)
            {
                var coordinates = _filter.Filter(text);

                if (coordinates.N != null)
                {
                    results.N ??= new List<string>();
                    results.N.AddRange(coordinates.N);
                }

                if (coordinates.E != null)
                {
                    results.E ??= new List<string>();
                    results.E.AddRange(coordinates.E);
                }

                if (coordinates.Latitude != null)
                {
                    results.Latitude ??= new List<string>();
                    results.Latitude.AddRange(coordinates.Latitude);
                }

                if (coordinates.Longitude != null)
                {
                    results.Longitude ??= new List<string>();
                    results.Longitude.AddRange(coordinates.Longitude);
                }
            }


            return results;
        }
        private int CalculateRichTextBoxHeight(RichTextBox richTextBox)
        {
            using (Graphics g = richTextBox.CreateGraphics())
            {
                SizeF preferredSize = g.MeasureString(richTextBox.Text, richTextBox.Font, richTextBox.Width);
                return (int)Math.Ceiling(preferredSize.Height);
            }
        }
        private void button6_MouseDown(object sender, EventArgs e)
        {
            button6.MouseDown -= button6_MouseDown;


            if (string.IsNullOrEmpty(tempFilePath))
            {
                MessageBox.Show("Selecione um arquivo PDF antes de continuar.");
            }
            else
            {
                try
                {
                    bool isSelectablePdf = IsSelectablePdf(tempFilePath);

                    if (isSelectablePdf)
                    {
                        List<string> textFromPdfText = PdfText(tempFilePath);
                        List<string> textFromPdfImg = PdfImg(tempFilePath);

                        if (textFromPdfText.Count > textFromPdfImg.Count)
                        {
                            texts = textFromPdfText;
                        }
                        else
                        {
                            texts = textFromPdfImg;
                        }
                    }
                    else
                    {
                        List<string> textFromPdfImg = PdfImg(tempFilePath);
                        texts = textFromPdfImg;
                    }
                    richTextBox2.Text = string.Join(Environment.NewLine, texts);

                    FilterService.FilteredData filteredData = ApplyFilter(texts);
                   

                    richTextBox4.Text = string.Join("\n", filteredData.E);
                    richTextBox5.Text = string.Join("\n", filteredData.N);
                    richTextBox6.Text = string.Join("\n", filteredData.Longitude);
                    richTextBox7.Text = string.Join("\n", filteredData.Latitude);

                    richTextBox4.Height = (int)(CalculateRichTextBoxHeight(richTextBox4) * 1.2);
                    richTextBox5.Height = (int)(CalculateRichTextBoxHeight(richTextBox5) * 1.2);
                    richTextBox6.Height = (int)(CalculateRichTextBoxHeight(richTextBox6) * 1.2);
                    richTextBox7.Height = (int)(CalculateRichTextBoxHeight(richTextBox7) * 1.2);

                }
                finally
                {
                    button6.Enabled = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}