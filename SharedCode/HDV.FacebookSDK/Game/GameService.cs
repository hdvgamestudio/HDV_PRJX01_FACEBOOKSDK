using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using HDV.FacebookSDK.GraphAPI;
using HDV.FacebookSDK.Models;

namespace HDV.FacebookSDK.Game
{
    /// <summary>
    /// Game Service của Facebook
    /// </summary>
    public class GameService
    {
        internal GameService()
        {

        }

        /// <summary>
        /// Lấy danh sách bạn bè có thể mời chơi qua facebook
        /// </summary>
        /// <returns></returns>
        public async Task<List<GraphUser>> GetInvitableFriendList(params string[] fields)
        {
            Request request = Request.CreateGetInvitableFriendListRequest(FacebookClient.Current.CurrentSession, fields);
            Respone respone = await request.ExecuteAsync();
            if (respone.RequestError != null)
            {
                throw new GraphAPIRequestException(respone.RequestError);
            }

            List<GraphUser> invitableFriends = new List<GraphUser>();
            if (respone.IsPagingResult && respone.GraphObjectList != null)
            {
                foreach (var graphObject in respone.GraphObjectList)
                {
                    invitableFriends.Add(graphObject.Cast<GraphUser>());
                }
            }

            return invitableFriends;
        }

        public async Task<List<FacebookScore>> GetUserScrores(string userId)
        {
            Request request = Request.CreateGetUserScoresRequest(FacebookClient.Current.CurrentSession, userId);
            Respone respone = await request.ExecuteAsync();

            List<FacebookScore> userScores = new List<FacebookScore>();
            if (respone.RequestError != null)
            {
                throw new GraphAPIRequestException(respone.RequestError);
            }

            GraphObject graphOject = respone.GraphObject;
            var data = graphOject.GetField<JToken>("data");
            if (data != null)
            {
                foreach (var scoreToken in data)
                {
                    FacebookScore scrore = new FacebookScore();
                }
            }

            return userScores;
        }
    }
}
