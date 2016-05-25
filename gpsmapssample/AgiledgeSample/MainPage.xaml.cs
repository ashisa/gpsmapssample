using DynamicRestProxy.PortableHttpClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AgiledgeSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geolocator geolocator;
        MapIcon mapIcon;
        bool tripStarted = false;
        MapPolyline tripRoute = new MapPolyline();
        DateTime timestamp1, timestamp2;
        List<BasicGeoposition> routePoints = new List<BasicGeoposition>();

        List<BasicGeoposition> mapElementList = new List<BasicGeoposition>();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                e.Handled = true;
            }
        }

        private async void testRest()
        {

            dynamic myRestClient = new DynamicRestClient("http://180.179.227.73/fsm/devicecommunicator");




            //RestClient restClient = new RestClient("http://180.179.227.73/fsm/devicecommunicator");
            //var request = new RestRequest("", Method.POST);

            //string json = "{ \"ACTION\": \"EMPLOYEE_TRACK\",\"STATUS\": \"" + status + "\",\"LATITUDE\":\"" + latitude + "\",\"LONGITUDE\":\"" + longitude + "\",\"NUANCE\":\"13000\",\"MAC_ADDRESS\":\"" + UDID + "\" , \"BATTERY\": \"" + bateryLevel + "%" + "\",   \"GPS\":\"" + gps + "\"}";
            //request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            //request.RequestFormat = DataFormat.Json;
            //restClient.ExecuteAsync(request, response =>
            //{

            //    if (ResponseStatus.Completed == response.ResponseStatus)
            //    {

            //        string x = response.Content;

            //    }
            //});

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            geolocator = new Geolocator();
            //geolocator.DesiredAccuracy = PositionAccuracy.High;
            //geolocator.MovementThreshold = 5;
            //Getting GPS fix
            Geoposition myPosition = await geolocator.GetGeopositionAsync();
            mapElementList.Add(myPosition.Coordinate.Point.Position);
            mapIcon = new MapIcon();
            mapIcon.Location = myPosition.Coordinate.Point;
            myMap.MapElements.Add(mapIcon);

            //Creating a list of positions and using that to creat Geopath element to be drawn on the map as the trip progresses
            routePoints.Add(myPosition.Coordinate.Point.Position);
            tripRoute.Path = new Geopath(routePoints);
            myMap.MapElements.Add(tripRoute);
            await myMap.TrySetViewAsync(myPosition.Coordinate.Point, 18D);

            //Creating customer locations (for sample purposes)
            await setCustomerLocation(myPosition);
            await myMap.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(mapElementList), new Thickness(50), MapAnimationKind.Linear);
 
        }

        private void tripControl_Click(object sender, RoutedEventArgs e)
        {
            string tripStatus = tripControl.Content.ToString();

            if (tripStatus == "Start Trip")
            {
                timestamp1 = DateTime.Now;
                startTime.Text = timestamp1.ToLocalTime().ToString();
                stopTime.Text = "In progress";
                tripControl.Content = "Stop Trip";
                geolocator.PositionChanged += Geolocator_PositionChanged;

            }
            else
            {
                timestamp2 = DateTime.Now;
                stopTime.Text = timestamp2.ToLocalTime().ToString() + "(" + (timestamp2 - timestamp1).TotalMinutes.ToString("F2") + ")";

                tripControl.Content = "Start Trip";
                geolocator.PositionChanged -= Geolocator_PositionChanged;
            }

        }

        private void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Geopoint myLocation = args.Position.Coordinate.Point;

            if (!tripStarted)
            {
                setMapPosition(args.Position);
            }
            else
            {
                setMapRoute(args.Position);
            }
        }

        private async void setMapPosition(Geoposition myPosition)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await myMap.TrySetViewAsync(myPosition.Coordinate.Point, 18D, myPosition.Coordinate.Heading, null, MapAnimationKind.Linear);
                tripStarted = true;
            });
        }

        private async void setMapRoute(Geoposition myPosition)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await myMap.TrySetViewAsync(myPosition.Coordinate.Point, myMap.ZoomLevel, myPosition.Coordinate.Heading, null);
            });

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                routePoints.Add(myPosition.Coordinate.Point.Position);
                tripRoute.Path = new Geopath(routePoints);
            });
        }

        private async Task setCustomerLocation(Geoposition myPosition)
        {
            StackPanel sp = new StackPanel();
            sp.Name = "sp";
            sp.Orientation = Orientation.Vertical;

            TextBlock tb = new TextBlock();
            tb.Name = "tb";
            tb.Foreground = new SolidColorBrush(Colors.Red);
            tb.Visibility = Visibility.Visible;
            tb.Text = "tap here";

            BitmapImage bImage = new BitmapImage();


            var imageResource = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PeoplePin.png"));
            await bImage.SetSourceAsync(await imageResource.OpenReadAsync());
            Image myImage = new Image();
            myImage.Name = "image";
            myImage.Source = bImage;
            myImage.Tapped += MyImage_Tapped;

            sp.Children.Add(tb);
            sp.Children.Add(myImage);
            myMap.Children.Add(sp);

            BasicGeoposition bgposition = new BasicGeoposition();
            bgposition.Latitude = myPosition.Coordinate.Point.Position.Latitude + .005;
            bgposition.Longitude = myPosition.Coordinate.Point.Position.Longitude + .005;
            mapElementList.Add(bgposition);
            MapControl.SetLocation(sp, new Geopoint(bgposition));
        }

        private void getSign_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Signature));
        }

        private void getPhoto_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CapturePhoto));
        }

        private void MyImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            var mapChildren = myMap.Children.ToList();
            foreach (UIElement uiE in mapChildren)
            {
                var uiEName = uiE.GetType().Name;
                if (uiEName == "StackPanel")
                {
                    if ((uiE as StackPanel).Name == "sp")
                    {
                        var test = (uiE as StackPanel).Children;
                        foreach (UIElement uiE2 in test)
                        {
                            if (uiE2.GetType().Name == "TextBlock")
                            {
                                timestamp2 = DateTime.Now;
                                (uiE2 as TextBlock).Text = "Customer Information";
                            }
                        }
                    }
                }
            }
        }
    }
}
