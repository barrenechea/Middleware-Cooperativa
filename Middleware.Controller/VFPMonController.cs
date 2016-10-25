using System;
using System.IO;

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
        public string LastSync { get; private set; }
        private FileSystemWatcher _foxProWatcher;
        public bool IsRunning { get; private set; }
        #endregion
        #region Constructor
        public VFPMonController()
        {
            IniRead();
            IsRunning = false;
            LastSync = "En espera";
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
                    writer.WriteLine("URL=");
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

                CheckChanges(Table.Sesion);
                CheckChanges(Table.Tabanco);
                CheckChanges(Table.Tabaux10);
                CheckChanges(Table.Maecue);

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
                CheckChanges(Table.Sesion);

            }
            else if (e.Name.ToLower().Contains("tabanco"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                CheckChanges(Table.Tabanco);
                
            }
            else if (e.Name.ToLower().Contains("tabaux10"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                CheckChanges(Table.Tabaux10);
            }
            else if (e.Name.ToLower().Contains("mae_cue"))
            {
                Log.Add($"Cambio en el archivo {e.Name}");
                CheckChanges(Table.Maecue);
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
        private void CheckChanges(Table table)
        {
            switch (table)
            {
                case Table.Sesion:
                    #region Sesion
                    var apiSesions = RestController.GetSesion(DbFolder);

                    if (apiSesions == null)
                    {
                        LastSync = "Error de conectividad";
                        return;
                    }

                    var sesionList = VFPConController.GetSesionList();

                    var newObjects = ComparisonController.GetNewObjects(sesionList, apiSesions);
                    var updatedObjects = ComparisonController.GetUpdatedObjects(sesionList, apiSesions);
                    var deletedObjects = ComparisonController.GetDeletedObjects(sesionList, apiSesions);

                    foreach (var obj in newObjects)
                    {
                        if (RestController.PostSesion(obj, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to insert object {obj.numero} in Sesion");
                    }

                    foreach (var obj in updatedObjects)
                    {
                        if (RestController.PutSesion(obj.Item1, obj.Item2, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to update object {obj.Item1} in Sesion");
                    }

                    foreach (var obj in deletedObjects)
                    {
                        if (RestController.DeleteSesion(obj.id))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to delete object {obj.id} in Sesion");
                    }
                    break;
                #endregion
                case Table.Tabanco:
                    #region Tabanco
                    var tabanco = RestController.GetTabanco(DbFolder);

                    if (tabanco == null)
                    {
                        LastSync = "Error de conectividad";
                        return;
                    }

                    var tabancoList = VFPConController.GetTabancoList();

                    var objects = ComparisonController.GetNewObjects(tabancoList, tabanco);
                    var tuples = ComparisonController.GetUpdatedObjects(tabancoList, tabanco);
                    var apiTabancos = ComparisonController.GetDeletedObjects(tabancoList, tabanco);

                    foreach (var obj in objects)
                    {
                        if (RestController.PostTabanco(obj, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to insert object {obj.codbanco} in Tabanco");
                    }

                    foreach (var obj in tuples)
                    {
                        if (RestController.PutTabanco(obj.Item1, obj.Item2, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to update object {obj.Item1} in Tabanco");
                    }

                    foreach (var obj in apiTabancos)
                    {
                        if (RestController.DeleteSesion(obj.id))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to delete object {obj.id} in Tabanco");
                    }
                    break;
                #endregion
                case Table.Tabaux10:
                    #region Tabaux10
                    var tabaux10 = RestController.GetTabaux10(DbFolder);

                    if (tabaux10 == null)
                    {
                        LastSync = "Error de conectividad";
                        return;
                    }

                    var tabaux10List = VFPConController.GetTabaux10List();

                    var objs = ComparisonController.GetNewObjects(tabaux10List, tabaux10);
                    var list = ComparisonController.GetUpdatedObjects(tabaux10List, tabaux10);
                    var apiTabaux10S = ComparisonController.GetDeletedObjects(tabaux10List, tabaux10);

                    foreach (var obj in objs)
                    {
                        if (RestController.PostTabaux10(obj, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to insert object {obj.kod} in Tabaux10");
                    }

                    foreach (var obj in list)
                    {
                        if (RestController.PutTabaux10(obj.Item1, obj.Item2, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to update object {obj.Item1} in Tabaux10");
                    }

                    foreach (var obj in apiTabaux10S)
                    {
                        if (RestController.DeleteSesion(obj.id))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to delete object {obj.id} in Tabaux10");
                    }
                    break;
                #endregion
                case Table.Maecue:
                    #region Maecue
                    var apiList = RestController.GetMaeCue(DbFolder);

                    if (apiList == null)
                    {
                        LastSync = "Error de conectividad";
                        return;
                    }

                    var localList = VFPConController.GetMaeCueList();

                    var news = ComparisonController.GetNewObjects(localList, apiList);
                    var updated = ComparisonController.GetUpdatedObjects(localList, apiList);
                    var deleted = ComparisonController.GetDeletedObjects(localList, apiList);

                    foreach (var obj in news)
                    {
                        if (RestController.PostMaeCue(obj, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to insert object {obj.codigo} in MaeCue");
                    }

                    foreach (var obj in updated)
                    {
                        if (RestController.PutMaeCue(obj.Item1, obj.Item2, DbFolder))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to update object {obj.Item1} in MaeCue");
                    }

                    foreach (var obj in deleted)
                    {
                        if (RestController.DeleteSesion(obj.id))
                            LastSync = DateTime.Now.ToLongTimeString();
                        else
                            Log.Add($"Failed to delete object {obj.id} in MaeCue");
                    }
                    break;
                    #endregion
            }
        }
        #endregion
    }
}

public enum Table
{
    Sesion, Tabanco, Tabaux10, Maecue
}