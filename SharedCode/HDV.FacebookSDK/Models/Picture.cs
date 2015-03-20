using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Models
{
    public enum PictureType
    {
        Square,
        Small,
        Normal,
        Large
    }

    /// <summary>
    /// Ảnh
    /// </summary>
    public class Picture : GraphObject
    {
        public const string URL_FIELD = "url";
        public const string IS_SILHOUETTE_FIELD = "is_silhouette";
        public const string HEIGHT_FIELD = "height";
        public const string WIDTH_FIELD = "width";
        private const string DATA_FIELD = "data";
        
        private string m_Url;
        private bool m_IsSilhouette;
        private int m_Height;
        private int m_Width;

        internal override void Initialize(JToken metadata)
        {
            metadata = metadata[DATA_FIELD];

            base.Initialize(metadata);

            //Url Field
            this.m_Url = GetField<string>(URL_FIELD);

            //Is Silhouette Field
            this.m_IsSilhouette = GetField<bool>(IS_SILHOUETTE_FIELD);

            //Height Field
            this.m_Height = GetField<int>(HEIGHT_FIELD);

            //Width Field
            this.m_Width = GetField<int>(WIDTH_FIELD);
        }

        public string Url
        {
            get
            {
                return m_Url;
            }
        }
        
        public bool IsShilhouette
        {
            get
            {
                return m_IsSilhouette;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }
        
    }
}
