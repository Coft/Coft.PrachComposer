using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.IO.Compression;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;

namespace Coft.PreachComposer.WPFClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand processCommand;

        public RelayCommand ProcessCommand
        {
            get { return processCommand; }
        }


        public MainViewModel()
        {

        }

    }
}
