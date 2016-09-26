using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using Hardcodet.Wpf.TaskbarNotification;
using MahApps.Metro.Controls.Dialogs;
using Middleware.Controller;

namespace Middleware.GUI
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class Main
    {
        #region Attributes
        private readonly VFPMonController _monitor;
        #endregion
        #region Constructor
        public Main()
        {
            InitializeComponent();
            _monitor = new VFPMonController();
            TextVersion.Text = "Sincronizador de datos versión "+ Assembly.GetEntryAssembly().GetName().Version;

            PrepareTrayIcon();
            
            ShowBalloon("El sistema de sincronización se está ejecutando en segundo plano");
        }
        #endregion
        #region Methods
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
            BtnSave.IsEnabled = false;
            CmbDrives.Items.Clear();
            var drives = Environment.GetLogicalDrives();
            
            foreach (var drive in drives)
            {
                CmbDrives.Items.Add(drive);
            }
            CmbDatabases.Items.Clear();
            LblSettingsStatus.Content = string.Empty;
        }

        private void ShowBalloon(string message)
        {
            TaskIcon.ShowBalloonTip("Sincronizador de datos", message, BalloonIcon.Info);
        }
        
        private async void ShowNormalDialog(string title, string message)
        {
            await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, new MetroDialogSettings { SuppressDefaultResources = true, CustomResourceDictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml") } });
        }
        #endregion
        #region Event Handler methods
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
            _monitor.Stop();
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
            PrepareSettings();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var drive = $"{CmbDrives.SelectedItem}";
            var folder = $"{CmbDatabases.SelectedItem}";
            if (!_monitor.ChangeConfig(drive, folder))
            {
                ShowNormalDialog("Error", "Hay un problema con la base de datos seleccionada.");
            }
            else
            {
                LblDrive.Content = _monitor.Drive;
                LblDbFolder.Content = _monitor.DbFolder;
                Tabs.SelectedIndex = 0;
                LblFoxProStatus.Content = _monitor.IsRunning ? "Operacional" : "Error";
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareSettings();
            if (!_monitor.Start()) ShowNormalDialog("Error", "Verifique su configuración");

            LblDrive.Content = _monitor.Drive;
            LblDbFolder.Content = _monitor.DbFolder;
            LblFoxProStatus.Content = _monitor.IsRunning ? "Operacional" : "Error";
            LblOnlineStatus.Content = "Detenido";
            //Todo eliminar estado online

            if (_monitor.IsRunning) WindowState = WindowState.Minimized;
        }
        #endregion
    }
}