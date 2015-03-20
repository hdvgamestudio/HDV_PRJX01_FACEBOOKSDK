using HDV.FacebookSDK.Constants;
using HDV.FacebookSDK.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.GraphAPI
{
    /// <summary>
    /// Lớp Request tới Facebook qua thư viện GraphAPI
    /// </summary>
    public class BatchRequest
    {
        private const string BODY = "body";

        private List<Request> requests;
        private FacebookSession session;

        public BatchRequest()
        {
            this.requests = new List<Request>();
        }

        public BatchRequest(FacebookSession session)
            : this()
        {
            this.session = session;   
        }

        public void AddRequest(Request request)
        {
            if (request == null)
                throw new ArgumentException("Request is null");

            if (requests.Contains(request))
                return;

            requests.Add(request);
        }

        public void RemoveRequest(Request request)
        {
            if (request == null)
                throw new ArgumentException("Request is null");

            if (requests.Contains(request))
                requests.Remove(request);
        }

        public async Task<Respone[]> ExecuteAsync()
        {
            if (requests.Count < 0)
                return null;

            #region Make Request Batch JSON
            StringBuilder batchString = new StringBuilder();
            batchString.Append('[');

            foreach (var request in requests)
            {
                StringBuilder relativeURL = new StringBuilder();
                relativeURL.Append(request.GraphPath);
                relativeURL.Append('?');

                if (request.Parameters != null && request.Parameters.Count > 0)
                {
                    bool isFirstParameter = true;
                    foreach (var parameter in request.Parameters)
                    {
                        if (!isFirstParameter)
                            relativeURL.Append('&');

                        relativeURL.AppendFormat("{0}={1}", parameter.Key, parameter.Value);
                        isFirstParameter = false;
                    }
                }

                batchString.AppendFormat("{{\"method\":\"{0}\",\"relative_url\":\"{1}\"}},", request.HttpMethod.ToString(), relativeURL.ToString());
            }

            batchString.Append(']');
            #endregion

            #region Make Request
            var requestUriString = HttpUtil.CreateGraphAPIUrl(
                                        string.Empty, 
                                        session.AccessToken, 
                                        new Dictionary<string, string> 
                                        {
                                            { "batch", batchString.ToString() }
                                        });


            HttpWebClient graphAPIClient = new HttpWebClient(requestUriString, HttpMethod.POST);

            string rawStringRespone = await graphAPIClient.ExecuteAsync();
            #endregion

            //Parse Respone
            if (string.IsNullOrEmpty(rawStringRespone))
                return null;

            JArray responeContainerArray = JArray.Parse(rawStringRespone);
            int responeCount = responeContainerArray.Count;
            Respone[] responeses = new Respone[responeCount];

            int index = 0;
            foreach (var responeContainerToken in responeContainerArray)
            {
                responeses[index] = new Respone(responeContainerToken[BODY].ToString());
                index++;
            }

            return responeses;
        }

    }
}
