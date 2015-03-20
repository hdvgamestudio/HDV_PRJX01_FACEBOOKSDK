using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HDV.FacebookSDK.Test.Resources;
using HDV.FacebookSDK.Models;
using HDV.FacebookSDK.GraphAPI;

namespace HDV.FacebookSDK.WP8.Test
{
    public partial class MainPage : PhoneApplicationPage
    {
        const string APP_ID = "431286293691374";
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var session = await FacebookClient.Current.LoginAsync(APP_ID, Permission.public_profile, Permission.user_friends,
                    Permission.user_games_activity, Permission.publish_actions);

                MessageBox.Show(string.Format("AcessToken = {0}", session.AccessToken), "Facebook Login", MessageBoxButton.OK);
            }
            catch
            {
                MessageBox.Show("Can't login with facebook", "Lỗi", MessageBoxButton.OK);
            }
        }

        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await FacebookClient.Current.LogoutAsync();
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra", "Lỗi", MessageBoxButton.OK);
            }
        }

        private async void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            Request getMyInfoRequest = Request.CreateGetUserInfoRequest(FacebookClient.Current.CurrentSession,
                "me", GraphUser.ID_FIELD, GraphUser.NAME_FIELD, GraphUser.BIRTHDAY_FIELD);

            var respone = await getMyInfoRequest.ExecuteAsync();

            if (respone != null)
            {
                if (respone.RequestError != null)
                {
                    var myExeception = respone.RequestError;

                    MessageBox.Show(myExeception.Message, "Error", MessageBoxButton.OK);
                }
                else if (respone.GraphObject != null)
                {
                    var myInfo = respone.GraphObject.Cast<GraphUser>();

                    MessageBox.Show(string.Format("ID: {0}\r\n Name: {1}\r\n Birthday: {2}", myInfo.ID, myInfo.Name, myInfo.Birthday), "My Info", MessageBoxButton.OK);
                }
            }
        }

        
    }
}