namespace FonksiyonOlusturma
{
    partial class systems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            label1 = new Label();
            button1 = new Button();
            textBox2 = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(36, 188);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(789, 515);
            dataGridView1.TabIndex = 5;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(245, 23);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(338, 159);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Sistem";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(106, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(206, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 29);
            label1.Name = "label1";
            label1.Size = new Size(89, 20);
            label1.TabIndex = 1;
            label1.Text = "Sistem No :";
            // 
            // button1
            // 
            button1.Location = new Point(121, 105);
            button1.Name = "button1";
            button1.Size = new Size(160, 48);
            button1.TabIndex = 0;
            button1.Text = "Kaydet";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(106, 59);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(206, 27);
            textBox2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 62);
            label2.Name = "label2";
            label2.Size = new Size(92, 20);
            label2.TabIndex = 3;
            label2.Text = "Sistem Adı :";
            // 
            // systems
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(860, 726);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox1);
            Name = "systems";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "systems";
            Load += systems_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private GroupBox groupBox1;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
        private TextBox textBox2;
        private Label label2;
    }
}