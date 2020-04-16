using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LatteMarche.Xamarin.Droid;
using LatteMarche.Xamarin.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceService))]
namespace LatteMarche.Xamarin.Droid
{
    public class DeviceService : IDevice
    {
        public string GetIdentifier()
        {
            return "123";
            //Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
    }
}