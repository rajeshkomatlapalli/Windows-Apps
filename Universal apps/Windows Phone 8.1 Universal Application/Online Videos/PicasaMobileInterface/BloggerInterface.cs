using Common.Library;
//using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace PicasaMobileInterface
{
    //BlogInterface
    public class BloggerInterface
    {
        AutoResetEvent auto = new AutoResetEvent(false);
        private string _userName = String.Empty;
        private string _password = String.Empty;
        private string _userId = String.Empty;
        private string _token = String.Empty;
        private string _type = String.Empty;

        public BloggerInterface(string UserName, string Password, string Type = "")
        {            
            _type = Type;
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
            _password = "Lartshowratings";
            Login();
        }
                
        private void Login()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/accounts/CLientLogin");
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackItem), request);
                auto.WaitOne();
            }
            catch (Exception ex)
            {                
                 Exceptions.SaveOrSendExceptions("Exception in Login Method In BloggerInterface.cs file", ex);
            }

        }
        private void GetRequestStreamCallbackItem(IAsyncResult asynchronousResult)
        {
            try
            {
                string postData = "accountType=" + CONST.AUTH_ACCOUNTTYPE;
                postData += ("&Email=" + _userName);
                postData += ("&Passwd=" + _password);
                postData += ("&service=" + CONST.AUTH_BSERVICE);
                postData += ("&source=" + CONST.AUTH_BSOURCE);

                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(postData));
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
                request.BeginGetResponse(new AsyncCallback(RequestBlogCompleted), request);
            }
            catch (Exception ex)
            {
                
                  Exceptions.SaveOrSendExceptions("Exception in GetRequestStreamCallbackItem Method In BloggerInterface.cs file", ex);
            }
        }
        private void RequestBlogCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);

                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string responseStr = responseReader.ReadToEnd();
                ParseAuthToken(responseStr);
                auto.Set();
            }
            catch (Exception ex)
            {
                
                 Exceptions.SaveOrSendExceptions("Exception in RequestBlogCompleted Method In BloggerInterface.cs file", ex);
            }
        }

        private string ParseAuthToken(string response)
        {
            string auth = "";
            try
            {               
                try
                {
                    auth = new Regex(@"Auth=(?<auth>\S+)").Match(response).Result("${auth}");
                }
                catch (Exception ex)
                {
                }
                CONST.AUTH_BToken = auth;
                AppSettings.blogtoken = auth;
                AppSettings.BloggerAccessToken = auth;
            }
            catch (Exception ex)
            {
                
                 Exceptions.SaveOrSendExceptions("Exception in ParseAuthToken Method In BloggerInterface.cs file", ex);
            }
            return auth;
        }
    }
}
