﻿

#pragma checksum "C:\Users\ashisa\Documents\Visual Studio 2015\Projects\AgiledgeSample\AgiledgeSample\Signature.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "29D5EE327DF396D2C1A6352D06506F7B"
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
    partial class Signature : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 13 "..\..\Signature.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerMoved += this.myCanvas_PointerMoved;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 15 "..\..\Signature.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.save_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 16 "..\..\Signature.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.show_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

