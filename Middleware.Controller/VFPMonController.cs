using System.Diagnostics;
using System.IO;

namespace Middleware.Controller
{

    public class VFPMonController
    {
        private FileSystemWatcher _foxProWatcher;
        private string directory = "C:\\DRYCONA4\\";

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
