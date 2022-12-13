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

namespace CopterCW
{
    public partial class log_in : Form
    {
        DataBase database = new DataBase();

        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }


        private void log_in_Load(object sender, EventArgs e)
        {
            //textBoxPassword.PasswordChar = '*';
            pictureBox3.Visible = false;
            textBoxLogin.MaxLength = 50;
            textBoxPassword.MaxLength = 50;

        }


        private void bSign_in_Click(object sender, EventArgs e)
        {
            var loginUser = textBoxLogin.Text;
            var passUser = textBoxPassword.Text;


            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();


            //створюємо SQL запит
            string querystring = $"select Id, Userlogin, Password, IsAdmin from Register where Userlogin = '{loginUser}' and Password = '{passUser}' ";

            SqlCommand command = new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);



            if (table.Rows.Count ==1)
            {

                var user = new CheckUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));

                MessageBox.Show("Successful entry", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 frm1 = new Form1(user);
                this.Hide();
                frm1.ShowDialog();
                this.Show();


            }
            else
            {
                MessageBox.Show("This account does not exist!", "account does not exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            database.closeConnection();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up frm_sign = new sign_up();
            frm_sign.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox2.Visible = true;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox2.Visible = false;
        }
    }


}
