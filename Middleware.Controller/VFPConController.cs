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
                    tipo = resultSet.Rows[i]["TIPO"].ToString().Trim(),
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
                    imp = resultSet.Rows[i]["IMP"].ToString().Trim(),
                    debe = Convert.ToInt32(resultSet.Rows[i]["DEBE"]),
                    haber = Convert.ToInt32(resultSet.Rows[i]["HABER"]),
                    estado = resultSet.Rows[i]["ESTADO"].ToString().Trim()
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
        public static List<VFPMaeCue> GetMaeCueList()
        {

            var list = new List<VFPMaeCue>();
            var resultSet = new DataTable();
            _parser = new IniParser("config.ini");
            var dataSource = $"{_parser.GetSetting("MIDDLEWARE", "DRIVE")}DRYSOFT\\DRYCON\\{_parser.GetSetting("MIDDLEWARE", "DBFOLDER")}";

            using (var dbConnection = new OleDbConnection($"Provider={Provider};Data Source={dataSource};"))
            {
                dbConnection.Open();

                if (dbConnection.State != ConnectionState.Open) return list;

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

            for (var i = 0; i < resultSet.Rows.Count; i++)
            {
                var maecue = new VFPMaeCue
                {
                    codigo = resultSet.Rows[i]["CODIGO"].ToString().Trim(),
                    codigofecu = resultSet.Rows[i]["CODIGOFECU"].ToString().Trim(),
                    clase = resultSet.Rows[i]["CLASE"].ToString().Trim(),
                    nivel = resultSet.Rows[i]["NIVEL"].ToString().Trim(),
                    subcta = resultSet.Rows[i]["SUBCTA"].ToString().Trim(),
                    ctacte = resultSet.Rows[i]["CTACTE"].ToString().Trim(),
                    ctacte2 = resultSet.Rows[i]["CTACTE2"].ToString().Trim(),
                    ctacte3 = resultSet.Rows[i]["CTACTE3"].ToString().Trim(),
                    ctacte4 = resultSet.Rows[i]["CTACTE4"].ToString().Trim(),
                    estado = resultSet.Rows[i]["ESTADO"].ToString().Trim(),
                    estado2 = resultSet.Rows[i]["ESTADO2"].ToString().Trim(),
                    nombre = resultSet.Rows[i]["NOMBRE"].ToString().Trim(),
                    codi = Convert.ToInt32(resultSet.Rows[i]["CODI"]),

                    debe0 = Convert.ToInt32(resultSet.Rows[i]["DEBE0"]),
                    haber0 = Convert.ToInt32(resultSet.Rows[i]["HABER0"]),
                    debe1 = Convert.ToInt32(resultSet.Rows[i]["DEBE1"]),
                    haber1 = Convert.ToInt32(resultSet.Rows[i]["HABER1"]),
                    debe2 = Convert.ToInt32(resultSet.Rows[i]["DEBE2"]),
                    haber2 = Convert.ToInt32(resultSet.Rows[i]["HABER2"]),
                    debe3 = Convert.ToInt32(resultSet.Rows[i]["DEBE3"]),
                    haber3 = Convert.ToInt32(resultSet.Rows[i]["HABER3"]),
                    debe4 = Convert.ToInt32(resultSet.Rows[i]["DEBE4"]),
                    haber4 = Convert.ToInt32(resultSet.Rows[i]["HABER4"]),
                    debe5 = Convert.ToInt32(resultSet.Rows[i]["DEBE5"]),
                    haber5 = Convert.ToInt32(resultSet.Rows[i]["HABER5"]),
                    debe6 = Convert.ToInt32(resultSet.Rows[i]["DEBE6"]),
                    haber6 = Convert.ToInt32(resultSet.Rows[i]["HABER6"]),
                    debe7 = Convert.ToInt32(resultSet.Rows[i]["DEBE7"]),
                    haber7 = Convert.ToInt32(resultSet.Rows[i]["HABER7"]),
                    debe8 = Convert.ToInt32(resultSet.Rows[i]["DEBE8"]),
                    haber8 = Convert.ToInt32(resultSet.Rows[i]["HABER8"]),
                    debe9 = Convert.ToInt32(resultSet.Rows[i]["DEBE9"]),
                    haber9 = Convert.ToInt32(resultSet.Rows[i]["HABER9"]),
                    debe10 = Convert.ToInt32(resultSet.Rows[i]["DEBE10"]),
                    haber10 = Convert.ToInt32(resultSet.Rows[i]["HABER10"]),
                    debe11 = Convert.ToInt32(resultSet.Rows[i]["DEBE11"]),
                    haber11 = Convert.ToInt32(resultSet.Rows[i]["HABER11"]),
                    debe12 = Convert.ToInt32(resultSet.Rows[i]["DEBE12"]),
                    haber12 = Convert.ToInt32(resultSet.Rows[i]["HABER12"]),

                    debep0 = Convert.ToInt32(resultSet.Rows[i]["DEBEP0"]),
                    haberp0 = Convert.ToInt32(resultSet.Rows[i]["HABERP0"]),
                    debep1 = Convert.ToInt32(resultSet.Rows[i]["DEBEP1"]),
                    haberp1 = Convert.ToInt32(resultSet.Rows[i]["HABERP1"]),
                    debep2 = Convert.ToInt32(resultSet.Rows[i]["DEBEP2"]),
                    haberp2 = Convert.ToInt32(resultSet.Rows[i]["HABERP2"]),
                    debep3 = Convert.ToInt32(resultSet.Rows[i]["DEBEP3"]),
                    haberp3 = Convert.ToInt32(resultSet.Rows[i]["HABERP3"]),
                    debep4 = Convert.ToInt32(resultSet.Rows[i]["DEBEP4"]),
                    haberp4 = Convert.ToInt32(resultSet.Rows[i]["HABERP4"]),
                    debep5 = Convert.ToInt32(resultSet.Rows[i]["DEBEP5"]),
                    haberp5 = Convert.ToInt32(resultSet.Rows[i]["HABERP5"]),
                    debep6 = Convert.ToInt32(resultSet.Rows[i]["DEBEP6"]),
                    haberp6 = Convert.ToInt32(resultSet.Rows[i]["HABERP6"]),
                    debep7 = Convert.ToInt32(resultSet.Rows[i]["DEBEP7"]),
                    haberp7 = Convert.ToInt32(resultSet.Rows[i]["HABERP7"]),
                    debep8 = Convert.ToInt32(resultSet.Rows[i]["DEBEP8"]),
                    haberp8 = Convert.ToInt32(resultSet.Rows[i]["HABERP8"]),
                    debep9 = Convert.ToInt32(resultSet.Rows[i]["DEBEP9"]),
                    haberp9 = Convert.ToInt32(resultSet.Rows[i]["HABERP9"]),
                    debep10 = Convert.ToInt32(resultSet.Rows[i]["DEBEP10"]),
                    haberp10 = Convert.ToInt32(resultSet.Rows[i]["HABERP10"]),
                    debep11 = Convert.ToInt32(resultSet.Rows[i]["DEBEP11"]),
                    haberp11 = Convert.ToInt32(resultSet.Rows[i]["HABERP11"]),
                    debep12 = Convert.ToInt32(resultSet.Rows[i]["DEBEP12"]),
                    haberp12 = Convert.ToInt32(resultSet.Rows[i]["HABERP12"]),
                };

                list.Add(maecue);
            }

            return list;
        }
        #endregion
    }
}
