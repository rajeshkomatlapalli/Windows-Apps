using Common.Library;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
#if WINDOWS_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#endif
using Windows.Storage;
using Windows.Storage.Streams;
using System.Reflection;

using OnlineVideos.Data;
using Windows.Graphics.Imaging;

namespace Windows.Common.Data
{
   public class AddShow
    {
       public Dictionary<string, int> GetCat()
       {
           Dictionary<string, int> cats = new Dictionary<string, int>();
           cats.Add("--Choose Category--",0);
            cats.Add("Hindi",20);
            cats.Add("Telugu",18);
            cats.Add("Tamil",19);
            return cats;
       }
       public List<Stories> GetStories()
       {
           List<Stories> stories = Task.Run(async () => await Constants.connection.Table<Stories>().ToListAsync()).Result;
           return stories;
       }
       public List<CatageoryTable> GetCatageories()
       {
           List<CatageoryTable> catageory =Task.Run(async()=>await Constants.connection.Table<CatageoryTable>().ToListAsync()).Result;
           return catageory;
       }
       public List<ShowCategories> GetCatageoriesFromShowCategories()
       {
           List<ShowCategories> catageory = Task.Run(async () => await Constants.connection.Table<ShowCategories>().ToListAsync()).Result;
           return catageory;
       }
       public List<ShowList> GetShowList()
       {
           List<ShowList> showlist =Task.Run(async()=>await  Constants.connection.Table<ShowList>().ToListAsync()).Result;
           return showlist;
       }
       public List<CastRoles> CastRoles()
       {
           List<CastRoles> castroles =Task.Run(async()=>await  Constants.connection.Table<CastRoles>().ToListAsync()).Result;
           return castroles;
       }
       public List<CastProfile> CastProfile()
       {
           List<CastProfile> castprofile = Task.Run(async()=>await Constants.connection.Table<CastProfile>().ToListAsync()).Result;
           return castprofile;
       }
       public List<BlogCategoryTable> BlogCategoryTable()
       {
           List<BlogCategoryTable> blogcategorytable = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().ToListAsync()).Result;
           return blogcategorytable;
       }
       public List<ShowLinks> GetShowLinks()
       {
           List<ShowLinks> showlinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
           return showlinks;
       }
       public List<QuizQuestions> GetQuizQuestions()
       {
           List<QuizQuestions> quizquestions = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().ToListAsync()).Result;
           return quizquestions;
       }

       public IRandomAccessStream GetImageFromStorage(string FolderName, string title)
       {          
           if (title.Contains(".jpg"))
           {
               title = title.Substring(0, title.IndexOf('.'));
           }

           StorageFolder store = ApplicationData.Current.LocalFolder;
           StorageFolder story = store;
           if (!string.IsNullOrEmpty(FolderName))
               story = Task.Run(async () => await store.CreateFolderAsync("Images\\" + FolderName, CreationCollisionOption.OpenIfExists)).Result;
           var ss = Task.Run(async () => await story.CreateFileAsync(title + ".jpg", CreationCollisionOption.OpenIfExists)).Result;
           IRandomAccessStream stream = Task.Run(async () => await ss.OpenAsync(FileAccessMode.ReadWrite)).Result;
           return stream;
       }
		#if WINDOWS_APP || WINDOWS_PHONE_APP
       public async Task GetOnlineImages(string type, string imagename)
       {
           try
           {
               var bingKey = Constants.bingsearch(ResourceHelper.AppName);
               var authentication = string.Format("{0}:{1}", string.Empty, bingKey);
               var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(authentication));
               HttpWebRequest request = default(HttpWebRequest);
               //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.datamarket.azure.com/Bing/Search/v1/Composite?Sources=%27Image%27&Query=%27Yevadu%27&Adult=%27Off%27&ImageFilters=%27Size%3AMedium%27");
               if (ResourceHelper.AppName == "Online_Education.WindowsPhone" || ResourceHelper.AppName == Apps.Driving_Exam.ToString())
                   request = (HttpWebRequest)WebRequest.Create("https://api.datamarket.azure.com/Bing/Search/v1/Image?Query='" + imagename + "'&ImageFilters=%27Style%3aPhoto%2bSize%3aMedium%2bAspect%3aTall%27&$top=50&$format=Atom");
               else
                   request = (HttpWebRequest)WebRequest.Create("https://api.datamarket.azure.com/Bing/Search/v1/Image?Query='" + imagename + type + "'&ImageFilters=%27Style%3aPhoto%2bSize%3aMedium%2bAspect%3aTall%27&$top=50&$format=Atom");
               request.Method = "GET";
               request.Headers["Authorization"] = "Basic " + encodedKey;
               WebResponse response = await request.GetResponseAsync();
               StreamReader responseReader = new StreamReader(response.GetResponseStream());
               string responseStr = responseReader.ReadToEnd();
               Stream strm = new MemoryStream(Encoding.UTF8.GetBytes(responseStr));
               XElement MyXMLConfig = XElement.Load(strm);
               XNamespace atomNS = "http://www.w3.org/2005/Atom";
               XNamespace Img = "http://schemas.microsoft.com/ado/2007/08/dataservices";
               XNamespace met = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
               Constants.OnlineImageUrls = new ObservableCollection<string>(MyXMLConfig.Descendants(atomNS + "entry").Descendants(met + "properties").Elements().Where(i => i.Name == Img + "MediaUrl").Select(i => i.Value).ToList());

           }
           catch(Exception ex)
           {
               string excp = ex.Message;
               string[] array1 = new string[2];
               array1[0] = ex.StackTrace.ToString();
               array1[1] = ex.Source.ToString();
           }
       }
		#endif
    }
}