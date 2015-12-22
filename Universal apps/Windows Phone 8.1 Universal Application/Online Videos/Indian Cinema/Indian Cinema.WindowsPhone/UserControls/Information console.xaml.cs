using OnlineMovies.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class Information_console : UserControl
    {
        string Youtube_Url = string.Empty;
        string play_url = string.Empty;
        string Vid_title = string.Empty;
        Uri url;

        public Information_console()
        {            
            try
            {
                this.InitializeComponent();
                Loaded += Information_console_Loaded;
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
            }
        }

        void Information_console_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        public void ShowPopup(string Url,string title)
        {
            try
            {
                PopUp_Message.IsOpen = true;
                play_url = Url;
                Vid_title = title;               
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
            }
        }

        public void ClosePopup()
        {
            PopUp_Message.IsOpen = false;
        }

        private async void download_video_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
           // var fianl_url = await MyToolkit.Multimedia.YouTube.GetVideoUriAsync(play_url, MyToolkit.Multimedia.YouTubeQuality.Quality480P);
           // string finale = fianl_url.Uri.AbsoluteUri.ToString();
            //string fin = fianl_url.ToString();
           // url = new Uri(finale);
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);            
            if (response.IsSuccessStatusCode)
            {
                var file = await response.Content.ReadAsInputStreamAsync();
                Stream str = file.AsStreamForRead();
                byte[] mybyte = ReadToEnd(str);
                //var file = await response.Content.ReadAsByteArrayAsync();
                StorageFile destinationFile = await KnownFolders.VideosLibrary.CreateFileAsync(Vid_title + ".mp4", CreationCollisionOption.ReplaceExisting);

                Windows.Storage.Streams.IRandomAccessStream stream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream output = stream.GetOutputStreamAt(0);

                DataWriter writer = new DataWriter(output);                
                writer.WriteBytes(mybyte);
                await writer.StoreAsync();
                await output.FlushAsync();
            }
        }

        private void Play_vid_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            p.Frame.Navigate(typeof(Youtube), play_url);
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            System.Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            System.Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }
                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    System.Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}