using Common.Library;
//using OnlineVideos.Library;
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
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{

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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnlineImages : Page
    {
        string type = string.Empty;
        AddShow addshow = new AddShow();
        public ObservableCollection<prop> images = new ObservableCollection<prop>();

        public OnlineImages()
        {
            this.InitializeComponent();
            Loaded += OnlineImages_Loaded;
        }

        void OnlineImages_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += bg_DoWork;
                bg.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnlineImages.xaml.cs page OnlineImages_Loaded method", ex);
            }
        }

        async void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Constants.NavigationFromPhotoChooser == true)
                {
                    Constants.NavigationFromPhotoChooser = false;
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                        frame.GoBack();
                    });
                }
                else
                {
                    if (Constants.OnlineImageUrls.Count() > 0)
                    {

                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            notavailablemes.Visibility = Visibility.Collapsed;
                            Imagelist.Visibility = Visibility.Visible;
                            Imagelist.ItemsSource = images;
                            progressbar.IsActive = true;
                        });
                        foreach (string url1 in Constants.OnlineImageUrls)
                        {
                            string url = url1;
                            if (!url.Contains("http:") && !url.Contains("https:"))
                            {
                                url = "http://" + url1;
                            }
                            prop p = new prop();
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
                                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                        {
                                            images.Add(p);

                                            BitmapEncoder bmpEncoder = Task.Run(async () => await BitmapEncoder.CreateAsync(
                                       BitmapEncoder.JpegEncoderId,
                                       randomAccessStream)).Result;

                                            //bmpEncoder.SetPixelData(
                                            //    BitmapPixelFormat.Bgra8,
                                            //    BitmapAlphaMode.Straight,
                                            //     (uint)Math.Sqrt((pixels.Length) / 4),
                                            //    (uint)Math.Sqrt((pixels.Length) / 4),
                                            //    decoder.DpiX,
                                            //    decoder.DpiY,
                                            //    pixels);

                                            Task.Run(async () => await bmpEncoder.FlushAsync());

                                            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                                            //StorageFile storageFile = Task.Run(async () => await localFolder.CreateFileAsync("sdsd.jpg", CreationCollisionOption.ReplaceExisting)).Result;

                                            //using (IRandomAccessStream fileStream = Task.Run(async () => await storageFile.OpenAsync(FileAccessMode.ReadWrite)).Result)
                                            //{

                                            //    using (IOutputStream outputStream4 = fileStream.GetOutputStreamAt(0))
                                            //    {
                                            //        using (DataWriter dataWriter = new DataWriter(outputStream4))
                                            //        {
                                            //            Stream ms = randomAccessStream.AsStream();
                                            //            MemoryStream sty=new MemoryStream();
                                            //            ms.CopyToAsync(sty);
                                            //            dataWriter.WriteBytes(sty.ToArray());
                                            //            Task.Run(async()=>await dataWriter.StoreAsync());
                                            //            //dataWriter.DetachStream();
                                            //        }
                                            //        Task.Run(async()=>await outputStream4.FlushAsync());
                                            //    }
                                            //}
                                            p.source.SetSource(randomAccessStream);
                                            p.source.ImageFailed += bi_ImageFailed;
                                            p.source.ImageOpened += source_ImageOpened;
                                        });
                                    }
                                    else
                                    {
                                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                        {
                                            images.Add(p);

                                            p.source.SetSource(randomAccessStream);
                                            p.source.ImageFailed += bi_ImageFailed;
                                            p.source.ImageOpened += source_ImageOpened;
                                        });
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            progressbar.IsActive = false;
                        });
                    }
                    else
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            notavailablemes.Visibility = Visibility.Visible;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnlineImages.xaml.cs page bg_dowork method", ex);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter != null)
                {
                    type = e.Parameter.ToString();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnlineImages.xaml.cs page OnNavigatedTo method", ex);
            }
        }

        void source_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (images.Count() > 0)
                notavailablemes.Visibility = Visibility.Collapsed;
            //if ((sender as BitmapImage).PixelHeight < 60 || (sender as BitmapImage).PixelWidth < 60)
            //{
            //images.Remove(images.Where(i => i.source == (sender as BitmapImage)).FirstOrDefault());
            //}
        }

        void bi_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            images.Remove(images.Where(i => i.source == (sender as BitmapImage)).FirstOrDefault());
            if (images.Count() == 0)
                notavailablemes.Visibility = Visibility.Visible;
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            frame.GoBack();
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
            //Frame.Navigate(typeof(PhotoChooser), array);

            var assemblyname = this.GetType().GetTypeInfo().Assembly.FullName;
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(assemblyname);
            frame.Navigate(typeof(PhotoChooser), array);
            Window.Current.Content = frame;
            Window.Current.Activate();
        }
    }
}