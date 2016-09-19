using Coft.PreachComposer.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coft.PreachComposer.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<ShowFileDialogProceed>(this, OnShowFileDialogProceed);
        }

        private void OnShowFileDialogProceed(ShowFileDialogProceed message)
        {
            if (message.IsFolder)
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                DialogResult dialogResult = folderBrowserDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<ShowFileDialogResult>(new ShowFileDialogResult() { IsFolder = message.IsFolder, FileType = message.FileType, Filename = folderBrowserDialog.SelectedPath, SafeFilename = folderBrowserDialog.SelectedPath });
                }
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                switch (message.FileType)
                {
                    case Models.Helpers.Enums.FileType.Audio:
                        openFileDialog.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
                        break;

                    case Models.Helpers.Enums.FileType.Image:
                        openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"; ;
                        break;

                    case Models.Helpers.Enums.FileType.Video:
                        openFileDialog.Filter = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                            " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";
                        break;
                }

                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<ShowFileDialogResult>(new ShowFileDialogResult() { IsFolder = message.IsFolder, FileType = message.FileType, Filename = openFileDialog.FileName, SafeFilename = openFileDialog.SafeFileName });
                }
            }
        }

    }
}
