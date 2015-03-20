using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HDV.FacebookSDK.Test
{
    public sealed partial class UserInfoView : UserControl
    {
        public static readonly DependencyProperty ProfileImageUriProperty = DependencyProperty.RegisterAttached(
            "ProfileImageUri", 
            typeof(Uri), typeof(UserInfoView), 
            new PropertyMetadata(default(Uri)));

        public UserInfoView()
        {
            this.InitializeComponent();
        }

        public Uri ProfileImageUri
        {
            set { SetValue(ProfileImageUriProperty, value); }
            get { return (Uri)GetValue(ProfileImageUriProperty); }
        }
    }
}
