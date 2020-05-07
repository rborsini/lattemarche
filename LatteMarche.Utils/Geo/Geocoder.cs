using Geocoding.Google;
using Geocoding.Microsoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Utils.Geo
{
    public class Geocoder
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Geocoder");

        private static string BingKey
        {
            get { return ConfigurationManager.AppSettings["bing_key"].ToString(); }
        }

        public static Point<double?> Geocode(string address)
        {
            Point<double?> point = new Point<double?>();

            if (!String.IsNullOrEmpty(address))
            {
                try
                {
                    BingMapsGeocoder bingCoder = new BingMapsGeocoder(BingKey);
                    GoogleGeocoder googleCoder = new GoogleGeocoder();

                    BingAddress bingAddress = null;
                    GoogleAddress googleAddress = null;

                    try { bingAddress = bingCoder.Geocode(address).ToList().FirstOrDefault(); }
                    catch (Exception exc) { log.Error(exc); }

                    try { googleAddress = googleCoder.Geocode(address).ToList().FirstOrDefault(); }
                    catch (Exception exc) { log.Error(exc); }

                    string bing = bingAddress == null ? "BING KO" : String.Format("{0} \t {1} \t {2}", bingAddress.Confidence.ToString().PadRight(8), bingAddress.FormattedAddress.PadRight(17), bingAddress.Coordinates.ToString());
                    string google = googleAddress == null ? "GOOGLE KO" : String.Format("{0} \t {1} \t {2}", googleAddress.IsPartialMatch, googleAddress.FormattedAddress.PadRight(17), googleAddress.Coordinates.ToString());

                    log.Info(String.Format("{0} \t {1} \t\t\t {2}", address.PadLeft(17), bing, google));

                    point = Evaluate(bingAddress, googleAddress);
                }
                catch (Exception exc)
                {
                    log.Warn("Bing gecode failed. (" + address + ")", exc);
                }
            }

            return point;
        }


        private static Point<double?> Evaluate(BingAddress bing, GoogleAddress google)
        {
            Point<double?> point = new Point<double?>();

            if (bing == null || google == null)
            {
                // se uno dei 2 è nullo, allora prendo quello non nullo
                if (bing != null)
                {
                    point.Latitude = bing.Coordinates.Latitude;
                    point.Longitude = bing.Coordinates.Longitude;
                }

                if (google != null)
                {
                    point.Latitude = google.Coordinates.Latitude;
                    point.Longitude = google.Coordinates.Longitude;
                }
            }

            // se entrambi sono diversi da null, valuto la qualità con priorità a bing
            if (bing != null && google != null)
            {
                if (bing.Confidence == ConfidenceLevel.High)
                {
                    point.Latitude = bing.Coordinates.Latitude;
                    point.Longitude = bing.Coordinates.Longitude;
                }
                else
                {
                    point.Latitude = google.Coordinates.Latitude;
                    point.Longitude = google.Coordinates.Longitude;
                }
            }

            return point;
        }

    }
}
