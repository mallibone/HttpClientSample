﻿using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HttpClientSample.Core.Services;
using HttpClientSample.Core.Services.Impl;
using HttpClientSample.Core.ViewModel;
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
                SimpleIoc.Default.Register<PersonDetailViewModel>();
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

        public PersonDetailViewModel Person => SimpleIoc.Default.GetInstance<PersonDetailViewModel>();
    }
}
