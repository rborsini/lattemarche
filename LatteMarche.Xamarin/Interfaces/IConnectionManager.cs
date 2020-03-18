using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IConnectionManager
    {
        void FindBluetoothPrinters(DiscoveryHandler discoveryHandler);

        Connection GetBluetoothConnection(string macAddress);

        StatusConnection GetBluetoothStatusConnection(string macAddress);

    }
}
