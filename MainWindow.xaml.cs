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

        private Dictionary<MeshGeometry3D, MeshGenerator.MeshInfo> meshIdentifiers = 
            new Dictionary<MeshGeometry3D, MeshGenerator.MeshInfo>();
        private int heights = 128;
        private double size = 256;
        private const string imagePath = @"Images\earth.jpg"; //@"C:\Users\tbozic\source\repos\MyHelix3DApp\earth.jpg";
        private string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
        private bool showWireframe = false; // Toggle state
        private MeshGeometry3D mesh = new MeshGeometry3D();
        private List<Point3DCollection> triangleEdges;

        public MainWindow()
        {
            InitializeComponent();

            Setup3DScene();
            //helixViewport.PanGesture = null;
            //helixViewport.PanGesture2 = new MouseGesture(MouseAction.LeftClick);
        }


        private void CreateMeshWithTextures()
        {
            MeshGeometry3D meshGeometry = new MeshGeometry3D();

            // Define vertices for four smaller quads
            Point3D[] positions = new Point3D[]
            {
                // First quad (bottom-left)
                new Point3D(0, 0, 0), new Point3D(1, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0),
                // Second quad (bottom-right)
                new Point3D(1, 0, 0), new Point3D(2, 0, 0), new Point3D(1, 1, 0), new Point3D(2, 1, 0),
                // Third quad (top-left)
                new Point3D(0, 1, 0), new Point3D(1, 1, 0), new Point3D(0, 2, 0), new Point3D(1, 2, 0),
                // Fourth quad (top-right)
                new Point3D(1, 1, 0), new Point3D(2, 1, 0), new Point3D(1, 2, 0), new Point3D(2, 2, 0)
            };

            // Define triangles for each quad
            Int32Collection triangleIndices = new Int32Collection
            {
                0, 2, 1, 1, 2, 3,   // First quad
                4, 6, 5, 5, 6, 7,   // Second quad
                8, 10, 9, 9, 10, 11, // Third quad
                12, 14, 13, 13, 14, 15 // Fourth quad
            };

            // Define UV coordinates for each quad
            PointCollection textureCoordinates = new PointCollection
            {
                // First quad
                new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1),
                // Second quad
                new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1),
                // Third quad
                new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1),
                // Fourth quad
                new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)
            };

            // Set vertices, UV coordinates and triangles in the mesh
            meshGeometry.Positions = new Point3DCollection(positions);
            meshGeometry.TriangleIndices = triangleIndices;
            meshGeometry.TextureCoordinates = textureCoordinates;

            // Load textures (for simplicity, using the same texture here)
            var bitmapImage = LoadBitmap(fullPath);

            // Create materials
            var material1 = new DiffuseMaterial(new ImageBrush(bitmapImage));
            var material2 = new DiffuseMaterial(new ImageBrush(bitmapImage));
            var material3 = new DiffuseMaterial(new ImageBrush(bitmapImage));
            var material4 = new DiffuseMaterial(new ImageBrush(bitmapImage));

            // Combine materials into one MaterialGroup
            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(material1);
            materialGroup.Children.Add(material2);
            materialGroup.Children.Add(material3);
            materialGroup.Children.Add(material4);

            // Create the model
            var geometryModel = new GeometryModel3D(meshGeometry, materialGroup);

            // Add the model to the viewport
            var modelGroup = new Model3DGroup();
            modelGroup.Children.Add(geometryModel);

            var modelVisual = new ModelVisual3D
            {
                Content = modelGroup
            };

            helixViewport.Children.Add(modelVisual);


            //var meshBuilder = new MeshBuilder();

            //// Define the vertices and UV coordinates
            //var positions = new Point3DCollection
            //{
            //    new Point3D(0, 0, 0), new Point3D(1, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0),
            //    new Point3D(1, 0, 0), new Point3D(2, 0, 0), new Point3D(1, 1, 0), new Point3D(2, 1, 0),
            //    new Point3D(0, 1, 0), new Point3D(1, 1, 0), new Point3D(0, 2, 0), new Point3D(1, 2, 0),
            //    new Point3D(1, 1, 0), new Point3D(2, 1, 0), new Point3D(1, 2, 0), new Point3D(2, 2, 0)
            //};

            //var textureCoordinates = new PointCollection
            //{
            //    new Point(0, 0), new Point(0.5, 0), new Point(0, 0.5), new Point(0.5, 0.5),
            //    new Point(0, 0), new Point(0.5, 0), new Point(0, 0.5), new Point(0.5, 0.5),
            //    new Point(0, 0), new Point(0.5, 0), new Point(0, 0.5), new Point(0.5, 0.5),
            //    new Point(0, 0), new Point(0.5, 0), new Point(0, 0.5), new Point(0.5, 0.5)
            //};

            //// Define the triangles
            //var triangleIndices = new Int32Collection
            //{
            //    0, 2, 1, 1, 2, 3, 4, 6, 5, 5, 6, 7, 8, 10, 9, 9, 10, 11, 12, 14, 13, 13, 14, 15
            //};

            //var meshGeometry = new MeshGeometry3D
            //{
            //    Positions = positions,
            //    TextureCoordinates = textureCoordinates,
            //    TriangleIndices = triangleIndices
            //};

            //BitmapImage bitmapImage = LoadBitmap(fullPath);

            //// Load textures
            //var image1 = bitmapImage;// new BitmapImage(new Uri("pack://application:,,,/Resources/texture1.png"));
            //var image2 = bitmapImage;//  new BitmapImage(new Uri("pack://application:,,,/Resources/texture2.png"));
            //var image3 = bitmapImage;//  new BitmapImage(new Uri("pack://application:,,,/Resources/texture3.png"));
            //var image4 = bitmapImage;//  new BitmapImage(new Uri("pack://application:,,,/Resources/texture4.png"));

            //// Create materials
            //var material1 = new DiffuseMaterial(new ImageBrush(image1));
            //var material2 = new DiffuseMaterial(new ImageBrush(image2));
            //var material3 = new DiffuseMaterial(new ImageBrush(image3));
            //var material4 = new DiffuseMaterial(new ImageBrush(image4));

            //// Combine materials into one MaterialGroup
            //var materialGroup = new MaterialGroup();
            //materialGroup.Children.Add(material1);
            //materialGroup.Children.Add(material2);
            //materialGroup.Children.Add(material3);
            //materialGroup.Children.Add(material4);

            //// Create the model
            //var geometryModel = new GeometryModel3D(meshGeometry, materialGroup);

            //// Add the model to the viewport
            //var modelGroup = new Model3DGroup();
            //modelGroup.Children.Add(geometryModel);

            //var modelVisual = new ModelVisual3D
            //{
            //    Content = modelGroup
            //};

            //helixViewport.Children.Add(modelVisual);
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
                Position = new Point3D(0, 0, size),
                LookDirection = new Vector3D(0, 0, -size),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 90

            };
            // Add default lights
            helixViewport.Children.Add(new DefaultLights());
            
            
            //CreateMeshWithTextures();


            CreateMeshAndTriangles();
            ModelVisual3D meshModel = CreateTextureModel();


            //CreateGlobeMesh();
            //ModelVisual3D meshModel = CreateTextureModel();
            ////CreateMeshAndTriangles();


            helixViewport.Children.Add(meshModel);
        }

        private void h_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Convert mouse position to a Point3D
            Point mousePos = e.GetPosition(helixViewport);
            FindHitPoint(mousePos);

            //if (hitPoint != null)
            //{
            //    MessageBox.Show($"Hit point: {hitPoint}");
            //}
        }

        private void FindHitPoint(Point mousePos)
        {
            // Perform the hit test
            var hitParams = new PointHitTestParameters(mousePos);

            VisualTreeHelper.HitTest(helixViewport, null, (hit) =>
            {
                var rayMeshResult = hit as RayMeshGeometry3DHitTestResult;
                if (rayMeshResult != null)
                {
                    Point3D pointHit = rayMeshResult.PointHit;
                    MeshGeometry3D meshHit = rayMeshResult.MeshHit;
                    //GeometryModel3D modelHit = rayMeshResult.ModelHit as GeometryModel3D;
                    if (meshHit != null && meshIdentifiers.TryGetValue(meshHit, out MeshGenerator.MeshInfo info))
                    {
                        // Display the results with the unique identifier for the mesh
                        string resultMessage = $"Hit point: {pointHit}\nMesh ID: {info.Id}\nCoordinates: {info.Coordinates}";
                        MessageBox.Show(resultMessage);
                    }
                    return HitTestResultBehavior.Stop;
                }
                return HitTestResultBehavior.Continue;
            }, hitParams);
        }

        private void CreateGlobeMesh()
        {
            MeshGlobeGenerator meshGlobeG = new MeshGlobeGenerator();
            mesh = meshGlobeG.CreateGlobeMesh(10);
            
        }

        private void CreateMeshAndTriangles()
        {
            MeshGenerator meshGenerator = new MeshGenerator();
            mesh = meshGenerator.GeneratePositionsAndTextureCoordinates
                (new Point3D(0,-1, 0), heights, size);
            MeshGenerator.MeshInfo info = meshGenerator.SetMeshInfo(1, new Point3D(0, -1, 0));
            meshIdentifiers[mesh] = info;
            triangleEdges = meshGenerator.GenerateTriangleIndices(mesh, heights);
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
