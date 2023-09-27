using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using Tesseract;
using PdfiumViewer;
using System.Drawing;
using AgroCoordenadas.Interface;
using AgroCoordenadas.Service;
using System.Windows.Forms;
using static AgroCoordenadas.Service.FilterService;

namespace AgroCoordenadas
{
    public partial class Form1 : Form
    {
        private string tempFilePath = "";
        private static List<string> texts = new List<string>();
        private readonly IFilter _filter;
        private VScrollBar vScrollBar1;
        private string eResult = null;
        private string nResult = null;
        private string latResult = null;
        private string longResult = null;



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


        private FilteredData ApplyFilter(List<string> texts)
        {
            FilteredData results = new FilteredData();

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

        private string EFilterResult(FilteredData data)
        {
            List<string> eArray = data.E ?? new List<string>();
            StringBuilder resultString = new StringBuilder();

            if (eArray.Count > 0)
            {
                resultString.AppendLine("E;");
                foreach (string e in eArray)
                {
                    string formattedE = e;
                    resultString.AppendLine(formattedE + ";");
                }
            }

            return resultString.ToString();
        }

        private string NFilterResult(FilteredData data)
        {
            List<string> nArray = data.N ?? new List<string>();
            StringBuilder resultString = new StringBuilder();

            if (nArray.Count > 0)
            {
                resultString.AppendLine("N");
                foreach (string n in nArray)
                {
                    string formattedN = n;
                    resultString.AppendLine(formattedN);
                }

            }

            return resultString.ToString();
        }

        private string LatitudeFilterResult(FilteredData data)
        {
            List<string> latArray = data.Latitude ?? new List<string>();
            StringBuilder resultString = new StringBuilder();

            if (latArray.Count > 0)
            {
                resultString.AppendLine("Latitude;");
                foreach (string lat in latArray)
                {
                    string formattedLat = lat;
                    resultString.AppendLine(formattedLat + ";");
                }
            }

            return resultString.ToString();
        }


        private string LongitudeFilterResult(FilteredData data)
        {
            List<string> longArray = data.Longitude ?? new List<string>();
            StringBuilder resultString = new StringBuilder();

            if (longArray.Count > 0)
            {

                resultString.AppendLine("Longitude");
                foreach (string longi in longArray)
                {
                    string formattedLong = longi;
                    resultString.AppendLine(formattedLong);
                }


            }

            return resultString.ToString();
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

                    FilteredData filteredData = ApplyFilter(texts);


                    richTextBox4.Text = EFilterResult(filteredData);
                    richTextBox5.Text = NFilterResult(filteredData);
                    richTextBox6.Text = LatitudeFilterResult(filteredData);
                    richTextBox7.Text = LongitudeFilterResult(filteredData);

                    richTextBox4.Height = (int)(CalculateRichTextBoxHeight(richTextBox4) * 1.2);
                    richTextBox5.Height = (int)(CalculateRichTextBoxHeight(richTextBox5) * 1.2);
                    richTextBox6.Height = (int)(CalculateRichTextBoxHeight(richTextBox6) * 1.2);
                    richTextBox7.Height = (int)(CalculateRichTextBoxHeight(richTextBox7) * 1.2);

                    eResult = EFilterResult(filteredData);
                    nResult = NFilterResult(filteredData);
                    latResult = LatitudeFilterResult(filteredData);
                    longResult = LongitudeFilterResult(filteredData);

                }
                finally
                {
                    button6.Enabled = true;
                }
            }
        }

        private string CombineContent(string eResultString, string nResultString, string longResultString, string latResultString)
        {
            string[] eLines = eResultString.Split('\n');
            string[] nLines = nResultString.Split('\n');
            string[] longLines = longResultString.Split('\n');
            string[] latLines = latResultString.Split('\n');

            StringBuilder combinedContent = new StringBuilder();

            int maxLines = Math.Max(Math.Max(eLines.Length, nLines.Length), Math.Max(longLines.Length, latLines.Length));

            for (int i = 0; i < maxLines; i++)
            {
                if (i < eLines.Length) combinedContent.Append(eLines[i]);
                if (i < nLines.Length) combinedContent.Append(nLines[i]);
                if (i < longLines.Length) combinedContent.Append(longLines[i]);
                if (i < latLines.Length) combinedContent.Append(latLines[i]);
            }

            return combinedContent.ToString();
        }


        //private string CombineContent(string eResultString, string nResultString, string longResultString, string latResultString)
        //{
        //    string[] eLines = eResultString.Split('\n');
        //    string[] nLines = nResultString.Split('\n');
        //    string[] longLines = longResultString.Split('\n');
        //    string[] latLines = latResultString.Split('\n');

        //    List<string> combinedLines = new List<string>();
        //    int maxLines = Math.Max(Math.Max(eLines.Length, nLines.Length), Math.Max(longLines.Length, latLines.Length));

        //    for (int i = 0; i < maxLines; i++)
        //    {
        //        string eLine = i < eLines.Length ? eLines[i] : "";
        //        string nLine = i < nLines.Length ? nLines[i] : "";
        //        string longLine = i < longLines.Length ? longLines[i] : "";
        //        string latLine = i < latLines.Length ? latLines[i] : "";

        //        if (!string.IsNullOrEmpty(nLine) || !string.IsNullOrEmpty(eLine) || !string.IsNullOrEmpty(latLine) || !string.IsNullOrEmpty(longLine))
        //        {
        //            combinedLines.Add($"{eLine}{nLine}{longLine}{latLine}");
        //        }
        //    }

        //    return string.Join("\n", combinedLines);
        //}
        private void CopyToClipboard()
        {
            string contentToCopy = CombineContent(eResult, nResult, latResult, longResult);
            Clipboard.SetText(contentToCopy);


            MessageBox.Show("Conteúdo copiado para a área de transferência.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox4.SelectionAlignment = HorizontalAlignment.Right;
            richTextBox6.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }
    }
}