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
    public partial class sign_up : Form
    {

        DataBase dataBase = new DataBase();


        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void bCreate_an_account_Click(object sender, EventArgs e)
        {
            
            if(checkuser())
            {
                return;
            }

            var login = textBoxLogin.Text;
            var password = textBoxPassword.Text;

            string querystring = $"insert into Register(Userlogin, Password, IsAdmin) values('{login}', '{password}', 0)";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Account successfully created", "successful");

                log_in frm_login = new log_in();
                this.Hide();
                frm_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Account has not been created");
            }
            dataBase.closeConnection();
        }

        private Boolean checkuser()
        {
            var loginUser = textBoxLogin.Text;
            var passUser = textBoxPassword.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select Id, Userlogin, Password, IsAdmin from Register where Userlogin = '{loginUser}' and Password = '{passUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0) // Перевіряємо чи є такий користувач в БД
            {
                MessageBox.Show("User already exists");
                return true;
            }

            else
            {
                return false;
            }

        }


        private void sign_up_Load(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            textBoxLogin.MaxLength = 50;
            textBoxPassword.MaxLength = 50;
        }


        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox2.Visible = false;
        }
    }
}
