using LatteMarche.Xamarin.Services;
using LatteMarche.Xamarin.Views;
using LatteMarche.Xamarin.Zebra;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<AllevamentiDataStore>();
            DependencyService.Register<PrelieviDataStore>();

            DependencyService.Register<RestService>();
            DependencyService.Register<Printer>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
