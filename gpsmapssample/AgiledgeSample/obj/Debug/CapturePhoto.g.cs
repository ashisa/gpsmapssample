﻿

#pragma checksum "C:\Users\ashisa\Documents\Visual Studio 2015\Projects\AgiledgeSample\AgiledgeSample\CapturePhoto.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2E84E4933CD51268B579BE8B14904AC1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgiledgeSample
{
    partial class CapturePhoto : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 16 "..\..\CapturePhoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.capturePhoto_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 17 "..\..\CapturePhoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.selectPhoto_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

