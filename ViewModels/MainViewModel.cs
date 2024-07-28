using HelixToolkit.Wpf;
using MyHelixApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MyHelixApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string tileSizeText;
        public string TileSizeText
        {
            get => tileSizeText;
            set
            {
                tileSizeText = value;
                OnPropertyChanged(nameof(TileSizeText));
            }
        }

        public HelixViewport3D Viewport { get; private set; }

        public MainViewModel(HelixViewport3D viewport)
        {
            Viewport = viewport;
            viewport.Camera.Changed += (s, e) => UpdateTileSize();
            UpdateTileSize();
        }

        public void UpdateTileSize()
        {
            var point3D = new Point3D(0, 0, 0);
            double estimatedTileSize = GeoCoordinateConverter.EstimateTileSizeOnScreen(point3D, Viewport);
            TileSizeText = $"tile: {estimatedTileSize:F2} piksela";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
