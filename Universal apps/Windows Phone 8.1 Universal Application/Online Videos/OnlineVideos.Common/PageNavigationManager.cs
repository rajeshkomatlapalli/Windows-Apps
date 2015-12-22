using System;
using Windows.UI.Xaml;
using System.Reflection;
using Common.Library;

namespace OnlineVideos.Common
{
    public class PageNavigationManager
    {
       public static object GetParentOfType(FrameworkElement childControl, Type parentType)
       {
           object page = null;
           try
           {
               object parent = (childControl).Parent;
               while (parent.GetType() != parentType)
               {
                   object o = new object();
                   o = parent;
                   parent = new object();
                   parent = ((FrameworkElement)o).Parent;
               }
               if (parent.GetType() == parentType)
                   page = parent;
           }
           catch (Exception ex)
           {
               Exceptions.SaveOrSendExceptions("Exception in GetParentOfType Method In PageNavigationManager.cs file", ex);
           }
            return page;
        }
       public static object GetParentOfTypePage(FrameworkElement childControl, Type parentType)
        {
            object page = null;
            try
            {
                object parent = (childControl).Parent;
                while (parent.GetType().GetTypeInfo().BaseType!=parentType)
                {
                    object o = new object();
                    o = parent;
                    parent = new object();
                    parent = ((FrameworkElement)o).Parent;
                }
                if (parent.GetType().GetTypeInfo().BaseType == parentType)
                    page = parent;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetParentOfTypePage Method In PageNavigationManager.cs file", ex);
            }
            return page;
        }
    }

}
