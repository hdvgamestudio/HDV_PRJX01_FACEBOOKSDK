using HDV.FacebookSDK.GraphAPI;
using HDV.FacebookSDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HDV.FacebookSDK.W8.Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string APPID = "431286293691374";

        public MainPage()
        {
            this.InitializeComponent();
            lstFriends.ItemsSource = friends;
        }

        private ObservableCollection<GraphUser> friends = new ObservableCollection<GraphUser>();

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var session = await FacebookClient.Current.LoginAsync(APPID, Permission.public_profile, Permission.user_friends, 
                /*Permission.user_games_activity, */Permission.publish_actions);

            if (session != null)
            {
                MessageDialog dialog = new MessageDialog(session.AccessToken, "Access Token Facebook");
                
                await dialog.ShowAsync();
            }
        }

        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            await FacebookClient.Current.LogoutAsync();
        }

        private async void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            /*
            string graphPath = "me";

            if (string.IsNullOrEmpty(graphPath))
                return;

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "fields", "id,name,birthday,gender,first_name,locale,location,middle_name,timezone"},
            };

            Request request = new Request(FacebookClient.Current.CurrentSession,
                Utils.HttpMethod.GET,
                graphPath,
                parameters);*/
            /*
            var respone = await request.ExecuteAsync();
            
            if (respone != null)
            {
                if (respone.RequestError != null)
                {
                    var requestError = respone.RequestError;

                    MessageDialog dialog = new MessageDialog(requestError.Message, requestError.Type);
                    await dialog.ShowAsync();
                }
                else if (respone.GraphObject != null)
                {
                    var graphObject = respone.GraphObject.Cast<GraphUser>();

                    MessageDialog dialog = new MessageDialog(respone.RawStringRespone, 
                        graphObject.ID);
                    await dialog.ShowAsync();
                }



            }*/

            /*
            BatchRequest batchRequest = new BatchRequest(FacebookClient.Current.CurrentSession);
            batchRequest.AddRequest(request);
            
            var responeses = await batchRequest.ExecuteAsync();

            foreach (var respone in responeses)
            {
                if (respone.RequestError != null)
                {
                    var requestError = respone.RequestError;

                    MessageDialog dialog = new MessageDialog(requestError.Message, requestError.Type);
                    dialog.ShowAsync();
                }
            }*/

            try
            {
                GraphUser aboutMe = await FacebookClient.Current.GetAboutMeAsync(GraphUser.ID_FIELD, GraphUser.NAME_FIELD, GraphUser.FIRST_NAME_FIELD, GraphUser.LAST_NAME_FIELD, GraphUser.PICTURE_FIELD+".type(large).width(200).height(200)");
                userInfoView.DataContext = aboutMe;
                userInfoView.ProfileImageUri = new Uri(aboutMe.Picture.Url);
                
                List<GraphUser> myFriends = await FacebookClient.Current.GameService.GetInvitableFriendList(GraphUser.ID_FIELD, GraphUser.NAME_FIELD, "picture.type(large).width(50).height(50)");
                friends.Clear();
                foreach (var friend in myFriends)
                {
                    friends.Add(friend);
                }

                await FacebookClient.Current.GameService.GetMyScrore();
            }
            catch (GraphAPIRequestException graphAPIException)
            {
                FacebookRequestError error = graphAPIException.FacebookRequestError;
                if (error.ShouldNotifyToUser)
                {
                    MessageDialog dialog = new MessageDialog(error.ErrorUserMessage, error.ErrorUserTitle);
                    dialog.ShowAsync();
                }

                System.Diagnostics.Debug.WriteLine("Graph API Error {0} - {1}", error.Code, error.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unhandled Exception in Graph API");
            }

            
        }
    }
}
