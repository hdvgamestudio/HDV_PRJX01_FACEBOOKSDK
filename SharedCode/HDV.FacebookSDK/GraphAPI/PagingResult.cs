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
    /// Lớp Request tới Facebook qua thư viện GraphAPI
    /// </summary>
    public class PagingResult
    {
        [JsonProperty("next")]
        public string NextPageUrl 
        { 
            set; 
            get; 
        }

        [JsonProperty("previous")]
        public string PreviousPageUrl
        {
            set;
            get;
        }
    }
}
