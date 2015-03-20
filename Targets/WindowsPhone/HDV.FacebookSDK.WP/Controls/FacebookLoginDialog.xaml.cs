using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using HDV.FacebookSDK.Login;

namespace HDV.FacebookSDK.Controls
{
    public partial class FacebookLoginDialog : UserControl
    {
        private string loginURL;
        private Dialog dialog;
        private TaskCompletionSource<string> tcs;
        private bool isProcessing = false;

        public FacebookLoginDialog()
        {
            InitializeComponent();
        }

        private void DoLogin(string loginURL, Dialog dialog, TaskCompletionSource<string> tcs)
        {
            this.loginURL = loginURL;
            this.dialog = dialog;
            this.tcs = tcs;
            this.isProcessing = true;

            webBrowser.NavigationFailed += OnNavigationFailed;
            webBrowser.Navigating += OnNavigating;
            dialog.Dismissed += OnDismissed;

            webBrowser.Navigate(new Uri(loginURL, UriKind.RelativeOrAbsolute));
        }

        private void OnDismissed(object sender, EventArgs e)
        {
            if (isProcessing)
                tcs.SetCanceled();
        }

        private void OnNavigating(object sender, NavigatingEventArgs e)
        {
            try
            {
                string accessToken;

                if (FacebookLogin.TryGetAccessTokenFromCallbackURL(
                    e.Uri.OriginalString, out accessToken)) //Xác thực hoàn thành
                {
                    //Trả ra kết quả
                    tcs.SetResult(accessToken);
                    isProcessing = false;

                    //Đóng view
                    webBrowser.Navigating -= OnNavigating;
                    webBrowser.NavigationFailed -= OnNavigationFailed;

                    dialog.Dismiss();
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (e.Exception != null)
                tcs.SetException(e.Exception);
            else
                tcs.SetException(new Exception("Navigation failed"));
            isProcessing = false;

            dialog.Dismiss();

            e.Handled = true;
            webBrowser.NavigationFailed -= OnNavigationFailed;
        }

        public static Task<string> ShowDialog(string loginURL)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            Dialog dialog = new Dialog();

            FacebookLoginDialog facebookPopupContent = new FacebookLoginDialog();
            dialog.DialogContent = facebookPopupContent;

            dialog.Showed += (s, e) =>
            {
                facebookPopupContent.DoLogin(loginURL, dialog, tcs);
            };

            dialog.Show();


            return tcs.Task;
        }
    }
}
