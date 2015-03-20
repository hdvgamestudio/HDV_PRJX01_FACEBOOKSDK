using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Models
{
    /// <summary>
    /// Điểm của người dùng trên Facebook
    /// </summary>
    public class FacebookScore : GraphObject
    {
        internal override void Initialize(Newtonsoft.Json.Linq.JToken metadata)
        {
            base.Initialize(metadata);
        }
    }
}
