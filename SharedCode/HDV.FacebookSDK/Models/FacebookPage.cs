using Newtonsoft.Json.Linq;
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
    public class FacebookPage : GraphObject
    {
        internal override void Initialize(JToken metadata)
        {
            base.Initialize(metadata);
        }
    }
}
