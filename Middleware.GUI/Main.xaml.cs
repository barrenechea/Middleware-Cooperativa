using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using Hardcodet.Wpf.TaskbarNotification;
using Middleware.Controller;

namespace Middleware.GUI
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class Main
    {

        private readonly VFPMonController _vfpMonitor;
        public Main()
        {
            InitializeComponent();
            _vfpMonitor = new VFPMonController();
            TextVersion.Text = "Sincronizador de datos versión "+ Assembly.GetEntryAssembly().GetName().Version;

            PrepareTrayIcon();

            
            _vfpMonitor.Start();
            ShowBalloon();
            //WindowState = WindowState.Minimized;
            PrepareSettings();
        }

        private void PrepareTrayIcon()
        {
            TaskIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            var open = new MenuItem {Header = "Abrir panel"};
            var quit = new MenuItem {Header = "Salir"};
            open.Click += Open_Click;
            quit.Click += Quit_Click;
            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(open);
            contextMenu.Items.Add(quit);
            TaskIcon.ContextMenu = contextMenu;
        }

        private void PrepareSettings()
        {
            LblSettingsStatus.Content = string.Empty;
            var drives = Environment.GetLogicalDrives();

            foreach (var drive in drives)
            {
                CmbDrives.Items.Add(drive);
            }
        }

        private void ShowBalloon()
        {
            TaskIcon.ShowBalloonTip("Sincronizador de datos", "El sistema de sincronización se está ejecutando en segundo plano", BalloonIcon.Info);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _vfpMonitor.Stop();
            TaskIcon.Dispose();
            base.OnClosing(e);
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != WindowState.Minimized;
        }

        private void CmbDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Directory.Exists(CmbDrives.SelectedItem + "DRYSOFT\\DRYCON\\"))
            {
                LblSettingsStatus.Content = $"¡La unidad {CmbDrives.SelectedItem} no posee sistema Drysoft!";
                CmbDatabases.IsEnabled = false;
                CmbDatabases.SelectedIndex = -1;
            }
            else
            {
                LblSettingsStatus.Content = string.Empty;
                CmbDatabases.IsEnabled = true;
                CmbDatabases.Items.Clear();
                var dirs = Directory.GetDirectories(CmbDrives.SelectedItem + "DRYSOFT\\DRYCON\\", "DRYCON*");
                foreach (var dir in dirs)
                {
                    CmbDatabases.Items.Add($"{dir.Replace(CmbDrives.SelectedItem + "DRYSOFT\\DRYCON\\", string.Empty)}\\");
                }
            }
        }

        private void CmbDatabases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fullDirectory = CmbDrives.SelectedItem + "DRYSOFT\\DRYCON\\" + CmbDatabases.SelectedItem;
            if (File.Exists(fullDirectory + "sesion.dbf") && File.Exists(fullDirectory + "tabanco.dbf") &&
                File.Exists(fullDirectory + "tabaux10.dbf") && File.Exists(fullDirectory + "mae_cue.dbf"))
            {
                BtnSave.IsEnabled = true;
                LblSettingsStatus.Content = string.Empty;
            }
            else
            {
                BtnSave.IsEnabled = false;
                LblSettingsStatus.Content = "¡La carpeta seleccionada no parece ser válida!";
            }
        }

        private void BtnUpdateList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
