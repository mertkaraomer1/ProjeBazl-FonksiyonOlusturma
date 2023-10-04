using FonksiyonOlusturma.MyDb;
using FonksiyonOlusturma.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FonksiyonOlusturma
{
    public partial class AtananIsler : Form
    {
        private MyDbContext dbContext;
        public AtananIsler()
        {
            dbContext = new MyDbContext();
            InitializeComponent();
        }

        private void AtananIsler_Load(object sender, EventArgs e)
        {
            using (var dbContext = new MyDbContext()) // DbContext'i kendi projenize göre değiştirin
            {
                var bittiModuleNames = dbContext.status
                    .Where(status => status.StatusName == "Bitti")
                    .Select(status => status.ModuleName)
                    .ToList();

                var statusList = dbContext.status
                    .Where(status => !bittiModuleNames.Contains(status.ModuleName))
                    .Select(status => new
                    {
                        StaffName = status.StaffName,
                        ProjectName = status.ProjectName,
                        FunctionName = status.FunctionName,
                        ModuleName = status.ModuleName,
                        StatusName = status.StatusName
                    })
                    .ToList();

                // DataGridView'e verileri eklemek için aşağıdaki kodu kullanabilirsiniz (dataGridView1 DataGridView nesnesi adınıza göre değiştirilmelidir).
                dataGridView1.DataSource = statusList;
            }



        }
    }
}
