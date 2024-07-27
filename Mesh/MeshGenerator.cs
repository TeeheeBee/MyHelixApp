using MyHelixApp.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Mesh
{
    public class MeshGenerator
    {
        public class MeshInfo
        {
            public int Id { get; set; }
            public Point3D Coordinates { get; set; }
        }

        public MeshGeometry3D GeneratePositionsAndTextureCoordinates(Point3D tileCoordinate, int heights, double size)
        {
            //double offset = size / 2.0;
            double heightDivision = size / heights;
            Random random = new Random();
            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int i = 0; i <= heights; i++)
            {
                for (int j = 0; j <= heights; j++)
                {
                    double x = i * heightDivision + tileCoordinate.X - Const.offset;
                    double y = j * heightDivision + tileCoordinate.Y - Const.offset;
                    double z = random.NextDouble() * 1 / 5;
                    mesh.Positions.Add(new Point3D(x, y, z));

                    double u = (double)j / heightDivision;
                    double v = (double)i / heightDivision;
                    mesh.TextureCoordinates.Add(new Point(v, 1.0 - u));
                }
            }
            return mesh;
        }

        public MeshInfo SetMeshInfo(int ID, Point3D tileCoordinate)
        {
            MeshInfo info1 = new MeshInfo
            {
                Id = ID,
                Coordinates = tileCoordinate
            };
            return info1;
        }

        public List<Point3DCollection> GenerateTriangleIndices(MeshGeometry3D mesh, int heights)
        {
            List<Point3DCollection> triangleEdges = new List<Point3DCollection>(); // Initialize triangleEdges

            for (int i = 0; i < heights; i++)
            {
                for (int j = 0; j < heights; j++)
                {
                    int topLeft = i * (heights + 1) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + (heights + 1);
                    int bottomRight = bottomLeft + 1;

                    mesh.TriangleIndices.Add(topLeft);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(topRight);
                    triangleEdges.Add(new Point3DCollection { mesh.Positions[topLeft], mesh.Positions[bottomLeft], mesh.Positions[topRight], mesh.Positions[topLeft] });

                    mesh.TriangleIndices.Add(topRight);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(bottomRight);
                    triangleEdges.Add(new Point3DCollection { mesh.Positions[topRight], mesh.Positions[bottomLeft], mesh.Positions[bottomRight], mesh.Positions[topRight] });

                }
            }
            return triangleEdges;
        }
    }
}
