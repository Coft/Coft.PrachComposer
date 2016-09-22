using Coft.PreachComposer.Models.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Coft.PreachComposer.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Logger Logger = LogManager.GetCurrentClassLogger();

        static App()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
        }

        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Nieznany błąd skontaktuj się z autorem {Configuration.AuthorEmail}. Zamykam aplikację.");
            Application.Current.Shutdown();
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
        }
    }
}
