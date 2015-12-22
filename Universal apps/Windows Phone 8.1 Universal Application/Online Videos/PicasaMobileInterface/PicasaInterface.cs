/* 
    Author: Manuel Conti
    WebSite : www.sintax.org
*/
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

using System.Collections.Generic;

using System.Xml;
using System.Threading;
using System.Text;
using Common.Library;

namespace PicasaMobileInterface
{
    //picasaInterface
    public class PicasaInterface
    {
        #region PRIVATE
        AutoResetEvent auto = new AutoResetEvent(false);

        private string _token = String.Empty;
        private string _userName = String.Empty;
        private string _userId = String.Empty;
        private string _password = String.Empty;

        #endregion

        #region PUBLIC

        #endregion

        public PicasaInterface(string UserName, string Password)
        {           
            _userName = UserName;
            _userId = _userName.Substring(0, _userName.IndexOf("@"));
            CONST.USER = _userId;
            _password = Password;
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
               Exceptions.SaveOrSendExceptions("Exception in Login Method In PicasaInterface.cs file", ex);
            }
        }
        private void GetRequestStreamCallbackItem(IAsyncResult asynchronousResult)
        {
            try
            {
                string postData = "accountType=" + CONST.AUTH_ACCOUNTTYPE;
                postData += ("&Email=" + _userName);
                postData += ("&Passwd=" + _password);
                postData += ("&service=" + CONST.AUTH_SERVICE);
                postData += ("&source=" + CONST.AUTH_SOURCE);

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
                Exceptions.SaveOrSendExceptions("Exception in GetRequestStreamCallbackItem Method In PicasaInterface.cs file", ex);
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
                auto.Set();
                Exceptions.SaveOrSendExceptions("Exception in RequestBlogCompleted Method In PicasaInterface.cs file", ex);
            }
        }

        private string ParseAuthToken(string response)
        {
            string auth = "";
            try
            {
                auth = new Regex(@"Auth=(?<auth>\S+)").Match(response).Result("${auth}");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ParseAuthToken Method In PicasaInterface.cs file", ex);
            }
            CONST.AUTH_Token = auth;
            return auth;
        }
    }
}