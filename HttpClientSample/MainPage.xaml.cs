using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using HttpClientSample.Core;
using HttpClientSample.Core.ViewModel;
using HttpClientSample.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HttpClientSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            _viewModel = ((MainViewModel) DataContext);
            _viewModel.ShowPerson = ShowPerson;
        }

        private void ShowPerson(int personId)
        {
            Frame.Navigate(typeof (PersonDetail), personId);
            _viewModel.SelectedPerson = null;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await _viewModel.Init();
        }

        //private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        //}

        //private void OnBackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (Frame.CanGoBack)
        //    {
                
        //    }
        //}
    }
}
