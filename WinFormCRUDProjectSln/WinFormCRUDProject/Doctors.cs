using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCRUDProject
{
    public partial class Doctors : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        int docId = 0;

        public Doctors()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Doctors_Load(object sender, EventArgs e)
        {
            LoadDoctorsInfo();
        }

        private void LoadDoctorsInfo()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Doctors";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                con.Close();
                dgvDoctorsData.DataSource = dt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;

                PatientsInfo pin = new PatientsInfo();
                pin.DoctorsName = txtDoctorsName.Text;
                pin.Designation = txtDesignation.Text;

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Doctors (DoctorsName, Designation) VALUES ('Md. Ahmed Sharif', 'Professor')";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Doctor Saved!");
                }
                LoadDoctorsInfo();
                ClearAll();
            }
        }

        private void ClearAll()
        {
            txtDoctorsName.Text = "";
            txtDesignation.Text = "";
        }

        private void tbnUpdate_Click(object sender, EventArgs e)
        {
            PatientsInfo pin = new PatientsInfo();
            pin.DoctorsName = txtDoctorsName.Text;
            pin.Designation = txtDoctorsName.Text;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Doctors SET DoctorsName = '"+pin.DoctorsName+"', Designation = '"+pin.Designation+"' WHERE DoctorsId = '"+pin.DoctorsId+"'";

                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Successfully Updated!");
                }
                con.Close();
                LoadDoctorsInfo();
                ClearAll();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                PatientsInfo pin = new PatientsInfo();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Doctors WHERE DoctorsId = '"+pin.DoctorsId+"'";
                con.Open();
                if (count > 0)
                {
                    MessageBox.Show("Doctors Deleted!");
                }
                con.Close();
                LoadDoctorsInfo();
                ClearAll();
            }
        }

        private void dgvDoctors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvDoctorsData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellId = e.RowIndex;
            DataGridViewRow row = dgvDoctorsData.Rows[cellId];

            try
            {
                docId = Convert.ToInt32(row.Cells[5].Value.ToString());
            }
            catch (Exception)
            {

                docId = 0;
            }

            txtDoctorsName.Text = row.Cells[1].Value.ToString();
            txtDesignation.Text = row.Cells[2].Value.ToString();
        }
    }
}
