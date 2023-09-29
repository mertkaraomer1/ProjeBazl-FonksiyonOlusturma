namespace FonksiyonOlusturma
{
    partial class KullaniciEkranı
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
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox4 = new ComboBox();
            groupBox1 = new GroupBox();
            button3 = new Button();
            button2 = new Button();
            comboBox3 = new ComboBox();
            button1 = new Button();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            label4 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(173, 33);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(211, 31);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(151, 48);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(211, 31);
            comboBox2.TabIndex = 1;
            // 
            // comboBox4
            // 
            comboBox4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(148, 138);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(211, 31);
            comboBox4.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(comboBox3);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(comboBox4);
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(25, 75);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(377, 336);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Modül Durumu";
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(265, 229);
            button3.Name = "button3";
            button3.Size = new Size(94, 75);
            button3.TabIndex = 10;
            button3.Text = "BİTİR";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(151, 229);
            button2.Name = "button2";
            button2.Size = new Size(94, 75);
            button2.TabIndex = 9;
            button2.Text = "ARAVER";
            button2.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            comboBox3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(150, 92);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(211, 31);
            comboBox3.TabIndex = 8;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(35, 229);
            button1.Name = "button1";
            button1.Size = new Size(94, 75);
            button1.TabIndex = 8;
            button1.Text = "BAŞLA";
            button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(72, 138);
            label3.Name = "label3";
            label3.Size = new Size(72, 23);
            label3.TabIndex = 6;
            label3.Text = "Modül :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(44, 95);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 5;
            label2.Text = "Fonksiyon :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(81, 51);
            label1.Name = "label1";
            label1.Size = new Size(61, 23);
            label1.TabIndex = 4;
            label1.Text = "Proje :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(79, 33);
            label4.Name = "label4";
            label4.Size = new Size(88, 23);
            label4.TabIndex = 9;
            label4.Text = "Sorumlu :";
            // 
            // KullaniciEkranı
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(431, 435);
            Controls.Add(label4);
            Controls.Add(groupBox1);
            Controls.Add(comboBox1);
            Name = "KullaniciEkranı";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "KullaniciEkranı";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox4;
        private GroupBox groupBox1;
        private Button button3;
        private Button button2;
        private Button button1;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label label4;
        private ComboBox comboBox3;
    }
}