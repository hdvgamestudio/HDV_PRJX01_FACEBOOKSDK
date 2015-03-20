using HDV.FacebookSDK.Models;
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
    public class Respone
    {
        private const string ERROR_PROPERTY = "error";
        private const string PAGING_PROPERTY = "paging";

        private string rawStringRespone;
        private FacebookRequestError requestError;

        private JToken metadata;
        private GraphObject graphOject;
        private List<GraphObject> graphOjectList;
        private PagingResult pagingResult;

        internal Respone(string rawStringRespone)
        {
            this.rawStringRespone = rawStringRespone;
            this.metadata = JToken.Parse(rawStringRespone);

            Parse();
        }

        internal Respone(JToken metadataToken)
        {
            this.rawStringRespone = metadataToken.ToString();
            this.metadata = metadataToken;

            Parse();
        }

        /// <summary>
        /// Parse các trường của string respone
        /// </summary>
        /// <param name="rawStringRespone"></param>
        private void Parse()
        {
            //Check lỗi
            JToken errorToken = metadata[ERROR_PROPERTY];
            if (errorToken != null)
            {
                requestError = errorToken.ToObject<FacebookRequestError>();
                return;
            }

            //Check paging
            JToken pagingToken = metadata[PAGING_PROPERTY];
            if (pagingToken != null)
            {
                pagingResult = pagingToken.ToObject<PagingResult>();
                graphOjectList = new List<Models.GraphObject>();
                var pagingData = metadata["data"];
                foreach (var item in pagingData)
                {
                    GraphObject itemGraphObject = new GraphObject();
                    itemGraphObject.Initialize(item);
                    graphOjectList.Add(itemGraphObject);
                }
                return;
            }

            //Là single graph oject
            this.graphOject = new GraphObject();
            graphOject.Initialize(metadata);
        }

        /// <summary>
        /// Thông tin về lỗi request
        /// </summary>
        public FacebookRequestError RequestError
        {
            get
            {
                return requestError;
            }
        }

        /// <summary>
        /// Là kết quả được trả về. Property này bằng null khi kết quả trả về là dạng Paging
        /// </summary>
        public GraphObject GraphObject
        {
            get
            {
                return graphOject;
            }
        }

        /// <summary>
        /// Xâu nguyên mẫu do Facebook trả về
        /// </summary>
        public string RawStringRespone
        {
            get
            {
                return rawStringRespone;
            }
        }
    
        public bool IsPagingResult
        {
            get
            {
                return pagingResult != null;
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return !string.IsNullOrEmpty(pagingResult.PreviousPageUrl);
            }
        }

        public bool IsLastPage
        {
            get
            {
                return !string.IsNullOrEmpty(pagingResult.NextPageUrl);
            }
        }

        public List<GraphObject> GraphObjectList
        {
            get
            {
                return graphOjectList;
            }
        }
    }
}
