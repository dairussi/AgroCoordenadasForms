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
            button1 = new Button();
            richTextBox1 = new RichTextBox();
            openFileDialog1 = new OpenFileDialog();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            richTextBox2 = new RichTextBox();
            richTextBox4 = new RichTextBox();
            richTextBox5 = new RichTextBox();
            richTextBox6 = new RichTextBox();
            richTextBox7 = new RichTextBox();
            panel3 = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.BackgroundImage = Properties.Resources.envio;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.Transparent;
            button1.Location = new Point(605, 99);
            button1.Name = "button1";
            button1.Size = new Size(34, 34);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.White;
            richTextBox1.BorderStyle = BorderStyle.FixedSingle;
            richTextBox1.Location = new Point(316, 95);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(280, 44);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.BackgroundImage = Properties.Resources.destaque;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.Transparent;
            button2.Location = new Point(822, 100);
            button2.Name = "button2";
            button2.Size = new Size(34, 34);
            button2.TabIndex = 3;
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.White;
            button3.BackgroundImage = Properties.Resources.lixeira;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.Transparent;
            button3.Location = new Point(861, 100);
            button3.Name = "button3";
            button3.Size = new Size(34, 34);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.White;
            button4.BackgroundImage = Properties.Resources.xls;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button4.FlatStyle = FlatStyle.Flat;
            button4.ForeColor = Color.Transparent;
            button4.Location = new Point(939, 100);
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
            button5.Location = new Point(900, 100);
            button5.Name = "button5";
            button5.Size = new Size(34, 34);
            button5.TabIndex = 6;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(32, 41, 86);
            button6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button6.ForeColor = Color.White;
            button6.Location = new Point(654, 95);
            button6.Name = "button6";
            button6.Size = new Size(140, 44);
            button6.TabIndex = 7;
            button6.Text = "Filtrar";
            button6.UseVisualStyleBackColor = false;
            button6.MouseDown += button6_MouseDown;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(20, 182);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.RightToLeft = RightToLeft.No;
            richTextBox2.Size = new Size(600, 600);
            richTextBox2.TabIndex = 0;
            richTextBox2.Text = "";
            // 
            // richTextBox4
            // 
            richTextBox4.BorderStyle = BorderStyle.None;
            richTextBox4.Location = new Point(11, 9);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.RightToLeft = RightToLeft.No;
            richTextBox4.Size = new Size(140, 580);
            richTextBox4.TabIndex = 9;
            richTextBox4.Text = "";
            // 
            // richTextBox5
            // 
            richTextBox5.BorderStyle = BorderStyle.None;
            richTextBox5.Location = new Point(155, 9);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.Size = new Size(140, 580);
            richTextBox5.TabIndex = 10;
            richTextBox5.Text = "";
            // 
            // richTextBox6
            // 
            richTextBox6.BorderStyle = BorderStyle.None;
            richTextBox6.Location = new Point(306, 9);
            richTextBox6.Name = "richTextBox6";
            richTextBox6.RightToLeft = RightToLeft.No;
            richTextBox6.Size = new Size(140, 580);
            richTextBox6.TabIndex = 11;
            richTextBox6.Text = "";
            // 
            // richTextBox7
            // 
            richTextBox7.BorderStyle = BorderStyle.None;
            richTextBox7.Location = new Point(450, 9);
            richTextBox7.Name = "richTextBox7";
            richTextBox7.Size = new Size(140, 580);
            richTextBox7.TabIndex = 12;
            richTextBox7.Text = "";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(richTextBox7);
            panel3.Controls.Add(richTextBox6);
            panel3.Controls.Add(richTextBox5);
            panel3.Controls.Add(richTextBox4);
            panel3.Location = new Point(634, 183);
            panel3.Name = "panel3";
            panel3.Size = new Size(600, 600);
            panel3.TabIndex = 13;
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
            pictureBox2.BackgroundImage = Properties.Resources.banner;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(447, 23);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(314, 51);
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1254, 831);
            Controls.Add(pictureBox2);
            Controls.Add(richTextBox2);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Controls.Add(panel3);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "AgroCoordenadas";
            Load += Form1_Load;
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private RichTextBox richTextBox1;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox4;
        private RichTextBox richTextBox5;
        private RichTextBox richTextBox6;
        private RichTextBox richTextBox7;
        private Panel panel3;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}