using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AgiledgeSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectPhoto : Page
    {
        List<imageClass> imageList = new List<imageClass>();
        public SelectPhoto()
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
            StorageFolder picFolder = KnownFolders.PicturesLibrary;
            var files = await picFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByDate);
            Debug.WriteLine(files.ToList().Count);
            foreach (StorageFile file in files)
            {
                if (file.Path.Contains(KnownFolders.CameraRoll.Path))
                {
                    BitmapImage bimage = new BitmapImage();
                    var thumbnail = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView, 100);
                    await bimage.SetSourceAsync(thumbnail);
                    imageList.Add(new imageClass() { Name = file.Name, bitmapImage = bimage, imagePath = file.Path });
                }
            }
            myList.ItemsSource = imageList;
        }
        public class imageClass
        {
            public string Name { get; set; }
            public string imagePath { get; set; }
            public BitmapImage bitmapImage { get; set; }
        }

        private async void myList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string filePath = (e.ClickedItem as imageClass).imagePath;
            Debug.WriteLine(filePath);

            StorageFile file2upload = await StorageFile.GetFileFromPathAsync(filePath);
            //var fileUploadUrl = @"http://<IPaddress>:<port>/fileupload";
            var fileUploadUrl = @"https://encodable.com/uploaddemo/files/cool";

            var client = new HttpClient();
            var request = new HttpMultipartFormDataContent();
            var fileContent = await file2upload.OpenReadAsync();
            var requestContent = new HttpStreamContent(fileContent);

            requestContent.Headers.Add("Content-type", fileContent.ContentType);

            request.Add(requestContent, file2upload.Name);

            HttpResponseMessage status = await client.PostAsync(new Uri(fileUploadUrl, UriKind.RelativeOrAbsolute), request);
            var statusMessage = status.StatusCode;
        }

    }
}
