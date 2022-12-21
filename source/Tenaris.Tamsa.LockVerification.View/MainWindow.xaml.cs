using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tenaris.Tamsa.LockVerification.ViewModel;

namespace Tenaris.Tamsa.LockVerification.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static string ImagesFolder;

        public MainWindow()
        {
            // Se busca eliminar otras instancias de la aplicación
            System.Diagnostics.Process[] P = System.Diagnostics.Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (P.Length > 0)
            {
                foreach (var p in P)
                {
                    if (p.Id != Process.GetCurrentProcess().Id)
                    {
                        p.Kill();
                    }
                }
            }

            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            try
            {
                //System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
                //string Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + System.IO.Path.DirectorySeparatorChar, "Images\\"));
                //ImagesFolder = "Images\\";
                //ni.Icon = new System.Drawing.Icon(ImagesFolder + "MainIcon.ico");
                //ni.Visible = true;
                //ni.DoubleClick += delegate(object sender, EventArgs args)
                //{
                //    Tenaris.Library.Log.Trace.Message("DblClicked icon for wakeup view..");
                //    this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                //    this.Show();
                //    this.Topmost = true;
                //    ViewModelInstances.Instance.MainViewModel.CurWindowState = this.WindowState = WindowState.Normal;
                //    this.Focus();
                //};
                //ni.BalloonTipClosed += (sender, e) => { var thisIcon = (System.Windows.Forms.NotifyIcon)sender; thisIcon.Visible = false; thisIcon.Dispose(); };
                //ni.Text = LanguageResource.Language.ViewTitle;                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Topmost = false;
                this.WindowState = ViewModelInstances.Instance.MainViewModel.CurWindowState = WindowState.Minimized;
                //this.Visibility = ViewModelInstances.Instance.MainViewModel.IsVisibility = Visibility.Hidden;
            }
            else
            {
                //this.Visibility = ViewModelInstances.Instance.MainViewModel.IsVisibility = Visibility.Visible;
                this.WindowState = ViewModelInstances.Instance.MainViewModel.CurWindowState = WindowState.Normal;
                this.Topmost = true;
            }

            base.OnStateChanged(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModelInstances.Instance.MainViewModel.CurWindowState = this.WindowState = WindowState.Minimized;
        }
    }
}
