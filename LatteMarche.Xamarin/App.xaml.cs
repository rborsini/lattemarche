using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Services;
using LatteMarche.Xamarin.Rest.Services;
using LatteMarche.Xamarin.Views;
using LatteMarche.Xamarin.Views.Synch;
using LatteMarche.Xamarin.Zebra;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<AcquirentiService>();
            DependencyService.Register<AllevamentiService>();
            DependencyService.Register<AutoCisterneService>();
            DependencyService.Register<DestinatariService>();
            DependencyService.Register<GiriService>();
            DependencyService.Register<PrelieviService>();
            DependencyService.Register<SincronizzazioneService>();
            DependencyService.Register<TemplateGiroService>();
            DependencyService.Register<TipiLatteService>();
            DependencyService.Register<TrasportatoriService>();


            DependencyService.Register<RestService>();
            DependencyService.Register<Printer>();

            AutomapperConfig.Configure();

            var trasporatori = DependencyService.Get<ITrasportatoriService>().GetItemsAsync().Result.Count();

            if (trasporatori > 0)
                MainPage = new MainPage();
            else
                MainPage = new RegisterPage();
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
