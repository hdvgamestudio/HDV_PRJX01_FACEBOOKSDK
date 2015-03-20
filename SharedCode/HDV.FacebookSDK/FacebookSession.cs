using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK
{
    /// <summary>
    /// Phiên làm việc với Facebook
    /// </summary>
    public class FacebookSession
    {
        /// <summary>
        /// App's ID
        /// </summary>
        public string AppID
        {
            internal set;
            get;
        }

        /// <summary>
        /// AccessToken của phiên làm việc
        /// </summary>
        public string AccessToken
        {
            internal set;
            get;
        }
    }
}
