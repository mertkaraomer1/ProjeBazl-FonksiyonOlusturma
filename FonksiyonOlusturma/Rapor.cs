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
            table.Columns.Add("Ara Süresi");
            table.Columns.Add("Toplam Bitiş Süresi");

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

                    var latestAraVerStatus = dbContext.status
                        .Where(a => a.ProjectName == item.ProjectName &&
                                    a.FunctionName == item.FunctionName &&
                                    a.ModuleName == item.ModuleName &&
                                    a.ModuleTip == item.ModuleTip &&
                                    a.StatusName == "Araver")
                        .Select(a => new
                        {
                            statusTime = a.StatusTime
                        })
                        .OrderByDescending(a => a.statusTime)
                        .FirstOrDefault();

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

                    // Araver'deki zamanı al
                    DateTime araverTime = latestAraVerStatus?.statusTime ?? DateTime.MinValue;
                    TimeSpan timeDifference = latestStatusTime - araverTime;

                    // CategoryTime'ı günlere ve saatlere dönüştür
                    int categoryTimeInHours = item.CategoryTime;
                    int days = categoryTimeInHours / 24;
                    int remainingHours = categoryTimeInHours % 24;
                    string categoryTimeFormatted = (days > 0) ?
                        string.Format("{0} gün {1} saat", days, remainingHours) :
                        string.Format("{0} saat", remainingHours);


                    // Süreleri "gün saat dakika" biçimine dönüştür
                    string timeDifferenceString = string.Format("{0} gün {1} saat {2} dakika",
                        timeDifference.Days, timeDifference.Hours, timeDifference.Minutes);

                    // Bitti zamanını al
                    DateTime bittiTime = latestBittiStatus?.statusTime ?? DateTime.MinValue;

                    // Bitti zamanından bir sonraki günün başlangıcı olarak bir DateTime oluştur
                    DateTime nextDayStart = bittiTime.Date.AddDays(1);

                    // Günlerin doğru hesaplanabilmesi için farkı hesaplayın
                    TimeSpan bittiDifference = (latestBittiStatus != null) ? nextDayStart - latestStatusTime : TimeSpan.Zero;

                    // Süreleri "gün saat dakika" biçimine dönüştür
                    string bittiDifferenceString = (latestBittiStatus != null) ?
                        string.Format("{0} gün {1} saat {2} dakika",
                            bittiDifference.Days, bittiDifference.Hours, bittiDifference.Minutes) :
                        "Devam Ediyor...";

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
                            timeDifferenceString,
                            bittiDifferenceString
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
                            timeDifferenceString,
                            bittiDifferenceString
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
                            timeDifferenceString,
                            bittiDifferenceString
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
