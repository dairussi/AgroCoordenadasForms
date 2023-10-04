using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using Tesseract;
using PdfiumViewer;
using AgroCoordenadas.Interface;
using AgroCoordenadas.Service;
using static AgroCoordenadas.Service.FilterService;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace AgroCoordenadas
{
    public partial class Form1 : Form
    {
        private string tempFilePath = "";
        public static List<string> texts = new List<string>();
        private readonly IFilter _filter;
        private string? eResult = null;
        private string? nResult = null;
        private string? latResult = null;
        private string? longResult = null;
        FilteredData results = new FilteredData();
        Form2 form2 = new Form2();


        public Form1()
        {
            InitializeComponent();
            _filter = new FilterService();
        }

        //envio e normalização do caminho do pdf
        private void button1_Click(object sender, EventArgs e)
        {
            texts.Clear();
            eResult = nResult = latResult = longResult = null;
            results = new FilteredData();
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao selecionar o PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //verifica se pdf é selecionável
        private bool IsSelectablePdf(string tempFilePath)
        {
            bool isSelectablePdf = false;
            try
            {
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
            }
            catch
            {
            }
            return isSelectablePdf;
        }

        //método de extração de texto caso o pdf seja selecionável
        private List<string> PdfText(string tempFilePath)
        {
            try
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
            }
            catch
            {
            }
            return texts;
        }

        //método de extração de texto caso o pdf não seja selecionável
        private List<string> PdfImg(string tempFilePath)
        {
            try
            {
                string outputFolder = "Tempory/";
                if (Directory.Exists(outputFolder))
                {
                    Directory.Delete(outputFolder, true);
                }
                Directory.CreateDirectory(outputFolder);

                //conversão escala de cinza
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

                //salva cada página como imagem
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

                //tesseract para extração do texto de imagens
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
            }
            catch
            {
            }
            return texts;
        }

        //aplicação da service filter
        private FilteredData ApplyFilter(List<string> texts)
        {
            try
            {
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
            }
            catch
            {
            }
            return results;
        }

        //métodos de formatação de resultados e tamanhos
        private string EFilterResult(FilteredData data)
        {
            List<string> eArray = data.E ?? new List<string>();
            StringBuilder resultString = new StringBuilder();
            if (eArray.Count > 0)
            {
                resultString.AppendLine("E;");
                foreach (string e in eArray)
                {
                    string formattedE = FormatValueUtm(e);
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
                    string formattedN = FormatValueUtm(n);
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
                    string formattedLat = FormatValueLatLong(lat);
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
                    string formattedLong = FormatValueLatLong(longi);
                    resultString.AppendLine(formattedLong);
                }
            }
            return resultString.ToString();
        }

        private string FormatValueUtm(string value)
        {
            string formattedValue = value.Replace(" ", "");
            if (formattedValue.Length > 4)
            {
                string lastFour = formattedValue.Substring(formattedValue.Length - 4);
                string everythingElse = formattedValue.Substring(0, formattedValue.Length - 4);
                lastFour = Regex.Replace(lastFour, @"[.,]", ",");
                everythingElse = Regex.Replace(everythingElse, @"[^\d]", "");
                formattedValue = everythingElse + lastFour;
            }
            else
            {
                formattedValue = formattedValue.TrimEnd('.', ',');
            }
            return formattedValue;
        }

        private string FormatValueLatLong(string value)
        {
            string formattedValue = value.Replace(" ", "");
            formattedValue = formattedValue.Replace(".", ",");
            return formattedValue;
        }

        private int CalculateRichTextBoxHeight(RichTextBox richTextBox)
        {
            using (Graphics g = richTextBox.CreateGraphics())
            {
                SizeF preferredSize = g.MeasureString(richTextBox.Text, richTextBox.Font, richTextBox.Width);
                return (int)Math.Ceiling(preferredSize.Height);
            }
        }

        private void button6_MouseDown(object? sender, EventArgs e)
        {
            button6.MouseDown -= button6_MouseDown;
            if (string.IsNullOrEmpty(tempFilePath))
            {
                MessageBox.Show("Selecione um arquivo PDF antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (form2 == null || form2.IsDisposed)
                {
                    form2 = new Form2();
                    form2.Show();
                }
                else
                {
                    form2.Show();
                }
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(button6_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(button6_RunWorkerCompleted);
                worker.RunWorkerAsync();
            }
        }

        private void button6_DoWork(object sender, DoWorkEventArgs e)
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
                    this.Invoke((MethodInvoker)delegate
                    {
                        richTextBox2.Text = string.Join(Environment.NewLine, texts);
                    });
                    FilteredData filteredData = ApplyFilter(texts);
                    this.Invoke((MethodInvoker)delegate
                    {
                    richTextBox4.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox6.SelectionAlignment = HorizontalAlignment.Right;
                    richTextBox4.Text = eResult = EFilterResult(filteredData);
                    richTextBox5.Text = nResult = NFilterResult(filteredData);
                    richTextBox6.Text = latResult = LatitudeFilterResult(filteredData);
                    richTextBox7.Text = longResult = LongitudeFilterResult(filteredData);

                    richTextBox4.Height = (int)(CalculateRichTextBoxHeight(richTextBox4) * 1.2);
                    richTextBox5.Height = (int)(CalculateRichTextBoxHeight(richTextBox5) * 1.2);
                    richTextBox6.Height = (int)(CalculateRichTextBoxHeight(richTextBox6) * 1.2);
                    richTextBox7.Height = (int)(CalculateRichTextBoxHeight(richTextBox7) * 1.2);

                    bool isPair1Visible = !string.IsNullOrEmpty(richTextBox4.Text) || !string.IsNullOrEmpty(richTextBox5.Text);
                    richTextBox4.Visible = isPair1Visible;
                    richTextBox5.Visible = isPair1Visible;
                    bool isPair2Visible = !string.IsNullOrEmpty(richTextBox6.Text) || !string.IsNullOrEmpty(richTextBox7.Text);
                    richTextBox6.Visible = isPair2Visible;
                    richTextBox7.Visible = isPair2Visible;

                    int availableWidth = panel3.Width;
                    List<RichTextBox> visibleRichTextBoxes = new List<RichTextBox>();
                    if (isPair1Visible)
                    {
                        visibleRichTextBoxes.Add(richTextBox4);
                        visibleRichTextBoxes.Add(richTextBox5);
                    }
                    if (isPair2Visible)
                    {
                        visibleRichTextBoxes.Add(richTextBox6);
                        visibleRichTextBoxes.Add(richTextBox7);
                    }
                    int totalRichTextBoxWidth = visibleRichTextBoxes.Count * 160;
                    int marginX = (availableWidth - totalRichTextBoxWidth) / 2;
                    foreach (RichTextBox richTextBox in visibleRichTextBoxes)
                    {
                        richTextBox.Width = 160;
                        richTextBox.Location = new Point(marginX, richTextBox.Location.Y);
                        marginX += 160;
                    }
                    });
                }
            catch (Exception ex)
            {             
                if (form2 != null && !form2.IsDisposed)
                {
                    form2.Invoke((MethodInvoker)delegate
                    {
                        form2.Close();
                    });
                }              
                button6.Invoke((MethodInvoker)delegate
                {
                    button6.MouseDown += button6_MouseDown;
                    button6.Enabled = true;
                });
                MessageBox.Show($"Ocorreu um erro", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
                {
                }
        }

        private void button6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (form2.InvokeRequired)
            {
                form2.Invoke(new MethodInvoker(() => form2.Close()));
            }
            else
            {
                form2.Close();
            }
            button6.MouseDown += button6_MouseDown;
            button6.Enabled = true;
        }


        //métodos de combinação de resultados para botões de ação
        private string CombineContent()
        {
            string[] eLines = (eResult ?? "").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string[] nLines = (nResult ?? "").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string[] longLines = (latResult ?? "").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string[] latLines = (longResult ?? "").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            StringBuilder combinedContent = new StringBuilder();
            int maxLines = Math.Max(
                Math.Max(eLines.Length, nLines.Length),
                Math.Max(longLines.Length, latLines.Length)
            );
            for (int i = 0; i < maxLines; i++)
            {
                if (i < eLines.Length && !string.IsNullOrWhiteSpace(eLines[i]))
                {
                    combinedContent.Append(eLines[i].Trim());
                }
                if (i < nLines.Length && !string.IsNullOrWhiteSpace(nLines[i]))
                {
                    combinedContent.Append(nLines[i].Trim());
                }
                if (i < longLines.Length && !string.IsNullOrWhiteSpace(longLines[i]))
                {
                    combinedContent.Append(longLines[i].Trim());
                }
                if (i < latLines.Length && !string.IsNullOrWhiteSpace(latLines[i]))
                {
                    combinedContent.Append(latLines[i].Trim());
                }
                combinedContent.AppendLine();
            }
            return combinedContent.ToString();
        }

        //botão destaque de texto
        private void HighlightMatches(RichTextBox richTextBox, List<string>? matches, Color color)
        {
            if (matches != null)
            {
                string text = richTextBox.Text;
                foreach (var match in matches)
                {
                    int startIndex = text.IndexOf(match);
                    while (startIndex >= 0)
                    {
                        richTextBox.SelectionStart = startIndex;
                        richTextBox.SelectionLength = match.Length;
                        richTextBox.SelectionBackColor = color;
                        startIndex = text.IndexOf(match, startIndex + 1);
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var text in texts)
            {
                var coordinates = _filter.Filter(text);
                HighlightMatches(richTextBox2, coordinates.N, Color.Orange);
                HighlightMatches(richTextBox2, coordinates.E, Color.Green);
                HighlightMatches(richTextBox2, coordinates.Latitude, Color.Yellow);
                HighlightMatches(richTextBox2, coordinates.Longitude, Color.Purple);
            }
        }

        //botão limpeza
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
        }

        //botão copia texto
        private void CopyToClipboard()
        {
            eResult = richTextBox4.Text;
            nResult = richTextBox5.Text;
            latResult = richTextBox6.Text;
            longResult = richTextBox7.Text;
            string contentToCopy = CombineContent();
            Clipboard.SetText(contentToCopy);
            MessageBox.Show("Conteúdo copiado para a área de transferência.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        //botão salva em excel
        public void SaveToExcel()
        {
            eResult = richTextBox4.Text;
            nResult = richTextBox5.Text;
            latResult = richTextBox6.Text;
            longResult = richTextBox7.Text;
            var cellData = CombineContent().Split('\n');
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Planilha");
            for (int i = 0; i < cellData.Length; i++)
            {
                var row = sheet.CreateRow(i);
                row.CreateCell(0).SetCellValue(cellData[i]);
            }
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel files (*.xls)|*.xls";
                    sfd.FilterIndex = 2;
                    sfd.RestoreDirectory = true;
                    sfd.FileName = "dados.xls";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var fileStream = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            workbook.Write(fileStream);
                        }
                        MessageBox.Show("Conteúdo salvo", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Operação cancelada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Operação cancelada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveToExcel();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
