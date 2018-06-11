using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ImgVwr.Models;

namespace ImgVwr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Engine _engine;

        public MainWindow(string currentFile = null)
        {
            _engine = new Engine(currentFile);

            InitializeComponent();

            if (!string.IsNullOrEmpty(currentFile))
            {
                SetImage(_engine.LoadImage(currentFile));
            }
            else
            {
                SetImage(_engine.Next());
            }

            RegisterAllEvents();
            SetUpImageZoom();
        }

        private void RegisterAllEvents()
        {
            KeyUp += (sender, args) =>
            {
                if (args.Key == Key.Left)
                {
                    SetImage(_engine.Previous());
                }
                if (args.Key == Key.Right)
                {
                    SetImage(_engine.Next());
                }
            };

            MouseWheel += MainWindow_OnMouseWheel;
        }

        private void SetImage(ImageModel model)
        {
            if (model?.ImageSource != null)
            {
                ImageHolder.Source = model.ImageSource;

                Title = model.Name;
            }
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_engine.IsValid())
            {
                var transformGroup = (TransformGroup)ImageHolder.RenderTransform;
                var transform = (ScaleTransform)transformGroup.Children[0];

                var zoom = e.Delta > 0 ? .2 : -.2;
                transform.ScaleX += zoom;
                transform.ScaleY += zoom;

                var position = e.GetPosition(ImageHolder);
                ImageHolder.RenderTransformOrigin = new Point(position.X / ImageHolder.ActualWidth,
                    position.Y / ImageHolder.ActualHeight);
            }
        }

        private void SetUpImageZoom()
        {
            var group = new TransformGroup();

            var xform = new ScaleTransform();
            group.Children.Add(xform);

            var tt = new TranslateTransform();
            group.Children.Add(tt);

            ImageHolder.RenderTransform = group;
        }
    }
}
