using Icm_Fiyat_Güncelleme;
using OtomatikSiparisGirisi;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace FonksiyonOlusturma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Fiyat> fiyatList = new List<Fiyat>();
        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            try
            {
                // Dosya seçme penceresi açmak için
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Excel Dosyasý |*.xlsx";
                file.ShowDialog();

                // seçtiðimiz excel'in tam yolu
                string tamYol = file.FileName;

                //Excel baðlantý adresi
                string baglantiAdresi = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + tamYol + ";Extended Properties='Excel 12.0;IMEX=1;'";

                //baðlantý 
                OleDbConnection baglanti = new(baglantiAdresi);

                //tüm verileri seçmek için select sorgumuz. Sayfa1 olan kýsmý Excel'de hangi sayfayý açmak istiyosanýz orayý yazabilirsiniz.
                OleDbCommand komut = new OleDbCommand("Select * From [" + textBox1.Text + "$]", baglanti);

                //baðlantýyý açýyoruz.
                baglanti.Open();

                //Gelen verileri DataAdapter'e atýyoruz.
                OleDbDataAdapter da = new OleDbDataAdapter(komut);

                //Grid'imiz için bir DataTable oluþturuyoruz.
                System.Data.DataTable data = new System.Data.DataTable();

                //DataAdapter'da ki verileri data adýndaki DataTable'a dolduruyoruz.
                da.Fill(data);

                //DataGrid'imizin kaynaðýný oluþturduðumuz DataTable ile dolduruyoruz.
                //dataGridView1.DataSource = data;

                fiyatList = DataTableConverter.ConvertTo<Fiyat>(data);
                //MessageBox.Show("Fiyat Listesi Yüklendi");
            }
            catch (Exception ex)
            {
                // Hata alýrsak ekrana bastýrýyoruz.
                MessageBox.Show(ex.Message);
            }
            // Sütunlarý eklemek için DataGridView'e sütun ekleyin
            dataGridView1.Columns.Add("GUÝD", "sGuid");
            dataGridView1.Columns.Add("PROJE KODU", "proje Kodu");
            dataGridView1.Columns.Add("FONKSIYONLAR", "fonsiyonlar");


            // ... Diðer sütunlarý burada ekleyin

            foreach (var fiyat in fiyatList)
            {
                int rowIndex = dataGridView1.Rows.Add(); // Yeni bir satýr ekleniyor

                // DataGridView hücrelerine verileri yazdýrma
                dataGridView1.Rows[rowIndex].Cells["GUÝD"].Value = fiyat.sGuid;
                dataGridView1.Rows[rowIndex].Cells["PROJE KODU"].Value = fiyat.PROJE_KODU;
                dataGridView1.Rows[rowIndex].Cells["FONKSIYONLAR"].Value = fiyat.FONKSIYONLAR;

                // ... Diðer kolonlara göre ayný þekilde devam edebilirsiniz
            }

            MessageBox.Show("Veriler DataGridView'e Yazdýrýldý");

        }
        SqlConnection baglanti;
        private void Form1_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection("Data Source=SRVMIKRO;Initial Catalog=MikroDB_V16_ICM;Integrated Security=True");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
                baglanti.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // 4. sütunun deðerini alýn ve null kontrolü yapýn
                    object cellValue = row.Cells[2].Value;
                    object cellValue1 = row.Cells[1].Value;
                    if (cellValue != null && cellValue1 != null)
                    {
                        string Sguid = Convert.ToString(row.Cells[0].Value); // DataGridView'deki RecordUid sütununun deðerini alýn
                        string projekod = Convert.ToString(row.Cells[1].Value); // DataGridView'deki projekod sütununun deðerini alýn
                        decimal fonksiyon = Convert.ToDecimal(row.Cells[2].Value); // DataGridView'deki fonksiyon sütununun deðerini alýn

                        // STOKLAR_USER tablosuna ekle
                        string insertUserQuery = "INSERT INTO [dbo].[ISEMIRLERI_USER] (" +
                                                "[Record_uid], [Yeni_Alan_1], [is_emri_tipi], [PROJE_KODU], [FONKSIYONLAR]) " +

                                                "VALUES (@Record_uid, @Yeni_Alan_1, @is_emri_tipi, @PROJE_KODU, @FONKSIYONLAR)";
                        using (SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, baglanti))
                        {
                            insertUserCommand.Parameters.AddWithValue("@Record_uid", Sguid);
                            insertUserCommand.Parameters.AddWithValue("@Yeni_Alan_1", "");
                            insertUserCommand.Parameters.AddWithValue("@is_emri_tipi", "");
                            insertUserCommand.Parameters.AddWithValue("@PROJE_KODU", projekod);
                            insertUserCommand.Parameters.AddWithValue("@FONKSIYONLAR", fonksiyon);

                            insertUserCommand.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("BAÞARI ÝLE EKLENDÝ...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA VAR!!!");
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}