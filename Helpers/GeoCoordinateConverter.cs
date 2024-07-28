using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Helpers
{

    /// <summary>
    /// Postoje tri koordinatna sustava
    /// 1. Koordinate svijeta - longituda i latituda su apsolutne koordinate na svijetu
    /// 2. Koordinate Mape - float vrijednost - ovisi o trenutnom Zoom-u
    ///     - cjelobrojna vrijednost određuje x-y koordinatu Tilea za traženi zoom (GetTileCoordFromMapPosition)
    ///     - vrijednost iza zareza pomnožena sa 256 (veličinom Tilea) određuje koordinatu unutar Tilea (GetMapShift)
    /// 3. Koordinate pixela - svaka koordinata računa se od centerpointa, koji je izabrana koordinata svijeta  
    ///                      - početna koordinata mape u pictureBoxu stalno se mijenja i ne može biti referenca
    /// </summary>
    /// 




    public static class GeoCoordinateConverter
    {

        /// <summary>
        /// Izračunava broj tileova po x osi za zadani nivo zumiranja.
        /// </summary>
        /// <param name="zoom">Nivo zumiranja (zoom level).</param>
        /// <returns>Broj tileova po x osi.</returns>
        public static int GetNumberOfTilesX(int zoom)
        {
            return 1 << zoom; // Isto kao Math.Pow(2, zoom)
        }

        /// <summary>
        /// Izračunava poziciju tilea za zadane geografske koordinate i nivo zumiranja.
        /// </summary>
        /// <param name="longitude">Geografska dužina.</param>
        /// <param name="latitude">Geografska širina.</param>
        /// <param name="zoom">Nivo zumiranja.</param>
        /// <returns>Pozicija tilea (X, Y).</returns>
        public static Point GetTilePosition(double longitude, double latitude, int zoom)
        {
            // Pretvorba stupnjeva u radijane
            double latRad = latitude * Const.DEG_TO_RAD;

            Point tilePosition = new Point
            {
                // Izračunavanje X tilea
                X = (int)((longitude + 180.0) / 360.0 * (1 << zoom)),
                // Izračunavanje Y tilea
                Y = (int)((1.0 - Math.Log(Math.Tan(latRad) + 1.0 / Math.Cos(latRad)) / Math.PI) / 2.0 * (1 << zoom))
            };
            return (tilePosition);
        }

        /// <summary>
        /// Izračunava geografske koordinate (longitude, latitude) iz zadanih koordinata tilea i nivoa zumiranja.
        /// </summary>
        /// <param name="x">X koordinata tilea.</param>
        /// <param name="y">Y koordinata tilea.</param>
        /// <param name="zoom">Nivo zumiranja (z).</param>
        /// <returns>Geografske koordinate (longitude, latitude).</returns>
        /// 
        //public static (double longitude, double latitude) GetGeoCoordinatesFromTile(double x, double y, int zoom)
        //{
        //    int numTiles = 1 << zoom;

        //    double lon = x / numTiles * 360.0 - 180.0;

        //    double n = Math.PI - (2.0 * Math.PI * y) / numTiles;
        //    double lat = 180.0 / Math.PI * Math.Atan(Math.Sinh(n));

        //    return (lon, lat);
        //}

        public static (double longitude, double latitude) GetGeoCoordinatesFromTile(double mapPositionX, double mapPositionY, int zoom)
        {
            int numTiles = 1 << zoom;
            // mapPositionX / Const.worldSize stvara raspon od 0 do 1 --- +0.5 pomiče koordinate tako da je 
            double tileX = mapPositionX / Const.worldSize + .5;
            double tileY = mapPositionY / Const.worldSize + .5;


            double lon = (double)((tileX / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            double n = (double)(Math.PI - ((2.0 * Math.PI * tileY) / Math.Pow(2.0, zoom)));
            double lat = -(double)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            //double lon = x / numTiles * 360.0 - 180.0;

            //double n = Math.PI - (2.0 * Math.PI * y) / numTiles;
            //double lat = 180.0 / Math.PI * Math.Atan(Math.Sinh(n));

            return (lon, lat);
        }

        /// <summary>
        /// Izračunava veličinu tilea za zadani nivo zumiranja.
        /// </summary>
        /// <param name="zoom">Nivo zumiranja (z).</param>
        /// <returns>Veličina tilea.</returns>
        public static int GetTileSize(int zoom)
        {
            return 1 << (25 - zoom); // Veličina tilea prema zadanim pravilima
        }

        ///// <summary>
        ///// Izračunava pixel poziciju točke na Mapi iz pozicije točke na svijetu preko pozicije centralne točke na svijetu
        ///// </summary>
        ///// <param name="Longitude">geografska duljina tražene točke</param>
        ///// <param name="Latitude">geografska širina tražene točke</param>
        ///// <param name="zoom">trenutni zoom</param>
        ///// <param name="latLong">geografska pozicija centralne referentne točke</param>
        ///// <returns>pixel pozicija tražene točke</returns>
        //public static Point GetPixelPosFromWorldPos(double Longitude, double Latitude, int zoom, Point latLong)
        //{
        //    Point pointerTilePosition = GetMapPosFromWorldPos(Longitude, Latitude, zoom);
        //    Point centerTilePosition = GetMapPosFromWorldPos(latLong.X, latLong.Y, zoom);
        //    Point pointMapPosition = new Point
        //    {
        //        X = /*(int)*/((pointerTilePosition.X - centerTilePosition.X) * Const.TILE_SIZE.X + Const.PB_CENTERPOINT.X),
        //        Y = /*(int)*/((pointerTilePosition.Y - centerTilePosition.Y) * Const.TILE_SIZE.Y + Const.PB_CENTERPOINT.Y)
        //    };
        //    return pointMapPosition;
        //}

        ///// <summary>
        ///// Izračunava poziciju na mapi iz pozicije na svijetu
        ///// </summary>
        ///// <param name="lon">geografska duljina tražene točke</param>
        ///// <param name="lat">geografska širina tražene točke</param>
        ///// <param name="zoom">trenutni zoom</param>
        ///// <returns>pozicija na mapi tražene točke</returns>
        //public static Point GetMapPosFromWorldPos(double lon, double lat, int zoom)
        //{
        //    Point p = new Point
        //    {
        //        X = (double)((lon + 180.0) / 360.0 * (1 << zoom)),
        //        Y = (double)((1.0 - Math.Log(Math.Tan(lat * Const.DEG_TO_RAD) +
        //        1.0 / Math.Cos(lat * Const.DEG_TO_RAD)) / Math.PI) / 2.0 * (1 << zoom))
        //    };
        //    return p;
        //}

        /// <summary>
        /// Izračunava poziciju na svijetu iz pozicije na mapi
        /// </summary>
        /// <param name="mapPositionX">pozicija X točke na mapi</param>
        /// <param name="mapPositionY">pozicija Y točke na mapi</param>
        /// <param name="zoom">trenutni Zoom</param>
        /// <returns>pozicija na svijetu za traženu točku</returns>
        public static Point GetWorldPosFromMapPos(double mapPositionX, double mapPositionY, int zoom)
        {
            Point p = new Point();
            double n = (double)(Math.PI - ((2.0 * Math.PI * mapPositionY) / Math.Pow(2.0, zoom)));

            p.X = (double)((mapPositionX / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            p.Y = (double)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }

        ///// <summary>
        ///// Izračunava poziciju na svijetu iz pixel pozicije preko centralne pixel pozicije
        ///// </summary>
        ///// <param name="mouseClickPosition">pixel pozicija na kojoj se kliknulo mišem</param>
        ///// <param name="MapPosition">pozicija na Mapi</param>
        ///// <param name="Zoom">trenutni zoom</param>
        ///// <returns>Point pozicije na svijetu</returns>
        //public static Point GetWorldPosFromPixelPos(Point mouseClickPosition, Point MapPosition, int Zoom)
        //{
        //    Point PositionFromCenter = new Point();
        //    Point MissionMapPosition = new Point();
        //    PositionFromCenter.X = ((mouseClickPosition.X) - Const.PB_CENTERPOINT.X);
        //    PositionFromCenter.Y = ((mouseClickPosition.Y) - Const.PB_CENTERPOINT.Y);
        //    MissionMapPosition.X = MapPosition.X + PositionFromCenter.X / Const.TILE_SIZE.X;
        //    MissionMapPosition.Y = MapPosition.Y + PositionFromCenter.Y / Const.TILE_SIZE.Y;
        //    Point WorldCoordinate = GetWorldPosFromMapPos(MissionMapPosition.X, MissionMapPosition.Y, Zoom);

        //    return WorldCoordinate;
        //}

        public static Point GetCoordsFromBearingAndDistance
            (double lng1, double lat1, double bearing, double distance)
        {
            //double d = speed * Const.KPH_TO_MPS * Mode.MainLoopInterval * 0.001;
            //http://www.movable-type.co.uk/scripts/latlong.html
            //φ2 = asin(sin φ1 ⋅ cos δ + cos φ1 ⋅ sin δ ⋅ cos θ)
            //λ2 = λ1 + atan2(sin θ ⋅ sin δ ⋅ cos φ1, cos δ − sin φ1 ⋅ sin φ2)
            //where φ is latitude, λ is longitude, θ is the bearing (clockwise from north),
            //δ is the angular distance d/ R; d being the distance travelled, R the earth’s radius
            //The longitude can be normalised to −180…+180 using (lon + 540)% 360 - 180

            double brngRad = bearing * Helpers.Const.DEG_TO_RAD;
            double latRad1 = lat1 * Helpers.Const.DEG_TO_RAD;
            double lngRad1 = lng1 * Helpers.Const.DEG_TO_RAD;
            double latRad2 = Math.Asin(Math.Sin(latRad1) * Math.Cos(distance / Helpers.Const.R) +
                       Math.Cos(latRad1) * Math.Sin(distance / Helpers.Const.R) * Math.Cos(brngRad));
            double lngRad2 = lngRad1 + Math.Atan2(Math.Sin(brngRad) * Math.Sin(distance / Helpers.Const.R) * Math.Cos(latRad1),
                       Math.Cos(distance / Helpers.Const.R) - Math.Sin(latRad1) * Math.Sin(latRad2));
            //Lat2 = latRad2 * Helpers.Const.RAD_TO_DEG;
            //Lng2 = lngRad2 * Helpers.Const.RAD_TO_DEG;
            Point coords = new Point
                (lngRad2 * Helpers.Const.RAD_TO_DEG,
                 latRad2 * Helpers.Const.RAD_TO_DEG);
            return coords;
        }

        public static double GetTwoPointsDistance(Point p1, Point p2)
        {
            return Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
        }

        /// <summary>
        /// Dohvaća duljinu i širinu iz textboxa inputa
        /// </summary>
        /// <param name="longitudeXtext"></param>
        /// <param name="latitudeYtext"></param>
        /// <returns></returns>
        public static Point GetLatLongFromInput(string longitudeXtext, string latitudeYtext)
        {
            Point latLong = new Point();
            if (Double.TryParse(longitudeXtext, out double longitudeX)) latLong.X = longitudeX;
            if (Double.TryParse(latitudeYtext, out double latitudeY)) latLong.Y = latitudeY;
            return latLong;
        }

        /// <summary>
        /// Dohvaća kurs iz input textboxa - samo za testiranje, textbox će u glavnoj aplikaciji biti skinut
        /// </summary>
        /// <param name="courseText"></param>
        /// <returns></returns>
        public static double GetCourseFromInput(string courseText)
        {
            if (Double.TryParse(courseText, out double course))
                return course;
            else return 0;
        }

        /// <summary>
        /// Izračunava poziciju Tilea iz pozicije na Mapi za zadani Zoom
        /// </summary>
        /// <param name="mapPosition">pozicija na Mapi</param>
        /// <returns>pozicija Tilea</returns>
        public static Point GetTilePosFromMapPos(Point mapPosition)
        {
            Point tilePosition = new Point
            {
                X = (int)mapPosition.X,
                Y = (int)mapPosition.Y
            };
            return tilePosition;
        }

        ///// <summary>
        ///// Izračunava shift mape da se poklope željena koordinata i sredina pictureboxa
        ///// </summary>
        ///// <param name="mapPosition">pozicija na Mapi</param>
        ///// <returns>pozicija unutar Tilea</returns>
        //public static Point GetMapShift(Point mapPosition)
        //{
        //    Point mapShift = new Point
        //    {
        //        X = (int)((mapPosition.X - (int)mapPosition.X) * Const.TILE_SIZE.X),
        //        Y = (int)((mapPosition.Y - (int)mapPosition.Y) * Const.TILE_SIZE.Y)
        //    };
        //    return mapShift;
        //}

        //public static double GetCartesianAngleFromBearing(double bearing)
        //{
        //    return Trnsfrm.Limit360(450.0f - bearing);
        //}

        /// <summary>
        /// Procjenjuje veličinu tile-a na ekranu na temelju udaljenosti kamere i zadanih 3D koordinata točke.
        /// </summary>
        /// <param name="point">3D točka u world spaceu.</param>
        /// <param name="camera">Perspective kamera.</param>
        /// <param name="helixViewport">Viewport3D kontrola.</param>
        /// <returns>Procijenjena veličina tile-a na ekranu.</returns>
        public static double EstimateTileSizeOnScreen(Point3D point, HelixViewport3D helixViewport)
        {
            // Izračunajte udaljenost između kamere i točke
            var distance = (helixViewport.Camera.Position - point).Length;

            // Kut vidnog polja kamere (Field of View) u radijanima
            if (helixViewport.Camera is PerspectiveCamera perspectiveCamera)
            {
                double fov = Const.DEG_TO_RAD * perspectiveCamera.FieldOfView;

                // Visina viewporta u pikselima
                double viewportHeight = helixViewport.ActualHeight;

                //// Izvorna veličina tile-a (256x256 piksela)
                //double originalTileSize = 256.0;

                // Procjena veličine tile-a na ekranu na temelju udaljenosti
                double screenTileSize = Const.worldSize / distance * 2 * (viewportHeight / (2.0 * Math.Tan(fov / 2.0)));

                return screenTileSize;
            }
            else
            {
                throw new InvalidOperationException("Kamera mora biti PerspectiveCamera.");
            }
        }
    }
}
