using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System.Data;

namespace FonksiyonOlusturma
{
    public partial class KullaniciEkranı : Form
    {
        private MyDbContext dbContext;
        public KullaniciEkranı()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
            yükle();
            advancedDataGridView1.CellClick += advancedDataGridView1_CellClick_1;
        }
        int kolon4Verisi;
        string selectedSystemName;
        DataTable table = new DataTable();
        public void yükle()
        {
            string selectedStaffName = comboBox1.Text;
            advancedDataGridView1.Columns.Clear();
            table.Rows.Clear();
            table.Columns.Clear();
            if (table.Columns.Count == 0)
            {
                table.Columns.Add("Sistem Numarası");
                table.Columns.Add("Proje Numarası");
                table.Columns.Add("Fonksiyon Numarası");
                table.Columns.Add("Modül Numarası");
                table.Columns.Add("Modül Adı");
                table.Columns.Add("Kategori Adı");
                table.Columns.Add("Kategori Zamanı");
                table.Columns.Add("Personel Adı");
                table.Columns.Add("Modül Türü");
                table.Columns.Add("Durum");
                table.Columns.Add("Kalan Süre");
                table.Columns.Add("Çalıştığı Toplam Süre");
            }

            var sorgu = dbContext.assignments
                .Where(a => a.Status != "False" && a.StaffName == selectedStaffName)
                .Select(a => new
                {
                    SistemAdı = a.SystemName,
                    ProjeAdı = a.ProjectName,
                    FonksiyonAdı = a.FunctionName,
                    ModülAdı = a.ModuleName,
                    ModülAçıklama = a.ModuleDescription,
                    ModülTürü = a.ModuleTip,
                    KategoriZamanı = a.CategoryTime,
                    PersonelAdı = a.StaffName,
                    KategoriAdı = a.CategoryName,
                    Durum = a.Status,
                })
                .ToList();

            if (sorgu.Any())
            {
                foreach (var item in sorgu)
                {
                    var enSonDurum = dbContext.status
                        .Where(a => a.ProjectName == item.ProjeAdı &&
                                    a.FunctionName == item.FonksiyonAdı &&
                                    a.ModuleName == item.ModülAdı &&
                                    a.ModuleTip == item.ModülTürü)
                        .Select(a => new
                        {
                            durumAdı = a.StatusName,
                            durumZamanı = a.StatusTime,
                            modultip=a.ModuleTip
                        })
                        .OrderByDescending(a => a.durumZamanı)
                        .FirstOrDefault();

                    var enSonBaslaDurumu = dbContext.status
                        .Where(a => a.ProjectName == item.ProjeAdı &&
                                    a.FunctionName == item.FonksiyonAdı &&
                                    a.ModuleName == item.ModülAdı &&
                                    a.ModuleTip == item.ModülTürü &&
                                    a.StatusName == "Başla")
                        .Select(a => new
                        {
                            durumZamanı = a.StatusTime,
                            kategoriZamanı = a.CategoryTime
                        })
                        .OrderBy(a => a.durumZamanı)
                        .FirstOrDefault();

                    var enSonDurum1 = dbContext.status
                        .Where(a => a.ProjectName == item.ProjeAdı &&
                                    a.FunctionName == item.FonksiyonAdı &&
                                    a.ModuleName == item.ModülAdı &&
                                    a.ModuleTip == item.ModülTürü)
                        .Select(a => new
                        {
                            durumAdı = a.StatusName,
                            durumZamanı = a.StatusTime
                        })
                        .OrderBy(a => a.durumZamanı)
                        .ToList();

                    var ilkBaslaDurumu = dbContext.status
                        .Where(a => a.ProjectName == item.ProjeAdı &&
                                    a.FunctionName == item.FonksiyonAdı &&
                                    a.ModuleName == item.ModülAdı &&
                                    a.ModuleTip == item.ModülTürü &&
                                    a.StatusName == "Başla")
                        .OrderBy(a => a.StatusTime)
                        .Select(a => new
                        {
                            durumAdı = a.StatusName,
                            durumZamanı = a.StatusTime
                        })
                        .FirstOrDefault();



                    if (enSonDurum != null)
                    {
                        // Initialize a variable to keep track of the sum of differences
                        TimeSpan totalDifference = TimeSpan.Zero;
                        TimeSpan totalDifference1 = TimeSpan.Zero;
                        TimeSpan zamanFarki = TimeSpan.Zero;
                        if (enSonDurum1.Count > 0)
                        {


                            // Find the index of the first "Araver" status
                            int startIndex = enSonDurum1.FindIndex(a => a.durumAdı == "Araver");

                            // Check if "Araver" status is found
                            if (startIndex != -1)
                            {
                                // Iterate through each status record starting from the "Araver" status
                                for (int i = startIndex; i < enSonDurum1.Count; i += 2)
                                {
                                    var statusItem = enSonDurum1[i];
                                    var baslaIndex = i + 1;

                                    if (baslaIndex < enSonDurum1.Count)
                                    {
                                        var baslaItem = enSonDurum1[baslaIndex];
                                        // Calculate the time difference
                                        TimeSpan difference = baslaItem.durumZamanı - statusItem.durumZamanı;

                                        // Add the difference to the total
                                        totalDifference += difference;
                                    }
                                    else
                                    {
                                        // Calculate the time difference from the last "Araver" status to now
                                        TimeSpan difference1 = DateTime.Now - statusItem.durumZamanı;

                                        // Add the difference to the total
                                        totalDifference1 += difference1;
                                    }
                                }
                            }
                        }
                        if (ilkBaslaDurumu != null)
                        {
                            zamanFarki = DateTime.Now - ilkBaslaDurumu.durumZamanı;

                        }


                        double AraverSuresi = (totalDifference + totalDifference1).TotalMinutes;


                        int kategoriZamanıSaatCinsinden = enSonBaslaDurumu.kategoriZamanı;
                        int kategoriZamanıDakikayaÇevir = kategoriZamanıSaatCinsinden * 60;
                        DateTime baslaZamanı = enSonBaslaDurumu.durumZamanı;
                        TimeSpan baslaZamanFarkı = DateTime.Now - baslaZamanı;
                        double dakikaCinsindenFark = baslaZamanFarkı.TotalMinutes;
                        int ayarlanmışKategoriZamanı = Convert.ToInt32(kategoriZamanıDakikayaÇevir - dakikaCinsindenFark + AraverSuresi);
                        int toplamDakika = ayarlanmışKategoriZamanı;
                        int saatler = toplamDakika / 60;
                        int dakikalar = toplamDakika % 60;
                        string ayarlanmışKategoriZamanıStr = $"{saatler:D2}:{dakikalar:D2}";


                        double ToplamÇalışmaSuresiDuble = zamanFarki.TotalMinutes;
                        int ToplamÇalışmaSuresi = Convert.ToInt32(ToplamÇalışmaSuresiDuble);

                        int toplamDakika2 = ToplamÇalışmaSuresi;
                        int Gunler2 = toplamDakika2 / (24 * 60); // Calculate days
                        int saatler2 = (toplamDakika2 % (24 * 60)) / 60; // Calculate hours
                        int dakikalar2 = toplamDakika2 % 60; // Calculate minutes

                        string TopÇalSure = $"{Gunler2:D2}:{saatler2:D2}:{dakikalar2:D2}";


                            // Satır ekleyin
                            if (enSonDurum != null && enSonDurum.durumAdı == "Başla")
                            {
                                // "Devam Ediyor(Ara Verildi.)..." durumu için satır ekle
                                table.Rows.Add(
                                    item.SistemAdı,
                                    item.ProjeAdı,
                                    item.FonksiyonAdı,
                                    item.ModülAdı,
                                    item.ModülAçıklama,
                                    item.KategoriAdı,
                                    item.KategoriZamanı,
                                    item.PersonelAdı,
                                    item.ModülTürü,
                                    "Devam Ediyor...",
                                    ayarlanmışKategoriZamanıStr,
                                    TopÇalSure
                                );
                            }
                            else if (enSonDurum != null && enSonDurum.durumAdı == "Araver")
                            {
                                // "Devam Ediyor..." durumu için satır ekle
                                table.Rows.Add(
                                    item.SistemAdı,
                                    item.ProjeAdı,
                                    item.FonksiyonAdı,
                                    item.ModülAdı,
                                    item.ModülAçıklama,
                                    item.KategoriAdı,
                                    item.KategoriZamanı,
                                    item.PersonelAdı,
                                    item.ModülTürü,
                                    "Araverildi...",
                                    ayarlanmışKategoriZamanıStr,
                                    TopÇalSure
                                );
                            }
                        
                    }

                    else if( enSonDurum == null)
                    {
                        // "Başlanmadı..." durumu için satır ekle
                        table.Rows.Add(
                            item.SistemAdı,
                            item.ProjeAdı,
                            item.FonksiyonAdı,
                            item.ModülAdı,
                            item.ModülAçıklama,
                            item.KategoriAdı,
                            item.KategoriZamanı,
                            item.PersonelAdı,
                            item.ModülTürü,
                            "Başlanmadı...",
                            "Başlanmadı...",
                            "Başlanmadı..."
                        );
                    }
                }
            }

            advancedDataGridView1.DataSource = table;

            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            UpdateButtonVisibleState();
        }




        string staffname;
        string moduleTip;

        private void button1_Click(object sender, EventArgs e)
        {

            string projectName = textBox2.Text.ToString();
            string functionName = textBox3.Text.ToString();
            string moduleName = textBox4.Text.ToString();
            var latestStatus = dbContext.status
                .Where(a => a.StaffName == staffname)
                .Select(a => new
                {
                    statusName = a.StatusName,
                    statusTime = a.StatusTime
                })
                .OrderByDescending(a => a.statusTime)
                .FirstOrDefault();
            if(latestStatus == null )
            {
                if (!string.IsNullOrEmpty(staffname) &&
                !string.IsNullOrEmpty(projectName) &&
                !string.IsNullOrEmpty(functionName) &&
                !string.IsNullOrEmpty(moduleName))
                {
                    // Status tablosuna yeni bir kayıt ekleyin
                    Status newStatus = new Status
                    {
                        ModuleName = moduleName,
                        FunctionName = functionName,
                        ProjectName = projectName,
                        StaffName = staffname,
                        CategoryTime = kolon4Verisi,
                        StatusName = "Başla",
                        StatusTime = DateTime.Now,
                        ModuleTip = moduleTip

                    };

                    dbContext.status.Add(newStatus);
                    dbContext.SaveChanges();

                    MessageBox.Show("Başlandı.");
                    UpdateButtonVisibleState();
                    yükle();
                }
                else
                {
                    MessageBox.Show("Lütfen tüm Textbox'ları doldurun.");
                }
            }
             else if (latestStatus!=null && latestStatus.statusName == "Araver" || latestStatus.statusName == "Bitti")
            {
                if (!string.IsNullOrEmpty(staffname) &&
                    !string.IsNullOrEmpty(projectName) &&
                    !string.IsNullOrEmpty(functionName) &&
                    !string.IsNullOrEmpty(moduleName))
                {
                    // Status tablosuna yeni bir kayıt ekleyin
                    Status newStatus = new Status
                    {
                        ModuleName = moduleName,
                        FunctionName = functionName,
                        ProjectName = projectName,
                        StaffName = staffname,
                        CategoryTime = kolon4Verisi,
                        StatusName = "Başla",
                        StatusTime = DateTime.Now,
                        ModuleTip = moduleTip

                    };

                    dbContext.status.Add(newStatus);
                    dbContext.SaveChanges();

                    MessageBox.Show("Başlandı.");
                    UpdateButtonVisibleState();
                    yükle();
                }
                else
                {
                    MessageBox.Show("Lütfen tüm Textbox'ları doldurun.");
                }
            }
            else if (latestStatus.statusName == "Başla")
            {
                MessageBox.Show("Önceden Başladığınız modülünüzü Bitir veya Araver yapıp tekrar deneyiniz...");
                button1.Enabled = false;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form sebepSecimiForm = new Form
            {
                Text = "Ara Verme Sebebinizi Seçiniz...",
                Size = new Size(300, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            ComboBox comboBox = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            comboBox.Items.AddRange(new string[]
            {
                "Mesai Bitimi",
                "İş değişimi(Tanımlı)",
                "Eğitim",
                "Toplantı",
                "Diğer"
            });

            TextBox textBox = new TextBox
            {
                Location = new Point(20, 60),
                Width = 200,
                Visible = false  // Başlangıçta görünmez
            };

            Button onayButton = new Button
            {
                Text = "Onay",
                Location = new Point(100, 100),
                Size = new Size(60, 30),
            };

            onayButton.Click += (s, e) =>
            {
                string selectedReason = comboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedReason))
                {
                    MessageBox.Show($"Ara verme sebebi: {selectedReason}\nAra vermek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo);

                    // ComboBox'lardan seçilen değerleri alın
                    string staffName = staffname; // Bu değişkeni nereden alacaksınız?
                    string projectName = textBox2.Text.ToString();
                    string functionName = textBox3.Text.ToString();
                    string moduleName = textBox4.Text.ToString();

                    if (!string.IsNullOrEmpty(staffName) &&
                        !string.IsNullOrEmpty(projectName) &&
                        !string.IsNullOrEmpty(functionName) &&
                        !string.IsNullOrEmpty(moduleName))
                    {
                        // Status tablosuna yeni bir kayıt ekleyin
                        Status newStatus = new Status
                        {
                            StaffName = staffName,
                            ProjectName = projectName,
                            FunctionName = functionName,
                            ModuleName = moduleName,
                            CategoryTime = kolon4Verisi, // Bu değişkeni nereden alacaksınız?
                            StatusName = "Araver",
                            StatusTime = DateTime.Now,
                            popup = comboBox.Text,
                            ModuleTip = moduleTip // Bu değişkeni nereden alacaksınız?
                        };

                        dbContext.status.Add(newStatus);
                        dbContext.SaveChanges();

                        MessageBox.Show("Araverildi.");
                        UpdateButtonVisibleState();
                        yükle();
                    }
                }

                sebepSecimiForm.Close();
            };

            comboBox.SelectedIndexChanged += (sender, e) =>
            {
                // "Diğer" seçildiğinde TextBox'ı görünür yap, diğer durumda gizle
                textBox.Visible = comboBox.SelectedItem?.ToString() == "Diğer";
            };

            sebepSecimiForm.Controls.AddRange(new Control[] { comboBox, textBox, onayButton });

            sebepSecimiForm.ShowDialog();


        }

        private void button3_Click(object sender, EventArgs e)
        {

            // ComboBox'lardan seçilen değerleri alın
            string staffName = staffname;
            string projectName = textBox2.Text;
            string functionName = textBox3.Text;
            string moduleName = textBox4.Text;

            if (!string.IsNullOrEmpty(staffName) &&
                !string.IsNullOrEmpty(projectName) &&
                !string.IsNullOrEmpty(functionName) &&
                !string.IsNullOrEmpty(moduleName))
            {
                using (var dbContext = new MyDbContext())
                {
                    // Status tablosuna yeni bir kayıt ekleyin
                    Status newStatus = new Status
                    {
                        StaffName = staffName,
                        ProjectName = projectName,
                        FunctionName = functionName,
                        ModuleName = moduleName,
                        CategoryTime = kolon4Verisi,
                        StatusName = "Bitti",
                        StatusTime = DateTime.Now,
                        ModuleTip = moduleTip
                    };

                    dbContext.status.Add(newStatus);
                    dbContext.SaveChanges();

                    MessageBox.Show("Bitirildi.");
                    UpdateButtonVisibleState();
                }

                using (var dbContext = new MyDbContext())
                {
                    var assignmentToUpdate = dbContext.assignments
                        .FirstOrDefault(a => a.StaffName == staffName &&
                                             a.ProjectName == projectName &&
                                             a.FunctionName == functionName &&
                                             a.ModuleName == moduleName);
                    if (assignmentToUpdate != null)
                    {

                        //  Status alanlarını güncelle
                        assignmentToUpdate.Status = "False";

                        // Null değerleri atamak için NullReferenceException hatasını önlemek için kontrol eklemeye gerek yok
                        dbContext.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm gerekli alanları doldurun.");
            }
            yükle();
        }

        private void UpdateButtonVisibleState()
        {

            using (var dbContext = new MyDbContext())
            {
                string staffName = staffname;
                string selectedProjectName = textBox2.Text;
                string selectedFunctionName = textBox3.Text;
                string selectedModuleName = textBox4.Text;

                if (staffName != null && selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null && moduleTip!=null)
                {
                    // Seçilen verilere göre Status tablosundaki en son StatusTime'ı al
                    var lastStatusTime = dbContext.status
                        .Where(s =>
                            s.ProjectName == selectedProjectName &&
                            s.FunctionName == selectedFunctionName &&
                            s.ModuleName == selectedModuleName &&
                            s.StaffName == staffName&&s.ModuleTip==moduleTip)
                        .OrderByDescending(s => s.StatusTime).Select(s => s.StatusName)
                        .FirstOrDefault();

                    if (lastStatusTime != null)
                    {
                        // En son StatusTime'a göre StatusName'i al
                        var lastStatus = dbContext.status
                            .Where(s =>
                                s.ProjectName == selectedProjectName &&
                                s.FunctionName == selectedFunctionName &&
                                s.ModuleName == selectedModuleName &&
                                s.StaffName == staffName&&s.ModuleTip==moduleTip) // Burada .StatusTime ile karşılaştırıyoruz
                            .Select(s => s.StatusName)
                            .FirstOrDefault();

                        // StatusName'e göre düğme durumlarını ayarla
                        if (lastStatusTime == "Bitti")
                        {
                            // "Bitti" durumunda tüm düğmeler gizlenmelidir
                            button1.Enabled = false;
                            button2.Enabled = false;
                            button3.Enabled = false;
                        }
                        else if (lastStatusTime == "Araver")
                        {
                            // "Araver" durumunda button2 ve button3 tıklanamaz
                            button1.Enabled = true; // Button1 etkin
                            button2.Enabled = false;
                            button3.Enabled = false;
                        }
                        else if (lastStatusTime == "Başla")
                        {
                            // "Başla" durumunda button1 tıklanamaz
                            button1.Enabled = false;
                            button2.Enabled = true; // Button2 ve Button3 etkin
                            button3.Enabled = true;
                        }
                    }
                    else
                    {
                        // Veri bulunamazsa tüm düğmeler etkin değil
                        button1.Enabled = true;
                        button2.Enabled = false;
                        button3.Enabled = false;
                    }
                }
                else
                {

                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                }
            }



        }



        private void KullaniciEkranı_Load(object sender, EventArgs e)
        {
            var Sorumlu = dbContext.staffs.Select(x => x.StaffName).ToList();
            Sorumlu.Insert(0, "Sorumlu Seçiniz..");
            comboBox1.DataSource = Sorumlu;


            UpdateButtonVisibleState();
            groupBox2.Visible = false;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                string selectedsystemname = selectedSystemName.ToString();
                string selectedProjectName = textBox2.Text.ToString();
                string selectedFunctionName = textBox3.Text.ToString();
                string selectedModuleName = textBox4.Text.ToString();
                string newModuleName = textBox1.Text;
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Seçilen ProjectName'e göre ProjectId'yi alın
                    int projectId = dbContext.projects
                        .Where(p => p.ProjectName == selectedProjectName)
                        .Select(p => p.ProjectId)
                        .FirstOrDefault();

                    if (projectId > 0)
                    {
                        // Seçilen FunctionName'e göre FunctionId'yi alın
                        int functionId = dbContext.functions
                            .Where(f => f.FunctionName == selectedFunctionName)
                            .Select(f => f.FunctionId)
                            .FirstOrDefault();

                        if (functionId > 0)
                        {
                            // Seçilen ModuleName'i güncellemek için ilgili Module kaydını bulun
                            var module = dbContext.modules
                                .FirstOrDefault(m => m.ModuleName == selectedModuleName && m.FunctionId == functionId && m.ProjectId == projectId);

                            if (module != null)
                            {
                                // TextBox1'den gelen veri ile ModuleName'i güncelleyin
                                module.ModuleName = textBox1.Text;

                                // Değişiklikleri veritabanına kaydedin
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Assignments tablosunda seçilen verilere göre ModuleName'i güncelle
                    var assignmentsToUpdate = dbContext.assignments
                        .Where(a => a.ProjectName == selectedProjectName &&
                                    a.FunctionName == selectedFunctionName &&
                                    a.ModuleName == selectedModuleName);

                    foreach (var assignment in assignmentsToUpdate)
                    {
                        assignment.ModuleName = textBox1.Text;
                    }

                    // Değişiklikleri veritabanına kaydet
                    dbContext.SaveChanges();
                }
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Assignments tablosunda seçilen verilere göre ModuleName'i güncelle
                    var RecordsToUpdate = dbContext.records
                        .Where(a => a.SystemName == selectedsystemname &&
                                    a.ProjectName == selectedProjectName &&
                                    a.FunctionName == selectedFunctionName &&
                                    a.ModuleName == selectedModuleName);

                    foreach (var records in RecordsToUpdate)
                    {
                        records.ModuleName = textBox1.Text;
                    }

                    // Değişiklikleri veritabanına kaydet
                    dbContext.SaveChanges();
                }
                if (selectedProjectName != null && selectedFunctionName != null && selectedModuleName != null)
                {
                    // Aynı ProjectName ve FunctionName'e sahip tüm ModuleName'leri al
                    var modulesToUpdate = dbContext.status
                        .Where(s => s.ProjectName == selectedProjectName &&
                                    s.FunctionName == selectedFunctionName)
                        .ToList();

                    foreach (var status in modulesToUpdate)
                    {
                        status.ModuleName = newModuleName;
                    }

                    // Değişiklikleri veritabanına kaydet
                    dbContext.SaveChanges();

                    MessageBox.Show("Değişiklikler Kaydedildi.");
                }


            }
        }



        private void advancedDataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satır tıklanmış mı kontrolü
            {
                DataGridViewRow selectedRow = advancedDataGridView1.Rows[e.RowIndex];

                // Eğer DataGridView'da daha fazla sütununuz varsa, sütun indekslerini düzenleyin
                string kolon1Verisi = selectedRow.Cells["Proje Numarası"].Value.ToString();
                string kolon2Verisi = selectedRow.Cells["Fonksiyon Numarası"].Value.ToString();
                string kolon3Verisi = selectedRow.Cells["Modül Numarası"].Value.ToString();
                kolon4Verisi = Convert.ToInt32(selectedRow.Cells["Kategori Zamanı"].Value.ToString());
                staffname = selectedRow.Cells["Personel Adı"].Value.ToString();
                moduleTip = selectedRow.Cells["Modül Türü"].Value.ToString();
                selectedSystemName = selectedRow.Cells["Sistem Numarası"].Value.ToString();
                // TextBox'lara verileri yaz
                textBox2.Text = kolon1Verisi;
                textBox3.Text = kolon2Verisi;
                textBox4.Text = kolon3Verisi;

                UpdateButtonVisibleState();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            yükle();
        }

        private void advancedDataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // Satırın üzerindeki verileri kontrol et
            if (e.RowIndex >= 0 && e.RowIndex < advancedDataGridView1.Rows.Count)
            {
                DataGridViewRow row = advancedDataGridView1.Rows[e.RowIndex];

                // İlgili sütunun değerini al (örneğin, "Durum" sütunu)
                string durum = row.Cells["Durum"].Value.ToString();

                // İstediğiniz duruma göre satırı özelleştir
                if (durum == "Devam Ediyor...")
                {
                    // Satırın arkaplan rengini mavi yap
                    row.DefaultCellStyle.BackColor = Color.Blue;

                    // Satırın metin rengini beyaz yap
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }
    }
}
