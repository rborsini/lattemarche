using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LatteMarche.Xamarin.Droid.Zebra;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra;
using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

[assembly: Dependency(typeof(ConnectionManager))]
namespace LatteMarche.Xamarin.Droid.Zebra
{
    public class ConnectionManager : IConnectionManager
    {
        public void FindBluetoothPrinters(DiscoveryHandler discoveryHandler)
        {
            BluetoothDiscoverer.FindPrinters(Android.App.Application.Context, discoveryHandler);
        }

        public Connection GetBluetoothConnection(string macAddress)
        {
            return new BluetoothConnection(macAddress);
        }

        public StatusConnection GetBluetoothStatusConnection(string macAddress)
        {
            return new BluetoothStatusConnection(macAddress);
        }

    }
}