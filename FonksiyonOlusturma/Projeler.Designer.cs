namespace FonksiyonOlusturma
{
    partial class Projeler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Projeler));
            groupBox1 = new GroupBox();
            textBox3 = new TextBox();
            label4 = new Label();
            comboBox1 = new ComboBox();
            label1 = new Label();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            groupBox2 = new GroupBox();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label5 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(417, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(373, 159);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(180, 47);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(187, 27);
            textBox3.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(93, 50);
            label4.Name = "label4";
            label4.Size = new Size(81, 20);
            label4.TabIndex = 3;
            label4.Text = "Proje Adı :";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(180, 16);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(187, 28);
            comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(80, 19);
            label1.Name = "label1";
            label1.Size = new Size(94, 20);
            label1.TabIndex = 1;
            label1.Text = "Proje Kodu :";
            // 
            // button1
            // 
            button1.Location = new Point(180, 108);
            button1.Name = "button1";
            button1.Size = new Size(117, 40);
            button1.TabIndex = 0;
            button1.Text = "Kaydet";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(25, 177);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(789, 515);
            dataGridView1.TabIndex = 3;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(25, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(338, 107);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Sistem";
            // 
            // textBox2
            // 
            textBox2.Enabled = false;
            textBox2.Location = new Point(105, 59);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(205, 27);
            textBox2.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 62);
            label3.Name = "label3";
            label3.Size = new Size(92, 20);
            label3.TabIndex = 3;
            label3.Text = "Sistem Adı :";
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(105, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(205, 27);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 29);
            label2.Name = "label2";
            label2.Size = new Size(105, 20);
            label2.TabIndex = 1;
            label2.Text = "Sistem Kodu :";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(180, 75);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(117, 27);
            dateTimePicker1.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(43, 80);
            label5.Name = "label5";
            label5.Size = new Size(131, 20);
            label5.TabIndex = 5;
            label5.Text = "Proje Bitiş Tarihi :";
            // 
            // Projeler
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(860, 726);
            Controls.Add(groupBox2);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Projeler";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Projeler";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private Button button1;
        private DataGridView dataGridView1;
        private ComboBox comboBox1;
        private GroupBox groupBox2;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Label label3;
        private Label label4;
        private TextBox textBox3;
        private DateTimePicker dateTimePicker1;
        private Label label5;
    }
}