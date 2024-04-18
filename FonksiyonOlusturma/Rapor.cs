using FonksiyonOlusturma.MyDb;
using System.Data;
using System.Windows.Forms;

namespace FonksiyonOlusturma
{
    public partial class Rapor : Form
    {
        MyDbContext dbContext;
        public Rapor()
        {
            dbContext = new MyDbContext();
            InitializeComponent();

        }
        DataTable table = new DataTable();
        public void Treeview()
        {
            // Verileri çek ve işle (Eager Loading kullanarak)
            var systems = dbContext.systems
                .Select(s => new
                {
                    s.SystemId,
                    s.SystemName,
                    Projects = s.Projects.Select(p => new
                    {
                        p.ProjectId,
                        p.ProjectName,
                        Functions = p.Functions.Select(f => new
                        {
                            f.FunctionId,
                            f.FunctionName,
                            Modules = dbContext.modules
                                .Where(m => m.FunctionId == f.FunctionId)
                                .Select(m => m.ModuleName)
                                .ToList()
                        })
                    })
                })
                .ToList();

            // TreeView kontrolünü doldur
            treeView1.Nodes.Clear(); // Varolan düğümleri temizle

            foreach (var system in systems)
            {
                var systemNode = new TreeNode(system.SystemName);

                foreach (var project in system.Projects)
                {
                    var projectNode = new TreeNode(project.ProjectName);

                    foreach (var function in project.Functions)
                    {
                        var functionNode = new TreeNode(function.FunctionName);

                        foreach (var module in function.Modules)
                        {
                            var moduleNode = new TreeNode(module);
                            functionNode.Nodes.Add(moduleNode);
                        }

                        projectNode.Nodes.Add(functionNode);
                    }

                    systemNode.Nodes.Add(projectNode);
                }

                treeView1.Nodes.Add(systemNode);
            }
        }
        public void Yükle()
        {
            advancedDataGridView1.Columns.Clear();
            table.Rows.Clear();
            table.Columns.Clear();

            // Kolonları ekle
            table.Columns.Add("System Number");
            table.Columns.Add("Project Number");
            table.Columns.Add("Function Number");
            table.Columns.Add("Module Number");
            table.Columns.Add("Module Name");
            table.Columns.Add("Category Name");
            table.Columns.Add("Category Time");
            table.Columns.Add("Staff Name");
            table.Columns.Add("Module Tip");
            table.Columns.Add("Status");
            table.Columns.Add("Toplam Çalışma Süresi");
            table.Columns.Add("Başlama Tarihi");
            table.Columns.Add("Bitiş Tarihi");
            //table.Columns.Add("Performans");

            var query = dbContext.assignments
                .Select(a => new
                {
                    SystemName = a.SystemName,
                    ProjectName = a.ProjectName,
                    FunctionName = a.FunctionName,
                    ModuleName = a.ModuleName,
                    ModuleDesciription = a.ModuleDescription,
                    ModuleTip = a.ModuleTip,
                    CategoryTime = a.CategoryTime,
                    StaffName = a.StaffName,
                    CategoryName = a.CategoryName,
                    Status = a.Status,
                })
                .ToList();

            foreach (var item in query)
            {
                TimeSpan totalDifferenceBasla = TimeSpan.Zero;
                TimeSpan totalDifference = TimeSpan.Zero;
                TimeSpan zamanFarki = TimeSpan.Zero;
                var latestStatus = dbContext.status
                    .Where(a => a.ProjectName == item.ProjectName &&
                                a.FunctionName == item.FunctionName &&
                                a.ModuleName == item.ModuleName &&
                                a.ModuleTip == item.ModuleTip)
                    .Select(a => new
                    {
                        statusName = a.StatusName,
                        statusTime = a.StatusTime
                    })
                    .OrderByDescending(a => a.statusTime)
                    .FirstOrDefault();

                if (latestStatus != null)
                {
                    // Veri bulunduğunda en son statusun zamanını al
                    DateTime latestStatusTime = latestStatus.statusTime;



                    var latestBittiStatus = dbContext.status
                        .Where(a => a.ProjectName == item.ProjectName &&
                                    a.FunctionName == item.FunctionName &&
                                    a.ModuleName == item.ModuleName &&
                                    a.ModuleTip == item.ModuleTip &&
                                    a.StatusName == "Bitti")
                        .Select(a => new
                        {
                            statusTime = a.StatusTime
                        })
                        .OrderByDescending(a => a.statusTime)
                        .FirstOrDefault();

                    var ilkBaslaDurumu = dbContext.status
                     .Where(a => a.ProjectName == item.ProjectName &&
                                 a.FunctionName == item.FunctionName &&
                                 a.ModuleName == item.ModuleName &&
                                 a.ModuleTip == item.ModuleTip &&
                                 a.StatusName == "Başla")
                     .OrderBy(a => a.StatusTime)
                     .Select(a => new
                     {
                         durumAdı = a.StatusName,
                         durumZamanı = a.StatusTime
                     })
                     .FirstOrDefault();

                    var enSonDurum1 = dbContext.status
                     .Where(a => a.ProjectName == item.ProjectName &&
                                 a.FunctionName == item.FunctionName &&
                                 a.ModuleName == item.ModuleName &&
                                 a.ModuleTip == item.ModuleTip)
                     .Select(a => new
                     {
                         durumAdı = a.StatusName,
                         durumZamanı = a.StatusTime
                     })
                     .OrderBy(a => a.durumZamanı)
                     .ToList();

                    // CategoryTime'ı günlere ve saatlere dönüştür
                    int categoryTimeInHours = item.CategoryTime;
                    int days = categoryTimeInHours / 24;
                    int remainingHours = categoryTimeInHours % 24;
                    string categoryTimeFormatted = (days > 0) ?
                        string.Format("{0} gün {1} saat", days, remainingHours) :
                        string.Format("{0} saat", remainingHours);


                    // "Başla" durumunun indekslerini bulun
                    List<int> baslaIndexes = enSonDurum1.Select((value, index) => new { value, index })
                                                          .Where(pair => pair.value.durumAdı == "Başla")
                                                          .Select(pair => pair.index)
                                                          .ToList();

                    // "Başla" ile "Araver" veya "Bitti" arasındaki süreleri hesaplayın
                    foreach (int baslaIndex in baslaIndexes)
                    {
                        // "Başla" durumunun ardından gelen ilk "Araver" veya "Bitti" durumunun indeksini bulun
                        int nextAraverOrBittiIndex = enSonDurum1.FindIndex(baslaIndex + 1, a => a.durumAdı == "Araver" || a.durumAdı == "Bitti");

                        // "Başla" ile "Araver" veya "Bitti" arasındaki zaman farkını hesaplayın ve toplam süreye ekleyin
                        if (nextAraverOrBittiIndex != -1)
                        {
                            TimeSpan difference = enSonDurum1[nextAraverOrBittiIndex].durumZamanı - enSonDurum1[baslaIndex].durumZamanı;
                            totalDifferenceBasla += difference;
                        }
                    }
                    if (enSonDurum1.Count > 0)
                    {
                        // İlk "Araver" durumunun dizindeki indeksini bulun
                        int startIndex = enSonDurum1.FindIndex(a => a.durumAdı == "Araver");

                        // "Araver" durumunun bulunup bulunmadığını kontrol edin
                        if (startIndex != -1)
                        {
                            // "Araver" durumundan başlayarak her durumu tek tek kontrol edin
                            for (int i = startIndex; i < enSonDurum1.Count; i++)
                            {
                                var statusItem = enSonDurum1[i];

                                // Mevcut durum "Araver" ise atlayın
                                if (statusItem.durumAdı == "Araver")
                                {
                                    continue; // "Araver" durumunu atlayın
                                }
                                else if (statusItem.durumAdı == "Başla")
                                {
                                    // "Araver" durumundan sonra "Başla" durumu varsa
                                    // "Araver" ile "Başla" arasındaki zaman farkını hesaplayın
                                    int nextBaslaIndex = enSonDurum1.FindIndex(i + 1, a => a.durumAdı == "Başla");

                                    // "Başla" durumu "Araver" durumundan sonra varsa
                                    if (nextBaslaIndex != -1)
                                    {
                                        // "Başla" ile "Araver" arasındaki zaman farkını hesaplayın ve toplam süreye ekleyin
                                        TimeSpan difference = enSonDurum1[nextBaslaIndex].durumZamanı - statusItem.durumZamanı;
                                        totalDifference += difference;
                                    }
                                    else
                                    {
                                        // "Başla" durumu "Araver" durumundan sonra yoksa, şu anki zamanı kullanarak farkı hesaplayın
                                        TimeSpan difference = DateTime.Now - statusItem.durumZamanı;
                                        totalDifference += difference;
                                    }
                                }
                            }
                        }
                    }
                    if (ilkBaslaDurumu != null && latestBittiStatus == null)
                    {
                        zamanFarki = DateTime.Now - ilkBaslaDurumu.durumZamanı;

                    }
                    else if (latestBittiStatus != null)
                    {
                        zamanFarki = latestBittiStatus.statusTime - ilkBaslaDurumu.durumZamanı;
                    }
                    //double performans;
                    //int CategoryMinutes;
                    double AraverSuresi = totalDifference.TotalMinutes;
                    double ToplamÇalışmaSuresiDuble = totalDifferenceBasla.TotalMinutes;
                    int ToplamÇalışmaSuresi = Convert.ToInt32(ToplamÇalışmaSuresiDuble);
                    //if (ToplamÇalışmaSuresi != 0)
                    //{
                    //    CategoryMinutes = item.CategoryTime * 60;
                    //    performans = (CategoryMinutes-ToplamÇalışmaSuresi)/CategoryMinutes;
                    //}
                    //else
                    //{
                    //    performans = double.MaxValue; // performans değerini en büyük double değeri olarak ayarlayabilirsiniz.
                    //}

                    int toplamDakika2 = ToplamÇalışmaSuresi;
                    int saatler2 = toplamDakika2 / 60; // Calculate hours
                    int dakikalar2 = toplamDakika2 % 60; // Calculate remaining minutes

                    int Gunler2 = 0;
                    if (saatler2 >= 24)
                    {
                        // Eğer saatler 8 saatten fazlaysa, günleri güncelle ve saatleri düzelt
                        Gunler2 = saatler2 / 24;
                        saatler2 %= 24;
                    }

                    string TopÇalSure = $"{Gunler2:D2} {saatler2:D2}:{dakikalar2:D2}";
                    string BaslamaTarihi = ilkBaslaDurumu.durumZamanı.ToString("dd.MM.yyyy");
                    string BitisTarihi = (latestBittiStatus != null && latestBittiStatus.statusTime != null)
                        ? latestBittiStatus.statusTime.ToString("dd.MM.yyyy")
                        : "Bitirilmedi..."; // Veya başka bir değer veya boş bir string
                    if (latestStatus.statusName == "Başla")
                    {
                        // Farkı datagridview'e yazdır
                        table.Rows.Add(
                            item.SystemName,
                            item.ProjectName,
                            item.FunctionName,
                            item.ModuleName,
                            item.ModuleDesciription,
                            item.CategoryName,
                            categoryTimeFormatted,
                            item.StaffName,
                            item.ModuleTip,
                            "Devam Ediyor...",
                            TopÇalSure,
                            BaslamaTarihi,
                            BitisTarihi
                            //performans
                        );
                    }
                    else if (latestStatus.statusName == "Araver")
                    {
                        // Farkı datagridview'e yazdır
                        table.Rows.Add(
                            item.SystemName,
                            item.ProjectName,
                            item.FunctionName,
                            item.ModuleName,
                            item.ModuleDesciription,
                            item.CategoryName,
                            categoryTimeFormatted,
                            item.StaffName,
                            item.ModuleTip,
                            "Ara Verildi...",
                            TopÇalSure,
                            BaslamaTarihi,
                            BitisTarihi
                            //performans
                        );
                    }
                    else if (latestStatus.statusName == "Bitti")
                    {
                        // Farkı datagridview'e yazdır
                        table.Rows.Add(
                            item.SystemName,
                            item.ProjectName,
                            item.FunctionName,
                            item.ModuleName,
                            item.ModuleDesciription,
                            item.CategoryName,
                            categoryTimeFormatted,
                            item.StaffName,
                            item.ModuleTip,
                            latestStatus.statusName,
                            TopÇalSure,
                            BaslamaTarihi,
                            BitisTarihi
                            //performans
                        );
                    }


                }
                else
                {
                    // CategoryTime'ı günlere ve saatlere dönüştür
                    int categoryTimeInHours = item.CategoryTime;
                    int days = categoryTimeInHours / 24;
                    int remainingHours = categoryTimeInHours % 24;
                    string categoryTimeFormatted = (days > 0) ?
                        string.Format("{0} gün {1} saat", days, remainingHours) :
                        string.Format("{0} saat", remainingHours);
                    // Veri bulunamadığında "Devam Ediyor" yazdır
                    table.Rows.Add(
                        item.SystemName,
                        item.ProjectName,
                        item.FunctionName,
                        item.ModuleName,
                        item.ModuleDesciription,
                        item.CategoryName,
                        categoryTimeFormatted,
                        item.StaffName,
                        item.ModuleTip,
                        "Başlanmadı...",
                        "Başlanmadı...",
                        "Başlanmadı...",
                        "Başlanmadı..."
                        //"Başlanmadı..."

                    );
                }
            }

            advancedDataGridView1.DataSource = table;
        }
        private void Rapor_Load(object sender, EventArgs e)
        {
            Yükle();
            Treeview();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // DataGridView'deki verileri bir DataTable'a kopyalayın
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn column in advancedDataGridView1.Columns)
            {
                // Eğer ValueType null ise, varsayılan bir veri türü kullanabilirsiniz.
                Type columnType = column.ValueType ?? typeof(string);
                dt.Columns.Add(column.HeaderText, columnType);
            }

            // Satırları ekle
            foreach (DataGridViewRow row in advancedDataGridView1.Rows)
            {
                DataRow dataRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dataRow);
            }

            // Excel uygulamasını başlatın
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true;

            // Yeni bir Excel çalışma kitabı oluşturun
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

            // DataTable'ı Excel çalışma sayfasına aktarın (tablo başlıklarını da dahil etmek için)
            int rowIndex = 1;

            // Başlıkları yaz
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                worksheet.Cells[1, j + 1] = dt.Columns[j].ColumnName;
                worksheet.Cells[1, j + 1].Font.Bold = true;
            }

            // Verileri yaz
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowIndex++;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[rowIndex, j + 1] = dt.Rows[i][j].ToString();
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Kullanıcı_Durumları KD=new Kullanıcı_Durumları();
            KD.Show();
        }
    }
}
