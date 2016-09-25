using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            TextVersion.Text = "Sincronizador de datos versión "+ Assembly.GetEntryAssembly().GetName().Version;

            PrepareTrayIcon();

            _vfpMonitor = new VFPMonController();
            _vfpMonitor.Start();
            
            //WindowState = WindowState.Minimized;

            ShowBalloon();
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
    }
}
