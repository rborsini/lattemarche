using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Utils.Geo
{
    public class Point<T>
    {
        public T Latitude { get; set; }
        public T Longitude { get; set; }
        public object Item { get; set; }

        public Point() { }

        public Point(T latitude, T longitude, object item)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Item = item;
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}, Item: {2}", this.Latitude, this.Longitude, this.Item);
        }

    }
}
