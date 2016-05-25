using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AgiledgeSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Signature : Page
    {
        public Signature()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        Point pt1 = new Point();
        Point pt2 = new Point();
        private void myCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var myPointer = e.GetCurrentPoint(sender as Canvas);

            try
            {

                if (pt1.X == 0 && pt1.Y == 0) pt1 = myPointer.Position;

                Polyline pl = new Polyline();
                pl.Stroke = new SolidColorBrush(Colors.Blue);
                pl.StrokeThickness = 5;
                pl.Points.Add(pt1);

                pt2 = new Point(myPointer.Position.X, myPointer.Position.Y);

                pl.Points.Add(pt2);
                pt1 = pt2;

                myCanvas.Children.Add(pl);

            }
            catch (Exception ex)
            {
                //Catch any exception here
                var abc = ex.Message;
            }
        }

        private async void SaveSign()
        {
            Rect rect = new Rect(new Point(0, 0), myCanvas.RenderSize);
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(myCanvas);

            StorageFile signFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("signature.png", CreationCollisionOption.ReplaceExisting);
            Stream signFileStream = await signFile.OpenStreamForWriteAsync();

            IBuffer buffer = await rtb.GetPixelsAsync();
            byte[] rawBytes = new byte[buffer.Length];

            using (var reader = DataReader.FromBuffer(buffer))
            {
                reader.ReadBytes(rawBytes);
            }

            BitmapEncoder bmpenc = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, signFileStream.AsRandomAccessStream());
            bmpenc.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)rtb.PixelWidth, (uint)rtb.PixelHeight, 96, 96, rawBytes);
            try
            {
                await bmpenc.FlushAsync();
                signFileStream.Flush();
                signFileStream.Dispose();
                show.IsEnabled = true;
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveSign();
        }

        private async void show_Click(object sender, RoutedEventArgs e)
        {
            await showSign();
        }

        private async Task showSign()
        {
            BitmapImage bImage = new BitmapImage();

            StorageFolder myFolder = ApplicationData.Current.LocalFolder;
            StorageFile signFile = await myFolder.GetFileAsync("signature.png");

            var fileData = await signFile.OpenReadAsync();
            await bImage.SetSourceAsync(fileData);

            signImage.Source = bImage;
        }
    }
}
