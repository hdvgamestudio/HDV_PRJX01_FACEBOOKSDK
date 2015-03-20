using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Models
{
    /// <summary>
    /// Dữ liệu trong Graph API
    /// </summary>
    public class GraphObject
    {
        protected JToken metadata;
        
        /// <summary>
        /// Khởi tạo giá trị ban đầu cho object
        /// </summary>
        /// <param name="metadata"></param>
        internal virtual void Initialize(JToken metadata)
        {
            this.metadata = metadata;
        }

        /// <summary>
        /// Lấy dữ liệu của một trường trong Graph Object
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của trường</typeparam>
        /// <param name="fieldName">Tên trường</param>
        /// <returns></returns>
        public T GetField<T>(string fieldName)
        {
            var fieldToken = metadata[fieldName];
            if (fieldToken == null)
                return default(T);

            return fieldToken.ToObject<T>();
        }

        /// <summary>
        /// Chuyển kiểu sang kiểu dữ liệu kế thừa từ GraphObject
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu(phải kế thừa từ GraphObject)</typeparam>
        /// <returns></returns>
        public T Cast<T>() 
            where T : GraphObject, new ()
        {
            T result = new T();
            result.Initialize(metadata);
            return result;
        }
    }
}
