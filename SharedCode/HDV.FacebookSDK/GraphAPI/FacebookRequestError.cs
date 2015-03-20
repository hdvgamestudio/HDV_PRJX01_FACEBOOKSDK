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
    /// Các lỗi xảy ra trong quá trình request
    /// </summary>
    public class FacebookRequestError
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        [JsonProperty(PropertyName="code")]
        public int Code 
        { 
            set; 
            get; 
        }

        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty(PropertyName="message")]
        public string Message
        {
            set;
            get;
        }

        /// <summary>
        /// Kiểu lỗi
        /// </summary>
        [JsonProperty(PropertyName="type")]
        public string Type
        {
            set;
            get;
        }

        /// <summary>
        /// Subcode của lỗi
        /// </summary>
        [JsonProperty(PropertyName="error_subcode")]
        public int ErrorSubcode
        {
            set;
            get;
        }

        /// <summary>
        /// Tiêu đề của dialog thông báo lỗi trực tiếp tới người dùng
        /// </summary>
        [JsonProperty(PropertyName="error_user_title")]
        public string ErrorUserTitle
        {
            set;
            get;
        }

        /// <summary>
        /// Message báo lối trực tiếp tới người dùng
        /// </summary>
        [JsonProperty(PropertyName="error_user_message")]
        public string ErrorUserMessage
        {
            set;
            get;
        }

        /// <summary>
        /// Message này có cần phải thông báo trực tiếp tới người dùng không
        /// </summary>
        [JsonIgnore()]
        public bool ShouldNotifyToUser
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorUserMessage);
            }
        }
    }
}
