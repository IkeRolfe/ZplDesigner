using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ZplDesigner.Library;
using RawPrinterUtilitiesLibrary;
using Label = RawPrinterUtilitiesLibrary.Label;

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
            _labelSpooler = new LabelSpooler();
            PrinterList.ItemsSource = _labelSpooler.GetAvailablePrinters();
        }

        private ZplImage _zplImage;
        private LabelSpooler _labelSpooler;

        private void LoadImage_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO custom open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //TODO: handle file type errors
            if (openFileDialog.ShowDialog() == true) {
                //Send file stream
                _zplImage = new ZplImage(openFileDialog.OpenFile());
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
            if (_zplImage == null)
            {
                return;
            }

            if (!int.TryParse(ScalePercentTextBox.Text, out var scale))
            {
                scale = 100;
            }
            OriginalImage.Source = BitmapToImageSource(_zplImage.OriginalBmp);
            ZplText.Text = _zplImage.ToZpl(out var previewBmp,scale,ditheringLevel);
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

        private void RenderHtml_OnClick(object sender, RoutedEventArgs e)
        {
            var html = HtmlText.Text;
            _zplImage = new ZplImage(html);
            RenderZplImage();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            _labelSpooler.AddLabel(new Label(ZplText.Text));
            _labelSpooler.PrintQueue();
        }

        private void PrinterList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var printerName = (string)e.AddedItems[0];
            _labelSpooler.PrinterName = printerName;
        }

        private async void LoadLabelary_OnClick(object sender, RoutedEventArgs e)
        {
            var image = await LabelaryClient.GetImage(ZplText.Text);
            LabelaryImage.Source = BitmapToImageSource(image);
        }
    }
}
