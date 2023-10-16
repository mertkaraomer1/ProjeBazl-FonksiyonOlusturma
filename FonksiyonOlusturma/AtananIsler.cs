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
            using (var dbContext = new MyDbContext())
            {
                var statusList = dbContext.assignments
                    .Select(status => new
                    {
                        Sorumlu = status.StaffName,
                        Proje = status.ProjectName,
                        Fonksiyon = status.FunctionName,
                        Modül = status.ModuleName,
                        StatusName = status.CategoryName,
                        StatusTime = status.CategoryTime,
                        GeçenZaman = (DateTime.Now - status.CategoryTime).ToString(@"hh\:mm\:ss")
                    })
                    .ToList();

                // DataGridView'e verileri eklemek için aşağıdaki kodu kullanabilirsiniz (dataGridView1 DataGridView nesnesi adınıza göre değiştirilmelidir).
                dataGridView1.DataSource = statusList;
            }
        }
    }
}
