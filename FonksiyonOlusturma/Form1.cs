using Icm_Fiyat_G�ncelleme;
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
                // Dosya se�me penceresi a�mak i�in
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Excel Dosyas� |*.xlsx";
                file.ShowDialog();

                // se�ti�imiz excel'in tam yolu
                string tamYol = file.FileName;

                //Excel ba�lant� adresi
                string baglantiAdresi = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + tamYol + ";Extended Properties='Excel 12.0;IMEX=1;'";

                //ba�lant� 
                OleDbConnection baglanti = new(baglantiAdresi);

                //t�m verileri se�mek i�in select sorgumuz. Sayfa1 olan k�sm� Excel'de hangi sayfay� a�mak istiyosan�z oray� yazabilirsiniz.
                OleDbCommand komut = new OleDbCommand("Select * From [" + textBox1.Text + "$]", baglanti);

                //ba�lant�y� a��yoruz.
                baglanti.Open();

                //Gelen verileri DataAdapter'e at�yoruz.
                OleDbDataAdapter da = new OleDbDataAdapter(komut);

                //Grid'imiz i�in bir DataTable olu�turuyoruz.
                System.Data.DataTable data = new System.Data.DataTable();

                //DataAdapter'da ki verileri data ad�ndaki DataTable'a dolduruyoruz.
                da.Fill(data);

                //DataGrid'imizin kayna��n� olu�turdu�umuz DataTable ile dolduruyoruz.
                //dataGridView1.DataSource = data;

                fiyatList = DataTableConverter.ConvertTo<Fiyat>(data);
                //MessageBox.Show("Fiyat Listesi Y�klendi");
            }
            catch (Exception ex)
            {
                // Hata al�rsak ekrana bast�r�yoruz.
                MessageBox.Show(ex.Message);
            }
            // S�tunlar� eklemek i�in DataGridView'e s�tun ekleyin
            dataGridView1.Columns.Add("GU�D", "sGuid");
            dataGridView1.Columns.Add("PROJE KODU", "proje Kodu");
            dataGridView1.Columns.Add("FONKSIYONLAR", "fonsiyonlar");


            // ... Di�er s�tunlar� burada ekleyin

            foreach (var fiyat in fiyatList)
            {
                int rowIndex = dataGridView1.Rows.Add(); // Yeni bir sat�r ekleniyor

                // DataGridView h�crelerine verileri yazd�rma
                dataGridView1.Rows[rowIndex].Cells["GU�D"].Value = fiyat.sGuid;
                dataGridView1.Rows[rowIndex].Cells["PROJE KODU"].Value = fiyat.PROJE_KODU;
                dataGridView1.Rows[rowIndex].Cells["FONKSIYONLAR"].Value = fiyat.FONKSIYONLAR;

                // ... Di�er kolonlara g�re ayn� �ekilde devam edebilirsiniz
            }

            MessageBox.Show("Veriler DataGridView'e Yazd�r�ld�");

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
                    // 4. s�tunun de�erini al�n ve null kontrol� yap�n
                    object cellValue = row.Cells[2].Value;
                    object cellValue1 = row.Cells[1].Value;
                    if (cellValue != null && cellValue1 != null)
                    {
                        string Sguid = Convert.ToString(row.Cells[0].Value); // DataGridView'deki RecordUid s�tununun de�erini al�n
                        string projekod = Convert.ToString(row.Cells[1].Value); // DataGridView'deki projekod s�tununun de�erini al�n
                        decimal fonksiyon = Convert.ToDecimal(row.Cells[2].Value); // DataGridView'deki fonksiyon s�tununun de�erini al�n

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
                MessageBox.Show("BA�ARI �LE EKLEND�...");
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