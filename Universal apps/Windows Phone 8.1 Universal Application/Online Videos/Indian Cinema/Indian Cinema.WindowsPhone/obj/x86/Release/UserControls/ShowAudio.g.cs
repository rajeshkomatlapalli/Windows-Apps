﻿

#pragma checksum "E:\Present Code_Mohan Rajesh\Universal apps\Windows Phone 8.1 Universal Application\Online Videos\Indian Cinema\Indian Cinema.WindowsPhone\..\..\OnlineMovies\UserControls\ShowAudio.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B7EE2E8E1DB3478BADF9ADBF1AEAAD07"
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
    partial class ShowAudio : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 9 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.UserControl_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 17 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.lbxAudioList_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 21 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.StackPanel_Holding;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 23 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.FlyoutBase)(target)).Opened += this.MenuFlyout_Opened;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 24 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.Pin_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 25 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.favorites_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 26 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 27 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItem_Click_1;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 28 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.Rating_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 29 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.del_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 34 "..\..\..\UserControls\ShowAudio.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.ringtone_PointerPressed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

