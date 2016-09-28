using System.Data;
using System.Data.OleDb;

namespace Middleware.Controller
{
    /// <summary>
    /// Visual FoxPro Connector Controller
    /// </summary>
    public static class VFPConController
    {
        #region Parameters
        private const string Provider = "VFPOLEDB.1";
        private static IniParser _parser;
        #endregion
        #region Methods
        public static DataTable GetSesionTable()
        {
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return resultSet;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    const string query = "select * from sesion order by tipo,numero";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            _parser = null;
            return resultSet;
        }
        public static DataTable GetTabancoTable()
        {
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return resultSet;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    const string query = "select * from tabanco order by codbanco";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            _parser = null;
            return resultSet;
        }
        public static DataTable GetTabaux10Table()
        {
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return resultSet;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    const string query = "select * from tabaux10 order by kod";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            _parser = null;
            return resultSet;
        }
        public static DataTable GetMaecueTable()
        {
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return resultSet;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    const string query = "select * from mae_cue order by codigo";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            _parser = null;
            return resultSet;
        }
        #endregion
    }
}
