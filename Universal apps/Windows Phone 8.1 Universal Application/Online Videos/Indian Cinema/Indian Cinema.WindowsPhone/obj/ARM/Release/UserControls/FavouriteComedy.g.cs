﻿

#pragma checksum "E:\Present Code_Mohan Rajesh\Universal apps\Windows Phone 8.1 Universal Application\Online Videos\Indian Cinema\Indian Cinema.WindowsPhone\..\..\OnlineMovies\UserControls\FavouriteComedy.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "078424B3932C4A8CD75898909BE0B256"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineVideos.UserControls
{
    partial class FavouriteComedy : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 9 "..\..\..\UserControls\FavouriteComedy.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.UserControl_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 14 "..\..\..\UserControls\FavouriteComedy.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.lbxFavoritecomedy_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 17 "..\..\..\UserControls\FavouriteComedy.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.StackPanel_Holding;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 20 "..\..\..\UserControls\FavouriteComedy.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

