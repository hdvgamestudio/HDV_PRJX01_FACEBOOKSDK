using HDV.FacebookSDK.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace HDV.FacebookSDK.Controls
{
    public sealed partial class FacebookLogoutPopup : UserControl
    {
        public FacebookLogoutPopup()
        {
            this.InitializeComponent();
        }

        private TaskCompletionSource<Object> tcs;
        private Popup popup;

        /// <summary>
        /// Gọi lệnh đăng xuất
        /// </summary>
        /// <param name="logoutURL"></param>
        /// <param name="tcs"></param>
        private void DoLogout(string logoutURL, Popup popup, TaskCompletionSource<Object> tcs)
        {
            this.popup = popup;
            this.tcs = tcs;

            webView.NavigationStarting += OnNavigationStarting;
            webView.Navigate(new Uri(logoutURL, UriKind.RelativeOrAbsolute));
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (FacebookLogin.IsRedirectURL(args.Uri.OriginalString))
            {
                tcs.SetResult(null);

                //Đóng view
                webView.NavigationStarting -= OnNavigationStarting;
                popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Show popup Logout
        /// </summary>
        public static Task ShowPopup(string logoutURL)
        {
            TaskCompletionSource<Object> tcs = new TaskCompletionSource<Object>();

            Popup logoutPopup = new Popup();
            logoutPopup.HorizontalOffset = 0;
            logoutPopup.VerticalOffset = 0;


            //Tạo control nội dung cho popup
            FacebookLogoutPopup popupContent = new FacebookLogoutPopup();
            logoutPopup.Child = popupContent;

            //Set toàn màn hình cho Popup
            var screenWidth = Window.Current.Bounds.Width;
            var screenHeight = Window.Current.Bounds.Height;

            popupContent.Width = screenWidth;
            popupContent.Height = screenHeight;

            logoutPopup.IsOpen = true;

            popupContent.DoLogout(logoutURL, logoutPopup, tcs);
                        
            return tcs.Task;
        }
    }
}
