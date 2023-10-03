using System.Windows.Forms;

namespace AgroCoordenadas
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            openFileDialog1 = new OpenFileDialog();
            richTextBox2 = new RichTextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            button6 = new Button();
            button4 = new Button();
            button5 = new Button();
            button2 = new Button();
            button3 = new Button();
            panel3 = new Panel();
            richTextBox7 = new RichTextBox();
            richTextBox6 = new RichTextBox();
            richTextBox5 = new RichTextBox();
            richTextBox4 = new RichTextBox();
            panel2 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel4 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(3, 3);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.RightToLeft = RightToLeft.No;
            richTextBox2.Size = new Size(594, 594);
            richTextBox2.TabIndex = 0;
            richTextBox2.Text = "";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 50);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top;
            pictureBox2.BackgroundImage = Properties.Resources.banner;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(384, 16);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(425, 77);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(pictureBox2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1254, 96);
            panel1.TabIndex = 15;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top;
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 88F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Controls.Add(richTextBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(button1, 1, 0);
            tableLayoutPanel1.Controls.Add(button6, 2, 0);
            tableLayoutPanel1.Controls.Add(button4, 6, 0);
            tableLayoutPanel1.Controls.Add(button5, 5, 0);
            tableLayoutPanel1.Controls.Add(button2, 3, 0);
            tableLayoutPanel1.Controls.Add(button3, 4, 0);
            tableLayoutPanel1.Location = new Point(262, 17);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(668, 47);
            tableLayoutPanel1.TabIndex = 16;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.White;
            richTextBox1.BorderStyle = BorderStyle.FixedSingle;
            richTextBox1.Location = new Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(280, 41);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.BackgroundImage = Properties.Resources.envio;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.Transparent;
            button1.Location = new Point(314, 3);
            button1.Name = "button1";
            button1.Size = new Size(34, 34);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(32, 41, 86);
            button6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button6.ForeColor = Color.White;
            button6.Location = new Point(360, 3);
            button6.Name = "button6";
            button6.Size = new Size(140, 41);
            button6.TabIndex = 7;
            button6.Text = "Filtrar";
            button6.UseVisualStyleBackColor = false;
            button6.MouseDown += button6_MouseDown;
            // 
            // button4
            // 
            button4.BackColor = Color.White;
            button4.BackgroundImage = Properties.Resources.xls;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button4.FlatStyle = FlatStyle.Flat;
            button4.ForeColor = Color.Transparent;
            button4.Location = new Point(630, 3);
            button4.Name = "button4";
            button4.Size = new Size(34, 34);
            button4.TabIndex = 5;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.White;
            button5.BackgroundImage = Properties.Resources.copy;
            button5.BackgroundImageLayout = ImageLayout.Stretch;
            button5.FlatStyle = FlatStyle.Flat;
            button5.ForeColor = Color.Transparent;
            button5.Location = new Point(590, 3);
            button5.Name = "button5";
            button5.Size = new Size(34, 34);
            button5.TabIndex = 6;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.BackgroundImage = Properties.Resources.destaque;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.Transparent;
            button2.Location = new Point(510, 3);
            button2.Name = "button2";
            button2.Size = new Size(34, 34);
            button2.TabIndex = 3;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.White;
            button3.BackgroundImage = Properties.Resources.lixeira;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.Transparent;
            button3.Location = new Point(550, 3);
            button3.Name = "button3";
            button3.Size = new Size(34, 34);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top;
            panel3.AutoScroll = true;
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(richTextBox7);
            panel3.Controls.Add(richTextBox6);
            panel3.Controls.Add(richTextBox5);
            panel3.Controls.Add(richTextBox4);
            panel3.Location = new Point(603, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(594, 594);
            panel3.TabIndex = 13;
            // 
            // richTextBox7
            // 
            richTextBox7.BorderStyle = BorderStyle.None;
            richTextBox7.Location = new Point(450, 9);
            richTextBox7.Name = "richTextBox7";
            richTextBox7.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox7.Size = new Size(140, 580);
            richTextBox7.TabIndex = 12;
            richTextBox7.Text = "";
            // 
            // richTextBox6
            // 
            richTextBox6.BorderStyle = BorderStyle.None;
            richTextBox6.Location = new Point(306, 9);
            richTextBox6.Name = "richTextBox6";
            richTextBox6.RightToLeft = RightToLeft.No;
            richTextBox6.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox6.Size = new Size(140, 580);
            richTextBox6.TabIndex = 11;
            richTextBox6.Text = "";
            // 
            // richTextBox5
            // 
            richTextBox5.BorderStyle = BorderStyle.None;
            richTextBox5.Location = new Point(155, 9);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox5.Size = new Size(140, 580);
            richTextBox5.TabIndex = 10;
            richTextBox5.Text = "";
            // 
            // richTextBox4
            // 
            richTextBox4.BorderStyle = BorderStyle.None;
            richTextBox4.Location = new Point(11, 9);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.RightToLeft = RightToLeft.No;
            richTextBox4.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox4.Size = new Size(140, 580);
            richTextBox4.TabIndex = 9;
            richTextBox4.Text = "";
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.Controls.Add(tableLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 96);
            panel2.Name = "panel2";
            panel2.Size = new Size(1254, 67);
            panel2.TabIndex = 17;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel3, 1, 0);
            tableLayoutPanel2.Controls.Add(richTextBox2, 0, 0);
            tableLayoutPanel2.Location = new Point(27, 85);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1200, 600);
            tableLayoutPanel2.TabIndex = 18;
            // 
            // panel4
            // 
            panel4.AutoSize = true;
            panel4.Controls.Add(tableLayoutPanel2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 163);
            panel4.Name = "panel4";
            panel4.Size = new Size(1254, 688);
            panel4.TabIndex = 19;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            ClientSize = new Size(1254, 965);
            Controls.Add(panel4);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AgroCoordenadas";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private OpenFileDialog openFileDialog1;
        private RichTextBox richTextBox2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private RichTextBox richTextBox1;
        private Button button1;
        private Button button6;
        private Button button4;
        private Button button5;
        private Button button2;
        private Button button3;
        private Panel panel3;
        private RichTextBox richTextBox7;
        private RichTextBox richTextBox6;
        private RichTextBox richTextBox5;
        private RichTextBox richTextBox4;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel4;
    }
}