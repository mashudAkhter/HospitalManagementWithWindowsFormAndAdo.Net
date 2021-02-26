using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace WinFormCRUDProject
{
    public partial class Login : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin ulog = new UserLogin();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT count(*) FROM Registration WHERE Username = '"+txtUsername.Text+"' AND Password = '"+txtPassword.Text+"'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Login Successfully");
                    
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            
        }
    }
}
