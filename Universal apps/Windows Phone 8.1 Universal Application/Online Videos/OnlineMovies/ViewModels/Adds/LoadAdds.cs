using Common.Utilities;
using Microsoft.Advertising.Mobile.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OnlineVideos
{
    public class LoadAdds
    {
        public static void AdControlForPro_New(Grid ContainerGrid, AdControl AdContainer)
        {
            AdControlForPro_New(ContainerGrid, AdContainer, 1);
        }
        public static void AdControlForPro_New(Grid ContainerGrid, AdControl AdContainer, int RowPosition)
        {
            if (!UtilitiesResources.ShowAdControl)
            {
                if (ContainerGrid.RowDefinitions.Count > RowPosition)
                {
                    RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
                    ContainerGrid.RowDefinitions.Remove(myrow);
                }
            }

        }
        public static void LoadAdControl_New(Grid ContainerGrid, AdControl AdContainer, int RowPosition)
        {
            if (!UtilitiesResources.ShowAdControl)
            {
                if (ContainerGrid.RowDefinitions.Count > RowPosition)
                {
                    RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
                    ContainerGrid.RowDefinitions.Remove(myrow);
                }
            }
            else
            {
                if (UtilitiesResources.ShowAdRotator)
                {
                    AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
                    AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
                }
                else
                {
                    AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
                    AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
                }
            }
        }
    }
}