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
using HDV.FacebookSDK.Login;

namespace HDV.FacebookSDK.Controls
{
    public partial class FacebookLogoutDialog : UserControl
    {
        private bool isProcessing = false;
        private Dialog dialog;
        private TaskCompletionSource<object> tcs;

        public FacebookLogoutDialog()
        {
            InitializeComponent();
        }

        private void DoLogout(string logoutURL, Dialog dialog, TaskCompletionSource<object> tcs)
        {
            this.dialog = dialog;
            this.tcs = tcs;
            this.isProcessing = true;

            webBrowser.NavigationFailed += OnNavigationFailed;
            webBrowser.Navigating += OnNavigating;
            dialog.Dismissed += OnDismissed;

            webBrowser.Navigate(new Uri(logoutURL, UriKind.RelativeOrAbsolute));
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
                if (FacebookLogin.IsRedirectURL(e.Uri.OriginalString)) //Xác thực hoàn thành
                {
                    //Trả ra kết quả
                    tcs.SetResult(null);
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

        public static Task ShowDialog(string logoutURL)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            Dialog dialog = new Dialog();

            FacebookLogoutDialog facebookPopupContent = new FacebookLogoutDialog();
            dialog.DialogContent = facebookPopupContent;

            dialog.Showed += (s, e) =>
            {
                facebookPopupContent.DoLogout(logoutURL, dialog, tcs);
            };

            dialog.Show();


            return tcs.Task;
        }
    }
}
