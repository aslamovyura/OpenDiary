﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebUI.Resources.ViewModels.Account {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResetPasswordViewModel_ru {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResetPasswordViewModel_ru() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("WebUI.Resources.ViewModels.Account.ResetPasswordViewModel.ru", typeof(ResetPasswordViewModel_ru).Assembly);
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
        
        internal static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        internal static string PasswordRequired {
            get {
                return ResourceManager.GetString("PasswordRequired", resourceCulture);
            }
        }
        
        internal static string PasswordLength {
            get {
                return ResourceManager.GetString("PasswordLength", resourceCulture);
            }
        }
        
        internal static string ConfirmPassword {
            get {
                return ResourceManager.GetString("ConfirmPassword", resourceCulture);
            }
        }
        
        internal static string PasswordConfirmationRequired {
            get {
                return ResourceManager.GetString("PasswordConfirmationRequired", resourceCulture);
            }
        }
        
        internal static string PasswordMismatch {
            get {
                return ResourceManager.GetString("PasswordMismatch", resourceCulture);
            }
        }
    }
}
