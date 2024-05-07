using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;

namespace FonksiyonOlusturma
{
    public partial class Modul : Form
    {
        private MyDbContext dbContext;
        public Modul()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            // TextBox'ı sadece okunabilir yap
            textBox1.ReadOnly = true;
            // TextBox'ı sadece okunabilir yap
            textBox2.ReadOnly = true;
        }
        public string TextBoxValue
        {
            get { return textBox1.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox1.Text = value; }
        }
        public string TextBoxValue1
        {
            get { return textBox2.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox2.Text = value; }
        }
        public string TextBoxValue2
        {
            get { return textBox5.Text; } // textBox1 burada TextBox'ın adı olmalı
            set { textBox5.Text = value; }
        }
        private void Modul_Load(object sender, EventArgs e)
        {
            var CategoryName = dbContext.categories.Select(s => s.CategoryName).ToList();
            comboBox1.DataSource = CategoryName;
            ModuleGoster();
        }
        public void ModuleGoster()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            // DataGridView sütunlarını oluşturun
            dataGridView1.Columns.Add("Column1", "SATIR NO");
            dataGridView1.Columns.Add("Column2", "MODÜLLER");
            dataGridView1.Columns.Add("Column3", "MODÜL AÇIKLAMASI");
            dataGridView1.Columns.Add("Column4", "TİP");
            dataGridView1.Columns.Add("Column5", "AÇIKLAMA");
            using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
            {
                string searchText1 = textBox5.Text; // TextBox1'den gelen veriyi alın
                string searchText5 = textBox2.Text;
                string searchText2 = textBox1.Text;

                var systemIds = dbContext.systems
                    .Where(p => p.SystemName == searchText1)
                    .Select(p => p.SystemId)
                    .ToList();

                // TextBox1'deki veriyi Projects tablosundaki ProjectName ile eşleştirin ve ProjectId'leri alın
                var projectIds = dbContext.projects
                    .Where(p => systemIds.Contains(p.SystemId) && p.ProjectName == searchText2)
                    .Select(p => p.ProjectId)
                    .ToList();


                var functionIds = dbContext.functions
                    .Where(f => projectIds.Contains(f.ProjectId) && f.FunctionName == searchText5)
                    .Select(f => f.FunctionId)
                    .ToList();

                // FunctionId'leri kullanarak Modules tablosundaki ModuleName'leri alın
                var moduleNames = dbContext.modules
                    .Where(m => functionIds.Contains(m.FunctionId))
                    .Select(m => new
                    {
                        moduleName = m.ModuleName,
                        moduleDescription = m.ModuleDescription,
                        modultip = m.ModuleTip,
                        ModülAciklama=m.Description
                        
                    })
                    .ToList();

                int satirNo = 1; // Başlangıç satır numarası 1 olarak ayarlanır

                // DataGridView'e satır ekleyin
                foreach (var module in moduleNames)
                {
                    // DataGridView'e yeni bir satır ekleyin
                    DataGridViewRow row = new DataGridViewRow();

                    // İlk sütunu (SATIR NO) satır numarası olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = satirNo.ToString() });

                    // İkinci sütunu (MODÜLLER) modül adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = module.moduleName });

                    // İkinci sütunu (MODÜLLER) modül adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = module.moduleDescription });


                    // İkinci sütunu (MODÜLLER) modül adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = module.modultip });

                    // İkinci sütunu (MODÜLLER) modül adı olarak ayarlayın
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = module.ModülAciklama });


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

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedSistemName = textBox5.Text;
            string selectedFunctionName = textBox2.Text;
            string selectedProjectName = textBox1.Text;
            string selectedmodulName = textBox3.Text;
            string selectedModuleDescriptionName = textBox4.Text;
            string selectedCategoryName = comboBox1.Text;
            string selectedDescription = textBox6.Text;

            if (!string.IsNullOrEmpty(selectedFunctionName) &&
                !string.IsNullOrEmpty(selectedProjectName) &&
                !string.IsNullOrEmpty(selectedmodulName))
            {
                try
                {
                    using (var dbContext = new MyDbContext()) // DbContext'inizi burada kullanmanız gerekiyor
                    {
                        int projectId = dbContext.projects
                            .Where(c => c.ProjectName == selectedProjectName)
                            .Select(c => c.ProjectId)
                            .FirstOrDefault();

                        int functionId = dbContext.functions
                            .Where(f => f.FunctionName == selectedFunctionName && f.ProjectId==projectId)
                            .Select(f => f.FunctionId)
                            .FirstOrDefault();



                        int CategoryId = dbContext.categories
                            .Where(c => c.CategoryName == selectedCategoryName)
                            .Select(c => c.CategoryID)
                            .FirstOrDefault();
                        int CategoryTime = dbContext.categories
                            .Where(c => c.CategoryName == selectedCategoryName)
                            .Select(c => c.CategoryTime)
                            .FirstOrDefault(); // or .SingleOrDefault() if CategoryName is unique

                        // Kontrol etmek için aynı modül adının ve ModuleTip'in mevcut olup olmadığını kontrol edin
                        bool isModuleExists = dbContext.modules
                            .Any(m =>
                                m.ProjectId == projectId &&
                                m.FunctionId == functionId &&
                                m.ModuleName == selectedmodulName &&
                                (m.ModuleTip == "3D" || m.ModuleTip == "2D"));

                        bool isRecordExists = dbContext.records
                            .Any(r =>
                                r.ProjectName == selectedProjectName &&
                                r.FunctionName == selectedFunctionName &&
                                r.ModuleName == selectedmodulName &&
                                (r.ModuleTip == "3D" || r.ModuleTip == "2D"));

                        if (!isModuleExists && !isRecordExists)
                        {
                            var yeniModul = new Modules
                            {
                                FunctionId = functionId,
                                ProjectId = projectId,
                                ModuleName = selectedmodulName,
                                ModuleDescription = selectedModuleDescriptionName,
                                CategoryId = CategoryId,
                                ModuleTip = "3D",
                                Description = selectedDescription
                            };
                            var RecordEt = new Records
                            {
                                SystemName = selectedSistemName,
                                ProjectName = selectedProjectName,
                                FunctionName = selectedFunctionName,
                                ModuleName = selectedmodulName,
                                ModuleDescription = selectedModuleDescriptionName,
                                CategoryName = selectedCategoryName,
                                CategoryTime = CategoryTime,
                                ModuleTip = "3D"
                            };
                            if (checkBox1.Checked == true)
                            {
                                // Eğer checkbox işaretliyse, yeni bir Modules ve Records nesnesi oluştur
                                var yeniModul1 = new Modules
                                {
                                    FunctionId = functionId,
                                    ProjectId = projectId,
                                    ModuleName = selectedmodulName,
                                    ModuleDescription = selectedModuleDescriptionName,
                                    CategoryId = CategoryId,
                                    ModuleTip = "2D",
                                    Description = selectedDescription
                                };

                                // Records nesnesi oluştur ve gerekli alanları doldur
                                var RecordEt1 = new Records
                                {
                                    SystemName = selectedSistemName,
                                    ProjectName = selectedProjectName,
                                    FunctionName = selectedFunctionName,
                                    ModuleName = selectedmodulName,
                                    ModuleDescription = selectedModuleDescriptionName,
                                    CategoryName = selectedCategoryName,
                                    CategoryTime = CategoryTime / 2, // Belirtilen zamanın yarısını al
                                    ModuleTip = "2D"
                                };

                                // Oluşturulan kayıtları veritabanına ekle
                                dbContext.records.Add(RecordEt1);
                                dbContext.modules.Add(yeniModul1);
                            }


                            dbContext.modules.Add(yeniModul); // Yeni modulu Moduls tablosuna ekleyin

                            dbContext.records.Add(RecordEt);
                            dbContext.SaveChanges(); // Değişiklikleri veritabanına kaydedin
                        }
                        else
                        {
                            MessageBox.Show("Bu modül zaten mevcut.");
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
                MessageBox.Show("Tüm alanları doldurmalısınız.");
            }
            textBox3.Clear();
            textBox4.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            ModuleGoster();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                Atamalar ata = new Atamalar();
                ata.Show();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            // TextBox'tan gelen verileri kullanarak ProjectId ve FunctionId'yi bulun
            string projectName = textBox1.Text;
            string functionName = textBox2.Text;

            int projectId = dbContext.projects
                .Where(p => p.ProjectName == projectName)
                .Select(p => p.ProjectId)
                .FirstOrDefault();

            int functionId = dbContext.functions
                .Where(f => f.FunctionName == functionName)
                .Select(f => f.FunctionId)
                .FirstOrDefault();

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;

            if (columnIndex == dataGridView1.Columns[5].Index)
            {
                // İlgili satırda bulunan verilere erişmek için veri modelini kullanabilirsiniz.
                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                {
                    Modules rowData = new Modules
                    {
                        ModuleName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(),
                        ModuleTip = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(),
                    };
                    Records recordData = new Records
                    {
                        ModuleName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(),
                        ProjectName = projectName,
                        FunctionName = functionName,
                        SystemName = textBox5.Text,
                        ModuleTip = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(), // Value eksikti
                    };

                    // Modules tablosundan belirtilen ProjectId, FunctionId ve ModuleName ile eşleşen satırı bulun
                    var moduleToDelete = dbContext.modules
                        .FirstOrDefault(m =>
                            m.ProjectId == projectId &&
                            m.FunctionId == functionId &&
                            m.ModuleName == rowData.ModuleName &&
                            m.ModuleTip == rowData.ModuleTip
                        );

                    // Records tablosundan belirtilen ProjectId, FunctionId ve ModuleName ile eşleşen satırı bulun
                    var RecordsToDelete = dbContext.records
                        .FirstOrDefault(m =>
                            m.SystemName == textBox5.Text &&
                            m.FunctionName == functionName &&
                            m.ModuleName == recordData.ModuleName &&
                            m.ModuleTip == recordData.ModuleTip
                        );

                    if (moduleToDelete != null && RecordsToDelete != null)
                    {
                        // Silinecek bir şey var, o zaman silme işlemini gerçekleştirin
                        dbContext.modules.Remove(moduleToDelete);
                        dbContext.records.Remove(RecordsToDelete);
                        dbContext.SaveChanges();
                        // Modules tablosunu güncellemek için kullanılan bir fonksiyonunuzu çağırın
                        ModuleGoster();
                    }
                    else
                    {
                        // Silinecek bir şey yoksa hata vermek yerine bir bildirim gösterebilirsiniz
                        MessageBox.Show("Silinecek bir şey bulunamadı.");
                    }
                }
            }




        }


    }
}
