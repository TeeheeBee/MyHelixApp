using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Mesh
{
    public class MeshGenerator
    {
        private List<Point3DCollection> triangleEdges = new List<Point3DCollection>(); // Initialize triangleEdges

        public MeshGeometry3D GenerateMesh(int tiles = 16, double size = 1.0)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            double offset = tiles * size / 2.0;
            Random random = new Random();

            //GeneratePositionsAndTextureCoordinates(tiles, size, offset, random);
            //GenerateTriangleIndices(mesh, tiles);

            return mesh;
        }

        public MeshGeometry3D GeneratePositionsAndTextureCoordinates(int tiles, double size, double offset, Random random)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int i = 0; i <= tiles; i++)
            {
                for (int j = 0; j <= tiles; j++)
                {
                    double x = i * size - offset;
                    double y = j * size - offset;
                    double z = random.NextDouble();
                    mesh.Positions.Add(new Point3D(x, y, z));

                    double u = (double)j / tiles;
                    double v = (double)i / tiles;
                    mesh.TextureCoordinates.Add(new Point(v, 1.0 - u));
                }
            }
            return mesh;
        }

        public List<Point3DCollection> GenerateTriangleIndices(MeshGeometry3D mesh, int tiles)
        {
            for (int i = 0; i < tiles; i++)
            {
                for (int j = 0; j < tiles; j++)
                {
                    int topLeft = i * (tiles + 1) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + (tiles + 1);
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
