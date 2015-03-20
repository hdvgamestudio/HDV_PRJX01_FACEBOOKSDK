using HDV.FacebookSDK.Constants;
using HDV.FacebookSDK.Models;
using HDV.FacebookSDK.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.GraphAPI
{
    /// <summary>
    /// Lớp Request tới Facebook qua thư viện GraphAPI
    /// </summary>
    public class Request
    {
        private string graphPath;
        private HttpMethod httpMethod;
        private FacebookSession session;
        private Dictionary<string, string> parameters;

        public Request(FacebookSession session, HttpMethod httpMethod, string graphPath, 
            Dictionary<string, string> parameters)
        {
            this.session = session;
            this.graphPath = graphPath;
            this.httpMethod = httpMethod;
            this.parameters = parameters;
        }

        public Request(FacebookSession session, string graphPath)
            : this(session, HttpMethod.GET, graphPath, null)
        { }

        public Request(FacebookSession session)
            : this(session, string.Empty)
        { }

        public Request(HttpMethod httpMethod, string graphPath, 
            Dictionary<string, string> parameters)
            : this(null, httpMethod, graphPath, parameters)
        { }

        public Request(string graphPath, Dictionary<string, string> parameters)
            : this(HttpMethod.GET, graphPath, parameters)
        { }

        public Request(string graphPath)
            : this(graphPath, null)
        { }

        public Request()
        { }

        public string GraphPath
        { 
            set
            {
                this.graphPath = value;
            }
            get
            {
                return graphPath;
            }
        }

        /// <summary>
        /// Phương thức HTTP của Request
        /// </summary>
        public HttpMethod HttpMethod
        {
            set
            {
                this.httpMethod = value;
            }
            get
            {
                return httpMethod;
            }
        }

        /// <summary>
        /// Danh sách các đối số được truyền vào Request
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            set
            {
                this.parameters = value;
            }
            get
            {
                return parameters;
            }
        }

        /// <summary>
        /// Tạo ra một Request để lấy thông tin User
        /// </summary>
        /// <param name="session">Phiên Facebook làm việc</param>
        /// <param name="userId">ID của User</param>
        /// <param name="fields">Danh sách các trường cần lấy</param>
        /// <returns></returns>
        public static Request CreateGetUserInfoRequest(FacebookSession session, 
            string userId, params string[] fields)
        {
            Request request = new Request(session, userId);
            request.HttpMethod = HttpMethod.GET;

            if (fields != null && fields.Length > 0)
            {
                StringBuilder fieldFilter = new StringBuilder();

                foreach (var field in fields)
                {
                    if (fieldFilter.Length > 0)
                        fieldFilter.Append(',');

                    fieldFilter.Append(field);
                }

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "fields", fieldFilter.ToString() }
                };

                request.parameters = parameters;
            }
           
            return request;
        }

        public static Request CreateGetUserPropfilePictureRequest(FacebookSession session,
            string userId, bool isRedirect=false, PictureType type=PictureType.Normal, int width=0, int height=0)
        {
            Request request = new Request(session, string.Format("{0}/picture", userId));
            request.HttpMethod = HttpMethod.GET;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("redirect", isRedirect ? "1" : "0");
            parameters.Add("type", type.ToString().ToLower());
            if (width > 0 && height > 0)
            {
                parameters.Add("width", width.ToString());
                parameters.Add("height", width.ToString());
            }
            request.Parameters = parameters;
            return request;
        }

        public static Request CreateGetFriendListOfUserRequest(FacebookSession session,
            string userId, params string[] fields)
        {
            Request request = new Request(session, string.Format("{0}/friends", userId));
            request.HttpMethod = HttpMethod.GET;

            if (fields != null && fields.Length > 0)
            {
                StringBuilder fieldFilter = new StringBuilder();

                foreach (var field in fields)
                {
                    if (fieldFilter.Length > 0)
                        fieldFilter.Append(',');

                    fieldFilter.Append(field);
                }

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "fields", fieldFilter.ToString() }
                };

                request.parameters = parameters;
            }

            return request;
        }

        public static Request CreateGetTaggableFriendListOfUserRequest(FacebookSession session,
            string userId, params string[] fields)
        {
            Request request = new Request(session, string.Format("{0}/taggable_friends", userId));
            request.HttpMethod = HttpMethod.GET;

            if (fields != null && fields.Length > 0)
            {
                StringBuilder fieldFilter = new StringBuilder();

                foreach (var field in fields)
                {
                    if (fieldFilter.Length > 0)
                        fieldFilter.Append(',');

                    fieldFilter.Append(field);
                }

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "fields", fieldFilter.ToString() }
                };

                request.parameters = parameters;
            }

            return request;
        }

        #region Game Services
        /// <summary>
        /// Tạo ra Graph API Request để lấy danh sách bạn bè có thể mời được
        /// </summary>
        /// <param name="session">Phiên làm việc</param>
        /// <param name="fields">Các trường cần lấy</param>
        /// <returns></returns>
        public static Request CreateGetInvitableFriendListRequest(FacebookSession session,
            params string[] fields)
        {
            Request request = new Request(session, string.Format("me/invitable_friends"));
            request.HttpMethod = HttpMethod.GET;

            if (fields != null && fields.Length > 0)
            {
                StringBuilder fieldFilter = new StringBuilder();

                foreach (var field in fields)
                {
                    if (fieldFilter.Length > 0)
                        fieldFilter.Append(',');

                    fieldFilter.Append(field);
                }

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "fields", fieldFilter.ToString() }
                };

                request.parameters = parameters;
            }

            return request;
        }

        public static Request CreateGetUserScoresRequest(FacebookSession session, 
            string userID)
        {
            Request request = new Request(session, string.Format("{0}/scores", userID));
            request.HttpMethod = HttpMethod.GET;

            return request;
        }
        #endregion

        public async Task<Respone> ExecuteAsync()
        {
            try
            {
                string requestUriString = HttpUtil.CreateGraphAPIUrl(
                    string.IsNullOrEmpty(graphPath) ? ConfigurationConstants.DEFAULT_GRAPHPATH : graphPath,
                    session.AccessToken,
                    parameters);

                HttpWebClient graphApiClient = new HttpWebClient(requestUriString, httpMethod, null);
                string rawStringRespone = await graphApiClient.ExecuteAsync();

                return new Respone(rawStringRespone);
            }
            catch (WebException webException)
            {
                var webRespone = webException.Response;
                if (webRespone != null)
                {
                    using (var webResponeStreamReader = new StreamReader(webRespone.GetResponseStream()))
                    {
                        string rawStringMessage = webResponeStreamReader.ReadToEnd();

                        return new Respone(rawStringMessage);
                    }
                }
            }

            return null;
        }
    }
}
