using System;
using System.Data;
using System.Data.OleDb;

namespace Middleware.Controller
{
    public class VisualFoxProConnector
    {
        private const string Provider = "VFPOLEDB.1";
        private const string DataSource = "C:\\DRYCONA4\\";

        private DataTable GetYourData()
        {
            var resultSet = new DataTable();
            
            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={DataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return resultSet;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    var query = "select * from sesion where tipo='I' order by numero";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            return resultSet;
        }
    }
}
