using Common.Library;
using InsertIntoDataBase;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace PicasaMobileInterface
{
    public class BlogDetails
    {
        public string title
        {
            get;
            set;
        }
        public string url
        {
            get;
            set;
        }

    }
    public class UploadToBlog
    {
        public int personcount = 0;
        public int numberofiterations = 0;
        public int numberofiterationscompleted = 0;
        string foldername = string.Empty;
        public IAsyncResult asynchronousResults = default(IAsyncResult);
        public HttpWebRequest requests = default(HttpWebRequest);
        public bool Dependent = false;
        public string Desc = string.Empty;
        public Page page = default(Page);
        public bool agent = false;
        public string BlogName = string.Empty;
        public string PostTitle = string.Empty;
        public string PostType = string.Empty;
        public StringBuilder XMLData;
        public int ShowID = default(int);
        public string Catname = string.Empty;
        public string ImageUrl = string.Empty;
        public string DefaultUrl = string.Empty;
        public AutoResetEvent auto1 = new AutoResetEvent(false);
        public AutoResetEvent auto = new AutoResetEvent(false);
        public List<ShareTables> TablesList = new List<ShareTables>();
        public string UserName = string.Empty;
        public string Password = string.Empty;

        public UploadToBlog(int showid, string catname = "", string Description = "", string Image = "", Page page1 = default(Page))
        {
            page = page1;
            Catname = catname;
            ShowID = showid;
            Desc = Description;
            if (!string.IsNullOrEmpty(Image))
            {
                ImageUrl = Image;
                agent = true;
            }
            if (ResourceHelper.AppName == Apps.Social_Celebrities.ToString())
                agent = false;
            PicasaLogin();
        }

        public void PicasaLogin()
        {
            try
            {
                DataManager<ShareTables> datamanager = new DataManager<ShareTables>();
                TablesList = datamanager.GetListData(i => i.ID == i.ID);
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += bg_DoWork;
                bg.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PicasaLogin Method In UploadToBlog.cs file", ex);
            }
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (ResourceHelper.AppName != Apps.Web_Tile.ToString() && ResourceHelper.AppName != Apps.Video_Mix.ToString() && ResourceHelper.AppName != Apps.Social_Celebrities.ToString())
                //if (AppSettings.ProjectName == "Indian Cinema")
                {
                    BlogCategoryTable blogs = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogType == "shows" || i.BlogType == "Shows").FirstOrDefaultAsync()).Result;
                    PicasaInterface pi = new PicasaInterface(blogs.BlogUserName, blogs.BlogPassword);

                    //ImageDic.Add("scale-100", string.Empty);
                    //ImageDic.Add("scale-140", string.Empty);
                    //ImageDic.Add("scale-180", string.Empty);
                    //ImageDic.Add("ListImages", string.Empty);
                    //ImageDic.Add("TileImages\\30-30", string.Empty);
                    //ImageDic.Add("TileImages\\150-150", string.Empty);
                    //while (ImageDic.Where(i => i.Value == string.Empty).FirstOrDefault().Value == string.Empty)
                    //{

                    //    foldername = ImageDic.Where(i => i.Value == string.Empty).FirstOrDefault().Key.ToString();
                    uploadImage();
                    //}
                    foreach (var s in TablesList.OrderByDescending(i => i.Type).GroupBy(i => i.Type))
                    {
                        string typ = s.FirstOrDefault().Type.ToLower();
                        BlogCategoryTable BlogDetails = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().Where(g => g.BlogType == typ).FirstOrDefaultAsync()).Result;
                        BlogName = BlogDetails.BlogName;
                        UserName = BlogDetails.BlogUserName;
                        Password = BlogDetails.BlogPassword;
                        PostType = s.FirstOrDefault().Type;
                        if (s.FirstOrDefault().DependentTable != null)
                            Dependent = true;
                        BloggerInterface BI = new BloggerInterface(UserName, Password);
                        uploadPost();

                    }
                }
                else
                {
                    if (agent == false)
                    {
                        BlogCategoryTable blogs = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogCategory == Catname).FirstOrDefaultAsync()).Result;
                        PicasaInterface pi = new PicasaInterface(blogs.BlogUserName, blogs.BlogPassword);
                        uploadImage();
                    }
                    foreach (var s in TablesList.OrderByDescending(i => i.Type).GroupBy(i => i.Type))
                    {
                        PostType = s.FirstOrDefault().Type;
                        BlogCategoryTable cattable = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogCategory == Catname).FirstOrDefaultAsync()).Result;
                        BlogName = cattable.BlogName;
                        UserName = cattable.BlogUserName;
                        Password = cattable.BlogPassword;
                        if (s.FirstOrDefault().DependentTable != null)
                            Dependent = true;
                        BloggerInterface BI = new BloggerInterface(UserName, Password);
                        uploadPost();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in bg_DoWork Method In UploadToBlog.cs file", ex);
            }
        }

        public void uploadPost()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.blogger.com/feeds/default/blogs/");
                request.Method = "GET";
                request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_BToken;
                request.BeginGetResponse(new AsyncCallback(RequestBlogCompleted), request);
                auto.WaitOne();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in uploadPost Method In UploadToBlog.cs file", ex);
            }
        }

        private void RequestBlogCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                //BlogCategoryTable cattable = Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogCategory == Catname).FirstOrDefaultAsync().Result;
                string Title = BlogName;
                string AttributeValue = "http://schemas.google.com/g/2005#post";
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                IEnumerable<BlogDetails> blogdetail = from item in MyXMLConfig.Descendants(atomNS + "entry")
                                                      select new BlogDetails
                                                      {
                                                          title = item.Element(atomNS + "title").Value,
                                                          url = item.Elements(atomNS + "link").Where(a => a.Attribute("rel").Value == AttributeValue).Select(a => a.Attribute("href").Value).FirstOrDefault(),
                                                      };
                foreach (BlogDetails bb in blogdetail)
                {
                    if (Title == bb.title)
                    {
                        DefaultUrl = bb.url;
                        break;
                    }
                }
                UploadItem();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RequestBlogCompleted Method In UploadToBlog.cs file", ex);
            }
        }

        public void GetShow(Type TableType, string ColoumnName, string ColoumnValue, object queryableData, object TableInstance)
        {
            try
            {
                ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
                System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName));
                System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetRuntimeProperty(ColoumnName).PropertyType, null));
                System.Linq.Expressions.Expression e1 = System.Linq.Expressions.Expression.Equal(left, right);
                var comparison = System.Linq.Expressions.Expression.Equal(left, right);
                var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
                var whereCallExpression = System.Linq.Expressions.Expression.Lambda(delegateType, comparison, pe);
                var d2 = typeof(DataManager<>);
                Type[] typeArgs2 = { TableType };
                var makeme2 = d2.MakeGenericType(typeArgs2);
                object o2 = Activator.CreateInstance(makeme2);

                IEnumerable<object> results = (IEnumerable<object>)o2.GetType().GetTypeInfo().GetDeclaredMethod("GetListData").Invoke(o2, new object[] { whereCallExpression });
                foreach (var result in results)
                {
                    if (PostTitle == string.Empty)
                    {
                        if (ResourceHelper.AppName == Apps.Web_Tile.ToString() || ResourceHelper.AppName == Apps.Video_Mix.ToString())
                            PostTitle = result.GetType().GetRuntimeProperty("Title").GetValue(result, null).ToString();
                        else
                            PostTitle = result.GetType().GetRuntimeProperty(ColoumnName).GetValue(result, null).ToString();
                    }
                    var InsertType = typeof(InsertData<>);
                    Type[] Typeargs = { TableType };
                    var makeme = InsertType.MakeGenericType(Typeargs);
                    object InsertInstance = Activator.CreateInstance(makeme);
                    XMLData.Append(InsertInstance.GetType().GetTypeInfo().GetDeclaredMethod("EntityToXML").Invoke(InsertInstance, new object[] { result }).ToString());
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in GetShow Method In UploadToBlog.cs file", ex);
            }
        }
        public void UploadItem()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DefaultUrl);
                request.Method = "POST";
                request.ContentType = "application/atom+xml";
                request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_BToken;
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackItem), request);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadItem Method In UploadToBlog.cs file", ex);
            }
        }

        private void GetRequestStreamCallbackItem(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                if (Dependent == false)
                {
                    personcount = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == ShowID).CountAsync()).Result;
                    XMLData = new StringBuilder("<NewDataSet>");
                    foreach (ShareTables st in TablesList.Where(i => i.Type == PostType).OrderBy(i => i.TableOrder))
                    {
                        if (st.TableName != "ShowImages" && st.TableName != "StoryImages" && st.TableName != "QuizImages")
                        //if (st.TableName != "ShowImages")
                        {
#if WINDOWS_APP
                        Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideosWin8.Entities"));
#endif
#if WINDOWS_PHONE_APP
                            Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
                            Type TableType = DataAssembly.GetType("OnlineVideos.Entities." + st.TableName.Trim());
                            object TableInstance = Activator.CreateInstance(TableType);
                            var d1 = typeof(DataManager<>);
                            Type[] typeArgs = { TableType };
                            var makeme = d1.MakeGenericType(typeArgs);
                            object o = Activator.CreateInstance(makeme);
                            var queryableData = o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").Invoke(o, null);
                            if (st.TableName == "ShowCategories")
                            {
                                int showid = AppSettings.ShowUniqueID;
                                CategoriesByShowID cs = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
                                ShowCategories sc = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryID == cs.CatageoryID).FirstOrDefaultAsync()).Result;
                                GetShow(TableType, st.WhereCondition, sc.CategoryID.ToString(), queryableData, TableInstance);
                            }
                            else
                            {
                                GetShow(TableType, st.WhereCondition, ShowID.ToString(), queryableData, TableInstance);
                            }
                        }
                        else if (st.TableName == "StoryImages")
                        {
                            OnlineVideos.Entities.StoryImages storyimages = new OnlineVideos.Entities.StoryImages();
                            storyimages.ShowID = ShowID.ToString();
                            //storyimages.FlickrStoryImageUrl=
                            List<Stories> ss = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
                            foreach (var s in ss)
                            {
                                string num = s.Image;
                                if (!string.IsNullOrEmpty(num))
                                {
                                    storyimages.ImageNo = num.Substring(0, num.IndexOf(".jpg"));
                                    storyimages.FlickrStoryImageUrl = s.FlickrStoryImageUrl;
                                    XDocument xmlDoc;
                                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(OnlineVideos.Entities.StoryImages));
                                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                                    ns.Add("", "");
                                    using (MemoryStream xmlStream = new MemoryStream())
                                    {
                                        xmlSerializer.Serialize(xmlStream, storyimages, ns);
                                        xmlStream.Position = 0;
                                        xmlDoc = XDocument.Load(xmlStream);
                                        XMLData.Append(xmlDoc.ToString());
                                    }
                                }

                            }
                        }
                        else if (st.TableName == "QuizImages")
                        {
                            OnlineVideos.Entities.QuizImages quizimages = new OnlineVideos.Entities.QuizImages();
                            //List<QuizImages> quizimages = Task.Run(async () => await Constants.connection.Table<QuizImages>().ToListAsync()).Result;
                            quizimages.ShowID = ShowID.ToString();
                            //storyimages.FlickrStoryImageUrl=
                            List<QuizQuestions> ss = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
                            foreach (var s in ss)
                            {
                                string num = s.Image;
                                if (!string.IsNullOrEmpty(num))
                                {
                                    quizimages.ImageNo = num;
                                    quizimages.FlickrQuizImageUrl = s.FlickrQuizImageUrl;
                                    XDocument xmlDoc;
                                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(OnlineVideos.Entities.QuizImages));
                                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                                    ns.Add("", "");
                                    using (MemoryStream xmlStream = new MemoryStream())
                                    {
                                        xmlSerializer.Serialize(xmlStream, quizimages, ns);
                                        xmlStream.Position = 0;
                                        xmlDoc = XDocument.Load(xmlStream);
                                        XMLData.Append(xmlDoc.ToString());
                                    }
                                }
                            }
                        }
                        if (st.TableName == "ShowImages")
                        {
                            OnlineVideos.Entities.ShowImages showimages = new OnlineVideos.Entities.ShowImages();
                            showimages.ShowID = ShowID.ToString();
                            showimages.TileImage = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result.TileImage;
                            showimages.PivotImage = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result.PivotImage;
                            showimages.FlickrImageUrl = ImageUrl;
                            showimages.FlickrPivotImageUrl = string.Empty;
                            XDocument xmlDoc;
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OnlineVideos.Entities.ShowImages));
                            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                            ns.Add("", "");
                            using (MemoryStream xmlStream = new MemoryStream())
                            {
                                xmlSerializer.Serialize(xmlStream, showimages, ns);
                                xmlStream.Position = 0;
                                xmlDoc = XDocument.Load(xmlStream);
                                XMLData.Append(xmlDoc.ToString());
                            }
                        }
                    }
                    XMLData.Append("</NewDataSet>");
                    BlogData(asynchronousResult, request);
                }
                else
                {
                    ShareTables sharetable = TablesList.Where(i => i.Type == PostType).OrderBy(i => i.TableOrder).FirstOrDefault();
#if WINDOWS_APP
                    Assembly DataAssembly1 = Assembly.Load(new AssemblyName("OnlineVideosWin8.Entities"));
#endif
#if WINDOWS_PHONE_APP
                    Assembly DataAssembly1 = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
                    Type TableType1 = DataAssembly1.GetType("OnlineVideos.Entities." + sharetable.DependentTable.Trim());
                    object TableInstance1 = Activator.CreateInstance(TableType1);
                    ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType1, "i");
                    System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType1.GetRuntimeProperty(sharetable.DependentWhereCondition));
                    System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ShowID, TableType1.GetRuntimeProperty(sharetable.DependentWhereCondition).PropertyType, null));
                    System.Linq.Expressions.Expression e1 = System.Linq.Expressions.Expression.Equal(left, right);
                    var comparison = System.Linq.Expressions.Expression.Equal(left, right);
                    var delegateType = typeof(Func<,>).MakeGenericType(TableType1, typeof(bool));
                    var whereCallExpression = System.Linq.Expressions.Expression.Lambda(delegateType, comparison, pe);
                    var d2 = typeof(DataManager<>);
                    Type[] typeArgs2 = { TableType1 };
                    var makeme2 = d2.MakeGenericType(typeArgs2);
                    object o2 = Activator.CreateInstance(makeme2);

                    IEnumerable<object> results = (IEnumerable<object>)o2.GetType().GetTypeInfo().GetDeclaredMethod("GetListData").Invoke(o2, new object[] { whereCallExpression });
                    foreach (var result in results)
                    {
                        numberofiterations++;
                        Convert.ChangeType(result, TableType1);
                        XMLData = new StringBuilder("<NewDataSet>");
                        foreach (var st in TablesList.Where(i => i.Type == PostType).OrderBy(i => i.TableOrder).GroupBy(i => i.TableName))
                        {
                            if (st.FirstOrDefault().TableName != "PersonImages")
                            {
#if WINDOWS_APP
                            Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideosWin8.Entities"));
#endif
#if WINDOWS_PHONE_APP
                                Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
                                Type TableType = DataAssembly.GetType("OnlineVideos.Entities." + st.FirstOrDefault().TableName.Trim());
                                object TableInstance = Activator.CreateInstance(TableType);
                                var d1 = typeof(DataManager<>);
                                Type[] typeArgs = { TableType };
                                var makeme = d1.MakeGenericType(typeArgs);
                                object o = Activator.CreateInstance(makeme);
                                var queryableData = o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").Invoke(o, null);
                                GetShow(TableType, st.FirstOrDefault().WhereCondition, result.GetType().GetRuntimeProperty(st.FirstOrDefault().WhereCondition).GetValue(result, null).ToString(), queryableData, TableInstance);
                            }
                            else
                            {
                                OnlineVideos.Entities.PersonImages personimages = new OnlineVideos.Entities.PersonImages();
                                personimages.PersonID = Convert.ToInt32(result.GetType().GetRuntimeProperty(st.FirstOrDefault().WhereCondition).GetValue(result, null).ToString());
                                if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personimages.PersonID).FirstOrDefaultAsync()).Result != null)
                                    personimages.FlickrPersonImageUrl = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personimages.PersonID).FirstOrDefaultAsync()).Result.FlickrPersonImageUrl;
                                else
                                    personimages.FlickrPersonImageUrl = "http://www.bridge-bd.com/images/no-image-small.png";
                                XDocument xmlDoc;
                                XmlSerializer xmlSerializer = new XmlSerializer(typeof(OnlineVideos.Entities.PersonImages));
                                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                                ns.Add("", "");
                                using (MemoryStream xmlStream = new MemoryStream())
                                {
                                    xmlSerializer.Serialize(xmlStream, personimages, ns);
                                    xmlStream.Position = 0;
                                    xmlDoc = XDocument.Load(xmlStream);
                                    XMLData.Append(xmlDoc.ToString());
                                }
                            }
                        }
                        XMLData.Append("</NewDataSet>");
                        lock (this)
                        {
                            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(DefaultUrl);
                            webrequest.Method = "POST";
                            webrequest.ContentType = "application/atom+xml";
                            webrequest.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_BToken;
                            webrequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackItems), webrequest);
                            Monitor.Wait(this);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRequestStreamCallbackItem Method In UploadToBlog.cs file", ex);
            }
        }
        private void GetRequestStreamCallbackItems(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                asynchronousResults = asynchronousResult;
                requests = request;
                BlogData(asynchronousResults, requests);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRequestStreamCallbackItems Method In UploadToBlog.cs file", ex);
            }

        }

        private void BlogData(IAsyncResult asynchronousResult, HttpWebRequest request)
        {
            try
            {
                string contentformat = string.Empty;
                if (ResourceHelper.AppName == Apps.Web_Tile.ToString() || ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    contentformat = "<div style='float: left;'><table><tr><td><div style='float: top;'><img  src='" + ImageUrl + "' width='200px' height='200px'/></div></td><td><p>" + Desc + "</p></td></tr></table>" + "<div style='display: none;' xmlns='http://www.w3.org/1999/xhtml'>" +
                              "<![CDATA[" + "??" + XMLData.ToString() + "]]>" +
                                "</div></div>";
                }
                else
                {
                    contentformat = "<div xmlns='http://www.w3.org/1999/xhtml'>" +
                 "<![CDATA[" + XMLData.ToString() + "]]>" +
                   "</div>";
                }
                string contents = "<entry xmlns='http://www.w3.org/2005/Atom'>" +
                                  "<title type='text'>" + PostTitle + "</title>" +
                                  "<category scheme='http://www.blogger.com/atom/ns#' term='" + PostTitle + "'/>" +
                                  "<category scheme='http://www.blogger.com/atom/ns#' term='hide'/>" +
                                  "<content type='xhtml'>" +
                                    contentformat +
                                  "</content>" +
                                  "</entry>";
                PostTitle = string.Empty;
                Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(contents));
                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                byte[] buffer = new byte[ms.Length / 4];
                int bytesRead = -1;
                ms.Position = 0;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }
                ms.Dispose();
                postStream.Dispose();
                request.BeginGetResponse(new AsyncCallback(RequestAlbumCompletedItem), request);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in BlogData Method In UploadToBlog.cs file", ex);
            }
        }

        private async void RequestAlbumCompletedItem(IAsyncResult result)
        {
            try
            {
                lock (this)
                {
                    var request = (HttpWebRequest)result.AsyncState;
                    var response = (HttpWebResponse)request.EndGetResponse(result);
                    StreamReader responseReader = new StreamReader(response.GetResponseStream());
                    string responseStr = responseReader.ReadToEnd();
                    if (agent == true)
                    {
                        var d2 = typeof(DataManager<>);
                        Type[] typeArgs2 = { typeof(ShareTable) };
                        var makeme2 = d2.MakeGenericType(typeArgs2);
                        object o2 = Activator.CreateInstance(makeme2);
                        ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(typeof(ShareTable), "i");
                        System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, typeof(ShareTable).GetTypeInfo().GetDeclaredProperty("ShowID"));
                        System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ShowID, typeof(ShareTable).GetTypeInfo().GetDeclaredProperty("ShowID").PropertyType, null));
                        System.Linq.Expressions.Expression e1 = System.Linq.Expressions.Expression.Equal(left, right);
                        var comparison = System.Linq.Expressions.Expression.Equal(left, right);
                        var delegateType = typeof(Func<,>).MakeGenericType(typeof(ShareTable), typeof(bool));
                        var whereCallExpression = System.Linq.Expressions.Expression.Lambda(delegateType, comparison, pe);
                        var results = o2.GetType().GetTypeInfo().GetDeclaredMethod("DeleteFromList").Invoke(o2, new object[] { whereCallExpression });
                    }

                    Monitor.Pulse(this);
                }
                auto.Set();
                if (page != default(Page))
                {
                    if (TablesList.Where(i => i.Type == "People").FirstOrDefault() != null && numberofiterations > 0)
                    {
                        numberofiterationscompleted++;
                        if (personcount == numberofiterationscompleted)
                        {

                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                                Popup Adc = frameBorder.FindName("pop") as Popup;
                                Adc.IsOpen = true;
                            });

                            numberofiterations = 0;
                            numberofiterationscompleted = 0;
                            ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                            showlist.ShareStatus = "Shared To Blog";
                            Constants.connection.UpdateAsync(showlist);
                        }
                    }
                    else if (TablesList.Where(i => i.Type == "People").FirstOrDefault() == null)
                    {
                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                        {
                            Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                            Popup Adc = frameBorder.FindName("pop") as Popup;
                            Adc.IsOpen = true;
                        });
                        ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                        showlist.ShareStatus = "Shared To Blog";
                        Constants.connection.UpdateAsync(showlist);
                    }
                    else if (TablesList.Where(i => i.Type == "People").FirstOrDefault() != null && personcount == 0)
                    {
                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                        {
                            Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                            Popup Adc = frameBorder.FindName("pop") as Popup;
                            Adc.IsOpen = true;
                        });
                        ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                        showlist.ShareStatus = "Shared To Blog";
                        Constants.connection.UpdateAsync(showlist);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RequestAlbumCompletedItem Method In UploadToBlog.cs file", ex);
                if (agent == true)
                {
                    if (Task.Run(async () => await Constants.connection.Table<ShareTable>().FirstOrDefaultAsync()).Result != null)
                    {
                        if (Task.Run(async () => await Constants.connection.Table<ShareTable>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result == null)
                        {
                            ShareTable st = new ShareTable();
                            st.ShowID = ShowID;
                            st.Description = Desc;
                            st.BlogCategory = Catname;
                            st.ImageUrl = ImageUrl;
                            st.NextPostTime = System.DateTime.Now.AddDays(1);
                            Constants.connection.InsertAsync(st);
                        }
                        else
                        {
                            ShareTable st = Task.Run(async () => await Constants.connection.Table<ShareTable>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                            st.NextPostTime = st.NextPostTime.AddDays(1);
                            st.BlogCategory = Catname;
                            Constants.connection.UpdateAsync(st);
                        }
                    }
                    else
                    {
                        ShareTable st = new ShareTable();
                        st.ShowID = ShowID;
                        st.Description = Desc;
                        st.BlogCategory = Catname;
                        st.ImageUrl = ImageUrl;
                        st.NextPostTime = System.DateTime.Now.AddDays(1);
                        Constants.connection.InsertAsync(st);
                    }
                }
                Monitor.Pulse(this);
                auto.Set();
            }
        }
        public void uploadImage()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://picasaweb.google.com/data/feed/api/user/" + CONST.USER);
                request.Method = "POST";
                request.ContentType = "image/jpeg";
                request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_Token;
                request.BeginGetRequestStream(new AsyncCallback(GetPicasaStreamCallback), request);
                auto.WaitOne();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in uploadImage Method In UploadToBlog.cs file", ex);
            }
        }

        private void GetPicasaStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                string ImageName = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result.TileImage;
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = default(StorageFile);
                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
#if WINDOWS_APP
            string FolderName = "scale-100";
#endif
#if WINDOWS_PHONE_APP
                    string FolderName = "Images\\";
#endif
#if WINDOWS_APP
                file = Task.Run(async () => await store.CreateFileAsync("Images\\" + FolderName + "\\" + ImageName, CreationCollisionOption.OpenIfExists)).Result;
#endif
#if WINDOWS_PHONE_APP
                    file = Task.Run(async () => await store.CreateFileAsync("Images\\" + ImageName, CreationCollisionOption.OpenIfExists)).Result;
#endif
                }
                else
                {
                    file = Task.Run(async () => await store.CreateFileAsync("Images\\" + ImageName, CreationCollisionOption.OpenIfExists)).Result;
                }
                IRandomAccessStream stream = Task.Run(async () => await file.OpenAsync(Windows.Storage.FileAccessMode.Read)).Result;
                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                byte[] buffer = new byte[stream.AsStream().Length / 4];
                int bytesRead = -1;
                stream.AsStream().Position = 0;
                while ((bytesRead = stream.AsStream().Read(buffer, 0, buffer.Length)) > 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }

                stream.AsStream().Dispose();
                postStream.Dispose();
                request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPicasaStreamCallback Method In UploadToBlog.cs file", ex);
            }
        }

        private void RequestPicasaCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;

                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                ImageUrl = MyXMLConfig.Descendants(atomNS + "content").Attributes().Where(i => i.Name == "src").FirstOrDefault().Value;
                auto.Set();
            }
            catch (System.Net.WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                Exceptions.SaveOrSendExceptions("Exception in RequestPicasaCompleted Method In UploadToBlog.cs file", ex);
            }
        }
    }
}
