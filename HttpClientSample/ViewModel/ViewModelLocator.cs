using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HttpClientSample.Core.Services;
using HttpClientSample.Core.Services.Impl;
using Microsoft.Practices.ServiceLocation;

namespace HttpClientSample.ViewModel
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                if (ViewModelBase.IsInDesignModeStatic)
                {
                    //if (!SimpleIoc.Default.IsRegistered<GalaSoft.MvvmLight.Views.INavigationService>())
                    //{
                    //    SimpleIoc.Default.Register<GalaSoft.MvvmLight.Views.INavigationService, DesignNavigationService>();
                    //}

                    SimpleIoc.Default.Register<IPersonService, PersonService>();
                }
                else
                {
                    SimpleIoc.Default.Register<IPersonService, PersonService>();
                }

                SimpleIoc.Default.Register<MainViewModel>();
                SimpleIoc.Default.Register<PersonViewModel>();
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

        public PersonViewModel Person => SimpleIoc.Default.GetInstance<PersonViewModel>();
    }
}
