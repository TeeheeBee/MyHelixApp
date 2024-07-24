using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using MyHelixApp.Mesh;
using MyHelixApp.Visualization;

namespace MyHelixApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml  !!!!
    /// </summary>
    public partial class MainWindow : Window
    {
        private int tiles = 16;
        private double size = 1.0;
        private const string imagePath = @"Images\earth.jpg"; //@"C:\Users\tbozic\source\repos\MyHelix3DApp\earth.jpg";
        private string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
        private bool showWireframe = false; // Toggle state
        private MeshGeometry3D mesh = new MeshGeometry3D();
        private List<Point3DCollection> triangleEdges;

        public MainWindow()
        {
            InitializeComponent();
            Setup3DScene();
            helixViewport.PanGesture = null;
            helixViewport.PanGesture2 = new MouseGesture(MouseAction.LeftClick);
        }

        private BitmapImage LoadBitmap(string fullPath)
        {
            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
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
            CreateMeshAndTriangles();
            ModelVisual3D meshModel = CreateTextureModel();
            helixViewport.Children.Add(meshModel);
        }

        private void CreateMeshAndTriangles()
        {
            double offset = tiles * size / 2.0;
            Random random = new Random();
            MeshGenerator meshGenerator = new MeshGenerator();
            mesh = meshGenerator.GeneratePositionsAndTextureCoordinates(tiles, size, offset, random);
            triangleEdges = meshGenerator.GenerateTriangleIndices(mesh, tiles);
        }

        private ModelVisual3D CreateTextureModel()
        {
            BitmapImage bitmapImage = LoadBitmap(fullPath);
            // Create the image brush
            ImageBrush imageBrush = new ImageBrush(bitmapImage);
            //wireframe = modelVisualCreator.CreateWireframe(mesh, triangleEdges);
            ModelVisualCreator modelVisualCreator = new ModelVisualCreator();
            // Create the ModelVisual3D
            return modelVisualCreator.CreateModelVisualTexture(mesh, imageBrush);
        }

        private void OnToggleViewButtonClick(object sender, RoutedEventArgs e)
        {
            showWireframe = !showWireframe;

            helixViewport.Children.Clear();
            helixViewport.Children.Add(new DefaultLights());

            if (showWireframe)
            {
                ModelVisualCreator modelVisualCreator = new ModelVisualCreator();
                ModelVisual3D meshModel = modelVisualCreator.CreateModelVisualWireframe();
                List<LinesVisual3D> wireframe = modelVisualCreator.CreateWireframe(triangleEdges);

                foreach (var wire in wireframe)
                {
                    helixViewport.Children.Add(wire);
                };
                helixViewport.Children.Add(meshModel);
            }
            else
            {
                ModelVisual3D meshModel = CreateTextureModel();
                helixViewport.Children.Add(meshModel);
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
