
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
                if (!string.IsNullOrEmpty(projeKodu)) // TextBox boþ deðilse
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
                    {
                        // Projects tablosuna yeni bir proje eklemek için aþaðýdaki gibi bir LINQ sorgusu kullanabilirsiniz:
                        var yeniProje = new Projects
                        {
                            ProjectName = projeKodu
                            // Diðer alanlara da deðer atayabilirsiniz, gerekirse.
                        };

                        dbContext.projects.Add(yeniProje); // Yeni proje nesnesini Projects tablosuna ekleyin
                        dbContext.SaveChanges(); // Deðiþiklikleri veritabanýna kaydedin
                    }
                    MessageBox.Show("Proje Kaydedildi...");
                }
                else
                {
                    MessageBox.Show("Proje kodu girmelisiniz."); // TextBox boþsa kullanýcýya uyarý verin
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluþtu: " + ex.Message);
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
            // DataGridView sütunlarýný oluþturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "PROJE KODU");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanýz gerekiyor
            {
                // Projects tablosundaki verileri sorgulayýn
                var projects = dbContext.projects.ToList();

                int satirNo = 1; // Baþlangýç satýr numarasý 1 olarak ayarlanýr

                // DataGridView'e satýr ekleyin
                foreach (var project in projects)
                {
                    // DataGridView'e yeni bir satýr ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // Ýlk sütunu (SATIR NO) satýr numarasý olarak ayarlayýn
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // Ýkinci sütunu (PROJE KODU) proje kodu olarak ayarlayýn
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = project.ProjectName });

                    // DataGridView'e satýrý ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her satýr ekledikten sonra satýr numarasýný arttýrýn
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

                    Fonk.TextBoxValue = cellValue; // Form2'deki TextBox'a deðeri aktar
                    Fonk.Show();
                }
            }
        }
    }
}