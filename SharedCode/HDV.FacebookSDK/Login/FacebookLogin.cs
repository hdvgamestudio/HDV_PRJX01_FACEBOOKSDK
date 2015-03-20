using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HDV.FacebookSDK.Utils;
using HDV.FacebookSDK.Constants;
using HDV.FacebookSDK.Models;

#if NETFX_CORE
using HDV.FacebookSDK.Controls;

#elif WINDOWS_PHONE
using HDV.FacebookSDK.Controls;

#elif ANDROID
using HDV.FacebookSDK.Controls;

#endif

namespace HDV.FacebookSDK.Login
{
    /// <summary>
    /// Lớp đăng nhập cho Facebook
    /// </summary>
    internal class FacebookLogin
    {
#if NETFX_CORE
        public const string DISPLAY = "popup";
        public const string RESPONE_TYPE = "token";
        public const string REDIRECT_URI = "https://www.facebook.com/connect/login_success.html";

        private const string LOGIN_URL_TEMPLATE = 
            @"https://www.facebook.com/dialog/oauth?client_id={0}&display={1}&response_type={2}&redirect_uri={3}&scope={4}";

        private const string LOGOUT_URL_TEMPLATE =
            @"https://www.facebook.com/logout.php?next={0}&access_token={1}";

#elif WINDOWS_PHONE
        public const string DISPLAY = "touch";
        public const string RESPONE_TYPE = "token";
        public const string REDIRECT_URI = "https://www.facebook.com/connect/login_success.html";

        private const string LOGIN_URL_TEMPLATE =
            @"https://m.facebook.com/dialog/oauth?client_id={0}&display={1}&response_type={2}&redirect_uri={3}&scope={4}";

        private const string LOGOUT_URL_TEMPLATE =
            @"https://m.facebook.com/logout.php?next={0}&access_token={1}";
#elif ANDROID
		public const string DISPLAY = "touch";
		public const string RESPONE_TYPE = "token";
		public const string REDIRECT_URI = "https://www.facebook.com/connect/login_success.html";

		private const string LOGIN_URL_TEMPLATE =
			@"https://m.facebook.com/dialog/oauth?client_id={0}&display={1}&response_type={2}&redirect_uri={3}&scope={4}";

		private const string LOGOUT_URL_TEMPLATE =
			@"https://m.facebook.com/logout.php?next={0}&access_token={1}";
#endif

        private const char ACCESS_TOKEN_SEPARATOR = '#';

		#if ANDROID
		public static Task<string> Login(Android.Content.Context context, string appid, string permissions)
        #else
		/// <summary>
        /// Login bất đồng bộ với Facebook
        /// </summary>
        /// <param name="appid">Facebook App ID của ứng dưng</param>
		/// <param name="permissions">Danh sách quyền truy cập</param> 
        /// <returns>AccessToken của người dùng</returns>
        public static Task<string> Login(string appid, string permissions)
		#endif
        {
            if (string.IsNullOrEmpty(permissions))
                permissions = Permission.public_profile.ToString();

            string loginURL = string.Format(LOGIN_URL_TEMPLATE, appid, DISPLAY, RESPONE_TYPE, REDIRECT_URI, permissions);
#if NETFX_CORE
            return FacebookLoginPopup.ShowPopup(loginURL);
#elif WINDOWS_PHONE
            return FacebookLoginDialog.ShowDialog(loginURL);
#elif ANDROID
			return FacebookLoginDialog.ShowDialog(context, loginURL);
#endif
        }

        /// <summary>
        /// Đăng xuất phiên làm việc hiện tại
        /// </summary>
        /// <param name="accessToken">AccessToken của phiên làm việc hiện tại</param>
        /// <returns></returns>
        public static Task Logout(string accessToken)
        {
            string logoutURL = string.Format(LOGOUT_URL_TEMPLATE, REDIRECT_URI, accessToken);   
#if NETFX_CORE
            return FacebookLogoutPopup.ShowPopup(logoutURL);
#elif WINDOWS_PHONE
            return FacebookLogoutDialog.ShowDialog(logoutURL);
#elif ANDROID
			return null;
#endif
        }

        /// <summary>
        /// Kiểm tra xem URL có phải là Redirect URL hay không
        /// </summary>
        /// <param name="callbackURL"></param>
        /// <returns></returns>
        internal static bool IsRedirectURL(string callbackURL)
        {
            string[] urlParts = callbackURL.Split(
               new char[] 
                {
                    ACCESS_TOKEN_SEPARATOR
                });

            return urlParts[0] == REDIRECT_URI;
        }

        /// <summary>
        /// Lấy access token từ URL Callback của Facebook Login
        /// </summary>
        /// <param name="callbackURL">Callback URL</param>
        /// <param name="accessToken">AcessToken lấy được từ URL</param>
        /// <returns>Kết quả</returns>
        internal static bool TryGetAccessTokenFromCallbackURL(string callbackURL, out string accessToken)
        {
            accessToken = string.Empty;

            string[] urlParts = callbackURL.Split(
                new char[] 
                {
                    ACCESS_TOKEN_SEPARATOR
                });

            if (urlParts[0] == REDIRECT_URI)
            {
                //Xử lý dữ liệu để lấy Access Token

                Dictionary<string, string> urlParams;
                if (HttpUtil.TryExtractURLParams(urlParts[1], out urlParams)
                    && urlParams.ContainsKey(FacebookAttributeNames.ACCESS_TOKEN))
                {
                    accessToken = urlParams[FacebookAttributeNames.ACCESS_TOKEN];
                }

                return true;
            }

            return false;
        }
    }
}
