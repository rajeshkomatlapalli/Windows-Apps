using Common.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class prop
    {
        public Stream Uri
        {
            get;
            set;
        }
        public BitmapImage source
        {
            get;
            set;
        }
        public string source1
        {
            get;
            set;
        }
    }

    public sealed partial class OnlineImages_New : Page
    {
        string type = string.Empty;
        AddShow addshow = new AddShow();
        public ObservableCollection<prop> images = new ObservableCollection<prop>();

        public OnlineImages_New()
        {
            this.InitializeComponent();
            Loaded += OnlineImages_New_Loaded;
        }

        void OnlineImages_New_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerAsync();
        }

        async void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                if (Constants.NavigationFromPhotoChooser == true)
                {
                    Constants.NavigationFromPhotoChooser = false;
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {                       
                        Frame.GoBack();
                    });
                }
                else
                {
                    if (Constants.OnlineImageUrls.Count() > 0)
                    {

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            notavailablemes.Visibility = Visibility.Collapsed;
                            Imagelist.Visibility = Visibility.Visible;
                            Imagelist.ItemsSource = images;
                            //progressbar.IsActive = true;
                        });
                        foreach (string url1 in Constants.OnlineImageUrls)
                        {
                            string url = url1;
                            if (!url.Contains("http:") && !url.Contains("https:"))
                            {
                                url = "http://" + url1;
                            }
                            prop p = new prop();
                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                p.source1 = url;
                                p.source = new BitmapImage();
                                p.source.CreateOptions = BitmapCreateOptions.None;
                            });
                            try
                            {
                                IRandomAccessStream streams = new InMemoryRandomAccessStream();
                                HttpClient http = new HttpClient();
                                HttpResponseMessage response = Task.Run(async () => await http.GetAsync(url)).Result;
                                if (response != null)
                                {
                                    MemoryStream mm = new MemoryStream();
                                    Stream Downloadstream = Task.Run(async () => await response.Content.ReadAsStreamAsync()).Result;
                                    Downloadstream.CopyTo(mm);
                                    p.Uri = Downloadstream;
                                    var randomAccessStream = new InMemoryRandomAccessStream();
                                    var outputStream = randomAccessStream.GetOutputStreamAt(0);
                                    var dw = new DataWriter(outputStream);
                                    var task = Task.Factory.StartNew(() => dw.WriteBytes(mm.ToArray()));
                                    await task;
                                    await dw.StoreAsync();
                                    await outputStream.FlushAsync();
                                    if (url.EndsWith(".gif") && ResourceHelper.AppName == Apps.Online_Education.ToString())
                                    {
                                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                        {
                                            images.Add(p);

                                            BitmapEncoder bmpEncoder = Task.Run(async () => await BitmapEncoder.CreateAsync(
                                       BitmapEncoder.JpegEncoderId,
                                       randomAccessStream)).Result;

                                            Task.Run(async () => await bmpEncoder.FlushAsync());                                            
                                            p.source.SetSource(randomAccessStream);
                                            p.source.ImageFailed += source_ImageFailed;
                                            p.source.ImageOpened += source_ImageOpened;
                                        });
                                    }
                                    else
                                    {
                                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                        {
                                            images.Add(p);

                                            p.source.SetSource(randomAccessStream);
                                            p.source.ImageFailed += source_ImageFailed;
                                            p.source.ImageOpened += source_ImageOpened;
                                        });
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                           
                        });
                    }
                    else
                    {
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            notavailablemes.Visibility = Visibility.Visible;

                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        void source_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (images.Count() > 0)
                notavailablemes.Visibility = Visibility.Collapsed;
        }

        void source_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            images.Remove(images.Where(i => i.source == (sender as BitmapImage)).FirstOrDefault());
            if (images.Count() == 0)
                notavailablemes.Visibility = Visibility.Visible;
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            if (e.Parameter != null)
            {
                type = e.Parameter.ToString();
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }       

        private async void lbxImages_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Constants.UserImage = (Stream)((prop)((sender as ListView).SelectedItem)).Uri;
            using (IRandomAccessStream fileStream = addshow.GetImageFromStorage(string.Empty, "SelectedImage.jpg"))
            {

                using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
                {
                    using (DataWriter dataWriter = new DataWriter(outputStream))
                    {
                        MemoryStream ms = new MemoryStream();
                        Constants.UserImage.Position = 0;
                        Constants.UserImage.CopyTo(ms);
                        dataWriter.WriteBytes(ms.ToArray());
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }
                    await outputStream.FlushAsync();
                }
            }
            string[] array = new string[2];
            array[0] = type;
            array[1] = AppSettings.ImageTitle.ToString();


            Frame.Navigate(typeof(PhotoChooser_New), array);
            Window.Current.Content = Frame;
            Window.Current.Activate();
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
