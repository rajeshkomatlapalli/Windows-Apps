using Common.Library;
//using Microsoft.Phone.Controls;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
//using System.Windows.Threading;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OnlineVideos.ViewModels
{
  public class BackGroundCheck
    {
       public DispatcherTimer timer = new DispatcherTimer();
        int CheckShowID = 0;
        int ShowID = default(int);
       // WebBrowser wb = new WebBrowser();
        WebView wb = new WebView();

        
      public async void CheckData()
      {
          try
          {
              await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
              //Deployment.Current.Dispatcher.BeginInvoke(() =>
             {
                 timer.Stop();
             });
              if (Task.Run(async () => await Constants.connection.Table<WebInformation>().FirstOrDefaultAsync()).Result != null)
              {
                  if (Task.Run(async()=>await Constants.connection.Table<WebDailyTable>().FirstOrDefaultAsync()).Result != null)
                  {
                      List<WebInformation> WebList = Task.Run(async () => await Constants.connection.Table<WebInformation>().OrderBy(i => i.ID).ToListAsync()).Result;
                      foreach (WebInformation wt in WebList)
                      {
                          int showid = wt.ShowID;
                          if (Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result != null)
                          {
                              WebDailyTable WebList1 = Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID ==showid).FirstOrDefaultAsync()).Result;

                              if (WebList1.Date < DateTime.Now.Date && WebList1.SelectedText != "Error")
                              {
                                  CheckShowID = WebList1.ShowID;
                                  break;
                              }
                          }
                          else
                          {
                              CheckShowID = wt.ShowID;
                              break;
                          }
                      }
                  }
                  else
                  {
                      CheckShowID = 1;
                  }

                  if (CheckShowID != 0)
                  {
                      await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      //Deployment.Current.Dispatcher.BeginInvoke(() =>
                      {
                          WebInformation Shows = Task.Run(async () => await Constants.connection.Table<WebInformation>().Where(i => i.ShowID == CheckShowID).OrderBy(i => i.ShowID).FirstOrDefaultAsync()).Result;
                          ShowID = Shows.ShowID;
                          wb.Navigate(new Uri(Shows.NavigationUri, UriKind.RelativeOrAbsolute));
                          //wb.Navigate(new Uri(Shows.NavigationUri, UriKind.RelativeOrAbsolute), null, "User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)");
                         // wb.IsScriptEnabled = true;
                          wb.LoadCompleted += wb_LoadCompleted;
                          wb.ScriptNotify += wb_ScriptNotify;
                      });
                  }
                  else
                  {
                      await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      //Deployment.Current.Dispatcher.BeginInvoke(() =>
                      {
                          timer.Start();
                      });
                  }
              }
              else
              {
                  await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                  //Deployment.Current.Dispatcher.BeginInvoke(() =>
                  {
                      timer.Start();
                  });
              }
          }
          catch (Exception)
          {
             
          }
      }
      public int getNumberOfOccurencies(String inputString, String checkString)
      {
          try
          {
              int lengthDifference = inputString.Length - checkString.Length;
              int occurencies = 0;
              for (int i = 0; i < lengthDifference; i++)
              {
                  if (inputString.Substring(i, checkString.Length).Equals(checkString)) { occurencies++; i += checkString.Length - 1; }
              }
              return occurencies;
          }
          catch (Exception)
          {
              return 0;
          }
      }
      async void wb_ScriptNotify(object sender, NotifyEventArgs e)
      {
          try
          {
              if (e.Value != null)
              {
                  string CurrentSelectedText = string.Empty;
                  try
                  {
                      int BeforeTextPosition = default(int);
                      int BeforeTextLength = default(int);
                      int NoOfOccurrences = default(int);
                      WebInformation wi = Task.Run(async () => await Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                      string SelectedText = wi.SelectedText.Replace("\n", "").Replace("\r", "").Replace("<br>", "<br />").Replace("\t", "");
                      string BeforeText = wi.BeforeText.Replace("\n", "").Replace("\r", "").Replace("<br>", "<br />").Replace("\t", "");
                      string AfterText = wi.AfterText.Replace("\n", "").Replace("\r", "").Replace("<br>", "<br />").Replace("\t", "");
                      if (e.Value != null)
                      {
                          string PageHtml = e.Value.ToString().Replace("\n", "").Replace("\r", "").Replace("<br>", "<br />").Replace("\t", "");
                          NoOfOccurrences = getNumberOfOccurencies(PageHtml, BeforeText);
                          if (NoOfOccurrences == 1)
                          {
                              BeforeTextPosition = PageHtml.IndexOf(BeforeText);
                              BeforeTextLength = BeforeText.Length;
                              int BeforeTextEndPosition = BeforeTextPosition + BeforeTextLength;
                              string RemainingText = PageHtml.Substring(BeforeTextEndPosition);
                              int AfterTextposition = RemainingText.IndexOf(AfterText);
                              CurrentSelectedText = RemainingText.Substring(0, AfterTextposition);
                          }
                          else
                          {
                              CurrentSelectedText = "Error";
                          }
                         
                          if ( Task.Run(async () => await Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result.SelectedText != CurrentSelectedText)
                          {
                              WebDailyTable wd = new WebDailyTable();
                              wd.ShowID = ShowID;
                              wd.SelectedText = CurrentSelectedText;
                              wd.Date = System.DateTime.Now;
                              await Constants.connection.InsertAsync(wd);
                          }
                      }
                      if (CurrentSelectedText != "Error" && Task.Run(async () => await Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result.SelectedText != CurrentSelectedText)
                      {
                          List<WebDailyTable> Lweb = Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
                          foreach (WebDailyTable wt in Lweb)
                          {
                              wt.Status = "Changed";
                              await Constants.connection.UpdateAsync(wt);
                          }
                      }
                      await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      //Deployment.Current.Dispatcher.BeginInvoke(() =>
                      {
                          timer.Start();
                      });
                  }
                  catch (Exception)
                  {
                      WebDailyTable wd = new WebDailyTable();
                      wd.ShowID = ShowID;
                      wd.SelectedText = "Error";
                      wd.Date = System.DateTime.Now;
                      Constants.connection.InsertAsync(wd);
                       Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      //Deployment.Current.Dispatcher.BeginInvoke(() =>
                      {
                          timer.Start();
                      });
                  }
              }
              else
              {
                  await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                  //Deployment.Current.Dispatcher.BeginInvoke(() =>
                  {
                      timer.Start();
                  });

              }
          }
          catch (Exception)
          {
             
          }
      }

      void wb_LoadCompleted(object sender, NavigationEventArgs e)
      {
          try
          {
              //wb.InvokeScript("eval", "this.newfunc_eventHandler=function(e){var fullhtml=document.documentElement.outerHTML.replace(/\\n/g, '').replace(/<br>/g, '<br />').replace(/\\r/g, '').replace(/\\t/g, '');window.external.notify(fullhtml);}");
              //wb.InvokeScript("eval", "newfunc_eventHandler()");
          }
          catch (Exception ex)
          {
           Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
              //Deployment.Current.Dispatcher.BeginInvoke(() =>
              {
                  timer.Start();
              });
          }
      }
    }
}
