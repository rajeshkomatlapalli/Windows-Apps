﻿

#pragma checksum "E:\Present Code_Mohan Rajesh\Universal apps\Windows Phone 8.1 Universal Application\Online Videos\OnlineVideosWin81.Controls\..\..\Common\CommonControls81\CommonControls\UserControls\MovieFavorite.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1B32BCAAA2153353058623540C89B2B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineVideosWin81.Controls
{
    partial class MovieFavorite : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 14 "..\..\..\MovieFavorite.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).RightTapped += this.StackPanel_RightTapped_1;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 32 "..\..\..\MovieFavorite.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.grdvwRelatedMovies_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

