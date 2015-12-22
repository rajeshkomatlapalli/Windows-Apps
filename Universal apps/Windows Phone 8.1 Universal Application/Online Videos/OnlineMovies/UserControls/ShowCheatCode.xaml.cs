using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class ShowCheatCode : UserControl
    {
        int CheatCodeListPageStart = 10;
        public ShowCheatCode()
        {
            this.InitializeComponent();
        }

        private void lbxCheatcode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxCheatcode.SelectedIndex != -1)
            {
                string title = (lbxCheatcode.SelectedItem as GameCheats).CheatName;
                if (title == "more cheats")
                {
                    CheatCodeListPageStart = CheatCodeListPageStart + 10;
                    lbxCheatcode.ItemsSource = OnlineShow.GetGameCheatCodes(AppSettings.ShowUniqueID, CheatCodeListPageStart);
                    lbxCheatcode.ScrollIntoView(lbxCheatcode.Items[CheatCodeListPageStart - 10]);
                }
                else
                    PageHelper.NavigateTo(NavigationHelper.getCheatCodePage((lbxCheatcode.SelectedItem as GameCheats).CheatID.ToString(), AppSettings.ShowID));                
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<GameCheats> list = new List<GameCheats>();
            list = OnlineShow.GetGameCheatCodes(AppSettings.ShowUniqueID, Constants.PageSize);
            if (list.Count() != 0)
            {
                lbxCheatcode.ItemsSource = list;
            }
            else
            {
                tblkmsg.Visibility = Visibility.Visible;
            }
        }
    }
}
