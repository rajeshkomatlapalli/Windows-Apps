﻿

#pragma checksum "E:\Present Code_Mohan Rajesh\Universal apps\Windows Phone 8.1 Universal Application\Online Videos\Indian Cinema\Indian Cinema.WindowsPhone\..\..\OnlineMovies\UserControls\ShowCast.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C1A0138F455F6813169B0EEB471F6973"
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
    partial class ShowCast : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 9 "..\..\..\UserControls\ShowCast.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.UserControl_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 14 "..\..\..\UserControls\ShowCast.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.lbxCast_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 17 "..\..\..\UserControls\ShowCast.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.StackPanel_Holding;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 19 "..\..\..\UserControls\ShowCast.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.FlyoutBase)(target)).Opened += this.MenuFlyout_Opened;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 20 "..\..\..\UserControls\ShowCast.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.deletecast_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

