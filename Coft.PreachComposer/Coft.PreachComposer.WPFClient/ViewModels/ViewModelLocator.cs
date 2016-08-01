using Coft.PreachComposer.Models.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coft.PreachComposer.WPFClient.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IVideoService, VideoService>();

            SimpleIoc.Default.Register<MainViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
            }

            ServiceLocator.Current.GetInstance<MainViewModel>().AudioPath = "samples/sample1.mp3";
            ServiceLocator.Current.GetInstance<MainViewModel>().ImagePath = "samples/sample1.png";
            ServiceLocator.Current.GetInstance<MainViewModel>().VideoPath = "samples/video5.mp4";
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}
