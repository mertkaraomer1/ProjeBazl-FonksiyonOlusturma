
using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace FonksiyonOlusturma
{
    public partial class Form1 : Form
    {
        private MyDbContext dbContext;
        public Form1()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string projeKodu = textBox1.Text;
            try
            {
                if (!string.IsNullOrEmpty(projeKodu)) // TextBox bo� de�ilse
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
                    {
                        // Projects tablosuna yeni bir proje eklemek i�in a�a��daki gibi bir LINQ sorgusu kullanabilirsiniz:
                        var yeniProje = new Projects
                        {
                            ProjectName = projeKodu
                            // Di�er alanlara da de�er atayabilirsiniz, gerekirse.
                        };

                        dbContext.projects.Add(yeniProje); // Yeni proje nesnesini Projects tablosuna ekleyin
                        dbContext.SaveChanges(); // De�i�iklikleri veritaban�na kaydedin
                    }
                    MessageBox.Show("Proje Kaydedildi...");
                }
                else
                {
                    MessageBox.Show("Proje kodu girmelisiniz."); // TextBox bo�sa kullan�c�ya uyar� verin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata olu�tu: " + ex.Message);
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            ProjeAtama();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProjeAtama();
        }
        public void ProjeAtama()
        {
            // DataGridView s�tunlar�n� olu�turun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "PROJE KODU");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanman�z gerekiyor
            {
                // Projects tablosundaki verileri sorgulay�n
                var projects = dbContext.projects.ToList();

                int satirNo = 1; // Ba�lang�� sat�r numaras� 1 olarak ayarlan�r

                // DataGridView'e sat�r ekleyin
                foreach (var project in projects)
                {
                    // DataGridView'e yeni bir sat�r ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // �lk s�tunu (SATIR NO) sat�r numaras� olarak ayarlay�n
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // �kinci s�tunu (PROJE KODU) proje kodu olarak ayarlay�n
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = project.ProjectName });

                    // DataGridView'e sat�r� ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her sat�r ekledikten sonra sat�r numaras�n� artt�r�n
                }
            }
        }
        public Fonksiyonlar Fonk;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                if (Fonk == null || Fonk.IsDisposed)
                {
                    Fonk = new Fonksiyonlar();
                    DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string cellValue = clickedCell.Value.ToString();

                    Fonk.TextBoxValue = cellValue; // Form2'deki TextBox'a de�eri aktar
                    Fonk.Show();
                }
            }
        }
    }
}