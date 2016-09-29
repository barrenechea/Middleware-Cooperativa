using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Middleware.Models.Local;

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
        public static List<VFPSesion> GetSesionList()
        {
            var list = new List<VFPSesion>();
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return list;

                using (var dataAdapter = new OleDbDataAdapter())
                {
                    const string query = "select * from sesion order by numero";

                    using (var dbCommand = new OleDbCommand(query, dbConnection))
                    {
                        dataAdapter.SelectCommand = dbCommand;
                        dataAdapter.Fill(resultSet);
                    }
                }
                dbConnection.Close();
            }
            _parser = null;
            
            for (var i = 0; i < resultSet.Rows.Count; i++)
            {
                var sesion = new VFPSesion
                {
                    tipo = Convert.ToChar(resultSet.Rows[i]["TIPO"]),
                    numero = Convert.ToInt32(resultSet.Rows[i]["NUMERO"]),
                    correl = Convert.ToInt32(resultSet.Rows[i]["CORREL"]),
                    va_ifrs = Convert.ToInt32(resultSet.Rows[i]["VA_IFRS"]),
                    canbco = Convert.ToInt32(resultSet.Rows[i]["CANBCO"]),
                    banco = resultSet.Rows[i]["BANCO"].ToString().Trim(),
                    cuenta = resultSet.Rows[i]["CUENTA"].ToString().Trim(),
                    cheque = Convert.ToInt32(resultSet.Rows[i]["CHEQUE"]),
                    fecha = Convert.ToDateTime(resultSet.Rows[i]["FECHA"]).ToString("yyyy-MM-dd"),
                    glosa = resultSet.Rows[i]["GLOSA"].ToString().Trim(),
                    benefi = resultSet.Rows[i]["BENEFI"].ToString().Trim(),
                    fechach = Convert.ToDateTime(resultSet.Rows[i]["FECHACH"]).ToString("yyyy-MM-dd"),
                    area = resultSet.Rows[i]["AREA"].ToString().Trim(),
                    linea = Convert.ToInt32(resultSet.Rows[i]["LINEA"]),
                    codigo = resultSet.Rows[i]["CODIGO"].ToString().Trim(),
                    tipdoc = resultSet.Rows[i]["TIPDOC"].ToString().Trim(),
                    fechafac = Convert.ToDateTime(resultSet.Rows[i]["FECHAFAC"]).ToString("yyyy-MM-dd"),
                    fac = Convert.ToInt32(resultSet.Rows[i]["FAC"]),
                    corrfac = Convert.ToInt32(resultSet.Rows[i]["CORRFAC"]),
                    detalle1 = resultSet.Rows[i]["DETALLE1"].ToString().Trim(),
                    detalle2 = resultSet.Rows[i]["DETALLE2"].ToString().Trim(),
                    detalle3 = resultSet.Rows[i]["DETALLE3"].ToString().Trim(),
                    detalle4 = resultSet.Rows[i]["DETALLE4"].ToString().Trim(),
                    imp = Convert.ToChar(resultSet.Rows[i]["IMP"]),
                    debe = Convert.ToInt32(resultSet.Rows[i]["DEBE"]),
                    haber = Convert.ToInt32(resultSet.Rows[i]["HABER"]),
                    estado = Convert.ToChar(resultSet.Rows[i]["ESTADO"])
                };

                list.Add(sesion);
            }

            return list;
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
