using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelixApp.Helpers
{
    public static class Const
    {
        public const int maxZoom = 25;
        public static double worldSize = GeoCoordinateConverter.GetNumberOfTilesX(Const.maxZoom);
        public static double offset = worldSize / 2.0;
        public const int StbPoints = 3;
        public static double KPH_TO_MPS = 0.277777777777777;
        public static double MTR_TO_DEG = 0.000009;
        public const double DEG_TO_RAD = 0.017453292519943295769236907684886;
        public const double RAD_TO_DEG = 57.295779513082320876798154814105;
        public const double R = 6371000.0;
    }
}
