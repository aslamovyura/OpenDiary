﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebUI.Resources.ViewModels.Posts {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CreatePostViewModel_en {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CreatePostViewModel_en() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("WebUI.Resources.ViewModels.Posts.CreatePostViewModel.en", typeof(CreatePostViewModel_en).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
        
        internal static string TitleRequired {
            get {
                return ResourceManager.GetString("TitleRequired", resourceCulture);
            }
        }
        
        internal static string TitleMaxLength {
            get {
                return ResourceManager.GetString("TitleMaxLength", resourceCulture);
            }
        }
        
        internal static string Topic {
            get {
                return ResourceManager.GetString("Topic", resourceCulture);
            }
        }
        
        internal static string TopicRequired {
            get {
                return ResourceManager.GetString("TopicRequired", resourceCulture);
            }
        }
        
        internal static string TopicMaxLength {
            get {
                return ResourceManager.GetString("TopicMaxLength", resourceCulture);
            }
        }
        
        internal static string Text {
            get {
                return ResourceManager.GetString("Text", resourceCulture);
            }
        }
        
        internal static string TextRequired {
            get {
                return ResourceManager.GetString("TextRequired", resourceCulture);
            }
        }
        
        internal static string TextMinLength {
            get {
                return ResourceManager.GetString("TextMinLength", resourceCulture);
            }
        }
    }
}