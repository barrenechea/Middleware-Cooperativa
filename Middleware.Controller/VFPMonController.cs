using System.Diagnostics;
using System.IO;

namespace Middleware.Controller
{

    public class VFPMonController
    {
        private IniParser _parser;
        private string _drive;
        private string _dbFolder;
        private FileSystemWatcher _foxProWatcher;
        private string directory = "C:\\DRYCONA4\\";

        public VFPMonController()
        {
            IniRead();
        }

        private void IniRead()
        {
            if (File.Exists("config.ini"))
            {
                _parser = new IniParser("config.ini");
                _drive = _parser.GetSetting("MIDDLEWARE", "DRIVE");
                _dbFolder = _parser.GetSetting("MIDDLEWARE", "DBFOLDER");
            }
            else
            {
                using (var writer = new StreamWriter("config.ini"))
                {
                    writer.WriteLine("[MIDDLEWARE]");
                    writer.WriteLine("DRIVE=");
                    writer.WriteLine("DBFOLDER=");
                }
                _drive = string.Empty;
                _dbFolder = string.Empty;
                _parser = new IniParser("config.ini");
            }
        }
        public bool InitialCheck()
        {
            return !_drive.Equals(string.Empty) && !_dbFolder.Equals(string.Empty);
        }

        public void Start()
        {
            _foxProWatcher = new FileSystemWatcher
            {
                Filter = "*.dbf",
                Path = directory,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };


            _foxProWatcher.Changed += OnChanged;
            _foxProWatcher.Created += OnChanged;
            _foxProWatcher.Deleted += OnChanged;
            _foxProWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
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
            _foxProWatcher.EnableRaisingEvents = false;
            _foxProWatcher.Dispose();
        }
    }
}
