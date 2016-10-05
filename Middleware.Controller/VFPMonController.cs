using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Middleware.Models.Local;
using RestSharp;

namespace Middleware.Controller
{
    /// <summary>
    /// Visual FoxPro Monitor Controller
    /// </summary>
    public class VFPMonController
    {
        #region Attributes
        private IniParser _parser;
        public string Drive { get; private set; }
        public string DbFolder { get; private set; }
        private FileSystemWatcher _foxProWatcher;
        public bool IsRunning { get; private set; }
        #endregion
        #region Constructor
        public VFPMonController()
        {
            IniRead();
            IsRunning = false;
        }
        #endregion
        #region Methods
        private void IniRead()
        {
            if (File.Exists("config.ini"))
            {
                if(_parser == null) _parser = new IniParser("config.ini");
                Drive = _parser.GetSetting("MIDDLEWARE", "DRIVE");
                DbFolder = _parser.GetSetting("MIDDLEWARE", "DBFOLDER");
            }
            else
            {
                Log.Add("El archivo config.ini no existe");
                using (var writer = new StreamWriter("config.ini"))
                {
                    writer.WriteLine("[MIDDLEWARE]");
                    writer.WriteLine("DRIVE=");
                    writer.WriteLine("DBFOLDER=");
                }
                Drive = string.Empty;
                DbFolder = string.Empty;
                _parser = new IniParser("config.ini");
                Log.Add("Se creó el archivo config.ini");
            }
        }

        private bool ValidateConfig()
        {
            if (Drive.Equals(string.Empty) || DbFolder.Equals(string.Empty)) return false;

            return File.Exists($"{Drive}DRYSOFT\\DRYCON\\{DbFolder}sesion.dbf") && File.Exists($"{Drive}DRYSOFT\\DRYCON\\{DbFolder}tabanco.dbf") &&
                   File.Exists($"{Drive}DRYSOFT\\DRYCON\\{DbFolder}tabaux10.dbf") && File.Exists($"{Drive}DRYSOFT\\DRYCON\\{DbFolder}mae_cue.dbf");
        }

        public bool ChangeConfig(string drive, string folder)
        {
            if(IsRunning) { Stop();}
            _parser.AddSetting("MIDDLEWARE", "DRIVE", drive);
            _parser.AddSetting("MIDDLEWARE", "DBFOLDER", folder);
            _parser.SaveSettings();
            IniRead();
            Log.Add("Se actualizó el archivo config.ini");
            return Start();
        }

        public bool Start()
        {
            if (!ValidateConfig())
            {
                Log.Add($"Ocurrió un error al validar el directorio {Drive}DRYSOFT\\DRYCON\\{DbFolder}");
                return false;
            }
            if (IsRunning) return true;
            try
            {

                _foxProWatcher = new FileSystemWatcher
                {
                    Filter = "*.dbf",
                    Path = $"{Drive}DRYSOFT\\DRYCON\\{DbFolder}",
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };


                _foxProWatcher.Changed += FileChanged;
                _foxProWatcher.Created += FileChanged;
                _foxProWatcher.Deleted += FileChanged;
                _foxProWatcher.EnableRaisingEvents = true;
                IsRunning = true;
                Log.Add($"Se inició monitoreo al directorio {Drive}DRYSOFT\\DRYCON\\{DbFolder}");
                return true;
            }
            catch
            {
                Log.Add($"Ocurrió un error al intentar monitorear el directorio {Drive}DRYSOFT\\DRYCON\\{DbFolder}");
                return false;
            }
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name.ToLower().Contains("sesion"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                
                var localList = VFPConController.GetSesionList();
                var apiList = RestController.GetSesion(DbFolder);

                //Todo Comparison logic

                foreach (var sesion in localList)
                {
                    if (!RestController.PostSesion(sesion, DbFolder))
                    {
                        // Todo logic
                        throw new Exception();
                    }
                }
                
            }
            else if (e.Name.ToLower().Contains("tabanco"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                //Todo tabanco.dbf logic
            }
            else if (e.Name.ToLower().Contains("tabaux10"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                //Todo tabaux10.dbf logic
            }
            else if (e.Name.ToLower().Contains("mae_cue"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");

                var localList = VFPConController.GetMaeCueList();
                var apiList = RestController.GetMaeCue(DbFolder);

                //Todo Comparison logic

                foreach (var sesion in localList)
                {
                    if (!RestController.PostMaeCue(sesion, DbFolder))
                    {
                        // Todo logic
                        throw new Exception();
                    }
                }
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _foxProWatcher.EnableRaisingEvents = false;
            _foxProWatcher.Dispose();
            IsRunning = false;
            Log.Add($"Se detuvo monitoreo al directorio {Drive}DRYSOFT\\DRYCON\\{DbFolder}");
        }
        #endregion

        #region Datatable to List methods
        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            return (from DataRow row in dt.Rows select GetItem<T>(row)).ToList();
        }
        private T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion
    }
}