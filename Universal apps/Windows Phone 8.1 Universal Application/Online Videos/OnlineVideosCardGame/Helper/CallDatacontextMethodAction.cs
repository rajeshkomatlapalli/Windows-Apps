using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OnlineVideosCardGame.Helper
{
    public class CallDatacontextMethodAction
    {
        public string MethodName { get; set; }
        public CallDatacontextMethodAction(object o, string Method)
        {
            try
            {
                MethodName = Method;

                FrameworkElement fwe = o as FrameworkElement;
                if (fwe == null)
                    return;

                var aodc = fwe.DataContext;
                bool successful = false;
                object lastdc = null;
                while (fwe != null)
                {
                    var fwedc = fwe.DataContext;
                    if (fwedc == null || fwedc.Equals(lastdc))
                    {
                        fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                        continue;
                    }

                    MethodInfo mi = fwedc.GetType().GetTypeInfo().GetDeclaredMethod(MethodName);
                    if (mi == null)
                    {
                        fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                        continue;
                    }
                    else
                    {
                        if (mi.GetParameters().Length == 1)
                            mi.Invoke(fwedc, new object[1] { aodc });
                        else
                            mi.Invoke(fwedc, null);
                        successful = true;
                        break;
                    }
                }
                if (!successful)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
