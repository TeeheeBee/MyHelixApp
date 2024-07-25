using HelixToolkit.Wpf;
using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Mesh
{
    public class MeshGlobeGenerator
    {
        public MeshGeometry3D CreateGlobeMesh(int zoomLevel)
        {
            var mesh = new MeshGeometry3D();
            var positions = new Point3DCollection();
            var triangleIndices = new Int32Collection();
            var textureCoordinates = new PointCollection();

            int segments = zoomLevel * 4;

            for (int lat = 0; lat <= segments; lat++)
            {
                double theta = lat * Math.PI / segments;
                double sinTheta = Math.Sin(theta);
                double cosTheta = Math.Cos(theta);

                for (int lon = 0; lon <= segments; lon++)
                {
                    double phi = lon * 2 * Math.PI / segments;
                    double x = sinTheta * Math.Cos(phi);
                    double y = cosTheta;
                    double z = sinTheta * Math.Sin(phi);

                    positions.Add(new Point3D(x, y, z));
                    textureCoordinates.Add(new System.Windows.Point((double)lon / segments, (double)lat / segments));
                }
            }

            for (int lat = 0; lat < segments; lat++)
            {
                for (int lon = 0; lon < segments; lon++)
                {
                    int current = lat * (segments + 1) + lon;
                    int next = current + segments + 1;

                    triangleIndices.Add(current);
                    triangleIndices.Add(next);
                    triangleIndices.Add(current + 1);

                    triangleIndices.Add(current + 1);
                    triangleIndices.Add(next);
                    triangleIndices.Add(next + 1);
                }
            }

            mesh.Positions = positions;
            mesh.TriangleIndices = triangleIndices;
            mesh.TextureCoordinates = textureCoordinates;

            return mesh;



            //var mesh = new MeshGeometry3D();
            //var positions = new Point3DCollection();
            //var triangleIndices = new Int32Collection();
            //var textureCoordinates = new PointCollection();

            //int segments = zoomLevel * 4;

            //for (int lat = 0; lat <= segments; lat++)
            //{
            //    double theta = lat * Math.PI / segments;
            //    double sinTheta = Math.Sin(theta);
            //    double cosTheta = Math.Cos(theta);

            //    for (int lon = 0; lon <= segments; lon++)
            //    {
            //        double phi = lon * 2 * Math.PI / segments;
            //        double x = sinTheta * Math.Cos(phi);
            //        double y = cosTheta;
            //        double z = sinTheta * Math.Sin(phi);

            //        positions.Add(new Point3D(x, y, z));
            //        textureCoordinates.Add(new System.Windows.Point((double)lon / segments, 1 - (double)lat / segments));
            //    }
            //}

            //for (int lat = 0; lat < segments; lat++)
            //{
            //    for (int lon = 0; lon < segments; lon++)
            //    {
            //        int current = lat * (segments + 1) + lon;
            //        int next = current + segments + 1;

            //        triangleIndices.Add(current);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(current + 1);

            //        triangleIndices.Add(current + 1);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(next + 1);
            //    }
            //}

            //mesh.Positions = positions;
            //mesh.TriangleIndices = triangleIndices;
            //mesh.TextureCoordinates = textureCoordinates;

            //return mesh;



            //var mesh = new MeshGeometry3D();
            //var positions = new Point3DCollection();
            //var triangleIndices = new Int32Collection();
            //var textureCoordinates = new PointCollection();

            //int segments = zoomLevel * 16; // povećanje broja segmenata sa zoom levelom

            //for (int lat = 0; lat <= segments; lat++)
            //{
            //    double theta = lat * Math.PI / segments;
            //    double sinTheta = Math.Sin(theta);
            //    double cosTheta = Math.Cos(theta);

            //    for (int lon = 0; lon <= segments; lon++)
            //    {
            //        double phi = lon * 2 * Math.PI / segments;
            //        double x = sinTheta * Math.Cos(phi);
            //        double y = cosTheta;
            //        double z = sinTheta * Math.Sin(phi);

            //        positions.Add(new Point3D(x, y, z));
            //        textureCoordinates.Add(new System.Windows.Point((double)lon / segments, (double)lat / segments));
            //    }
            //}

            //for (int lat = 0; lat < segments; lat++)
            //{
            //    for (int lon = 0; lon < segments; lon++)
            //    {
            //        int current = lat * (segments + 1) + lon;
            //        int next = current + segments + 1;

            //        triangleIndices.Add(current);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(current + 1);

            //        triangleIndices.Add(current + 1);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(next + 1);
            //    }
            //}

            //mesh.Positions = positions;
            //mesh.TriangleIndices = triangleIndices;
            //mesh.TextureCoordinates = textureCoordinates;

            //return mesh;


            //var mesh = new MeshGeometry3D();
            //var positions = new Point3DCollection();
            //var triangleIndices = new Int32Collection();
            //var textureCoordinates = new PointCollection();

            //int segments = zoomLevel * 1; // povećanje broja segmenata sa zoom levelom

            //for (int lat = 0; lat <= segments; lat++)
            //{
            //    double theta = lat * Math.PI / segments;
            //    double v = (1.0 - Math.Log(Math.Tan(Math.PI / 4.0 + theta / 2.0)) / Math.PI) / 2.0; // prilagodba za Mercator

            //    for (int lon = 0; lon <= segments; lon++)
            //    {
            //        double phi = lon * 2 * Math.PI / segments;
            //        double x = Math.Sin(theta) * Math.Cos(phi);
            //        double y = Math.Cos(theta);
            //        double z = Math.Sin(theta) * Math.Sin(phi);
            //        positions.Add(new Point3D(x, y, z));
            //        textureCoordinates.Add(new System.Windows.Point((double)lon / segments, v)); // prilagodba UV koordinata
            //    }
            //}

            //for (int lat = 0; lat < segments; lat++)
            //{
            //    for (int lon = 0; lon < segments; lon++)
            //    {
            //        int current = lat * (segments + 1) + lon;
            //        int next = current + segments + 1;

            //        triangleIndices.Add(current);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(current + 1);

            //        triangleIndices.Add(current + 1);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(next + 1);
            //    }
            //}

            //mesh.Positions = positions;
            //mesh.TriangleIndices = triangleIndices;
            //mesh.TextureCoordinates = textureCoordinates;

            //return mesh;



            //var mesh = new MeshGeometry3D();
            //var positions = new Point3DCollection();
            //var triangleIndices = new Int32Collection();
            //var textureCoordinates = new PointCollection();

            //int segments = zoomLevel * 4; // povećanje broja segmenata sa zoom levelom

            //for (int lat = 0; lat <= segments; lat++)
            //{
            //    double theta = lat * Math.PI / segments;
            //    double v = (1.0 - Math.Log(Math.Tan(Math.PI / 4.0 + theta / 2.0)) / Math.PI) / 2.0; // prilagodba za Mercator

            //    for (int lon = 0; lon <= segments; lon++)
            //    {
            //        double phi = lon * 2 * Math.PI / segments;
            //        double x = Math.Sin(theta) * Math.Cos(phi);
            //        double y = Math.Cos(theta);
            //        double z = Math.Sin(theta) * Math.Sin(phi);
            //        vertices.Add(new Point3D(x, y, z));
            //        uv.Add(new System.Windows.Point((double)lon / segments, v)); // prilagodba UV koordinata
            //    }
            //}


            ////for (int lat = 0; lat <= segments; lat++)
            ////{
            ////    double theta = lat * Math.PI / segments;
            ////    for (int lon = 0; lon <= segments; lon++)
            ////    {
            ////        double phi = lon * 2 * Math.PI / segments;
            ////        double x = Math.Sin(theta) * Math.Cos(phi);
            ////        double y = Math.Cos(theta);
            ////        double z = Math.Sin(theta) * Math.Sin(phi);
            ////        positions.Add(new Point3D(x, y, z));
            ////        textureCoordinates.Add(new System.Windows.Point((double)lon / segments, (double)lat / segments));
            ////    }
            ////}

            //for (int lat = 0; lat < segments; lat++)
            //{
            //    for (int lon = 0; lon < segments; lon++)
            //    {
            //        int current = lat * (segments + 1) + lon;
            //        int next = current + segments + 1;

            //        triangleIndices.Add(current);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(current + 1);

            //        triangleIndices.Add(current + 1);
            //        triangleIndices.Add(next);
            //        triangleIndices.Add(next + 1);
            //    }
            //}

            //mesh.Positions = positions;
            //mesh.TriangleIndices = triangleIndices;
            //mesh.TextureCoordinates = textureCoordinates;

            //return mesh;
        }
    }
}
