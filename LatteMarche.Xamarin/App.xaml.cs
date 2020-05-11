using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Services;
using LatteMarche.Xamarin.Rest.Services;
using LatteMarche.Xamarin.Views;
using LatteMarche.Xamarin.Views.Synch;
using LatteMarche.Xamarin.Zebra;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Rest.Interfaces;
using System.Threading.Tasks;
using System;
using Sentry;
using System.Collections.Generic;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Newtonsoft.Json;

namespace LatteMarche.Xamarin
{
    public partial class App : Application
    {

        #region Fields

        private IDevice device = DependencyService.Get<IDevice>();
        private IRestService restService => DependencyService.Get<IRestService>();
        private IGiriService giriService => DependencyService.Get<IGiriService>();

        private ISincronizzazioneService sincronizzazioneService = DependencyService.Get<ISincronizzazioneService>();

        #endregion

        #region Constructor

        public App()
        { }

        #endregion

        #region Methods

        protected override async void OnStart()
        {
            InitializeComponent();

            AppCenter.Start("android=2676a594-ff4a-483a-8178-ca2377f493d2;", typeof(Analytics), typeof(Crashes));
            SentrySdk.Init("https://a446f661b09343b8a3f828d89f198085@o382996.ingest.sentry.io/5219587");

            XF.Material.Forms.Material.Init(this, "Material.Configuration");

            DependencyService.Register<AcquirentiService>();
            DependencyService.Register<AllevamentiService>();
            DependencyService.Register<AutoCisterneService>();
            DependencyService.Register<DestinatariService>();
            DependencyService.Register<GiriService>();
            DependencyService.Register<PrelieviService>();
            DependencyService.Register<SincronizzazioneService>();
            DependencyService.Register<StampantiService>();
            DependencyService.Register<TemplateGiroService>();
            DependencyService.Register<TipiLatteService>();
            DependencyService.Register<TrasportatoriService>();


            DependencyService.Register<RestService>();
            DependencyService.Register<Printer>();

            AutomapperConfig.Configure();

            //var trasporatori = await DependencyService.Get<ITrasportatoriService>().GetItemsAsync();

            Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            
            //if (trasporatori.Count() > 0)
                MainPage = new MainPage();
            //else
            //    MainPage = new RegisterPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //Task.Run(() =>
            //{
            //    try
            //    {
            //        var ultimoDownload = this.sincronizzazioneService.GetLastAysnc(Enums.SynchType.Download).Result;
            //        if ((ultimoDownload.Timestamp - DateTime.Now).TotalDays > 1)
            //        {
            //            var dto = this.restService.Download(this.device.GetIdentifier()).Result;
            //            this.sincronizzazioneService.UpdateDatabaseSync(dto).Wait();

            //            Analytics.TrackEvent("Download avvenuto", new Dictionary<string, string>() { { "dto", JsonConvert.SerializeObject(dto) } });
            //            SentrySdk.CaptureMessage("Download avvenuto", Sentry.Protocol.SentryLevel.Info);
            //        }
            //    }
            //    catch(Exception exc)
            //    {
            //        SentrySdk.CaptureException(exc);
            //        Crashes.TrackError(exc);
            //    }

            //});
        }

        #endregion

    }
}
