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
using Coft.PreachComposer.Models.Services;
using GalaSoft.MvvmLight.Threading;
using Coft.PreachComposer.Models.Messages;

namespace Coft.PreachComposer.WPFClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool isProcessing;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { Set(ref isProcessing, value); }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { Set(ref imagePath, value); }
        }

        private string imageFilename;

        public string ImageFilename
        {
            get { return imageFilename; }
            set { Set(ref imageFilename, value); }
        }

        private string audioFilename;

        public string AudioFilename
        {
            get { return audioFilename; }
            set { Set(ref audioFilename, value); }
        }

        private string audioPath;

        public string AudioPath
        {
            get { return audioPath; }
            set { Set(ref audioPath, value); }
        }

        public string VideoPath
        {
            get { return Path.Combine(VideoDirectoryPath, $"{VideoFilename}.mp4"); }
        }

        private string videoFilename;

        public string VideoFilename
        {
            get { return videoFilename; }
            set { Set(ref videoFilename, value); }
        }

        private string videoDirectoryPath;

        public string VideoDirectoryPath
        {
            get { return videoDirectoryPath; }
            set { Set(ref videoDirectoryPath, value); }
        }

        private int progress;

        public int Progress
        {
            get { return progress; }
            set { Set(ref progress, value); }
        }

        private RelayCommand choseImageCommand;

        public RelayCommand ChoseImageCommand
        {
            get { return choseImageCommand??
                    (choseImageCommand = new RelayCommand(
                        () => {
                            Messenger.Default.Send<ShowFileDialogProceed>(new ShowFileDialogProceed() { IsFolder = false, FileType = Models.Helpers.Enums.FileType.Image });
                        }, 
                        () => true)); }
        }

        private RelayCommand choseAudioCommand;

        public RelayCommand ChoseAudioCommand
        {
            get
            {
                return choseAudioCommand ??
                  (choseAudioCommand = new RelayCommand(
                      () => {
                          Messenger.Default.Send<ShowFileDialogProceed>(new ShowFileDialogProceed() { IsFolder = false, FileType = Models.Helpers.Enums.FileType.Audio });
                      },
                      () => true));
            }
        }

        private RelayCommand choseVideoDirectoryCommand;

        public RelayCommand ChoseVideoDirectoryCommand
        {
            get
            {
                return choseVideoDirectoryCommand ??
                  (choseVideoDirectoryCommand = new RelayCommand(
                      () => {
                          Messenger.Default.Send<ShowFileDialogProceed>(new ShowFileDialogProceed() { IsFolder = true, FileType = Models.Helpers.Enums.FileType.Video});
                      },
                      () => true));
            }
        }

        private RelayCommand processCommand;

        public RelayCommand ProcessCommand
        {
            get
            {
                return processCommand ??
                    (processCommand = new RelayCommand(
                        ExecutedProcessCommand,
                        () => !IsProcessing
                            && !string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath)
                            && !string.IsNullOrEmpty(AudioPath) && File.Exists(AudioPath)
                            && !string.IsNullOrEmpty(VideoFilename)
                            && !string.IsNullOrEmpty(VideoDirectoryPath) && !File.Exists(VideoDirectoryPath))
                    );
            }
        }


        private IVideoService videoService;

        public MainViewModel(IVideoService videoService)
        {
            this.videoService = videoService;
            
            videoService.AttachProgressAction((currentProgress) => {
                DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        Progress = currentProgress;
                    }
                );
            });

            MessengerInstance.Register<ShowFileDialogResult>(this, OnShowFileDialogResult);
        }

        public void ExecutedProcessCommand()
        {
            IsProcessing = true;

            Task.Run(() =>
            {
                videoService.CreateVideo(ImagePath, AudioPath, VideoPath);

                IsProcessing = false;
            });
        }

        public void OnShowFileDialogResult(ShowFileDialogResult message)
        {
            if (message.IsFolder)
            {
                VideoDirectoryPath = message.Filename;
            }
            else
            {
                switch (message.FileType)
                {
                    case Models.Helpers.Enums.FileType.Audio:
                        AudioPath = message.Filename;
                        AudioFilename = message.SafeFilename;
                        break;
                    case Models.Helpers.Enums.FileType.Image:
                        ImagePath = message.Filename;
                        ImageFilename = message.SafeFilename;
                        break;
                }
            }
        }
    }
}
