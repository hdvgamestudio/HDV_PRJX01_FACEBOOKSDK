using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Models
{
    /// <summary>
    /// Các quyền truy cập của API
    /// </summary>
    public enum Permission
    {
        public_profile = 0,
        user_friends = 1,
        user_birthday = 2,
        user_games_activity = 3,
        user_likes = 4,
        publish_actions = 5,
    }
}
