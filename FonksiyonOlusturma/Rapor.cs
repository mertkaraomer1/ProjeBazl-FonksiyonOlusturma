using FonksiyonOlusturma.MyDb;
using System.Data;

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

            var query = dbContext.assignments
                .Select(a => new
                {
                    SystemName = a.SystemName,
                    ProjectName = a.ProjectName,
                    FunctionName = a.FunctionName,
                    ModuleName = a.ModuleName,
                    ModuleDesciription=a.ModuleDescription,
                    ModuleTip = a.ModuleTip,
                    CategoryTime = a.CategoryTime,
                    StaffName = a.StaffName,
                    CategoryName = a.CategoryName,
                    Status = a.Status,
                })
                .ToList();

            foreach (var item in query)
            {
                TimeSpan totalDifference = TimeSpan.Zero;
                TimeSpan totalDifference1 = TimeSpan.Zero;
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
                                 a.ModuleTip == item.ModuleTip )
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
                    if (enSonDurum1.Count > 0)
                    {


                        // Find the index of the first "Araver" status
                        int startIndex = enSonDurum1.FindIndex(a => a.durumAdı == "Araver");

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

                    if (ilkBaslaDurumu != null)
                    {
                        zamanFarki = DateTime.Now - ilkBaslaDurumu.durumZamanı;

                    }
                    double AraverSuresi = (totalDifference + totalDifference1).TotalMinutes;
                    double ToplamÇalışmaSuresiDuble = zamanFarki.TotalMinutes;
                    int ToplamÇalışmaSuresi = Convert.ToInt32(ToplamÇalışmaSuresiDuble - AraverSuresi);
                    int toplamDakika2 = ToplamÇalışmaSuresi;
                    int saatler2 = toplamDakika2 / 60;
                    int dakikalar2 = toplamDakika2 % 60;
                    string TopÇalSure = $"{saatler2:D2}:{dakikalar2:D2}";
                    string BaslamaTarihi=ilkBaslaDurumu.durumZamanı.ToString("dd.MM.yyyy");
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
    }
}
