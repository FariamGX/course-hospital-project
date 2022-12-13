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
    public partial class Add_Form : Form
    {
        DataBase dataBase = new DataBase();

        public Add_Form()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var name = textBoxCRName.Text;
            var count = textBoxCRCount.Text;
            var FlRange = textBoxCRFLRange.Text;
            int price;
            // можливо треба логічне і щоб перевірити чи є інтом count і FlRange
            if (int.TryParse(textBoxCRPrice.Text, out price))
            {
                var addQuery = $"insert into Patient (UserName, UserSurname, Health, PhoneNumber) values( '{name}', '{count}', '{FlRange}', '{price}')";

                var command = new SqlCommand(addQuery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("entry successfully created", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                MessageBox.Show("failed to create entry", "failed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            dataBase.closeConnection();

        }

        private void Add_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
