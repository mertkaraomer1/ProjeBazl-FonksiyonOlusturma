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
            SuspendLayout();
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(107, 95);
            button3.Name = "button3";
            button3.Size = new Size(94, 84);
            button3.TabIndex = 6;
            button3.Text = "Görev Atama";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(267, 95);
            button1.Name = "button1";
            button1.Size = new Size(94, 84);
            button1.TabIndex = 7;
            button1.Text = "Sorumlu Ekranı";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // GirisSayfası
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(500, 292);
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
    }
}