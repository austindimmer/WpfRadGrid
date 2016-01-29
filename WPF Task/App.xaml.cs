using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Core;

namespace WPF_Task
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WireUpApplicationEvents();
            IoCContainer.Build();
            var mainWindow = IoCContainer.BaseContainer.Resolve<WPF_Task.MainWindow>();
            MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
        }

        private void WireUpApplicationEvents()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }
    }
}
