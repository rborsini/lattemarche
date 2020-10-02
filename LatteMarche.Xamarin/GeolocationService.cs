using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace LatteMarche.Xamarin
{
    public class GeolocationService
    {

        /// <summary>
        /// Rilevamento posizione
        /// </summary>
        /// <returns></returns>
        public static Location GetLocation()
        {
            var permissionStatus = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().Result;

            var location = Geolocation.GetLocationAsync().Result;

            if (permissionStatus == PermissionStatus.Granted)
                return location;
            else
                return null;
        }

    }
}
