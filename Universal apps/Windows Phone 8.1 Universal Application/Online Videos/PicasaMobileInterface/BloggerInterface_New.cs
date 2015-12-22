using Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PicasaMobileInterface
{
    public class BloggerInterface_New
    {
        private string _userName = String.Empty;
        private string _password = String.Empty;
        private string _userId = String.Empty;
        private string _token = String.Empty;
        private string _type = String.Empty;
        public BloggerInterface_New(string UserName, string Password, string Blogname)
        {            
            _userName = UserName;
            if (_userName.Contains("@"))
                _userId = _userName.Substring(0, _userName.IndexOf("@"));
            else
                _userId = _userName;
            if (!string.IsNullOrEmpty(_type))
            {
                CONST.USER = _userId;
                AppSettings.BlogUserName = _userId;
            }
            if (ResourceHelper.AppName == Apps.Social_Celebrities.ToString())
            {
                AppSettings.BlogUserName = _userId;
            }
            _password = Password;
            Login(UserName, Password, Blogname);
        }

        private async void Login(string UserName, string Password, string Blogname)
        {
            try
            {
                CookieContainer myContainer = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Blogname + ".blogspot.com/feeds/posts/default");

                request.Credentials = new NetworkCredential(UserName, Password);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = myContainer;
                //request.PreAuthenticate = true;
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                Stream responseStream = response.GetResponseStream();
                StreamReader responseStreamReader = new StreamReader(responseStream);
                string result = responseStreamReader.ReadToEnd();//parse token from result

                XNamespace x = "http://www.w3.org/2005/Atom";
                XDocument xdoc = XDocument.Parse(result);
                var list = xdoc.Document.Descendants().Elements(x + "author").FirstOrDefault();
                var auth = list.Descendants(x + "uri").FirstOrDefault().Value.Substring(list.Descendants(x + "uri").FirstOrDefault().Value.LastIndexOf('/') + 1);

                CONST.AUTH_BToken = auth;
                AppSettings.blogtoken = auth;
                AppSettings.BloggerAccessToken = auth;
            }
            catch (Exception ex)
            {
                 Exceptions.SaveOrSendExceptions("Exception in Login Method In BloggerInterface_New.cs file", ex);
            }
        }
    }
}
