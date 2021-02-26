using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCRUDProject
{
    public partial class Report : Form
    {
        List<ViewReport> lst;
        public Report()
        {

        }
        public Report(List<ViewReport> objList)
        {
            InitializeComponent();
            lst = objList;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            CrystalReport1 rpt = new CrystalReport1();
            rpt.SetDataSource(lst);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }
    }
}
