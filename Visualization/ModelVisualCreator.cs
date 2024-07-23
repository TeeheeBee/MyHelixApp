using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyHelixApp.Visualization
{
    public class ModelVisualCreator
    {
        public ModelVisual3D CreateModelVisual(MeshGeometry3D mesh, ImageBrush imageBrush)
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
    }
}
