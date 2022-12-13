using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CopterCW
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-JNHN30Q\SQLEXPRESS;Initial Catalog=Hospital;Integrated Security=True");
  

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed) // Якщо зв'язок з БЗ закритий 
            {
                sqlConnection.Open();
            }
        }


        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open) // Якщо зв'язок з БЗ відкритий
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }


    }
}
