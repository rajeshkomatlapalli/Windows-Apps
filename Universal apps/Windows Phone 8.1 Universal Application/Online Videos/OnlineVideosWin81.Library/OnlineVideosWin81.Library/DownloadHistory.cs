using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using System.IO;
using OnlineVideos.Common;
using Common.Library;
using System.Linq;

namespace OnlineVideos.Library
{
    public class DownloadHistory
    {
        XDocument xdoc = null;
        public async void SaveHistory(string mid)
        {
            try
            {
                if (await Storage.FileExists("Historys.xml"))
                {
                    XDocument xdoc = await Storage.ReadFileAsDocument("Historys.xml");
                    if (xdoc.Document != null)
                    {
                        xdoc.Root.Add(new XElement("Historys",
                            new XElement("HistoryID", mid
                               )));
                        Storage.SaveFileFromDocument("Historys.xml", xdoc);
                    }
                }
                else
                {
                    xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                         new XElement("History",
                        new XElement("Historys",
                        new XElement("HistoryID", mid
                             ))));
                    Storage.SaveFileFromDocument("Historys.xml", xdoc);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveHistory Method In DownloadHistory.cs file", ex);
            }
        }
    }
}