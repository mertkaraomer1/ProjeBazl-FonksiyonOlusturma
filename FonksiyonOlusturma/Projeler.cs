
using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace FonksiyonOlusturma
{
    public partial class Projeler : Form
    {
        private MyDbContext dbContext;
        public Projeler()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ýn adý olmalý
            set { textBox1.Text = value; }
        }
        public string SystemDescription1
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ýn adý olmalý
            set { textBox2.Text = value; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string systemName = textBox1.Text;
            string projectName = comboBox1.Text;
            string projectDescription = textBox3.Text;

            try
            {
                if (!string.IsNullOrEmpty(systemName) && !string.IsNullOrEmpty(projectName))
                {
                    using (var dbContext = new MyDbContext())
                    {
                        // Önce Systems tablosundan SystemId'yi alýn
                        int systemId = dbContext.systems
                            .Where(s => s.SystemName == systemName)
                            .Select(s => s.SystemId)
                            .FirstOrDefault();

                        if (systemId != 0) // Eþleþen bir SystemId bulundu mu kontrol edin
                        {
                            // Projects tablosunda ayný ProjectName ile kayýt var mý kontrol edin
                            bool isDuplicate = dbContext.projects
                                .Any(p => p.ProjectName == projectName && p.SystemId == systemId);

                            if (!isDuplicate)
                            {
                                // Ayný ProjectName'e sahip kayýt yoksa yeni bir proje ekleyin
                                var yeniProje = new Projects
                                {
                                    ProjectName = projectName,
                                    ProjectDescription = projectDescription,
                                    SystemId = systemId // SystemId'yi atayýn
                                                        // Diðer alanlara da deðer atayabilirsiniz, gerekirse.
                                };

                                dbContext.projects.Add(yeniProje); // Yeni proje nesnesini Projects tablosuna ekleyin
                                dbContext.SaveChanges(); // Deðiþiklikleri veritabanýna kaydedin
                            }
                            else
                            {
                                MessageBox.Show("Bu proje zaten kayýtlý.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen SystemName ve ProjectName girin.");
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
            using (var dbContext = new Context())
            {
                var projectCodes = dbContext.PROJELERs
                    .Where(p => p.pro_kodu.CompareTo("21") > 0) // Baþlangýcý 20'den büyük olanlarý alýr
                    .Select(p => p.pro_kodu)
                    .ToList();

                // ComboBox1'e projeleri ekleyin
                comboBox1.DataSource = projectCodes;
            }

            ProjeAtama();
        }
        public void ProjeAtama()
        {
            string systemName = textBox1.Text;

            using (var dbContext = new MyDbContext())
            {
                // TextBox'tan gelen veriyi kullanarak SystemId'yi bulun
                int systemId = dbContext.systems
                    .Where(s => s.SystemName == systemName)
                    .Select(s => s.SystemId)
                    .FirstOrDefault();

                if (systemId != 0) // Eþleþen bir SystemId bulundu mu kontrol edin
                {
                    // Projects tablosundan belirtilen SystemId ile eþleþen ProjectName'leri alýn
                    var projectNames = dbContext.projects
                        .Where(p => p.SystemId == systemId)
                        .Select(p => new { ProjectName = p.ProjectName, ProjectDescription = p.ProjectDescription })
                        .ToList();


                    if (projectNames.Any()) // Eþleþen projeler bulundu mu kontrol edin
                    {
                        // DataGridView'i temizle
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();

                        // DataGridView sütunlarýný oluþturun
                        dataGridView1.Columns.Add("Column1", "SATIR NO");
                        dataGridView1.Columns.Add("Column2", "PROJELER");
                        dataGridView1.Columns.Add("Column3", "PROJE ADI");
                        // DataGridView kontrolünüze bir buton sütunu ekleyin.
                        DataGridViewImageColumn buttonColumn = new DataGridViewImageColumn();
                        buttonColumn.HeaderText = "SÝL"; // Sütun baþlýðý
                        buttonColumn.Image = Image.FromFile("delete.png"); // Silme resmini belirtin
                        buttonColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Resmi düzgün görüntülemek için ayar
                        dataGridView1.Columns.Add(buttonColumn);
                        dataGridView1.CellContentClick += dataGridView1_CellContentClick;
                        // ProjectName'leri DataGridView'e ekleyin
                        int rowNumber = 1;
                        foreach (var projectName in projectNames)
                        {
                            dataGridView1.Rows.Add(rowNumber, projectName.ProjectName, projectName.ProjectDescription);

                            rowNumber++;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Eþleþen SystemName bulunamadý.");
                }
            }




        }
        public Fonksiyonlar Fonk;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string SistemName = textBox1.Text;
            string SistemDescription = textBox2.Text;

            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                if (Fonk == null || Fonk.IsDisposed)
                {
                    Fonk = new Fonksiyonlar();
                    DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    string ProjectDescription = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string cellValue = clickedCell.Value.ToString();
                    Fonk.TextBoxValue1 = SistemName;
                    Fonk.SystemDes = SistemDescription;
                    Fonk.ProjeDes = ProjectDescription;
                    Fonk.TextBoxValue = cellValue; // Form2'deki TextBox'a deðeri aktar
                    Fonk.Show();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[3].Index && e.RowIndex >= 0)
            {
                string SistemName = textBox1.Text;

                using (var dbContext = new MyDbContext())
                {
                    // TextBox'tan gelen veriyi kullanarak SystemId'yi bulun
                    int systemId = dbContext.systems
                        .Where(s => s.SystemName == SistemName)
                        .Select(s => s.SystemId)
                        .FirstOrDefault();

                    if (systemId != 0)
                    {
                        if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                        {
                            string projectName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                            // Projects tablosunda belirtilen SystemId ve ProjectName ile eþleþen satýrý bulun
                            var projectToDelete = dbContext.projects
                                .FirstOrDefault(p =>
                                    p.SystemId == systemId &&
                                    p.ProjectName == projectName
                                );

                            if (projectToDelete != null)
                            {
                                // Silinecek bir þey var, o zaman silme iþlemini gerçekleþtirin
                                dbContext.projects.Remove(projectToDelete);
                                dbContext.SaveChanges();

                                // Projects tablosunu güncellemek için kullanýlan bir fonksiyonunuzu çaðýrýn
                                ProjeAtama();

                                // DataGridView'den de satýrý kaldýrýn
                                dataGridView1.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                MessageBox.Show("Silinecek bir proje bulunamadý.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sistem bulunamadý.");
                    }
                }
            }

        }
    }
}