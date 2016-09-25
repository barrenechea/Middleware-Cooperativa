using System.Diagnostics;
using System.IO;

namespace Middleware.Controller
{

    public class VFPMonController
    {
        private IniParser _parser;
        public string Drive { get; private set; }
        public string DbFolder { get; private set; }
        private FileSystemWatcher _foxProWatcher;
        public bool IsRunning { get; private set; }

        public VFPMonController()
        {
            IniRead();
            IsRunning = false;
        }
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
                using (var writer = new StreamWriter("config.ini"))
                {
                    writer.WriteLine("[MIDDLEWARE]");
                    writer.WriteLine("DRIVE=");
                    writer.WriteLine("DBFOLDER=");
                }
                Drive = string.Empty;
                DbFolder = string.Empty;
                _parser = new IniParser("config.ini");
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
            return Start();
        }

        public bool Start()
        {
            if (!ValidateConfig()) return false;
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

            }
            catch
            {
                return false;
            }

            return true;
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name.ToLower().Contains("sesion"))
            {
                Debug.WriteLine($"Cambio en el archivo {e.Name}");
                //Todo sesion.dbf logic
            }
            else if (e.Name.ToLower().Contains("tabanco"))
            {
                Debug.WriteLine($"Cambio en el archivo {e.Name}");
                //Todo tabanco.dbf logic
            }
            else if (e.Name.ToLower().Contains("tabaux10"))
            {
                Debug.WriteLine($"Cambio en el archivo {e.Name}");
                //Todo tabaux10.dbf logic
            }
            else if (e.Name.ToLower().Contains("mae_cue"))
            {
                Debug.WriteLine($"Cambio en el archivo {e.Name}");
                //Todo mae_cue.dbf logic
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _foxProWatcher.EnableRaisingEvents = false;
            _foxProWatcher.Dispose();
            IsRunning = false;
        }
    }
}
