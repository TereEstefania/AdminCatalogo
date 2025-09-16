using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ConnectionDataBase
    {
        private SqlConnection connection;
        private SqlCommand command ;
        private SqlDataReader reader;

        public SqlDataReader Reader
        {
            get { return reader; }
        }

        public ConnectionDataBase()
        {
            connection = new SqlConnection("server=DESKTOP-OJ2J6KS; database=CATALOGO_DB; integrated security=true");
            command = new SqlCommand();
        }

        public void ExecuteRead()
        {
            command.Connection = connection;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            if (reader != null)
                reader.Close();
            connection.Close();
        }

        public void SetQuery(string query)
        {
            command.Parameters.Clear();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
        }

        public void SetParams(string name, object value)
        {
            command.Parameters.AddWithValue(name, value);            
        }


    }
}
