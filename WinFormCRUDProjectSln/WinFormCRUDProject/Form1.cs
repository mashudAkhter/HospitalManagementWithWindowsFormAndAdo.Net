using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCRUDProject
{
    public partial class Form1 : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            HideSubManue();
        }

        private void HideSubManue()
        {
            subManuePanel.Visible = false;
        }
        private void ShowSubManuePanel()
        {
            if (subManuePanel.Visible == false)
            {
                HideSubManue();
                subManuePanel.Visible = true;
                btnPatient.Visible = false;
                btnReport.Visible = false;
            }
            else
            {
                subManuePanel.Visible = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
            pnlLogin.Visible = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ShowSubManuePanel();
        }
        private Form activeFrom = null;
        private void openChildFormPanel(Form childForm)
        {
            if (activeFrom != null)
            {
                activeFrom.Close();
            }
                activeFrom = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(childForm);
                contentPanel.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();
            
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //openChildFormPanel(new Login());
            pnlLogin.Visible = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            openChildFormPanel(new Register());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            openChildFormPanel(new Doctors());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildFormPanel(new Patients());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            List<ViewReport> list = new List<ViewReport>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT PatientsName, Age, Diseases, DoctorsName, Designation FROM Patients JOIN Doctors ON Patients.DoctorsId = Doctors.DoctorsId";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ViewReport obj = new ViewReport();
                    obj.PatientName = dt.Rows[i]["PatientsName"].ToString();
                    obj.Age = Convert.ToInt32(dt.Rows[i]["Age"].ToString());
                    obj.Desises = dt.Rows[i]["Diseases"].ToString();
                    obj.DoctorName = dt.Rows[i]["DoctorsName"].ToString();
                    obj.Designation = dt.Rows[i]["Designation"].ToString();
                    list.Add(obj);
                }
            }

            using (Report frm = new Report(list))
            {
                frm.ShowDialog();
            }
            openChildFormPanel(new Report());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = false;
            pictureBox2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserLogin ulog = new UserLogin();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT count(*) FROM Registration WHERE Username = '" + txtUsername.Text + "' AND Password = '" + txtPassword.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Login Successfully");
                    btnPatient.Visible = true;
                    btnReport.Visible = true;
                    pnlLogin.Visible = false;
                    pictureBox2.Visible = true;
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
            clearLogData();
        }

        private void clearLogData()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
