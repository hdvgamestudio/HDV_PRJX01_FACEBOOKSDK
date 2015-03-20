using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.GraphAPI
{
    /// <summary>
    /// Exception khi request Graph API
    /// </summary>
    public class GraphAPIRequestException : Exception
    {
        private FacebookRequestError m_FacebookRequestError;

        public GraphAPIRequestException(FacebookRequestError requestError)
            : base(requestError.Message)
        {
            this.m_FacebookRequestError = requestError;
        }

        public FacebookRequestError FacebookRequestError
        {
            get
            {
                return m_FacebookRequestError;
            }
        }
    }
}
