using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using ZplDesigner.Library;

namespace ZplDesigner.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ZplImageConverter _zplImageConverter;

        private void LoadImage_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _zplImageConverter = new ZplImageConverter(openFileDialog.FileName);
                RenderZplImage();
            }
                
        }

        private void Render_OnClick(object sender, RoutedEventArgs e)
        {
            RenderZplImage((int)DitheringSlider.Value);
        }

        private void DitherValue_OnChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (Slider) sender;
            var level = (int) slider.Value;
            RenderZplImage(level);
        }

        private void RenderZplImage(int ditheringLevel = 0)
        {
            if (_zplImageConverter == null)
            {
                return;
            }

            if (!int.TryParse(ScalePercentTextBox.Text, out var scale))
            {
                scale = 100;
            }
            OriginalImage.Source = BitmapToImageSource(_zplImageConverter.OriginalBmp);
            ZplText.Text = _zplImageConverter.ToZpl(out var previewBmp,scale,ditheringLevel);
            ReferenceImage.Source = BitmapToImageSource(previewBmp);
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            
            using MemoryStream memory = new MemoryStream();
            //PNG to support transparency
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }
    }
}
