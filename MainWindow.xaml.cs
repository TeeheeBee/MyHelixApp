using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Input;
using System.Windows.Threading;

namespace MyHelixApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml  !!!!
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string imagePath = @"C:\Users\tbozic\source\repos\MyHelix3DApp\earth.jpg";
        private BitmapImage bitmapImage;
        private bool showWireframe = false; // Toggle state
        private ModelVisual3D meshModel;
        private GeometryModel3D geometryModel;
        private List<Point3DCollection> triangleEdges;

        public MainWindow()
        {
            InitializeComponent();
            LoadBitmap();
            Setup3DScene();
            helixViewport.PanGesture = null;
            helixViewport.PanGesture2 = new MouseGesture(MouseAction.LeftClick);
        }

        private void LoadBitmap()
        {
            bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }

        private void Setup3DScene()
        {
            // Set the camera
            helixViewport.Camera = new PerspectiveCamera
            {
                Position = new Point3D(5, 5, 10),
                LookDirection = new Vector3D(-5, -5, -10),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 90

            };



            // Add default lights
            helixViewport.Children.Add(new DefaultLights());

            // Create a mesh and add it to the viewport
            meshModel = CreateMesh();
            helixViewport.Children.Add(meshModel);
        }

        private ModelVisual3D CreateMesh()
        {
            int tiles = 16; // Number of tiles per side
            float size = 1f; // Size of each tile
            float offset = tiles * size / 2f; // Offset to center the mesh
            Random random = new Random();
            MeshGeometry3D mesh = new MeshGeometry3D();
            triangleEdges = new List<Point3DCollection>(); // Initialize triangleEdges

            // Generate positions and texture coordinates
            for (int i = 0; i <= tiles; i++)
            {
                for (int j = 0; j <= tiles; j++)
                {
                    float x = i * size - offset;
                    float y = j * size - offset;
                    float z = (float)random.NextDouble();
                    mesh.Positions.Add(new Point3D(x, y, z));

                    // Adjust texture coordinates to rotate 90 degrees counter-clockwise and swap left-right
                    float u = (float)j / tiles;
                    float v = (float)i / tiles;
                    mesh.TextureCoordinates.Add(new Point(v, 1.0f - u));
                }
            }

            // Generate triangle indices and store edges
            for (int i = 0; i < tiles; i++)
            {
                for (int j = 0; j < tiles; j++)
                {
                    int topLeft = i * (tiles + 1) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + (tiles + 1);
                    int bottomRight = bottomLeft + 1;

                    // First triangle of the tile
                    mesh.TriangleIndices.Add(topLeft);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(topRight);
                    triangleEdges.Add(new Point3DCollection { mesh.Positions[topLeft], mesh.Positions[bottomLeft], mesh.Positions[topRight], mesh.Positions[topLeft] });

                    // Second triangle of the tile
                    mesh.TriangleIndices.Add(topRight);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(bottomRight);
                    triangleEdges.Add(new Point3DCollection { mesh.Positions[topRight], mesh.Positions[bottomLeft], mesh.Positions[bottomRight], mesh.Positions[topRight] });
                }
            }

            // Create a material with the bitmap
            ImageBrush imageBrush = new ImageBrush(bitmapImage);
            DiffuseMaterial textureMaterial = new DiffuseMaterial(imageBrush);

            // Create a solid color material for wireframe mode
            DiffuseMaterial solidColorMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));

            // Create a GeometryModel3D for the mesh
            geometryModel = new GeometryModel3D
            {
                Geometry = mesh,
                Material = textureMaterial,
                BackMaterial = textureMaterial // Apply material to the back faces as well
            };

            // Create a ModelVisual3D for the mesh
            ModelVisual3D modelVisual = new ModelVisual3D
            {
                Content = geometryModel
            };

            return modelVisual;
        }

        private void OnToggleViewButtonClick(object sender, RoutedEventArgs e)
        {
            showWireframe = !showWireframe;

            if (showWireframe)
            {
                // Switch to wireframe view
                geometryModel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));
                geometryModel.BackMaterial = null;

                // Add wireframe lines
                foreach (var edge in triangleEdges)
                {
                    var wireframe = new LinesVisual3D
                    {
                        Thickness = 1,
                        Color = Colors.Black,
                        Points = edge
                    };
                    helixViewport.Children.Add(wireframe);
                }
            }
            else
            {
                // Remove wireframe and show textured mesh
                geometryModel.Material = new DiffuseMaterial(new ImageBrush(bitmapImage));
                geometryModel.BackMaterial = geometryModel.Material;

                // Remove all LinesVisual3D objects
                for (int i = helixViewport.Children.Count - 1; i >= 0; i--)
                {
                    if (helixViewport.Children[i] is LinesVisual3D)
                    {
                        helixViewport.Children.RemoveAt(i);
                    }
                }
            }
        }

        private void InspectMode_Click(object sender, RoutedEventArgs e)
        {
            helixViewport.CameraController.CameraMode = CameraMode.Inspect;
        }

        private void WalkaroundMode_Click(object sender, RoutedEventArgs e)
        {
            helixViewport.CameraController.CameraMode = CameraMode.WalkAround;
        }

        private void FixedPositionMode_Click(object sender, RoutedEventArgs e)
        {
            helixViewport.CameraController.CameraMode = CameraMode.FixedPosition;
        }
    }
}
