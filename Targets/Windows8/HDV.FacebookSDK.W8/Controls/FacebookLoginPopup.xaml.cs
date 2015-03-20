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
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HDV.FacebookSDK.Controls
{
    public sealed partial class FacebookLoginPopup : UserControl
    {
        private Popup loginPopup;
        private TaskCompletionSource<string> loginTcs;

        public FacebookLoginPopup()
        {
            this.InitializeComponent();
        }

       
        /// <summary>
        /// Thực hiện thao tác login
        /// </summary>
        /// <param name="loginURL">URL Login</param>
        /// <param name="popup">Popup chứa control</param>
        /// <param name="loginTcs">Task được truyền vào để kiểm soát</param>
        private void DoLogin(string loginURL, Popup popup, TaskCompletionSource<string> loginTcs)
        {
            this.loginPopup = popup;
            this.loginTcs = loginTcs;

            webView.NavigationCompleted += OnNavigationCompleted;
            webView.NavigationStarting += OnNavigationStarting;

            webView.Navigate(new Uri(loginURL, UriKind.RelativeOrAbsolute));
        }

        void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            try
            {
                string accessToken;

                if (FacebookLogin.TryGetAccessTokenFromCallbackURL(
                    args.Uri.OriginalString, out accessToken)) //Xác thực hoàn thành
                {
                    //Trả ra kết quả
                    loginTcs.SetResult(accessToken);

                    //Đóng view
                    webView.NavigationStarting -= OnNavigationStarting;
                    webView.NavigationCompleted -= OnNavigationCompleted;

                    loginPopup.IsOpen = false;
                }
            } 
            catch (Exception ex)
            {
                loginTcs.SetException(ex);
            }
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (!args.IsSuccess) //Load trang login thất bại
            {
                loginTcs.SetException(new Exception(string.Format("Web Error: {1}", args.WebErrorStatus)));
                loginPopup.IsOpen = false;
            }

            webView.NavigationCompleted -= OnNavigationCompleted;
        }

        /// <summary>
        /// Show popup login tới Facebook
        /// </summary>
        /// <param name="loginURL">URL Login</param>
        public static Task<string> ShowPopup(string loginURL)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            Popup loginPopup = new Popup();
            loginPopup.HorizontalOffset = 0;
            loginPopup.VerticalOffset = 0;


            //Tạo control nội dung cho popup
            FacebookLoginPopup popupContent = new FacebookLoginPopup();
            loginPopup.Child = popupContent;

            //Set toàn màn hình cho Popup
            var screenWidth = Window.Current.Bounds.Width;
            var screenHeight = Window.Current.Bounds.Height;

            popupContent.Width = screenWidth;
            popupContent.Height = screenHeight;

            loginPopup.IsOpen = true;

            popupContent.DoLogin(loginURL, loginPopup, tcs);

            return tcs.Task;
        }
    }
}
