﻿

#pragma checksum "E:\Present Code_Mohan Rajesh\Universal apps\Windows Phone 8.1 Universal Application\Online Videos\Indian Cinema\Indian Cinema.WindowsPhone\..\..\OnlineMovies\Views\PersonGalleryPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "107B173E054ADD3871F02DFA8D3D1738"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineVideos.Views
{
    partial class PersonGalleryPopup : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 25 "..\..\..\Views\PersonGalleryPopup.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationStarted += this.galleryimage_ManipulationStarted;
                 #line default
                 #line hidden
                #line 26 "..\..\..\Views\PersonGalleryPopup.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationCompleted += this.galleryimage_ManipulationCompleted;
                 #line default
                 #line hidden
                #line 29 "..\..\..\Views\PersonGalleryPopup.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).ManipulationDelta += this.galleryimage_ManipulationDelta;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

