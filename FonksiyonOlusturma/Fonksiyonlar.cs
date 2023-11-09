using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;

namespace FonksiyonOlusturma
{
    public partial class Fonksiyonlar : Form
    {
        private MyDbContext dbContext;
        private string? cellValue;
        public Fonksiyonlar()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            // TextBox'ı sadece okunabilir yap
            textBox1.ReadOnly = true;
        }
        public Fonksiyonlar(string? cellValue)
        {
            this.cellValue = cellValue;
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        public string TextBoxValue1
        {
            get { return textBox4.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox4.Text = value; }

        }
        public string SystemDes
        {
            get { return textBox5.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox5.Text = value; }
        }
        public string ProjeDes
        {
            get { return textBox6.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox6.Text = value; }
        }
        private void Fonksiyonlar_Load(object sender, EventArgs e)
        {
            FonksiyonGoruntuleme();
        }
        public void FonksiyonGoruntuleme()
        {
            // DataGridView'i temizleyin
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "FONKSİYONLAR");
            dataGridView1.Columns.Add("Column3", "FONKSİYON AÇIKLAMASI");

            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                string searchText = textBox1.Text; // TextBox1'den gelen veriyi alın

                // TextBox1'deki veriyi Projects tablosundaki ProjectName ile eşleştirin ve ProjectId'leri alın
                var projectIds = dbContext.projects
                    .Where(p => p.ProjectName == searchText)
                    .Select(p => p.ProjectId)
                    .ToList();

                // ProjectId'leri kullanarak Functions tablosundaki FunctionNamelerini ve FunctionDescription'ları alın
                var functionData = dbContext.functions
                    .Where(f => projectIds.Contains(f.ProjectId))
                    .Select(f => new
                    {
                        FunctionName = f.FunctionName,
                        FunctionDescription = f.FunctionDescription
                    })
                    .ToList();

                int satirNo = 1; // Başlangıç satır numarası 1 olarak ayarlanır

                // DataGridView'e satır ekleyin
                foreach (var function in functionData)
                {
                    // DataGridView'e yeni bir satır ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // İlk sütunu (SATIR NO) satır numarası olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // İkinci sütunu (FONKSİYONLAR) fonksiyon adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = function.FunctionName });

                    // Üçüncü sütunu (FONKSİYON AÇIKLAMASI) fonksiyon açıklaması olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = function.FunctionDescription });

                    // DataGridView'e satırı ekleyin
                    dataGridView1.Rows.Add(row);

                    satirNo++; // Her satır ekledikten sonra satır numarasını arttırın
                }
                // DataGridView kontrolünüze bir buton sütunu ekleyin.
                DataGridViewImageColumn buttonColumn = new DataGridViewImageColumn();
                buttonColumn.HeaderText = "SİL"; // Sütun başlığı
                buttonColumn.Image = Image.FromFile("delete.png"); // Silme resmini belirtin
                buttonColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Resmi düzgün görüntülemek için ayar
                dataGridView1.Columns.Add(buttonColumn);
                dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string selectedProjectName = textBox1.Text; // textbox1'den gelen proje adını alın
            string functionName = textBox2.Text; // TextBox2'den gelen FunctionName'i alın
            string FunctionDescription = textBox3.Text;

            if (!string.IsNullOrEmpty(selectedProjectName) && !string.IsNullOrEmpty(functionName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
                    {
                        // ComboBox1'den seçilen ProjectName ile eşleşen Projects tablosundaki ProjectId'yi bulun
                        int projectId = dbContext.projects
                            .Where(p => p.ProjectName == selectedProjectName)
                            .Select(p => p.ProjectId)
                            .FirstOrDefault();

                        if (projectId != 0) // ProjectId bulunduysa
                        {
                            // Functions tablosundan aynı ProjectId ve FunctionName ile kayıt var mı kontrol edin
                            bool isDuplicate = dbContext.functions
                                .Any(f => f.ProjectId == projectId && f.FunctionName == functionName);

                            if (!isDuplicate)
                            {
                                // Aynı ProjectId ve FunctionName'e sahip kayıt yoksa yeni bir fonksiyon ekleyin
                                var yeniFonksiyon = new Functions
                                {
                                    ProjectId = projectId,
                                    FunctionName = functionName,
                                    FunctionDescription = FunctionDescription
                                };

                                dbContext.functions.Add(yeniFonksiyon); // Yeni fonksiyonu Functions tablosuna ekleyin
                                dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                            }
                            else
                            {
                                MessageBox.Show("Aynı ProjectId ve FunctionName ile kayıtlı fonksiyon zaten mevcut.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Proje bulunamadı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Proje adı ve fonksiyon adı girmelisiniz.");
            }

            textBox2.Clear();
            textBox3.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            FonksiyonGoruntuleme();

        }
        Modul mod;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string SistemName = textBox4.Text;
                string textBoxValue = textBox1.Text; // Form2'deki TextBox'tan veriyi al
                Modul mod = new Modul();
                mod.TextBoxValue = textBoxValue; // Form3'teki TextBox'a veriyi aktar
                mod.TextBoxValue2 = SistemName;
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[1]; // Tıklanan hücreyi al
                string cellValue = clickedCell.Value.ToString();
                mod.TextBoxValue1 = cellValue;// Form3'teki TextBox2'ye veriyi aktar
                mod.Show();

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // TextBox'tan gelen verileri kullanarak ProjectId ve FunctionId'yi bulun
            string projectName = textBox1.Text;

            int projectId = dbContext.projects
                .Where(p => p.ProjectName == projectName)
                .Select(p => p.ProjectId)
                .FirstOrDefault();

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (columnIndex >= 0 && columnIndex < dataGridView1.Columns.Count && columnIndex == dataGridView1.Columns[3].Index) // "FunctionName" sütununda tıklama işlemi
            {
                // İlgili satırda bulunan verilere erişmek için veri modelini kullanabilirsiniz.
                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                {
                    Functions rowData = new Functions
                    {
                        FunctionName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(),
                        FunctionDescription = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString()
                    };

                    if (projectId != 0)
                    {
                        // Functions tablosundan belirtilen ProjectId ve FunctionName ile eşleşen satırı bulun
                        var functionToDelete = dbContext.functions
                            .FirstOrDefault(m =>
                                m.ProjectId == projectId &&
                                m.FunctionName == rowData.FunctionName
                            );

                        if (functionToDelete != null)
                        {
                            // Silinecek bir şey var, o zaman silme işlemini gerçekleştirin
                            dbContext.functions.Remove(functionToDelete);
                            dbContext.SaveChanges();

                            // Functions tablosunu güncellemek için kullanılan bir fonksiyonunuzu çağırın
                            FonksiyonGoruntuleme();
                        }
                        else
                        {
                            // Silinecek bir şey yoksa hata vermek yerine bir bildirim gösterebilirsiniz
                            MessageBox.Show("Silinecek bir fonksiyon bulunamadı.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Proje bulunamadı.");
                    }
                }
            }

        }
    }
}
