﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebUI.Resources.ViewModels.Profile {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ProfileViewModel_en {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProfileViewModel_en() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("WebUI.Resources.ViewModels.Profile.ProfileViewModel.en", typeof(ProfileViewModel_en).Assembly);
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
        
        internal static string FirstName {
            get {
                return ResourceManager.GetString("FirstName", resourceCulture);
            }
        }
        
        internal static string FirstNameRequired {
            get {
                return ResourceManager.GetString("FirstNameRequired", resourceCulture);
            }
        }
        
        internal static string LastName {
            get {
                return ResourceManager.GetString("LastName", resourceCulture);
            }
        }
        
        internal static string LastNameRequired {
            get {
                return ResourceManager.GetString("LastNameRequired", resourceCulture);
            }
        }
        
        internal static string Email {
            get {
                return ResourceManager.GetString("Email", resourceCulture);
            }
        }
        
        internal static string EmailRequired {
            get {
                return ResourceManager.GetString("EmailRequired", resourceCulture);
            }
        }
        
        internal static string EmailTypeRequired {
            get {
                return ResourceManager.GetString("EmailTypeRequired", resourceCulture);
            }
        }
        
        internal static string BirthDate {
            get {
                return ResourceManager.GetString("BirthDate", resourceCulture);
            }
        }
        
        internal static string BirthDateRequired {
            get {
                return ResourceManager.GetString("BirthDateRequired", resourceCulture);
            }
        }
        
        internal static string DateTypeRequired {
            get {
                return ResourceManager.GetString("DateTypeRequired", resourceCulture);
            }
        }
    }
}
