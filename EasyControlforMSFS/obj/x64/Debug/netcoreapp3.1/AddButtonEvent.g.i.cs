﻿#pragma checksum "..\..\..\..\AddButtonEvent.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "DC521B14E5DEAC4C1FC1A49723C4B985DD239A0E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EasyControlforMSFS;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace EasyControlforMSFS {
    
    
    /// <summary>
    /// AddButtonEvent
    /// </summary>
    public partial class AddButtonEvent : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Header_button;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Selectevents_text;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Selectevents_dropdowns;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ButtonNr;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Newevent_text;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel New_items_input;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\AddButtonEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewEventName;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/EasyControlforMSFS;V1.0.0.0;component/addbuttonevent.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\AddButtonEvent.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Header_button = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.Selectevents_text = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.Selectevents_dropdowns = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.ButtonNr = ((System.Windows.Controls.ComboBox)(target));
            
            #line 43 "..\..\..\..\AddButtonEvent.xaml"
            this.ButtonNr.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ButtonNr_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Newevent_text = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.New_items_input = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.NewEventName = ((System.Windows.Controls.TextBox)(target));
            
            #line 50 "..\..\..\..\AddButtonEvent.xaml"
            this.NewEventName.KeyUp += new System.Windows.Input.KeyEventHandler(this.NewEventName_KeyUp);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 54 "..\..\..\..\AddButtonEvent.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 56 "..\..\..\..\AddButtonEvent.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

