﻿using HelixToolkit.Wpf;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Visualization
{
    public class ModelVisualCreator
    {

        public ModelVisual3D CreateModelVisualTexture(MeshGeometry3D mesh, ImageBrush imageBrush)
        {
            DiffuseMaterial textureMaterial = new DiffuseMaterial(imageBrush);

            GeometryModel3D geometryModel = new GeometryModel3D
            {
                Geometry = mesh,
                Material = textureMaterial,
                BackMaterial = textureMaterial // Apply material to the back faces as well
            };

            ModelVisual3D modelVisual = new ModelVisual3D
            {
                Content = geometryModel
            };

            return modelVisual;
        }

        public List<LinesVisual3D> CreateWireframe(List<Point3DCollection> triangleEdges)
        {
            List<LinesVisual3D> wireframe = new List<LinesVisual3D>();
            foreach (var edge in triangleEdges)
            {
                var wire = new LinesVisual3D
                {
                    Thickness = 1,
                    Color = Colors.Green,
                    Points = edge
                };
                wireframe.Add(wire);
            }
            return wireframe;
        }

        public ModelVisual3D CreateModelVisualWireframe()
        {
            GeometryModel3D geometryModel = new GeometryModel3D();
            geometryModel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));
            geometryModel.BackMaterial = null;
            ModelVisual3D modelVisual = new ModelVisual3D
            {
                Content = geometryModel
            };

            return modelVisual;
        }
    }
}
