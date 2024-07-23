using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Mesh
{
    public class MeshGenerator
    {
        public MeshGeometry3D GenerateMesh(int tiles = 16, double size = 1.0)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            double offset = tiles * size / 2.0;
            Random random = new Random();

            GeneratePositionsAndTextureCoordinates(mesh, tiles, size, offset, random);
            GenerateTriangleIndices(mesh, tiles);

            return mesh;
        }

        private void GeneratePositionsAndTextureCoordinates(MeshGeometry3D mesh, int tiles, double size, double offset, Random random)
        {
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
        }

        private void GenerateTriangleIndices(MeshGeometry3D mesh, int tiles)
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

                    mesh.TriangleIndices.Add(topRight);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(bottomRight);
                }
            }
        }
    }
}
