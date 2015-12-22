using System;
using System.Net;
using System.Windows.Input;
using System.Collections.Generic;
using Common.Library;
using Common;
using Windows.UI.Xaml;

namespace OnlineVideos.UI
{
    public static class MenuHelper
    {
        public static List<MenuProperties> GetMainMenuItems(string ShowListMenuItemName)
        {
            try
            {
                List<MenuProperties> MainMenu = new List<MenuProperties>()
                                                {
                                                    new MenuProperties{ Id="1", Name=ShowListMenuItemName, ImageUri="Images/MainMenu/movielist.png"},
                                                    new MenuProperties{ Id="2", Name="online mixes", ImageUri="Images/MainMenu/online.png"},
                                                    new MenuProperties{ Id="3", Name="favorites", ImageUri="Images/MainMenu/favorites.png"},
                                                    new MenuProperties{ Id="4", Name="history", ImageUri="Images/MainMenu/history.png"},
                                                    new MenuProperties{ Id="5", Name="get started", ImageUri="Images/MainMenu/getstarted.png"},
                                                    new MenuProperties{ Id="6", Name="settings", ImageUri="Images/MainMenu/settings.png"},
                                                 };
                return MainMenu;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetMainMenuItems Method In MenuHelper file.", ex);
                return null;
            }
        }

        public static List<AboutUsProperties> GetAboutUsMenuItems()
        {
            try
            {
                List<AboutUsProperties> AboutUsMenu = new List<AboutUsProperties>()
                                                {
                                                    new AboutUsProperties{ Id="1", Name="review app", Count =0, CountVisible= Visibility.Collapsed},
                                                    new AboutUsProperties{ Id="2", Name="upgrade app", Count =0, CountVisible= Visibility.Collapsed},
                                                    new AboutUsProperties{ Id="3", Name="other apps", Count =10, CountVisible= Visibility.Visible},
                                                    new AboutUsProperties{ Id="4", Name="share link", Count =0, CountVisible= Visibility.Collapsed},
                                                    new AboutUsProperties{ Id="5", Name="feedback", Count =0, CountVisible= Visibility.Collapsed},
                                                 };
                return AboutUsMenu;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetAboutUsMenuItems Method In MenuHelper file.", ex);
                return null;
            }
        }


    }
}
