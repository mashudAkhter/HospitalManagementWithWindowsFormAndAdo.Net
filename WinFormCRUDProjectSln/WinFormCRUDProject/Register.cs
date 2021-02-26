using Microsoft.Win32;
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
    public partial class Register : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public Register()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            int count = 0;
            UserLogin ul = new UserLogin();
            ul.Userrname = txtUsername.Text;
            ul.Password = txtPassword.Text;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Registration (Username, Password) VALUES ('"+ul.Userrname+"', '"+ul.Password+"')";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Data saved successfully!");
                }
                con.Close();
            }
            ClearRegData();
            this.Close();
        }

        private void ClearRegData()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
