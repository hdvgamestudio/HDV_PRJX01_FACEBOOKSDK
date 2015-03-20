using HDV.FacebookSDK.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Utils
{
    public static class HttpUtil
    {
        internal const char URL_PARAM_SEPARATOR = '&';
        internal const char URL_PARAM_KEY_VALUE_SEPARATOR = '=';
        internal const string GRAPH_API_REQUEST_TEMPLATE = "{0}/{1}/{2}?access_token={3}";

        /// <summary>
        /// Tạo một URL đầy đủ cho một Http Request
        /// </summary>
        /// <param name="hostname">Địa chỉ host</param>
        /// <param name="urlParams">Danh sách các đối số URL</param>
        /// <returns></returns>
        public static string CreateHttpUrl(string hostname, Dictionary<string,string> urlParams)
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }

        /// <summary>
        /// Phân tích để lấy các cặp URL Parameter từ một chuỗi
        /// </summary>
        /// <param name="URLParamsString">Chuỗi Parameter</param>
        /// <param name="URLParams">Các cặp phân tích được</param>
        /// <returns></returns>
        public static bool TryExtractURLParams(string URLParamsString, out Dictionary<string, string> URLParams)
        {
            URLParams = null;

            var urlParamPairs = URLParamsString.Split(URL_PARAM_SEPARATOR);
            foreach (var urlParamPair in urlParamPairs)
            {
                var keyValuePair = urlParamPair.Split(URL_PARAM_KEY_VALUE_SEPARATOR);
                if (keyValuePair.Length > 1)
                {
                    if (URLParams == null)
                        URLParams = new Dictionary<string, string>();

                    URLParams.Add(keyValuePair[0], keyValuePair[1]);
                }
            }

            return true;
        }

        /// <summary>
        /// Tạo một URI gọi Graph API
        /// </summary>
        /// <param name="graphPath">Graph Path</param>
        /// <returns></returns>
        public static string CreateGraphAPIUrl(string graphPath, string accessToken, Dictionary<string, string> parameters)
        {
            string baseRequest = string.Format(GRAPH_API_REQUEST_TEMPLATE,
                                    ConfigurationConstants.GRAPH_API_URL,
                                    ConfigurationConstants.GRAPH_API_VERSION,
                                    graphPath,
                                    accessToken);

            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder requestParams = new StringBuilder();

                foreach (var parameter in parameters)
                {
                    requestParams.AppendFormat("&{0}={1}", parameter.Key, parameter.Value);    
                }

                baseRequest += requestParams.ToString();
            }

            return baseRequest;
        }

    }

    internal class HttpWebClient
    {
        private string httpMethod;
        private string requestUriString;
        private Dictionary<string, string> requestHeaders;

        public HttpWebClient(string requestUriString, HttpMethod httpMethod = HttpMethod.GET, 
            Dictionary<string, string> requestHeaders = null)
        {
            this.requestUriString = requestUriString;
            this.httpMethod = httpMethod.ToString();
            this.requestHeaders = requestHeaders;
        }

        /// <summary>
        /// Thực thi Http Request bất đồng bộ
        /// </summary>
        /// <returns></returns>
        public Task<string> ExecuteAsync()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(requestUriString);
                httpRequest.Method = httpMethod;
                

                if (requestHeaders != null && requestHeaders.Count() > 0)
                {
                    httpRequest.BeginGetRequestStream(OnGetRequestStreamCallback, 
                        new object[] { httpRequest, tcs });
                }
                else
                {
                    httpRequest.BeginGetResponse(OnGetResponeCallback, new object[] { httpRequest, tcs });
                }
            } 
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        private void OnGetRequestStreamCallback(IAsyncResult iar)
        {
            var parameters = iar.AsyncState as object[];
            HttpWebRequest httpRequest = parameters[0] as HttpWebRequest;
            TaskCompletionSource<string> tcs = parameters[1] as TaskCompletionSource<string>;

            try
            {
                //Write header here

                httpRequest.BeginGetResponse(OnGetResponeCallback, parameters);
            } 
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }

        private void OnGetResponeCallback(IAsyncResult iar)
        {
            var parameters = iar.AsyncState as object[];
            HttpWebRequest httpRequest = parameters[0] as HttpWebRequest;
            TaskCompletionSource<string> tcs = parameters[1] as TaskCompletionSource<string>;

            try
            {
                WebResponse respone = httpRequest.EndGetResponse(iar);

                using (var responeStream = respone.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responeStream))
                    {
                        string responeMessage = streamReader.ReadToEnd();

                        tcs.SetResult(responeMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }
    }
}
