namespace FonksiyonOlusturma
{
    partial class GirisSayfası
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
            button3 = new Button();
            button1 = new Button();
            button2 = new Button();
            button4 = new Button();
            button5 = new Button();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(111, 41);
            button3.Name = "button3";
            button3.Size = new Size(94, 84);
            button3.TabIndex = 6;
            button3.Text = "İş Atama";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(271, 41);
            button1.Name = "button1";
            button1.Size = new Size(94, 84);
            button1.TabIndex = 7;
            button1.Text = "Sorumlu Ekranı";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(271, 148);
            button2.Name = "button2";
            button2.Size = new Size(94, 84);
            button2.TabIndex = 9;
            button2.Text = "Rapor";
            button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button4.Location = new Point(111, 148);
            button4.Name = "button4";
            button4.Size = new Size(94, 84);
            button4.TabIndex = 8;
            button4.Text = "Atanan İşler";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button5.Location = new Point(189, 258);
            button5.Name = "button5";
            button5.Size = new Size(94, 84);
            button5.TabIndex = 10;
            button5.Text = "Uygun Olmayan İşler";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // GirisSayfası
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(500, 365);
            Controls.Add(button5);
            Controls.Add(button2);
            Controls.Add(button4);
            Controls.Add(button1);
            Controls.Add(button3);
            Name = "GirisSayfası";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GirisSayfası";
            ResumeLayout(false);
        }

        #endregion

        private Button button3;
        private Button button1;
        private Button button2;
        private Button button4;
        private Button button5;
    }
}