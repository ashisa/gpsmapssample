using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AgiledgeSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CapturePhoto : Page
    {
        MediaCapture captureManager = new MediaCapture();
        bool cameraInitialized = false;
        bool cameraPreviewing = false;
        public CapturePhoto()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            imagePreview.Visibility = Visibility.Collapsed; capturePreview.Visibility = Visibility.Visible;
            await initCamera();
            capturePreview.Source = captureManager;
            await captureManager.StartPreviewAsync();
            cameraPreviewing = true;
        }

        private async Task initCamera()
        {
            if (!cameraInitialized)
            {
                MediaCaptureInitializationSettings captureSettings = new MediaCaptureInitializationSettings();

                captureSettings.PhotoCaptureSource = PhotoCaptureSource.Photo;
                try
                {
                    await captureManager.InitializeAsync();
                    captureManager.SetPreviewRotation(VideoRotation.Clockwise180Degrees);
                    cameraInitialized = true;
                }
                catch
                {
                    cameraInitialized = false;
                }
            }
        }

        private async void stopPreview_Click(object sender, RoutedEventArgs e)
        {
            if (cameraPreviewing)
            {
                await captureManager.StopPreviewAsync();
                cameraPreviewing = false;
            }
        }
        private async void capturePhoto_Click(object sender, RoutedEventArgs e)
        {
            if (!cameraInitialized) await initCamera();

            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreatePng();

            // create storage file in local app storage
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                "TestPhoto.png",
                CreationCollisionOption.ReplaceExisting);

            // take photo
            await captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);

            // Get photo as a BitmapImage
            BitmapImage bmpImage = new BitmapImage(new Uri(file.Path));

            // imagePreview is a <Image> object defined in XAML
            imagePreview.Source = bmpImage;
            imagePreview.Visibility = Visibility.Visible;
            capturePreview.Visibility = Visibility.Collapsed;
        }

        private void selectPhoto_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectPhoto));
        }

    }
}
