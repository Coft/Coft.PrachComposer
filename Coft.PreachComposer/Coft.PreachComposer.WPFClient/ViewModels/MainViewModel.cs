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

        private string audioPath;

        public string AudioPath
        {
            get { return audioPath; }
            set { Set(ref audioPath, value); }
        }

        private string videoPath;

        public string VideoPath
        {
            get { return videoPath; }
            set { Set(ref videoPath, value); }
        }

        private int progress;

        public int Progress
        {
            get { return progress; }
            set { Set(ref progress, value); }
        }


        private RelayCommand processCommand;

        public RelayCommand ProcessCommand
        {
            get
            {
                if (processCommand == null)
                {
                    processCommand = new RelayCommand(
                        ExecutedProcessCommand, 
                        () => !IsProcessing 
                            && !string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath) 
                            && !string.IsNullOrEmpty(AudioPath) && File.Exists(AudioPath)
                            && !string.IsNullOrEmpty(VideoPath) && !File.Exists(VideoPath));
                }

                return processCommand;
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
        }

        public void ExecutedProcessCommand()
        {
            IsProcessing = true;

            Task.Run(() =>
            {
                videoService.CreateVideo(ImagePath, AudioPath, VideoPath);

                IsProcessing = false;
                //ImagePath = string.Empty;
                //AudioPath = string.Empty;
            });
        }
    }
}
