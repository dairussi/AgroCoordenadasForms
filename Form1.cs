using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using Tesseract;
using PdfiumViewer;
using System.Drawing;
using AgroCoordenadas.Interface;
using AgroCoordenadas.Service;

namespace AgroCoordenadas
{
    public partial class Form1 : Form
    {
        private string tempFilePath = "";
        private static List<string> texts = new List<string>();
        private readonly IFilter _filter;

        public Form1()
        {
            InitializeComponent();
            _filter = new FilterService();
        }


        private string FormatNFilterResult(Dictionary<string, List<string>> results)
        {
            List<string> nArray = results.ContainsKey("N") ? results["N"] : new List<string>();
            int maxItems = nArray.Count;
            StringBuilder resultString = new StringBuilder();

            if (maxItems > 0)
            {
                resultString.AppendLine("N");
                for (int i = 0; i < maxItems; i++)
                {
                    string n = nArray[i];
                    string formattedN = FormatValueUtm(n);
                    resultString.AppendLine(formattedN);
                }
            }

            return resultString.ToString();
        }

        private string FormatEFilterResult(Dictionary<string, List<string>> results)
        {
            List<string> eArray = results.ContainsKey("E") ? results["E"] : new List<string>();
            int maxItems = eArray.Count;
            StringBuilder resultString = new StringBuilder();

            if (maxItems > 0)
            {
                resultString.AppendLine("E;");
                for (int i = 0; i < maxItems; i++)
                {
                    string e = eArray[i];
                    string formattedE = FormatValueUtm(e);
                    resultString.AppendLine(formattedE);
                }
            }

            return resultString.ToString();
        }

        private string FormatLatFilterResult(Dictionary<string, List<string>> results)
        {
            List<string> latitudeArray = results.ContainsKey("Latitude") ? results["Latitude"] : new List<string>();
            int maxItems = latitudeArray.Count;
            StringBuilder resultString = new StringBuilder();

            if (maxItems > 0)
            {
                resultString.AppendLine("Latitude");
                for (int i = 0; i < maxItems; i++)
                {
                    string latitude = latitudeArray[i];
                    string formattedLatitude = FormatValueLatLong(latitude);
                    resultString.AppendLine(formattedLatitude);
                }
            }

            return resultString.ToString();
        }

        private string FormatLongFilterResult(Dictionary<string, List<string>> results)
        {
            List<string> longitudeArray = results.ContainsKey("Longitude") ? results["Longitude"] : new List<string>();
            int maxItems = longitudeArray.Count;
            StringBuilder resultString = new StringBuilder();

            if (maxItems > 0)
            {
                resultString.AppendLine("Longitude;");
                for (int i = 0; i < maxItems; i++)
                {
                    string longitude = longitudeArray[i];
                    string formattedLongitude = FormatValueLatLong(longitude);
                    resultString.AppendLine(formattedLongitude);
                }
            }

            return resultString.ToString();
        }

        private string CombineContent(Dictionary<string, List<string>> results)
        {
            List<string> eLines = results.ContainsKey("E") ? results["E"] : new List<string>();
            List<string> nLines = results.ContainsKey("N") ? results["N"] : new List<string>();
            List<string> latLines = results.ContainsKey("Latitude") ? results["Latitude"] : new List<string>();
            List<string> longLines = results.ContainsKey("Longitude") ? results["Longitude"] : new List<string>();

            int maxLines = Math.Max(Math.Max(eLines.Count, nLines.Count), Math.Max(latLines.Count, longLines.Count));
            StringBuilder resultString = new StringBuilder();

            resultString.AppendLine("E\tN\tLat\tLong");

            for (int i = 0; i < maxLines; i++)
            {
                string eLine = i < eLines.Count ? eLines[i] : "";
                string nLine = i < nLines.Count ? nLines[i] : "";
                string latLine = i < latLines.Count ? latLines[i] : "";
                string longLine = i < longLines.Count ? longLines[i] : "";

                resultString.AppendLine($"{eLine}\t{nLine}\t{latLine}\t{longLine}");
            }

            return resultString.ToString();
        }



        private string FormatValueUtm(string value)
        {
            string formattedValue = value;
            if (formattedValue.Length > 4)
            {
                string lastFour = formattedValue.Substring(formattedValue.Length - 4);
                string everythingElse = formattedValue.Substring(0, formattedValue.Length - 4);
                string lastFourFormatted = lastFour.Replace(",", ".");
                string everythingElseFormatted = new string(everythingElse.Where(char.IsDigit).ToArray());
                formattedValue = everythingElseFormatted + lastFourFormatted;
            }
            else
            {
                formattedValue = formattedValue.Replace(",", "").TrimEnd();
            }
            return formattedValue;
        }

        private string FormatValueLatLong(string value)
        {
            string formattedValue = value.Replace(".", ",");
            string finalFormattedValue = formattedValue.Replace("º", "°");
            return finalFormattedValue;
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


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private Dictionary<string, List<string>> ApplyFilter(List<string> texts)
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            foreach (var text in texts)
            {
                var coordinates = _filter.Filter(text);
                foreach (var kvp in coordinates)
                {
                    if (!results.ContainsKey(kvp.Key))
                    {
                        results[kvp.Key] = new List<string>();
                    }
                    results[kvp.Key].AddRange(kvp.Value);
                }
            }

            return results;
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

                    Dictionary<string, List<string>> filteredResults = ApplyFilter(texts);
                    richTextBox3.Text = CombineContent(filteredResults);
                }
                finally
                {
                    button6.Enabled = true;
                    texts.Clear();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}