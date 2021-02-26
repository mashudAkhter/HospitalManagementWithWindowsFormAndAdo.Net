using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace WinFormCRUDProject
{
    public partial class Patients : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        int patId = 0;
        int docId = 0;
        string ImageLocation = "";
        public Patients()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Patients_Load(object sender, EventArgs e)
        {
            LoadGvDoctors();
            LoadPatientsInfo();
        }

        private void LoadPatientsInfo()
        {
            using(SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id, PatientsName, Age, Diseases, DoctorsName, Patients.DoctorsId, Designation, ImageName, ImageData FROM Patients JOIN Doctors ON Patients.DoctorsId = Doctors.DoctorsId";
                
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                con.Close();
                dgvPatientData.DataSource = dt;

                dgvPatientData.RowTemplate.Height = 80;
                DataGridViewImageColumn image = new DataGridViewImageColumn();
                image = (DataGridViewImageColumn)dgvPatientData.Columns[8];
                image.ImageLayout = DataGridViewImageCellLayout.Stretch;
            }
        }

        private void LoadGvDoctors()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Doctors";
                con.Open();
                DataTable dt = new DataTable();
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr, LoadOption.Upsert);
                con.Close();
                cmbDoctors.ValueMember = "DoctorsId";
                cmbDoctors.DisplayMember = "DoctorsName";
                cmbDoctors.DataSource = dt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;

            PatientsInfo pin = new PatientsInfo();
            pin.PatientName = txtPatientName.Text;
            pin.Age = Convert.ToInt32(txtAge.Text);
            pin.Diseases = TxtDiseases.Text;
            pin.DoctorsId = Convert.ToInt32(cmbDoctors.SelectedValue.ToString());

            pin.ImageName = ImageLocation;
            byte[] images = null;
            FileStream stream = new FileStream(ImageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader brdr = new BinaryReader(stream);
            images = brdr.ReadBytes((int)stream.Length);
            pin.ImageData = images;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Patients (PatientsName, Age, Diseases, DoctorsId, ImageName, ImageData) VALUES ('" + pin.PatientName + "', '" + pin.Age + "', '" + pin.Diseases + "', '" + pin.DoctorsId + "', '"+pin.ImageName+"', @img)";
                cmd.Parameters.Add(new SqlParameter("@img", pin.ImageData));
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count>0)
                {
                    MessageBox.Show("Data saved successfully!");
                }
                con.Close(); 
                LoadPatientsInfo();
                ClearAll();
            }
        }

        private void ClearAll()
        {
            txtPatientName.Text = "";
            txtAge.Text = "";
            TxtDiseases.Text = "";
            txtDoctorsName.Text = "";
            txtDesignation.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            PatientsInfo pin = new PatientsInfo();
            pin.PatientName = txtPatientName.Text;
            pin.Age = Convert.ToInt32(txtAge.Text);
            pin.Diseases = TxtDiseases.Text;
            pin.DoctorsId = Convert.ToInt32(cmbDoctors.SelectedValue.ToString());

            byte[] images = null;
            FileStream stream = new FileStream(ImageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader brdr = new BinaryReader(stream);
            images = brdr.ReadBytes((int)stream.Length);
            pin.ImageData = images;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Patients SET PatientsName = '"+pin.PatientName+"', Age='"+pin.Age+"', Diseases='"+pin.Diseases+"', DoctorsId = '"+pin.DoctorsId+"', ImageName = '"+pin.ImageName+"', ImageData = @img WHERE Id = '"+patId+"'";

                cmd.Parameters.Add(new SqlParameter("@img", pin.ImageData));
                dgvPatientData.RowTemplate.Height = 80;
                con.Open();
                count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    MessageBox.Show("Successfully Updated!");
                }
                con.Close();
            }
            LoadPatientsInfo();
            ClearAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            PatientsInfo pin = new PatientsInfo();
            int count = 0;
            using(SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Patients WHERE Id = '" + patId + "'";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Successfully Deleted");
                }
            }
            LoadPatientsInfo();
            ClearAll();
            patId = 0;
        }

        private void dgvPatientData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImageLocation = ofd.FileName.ToString();
                pictureBox1.ImageLocation = ImageLocation;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                lblImageLocation.Text = Path.GetFileName(ImageLocation);
            }
        }

        private void dgvPatientData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellId = e.RowIndex;
            DataGridViewRow row = dgvPatientData.Rows[cellId];
            
            try
            {
                patId = Convert.ToInt32(row.Cells[0].Value.ToString());
            }
            catch (Exception)
            {

                patId = 0;
            }

            try
            {
                docId = Convert.ToInt32(row.Cells[5].Value.ToString());
            }
            catch (Exception)
            {

                docId = 0;
            }

            txtPatientName.Text = row.Cells[1].Value.ToString();
            txtAge.Text = row.Cells[2].Value.ToString();
            TxtDiseases.Text = row.Cells[3].Value.ToString();
            txtDoctorsName.Text = row.Cells[4].Value.ToString();
            txtDesignation.Text = row.Cells[6].Value.ToString();
            cmbDoctors.SelectedValue = docId;

            byte[] data = (byte[])row.Cells[8].Value;
            MemoryStream stream = new MemoryStream(data);
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Doctors (DoctorsName, Designation) VALUES ('"+txtDoctorsName.Text+"', '"+txtDesignation.Text+"')";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Doctor Saved!");
                }
                con.Close();
            }
            LoadGvDoctors();
            ClearAll();
        }

        private void tbnUpdate_Click(object sender, EventArgs e)
        {
            PatientsInfo pin = new PatientsInfo();
            pin.DoctorsName = txtDoctorsName.Text;
            pin.Designation = txtDesignation.Text;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Doctors SET DoctorsName = '" + pin.DoctorsName + "', Designation = '" + pin.Designation + "' WHERE DoctorsId = '" + docId + "'";

                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Successfully Updated!");
                }
                con.Close();
            }
            LoadGvDoctors();
            LoadPatientsInfo();
            ClearAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Doctors WHERE DoctorsId = '" + docId + "'";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Doctors Deleted!");
                }
                con.Close();
            }
            LoadPatientsInfo();
            ClearAll();
        }
    }
}
