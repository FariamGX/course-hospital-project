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
    enum RoWState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form1 : Form
    {
        private readonly CheckUser _user;

        DataBase dataBase = new DataBase();

        int selectedRow;


        public Form1(CheckUser user)
        {
            _user = user;
            InitializeComponent();

        }

        private void isAdmin()
        {
            administrationToolStripMenuItem.Enabled = _user.IsAdmin;
            buttonNewEntry.Enabled = _user.IsAdmin;
            buttonEdit.Enabled = _user.IsAdmin;
            buttonDelete.Enabled = _user.IsAdmin;
        }

        private void CreateColums()
        {
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("UserName", "UserName");
            dataGridView1.Columns.Add("UserSurname", "UserSurname");
            dataGridView1.Columns.Add("Health", "Health");
            dataGridView1.Columns.Add("PhoneNumber", "PhoneNumber");

        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4) , RoWState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Patient";

            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }

        private void Update()
        {
            dataBase.openConnection();

            for(int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RoWState)dataGridView1.Rows[index].Cells[5].Value;

                if (rowState == RoWState.Existed)
                    continue;


                if(rowState == RoWState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);

                    var deleteQuery = $"delete from Patient where id ={id}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();

                }
                if(rowState ==RoWState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var count = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var Frange = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"update Patient set UserName = '{name}', UserSurname = '{count}', PhoneNumber = '{Frange}', Health = '{price}' where Id = '{id}' ";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }


            }

            dataBase.closeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tlsUserStatus.Text = $"{_user.login} : {_user.Status}";
            isAdmin();
            CreateColums();
            RefreshDataGrid(dataGridView1);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBoxID.Text = row.Cells[0].Value.ToString();
                textBoxName.Text = row.Cells[1].Value.ToString(); 
                textBoxCount.Text = row.Cells[2].Value.ToString();
                textBoxFRange.Text = row.Cells[3].Value.ToString();
                textBoxPrice.Text = row.Cells[4].Value.ToString();

            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void buttonNewEntry_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void Search (DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from Patient where concat (Id, UserName, UserSurname, PhoneNumber, Health) like '%" + textBoxSearch.Text +"%'";

            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();

            while(read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();

        }


        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            //if(dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            //{
            //    dataGridView1.Rows[index].Cells[5].Value = RoWState.Deleted;
            //    return;
            //}

            dataGridView1.Rows[index].Cells[4].Value = RoWState.Deleted;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBoxID.Text;
            var name = textBoxName.Text;
            var count = textBoxCount.Text;
            var Frange = textBoxFRange.Text;
            int PhoneNumber;

            if(dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if(int.TryParse(textBoxPrice.Text, out PhoneNumber))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, name, count, Frange, PhoneNumber);
                    dataGridView1.Rows[selectedRowIndex].Cells[4].Value = RoWState.Modified;

                }
                else
                {
                    MessageBox.Show("The PhoneNumber field must be a number");
                }

            }

        }


        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void ClearFields()
        {
            textBoxID.Text = " ";
            textBoxName.Text = " ";
            textBoxCount.Text = " ";
            textBoxFRange.Text = " ";
            textBoxPrice.Text = " ";
        }

        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }


}
