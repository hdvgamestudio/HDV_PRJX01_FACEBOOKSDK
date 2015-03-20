using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDV.FacebookSDK.Models
{
    /// <summary>
    /// Thông tin của người dùng
    /// </summary>
    public class GraphUser : GraphObject
    {
        public const string ID_FIELD = "id";
        public const string BIRTHDAY_FIELD = "birthday";
        public const string EMAIL_FIELD = "email";
        public const string FIRST_NAME_FIELD = "first_name";
        public const string GENDER_FIELD = "gender";
        public const string LAST_NAME_FIELD = "last_name";
        public const string LINK_FIELD = "link";
        public const string LOCALE_FIELD = "locale";
        public const string LOCATION_FIELD = "location";
        public const string MIDDLE_NAME_FIELD = "middle_name";
        public const string NAME_FIELD = "name";
        public const string TIMEZONE_FIELD = "timezone";
        public const string PICTURE_FIELD = "picture";

        public static readonly string[] ALL_FIELDS = new string[]
        {
            ID_FIELD,
            BIRTHDAY_FIELD,
            EMAIL_FIELD,
            FIRST_NAME_FIELD,
            GENDER_FIELD,
            LAST_NAME_FIELD,
            LINK_FIELD,
            LOCALE_FIELD,
            LOCATION_FIELD,
            MIDDLE_NAME_FIELD,
            NAME_FIELD,
            TIMEZONE_FIELD,
            PICTURE_FIELD,
        };

        private string m_ID;
        private DateTime m_Birthday;
        private string email;
        private string firstName;
        private string gender;
        private string lastName;
        private string link;
        private string locale;
        private FacebookPage location;
        private string middleName;
        private string name;
        private int timeZone;
        private Picture picture;

        internal override void Initialize(JToken metadata)
        {
            base.Initialize(metadata);

            //ID Field
            this.m_ID = GetField<string>(ID_FIELD);

            //Birthday Field
            var birthdayString = GetField<string>(BIRTHDAY_FIELD);
            if (!string.IsNullOrEmpty(birthdayString))
            {
                try
                {
                    m_Birthday = DateTime.ParseExact(birthdayString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            //Email Field
            this.email = GetField<string>(EMAIL_FIELD);

            //First Name Field
            this.firstName = GetField<string>(FIRST_NAME_FIELD);

            //Gender Field
            this.gender = GetField<string>(GENDER_FIELD);

            //Last Name Field
            this.lastName = GetField<string>(LAST_NAME_FIELD);

            //Link Field
            this.link = GetField<string>(LINK_FIELD);

            //Locale Field
            this.locale = GetField<string>(LOCALE_FIELD);

            //Location Field 
            try
            {
                var locationToken = GetField<JToken>(LOCATION_FIELD);
                if (locationToken != default(JToken)) 
                {
                    FacebookPage locationPage = new FacebookPage();
                    locationPage.Initialize(locationToken);
                    this.location = locationPage;
                }
            } 
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            //Middle Name Field
            this.middleName = GetField<string>(MIDDLE_NAME_FIELD);

            //Name Field
            this.name = GetField<string>(NAME_FIELD);

            //Time Zone Field
            this.timeZone = GetField<int>(TIMEZONE_FIELD);

            //Picture Field
            try
            {
                var pictureToken = GetField<JToken>(PICTURE_FIELD);
                if (pictureToken != default(JToken))
                {
                    Picture profilePicture = new Picture();
                    profilePicture.Initialize(pictureToken);
                    this.picture = profilePicture;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// ID của User
        /// </summary>
        public string ID
        {
            get
            {
                return m_ID;
            }
        }

        /// <summary>
        /// Ngày sinh của User
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return m_Birthday;
            }
        }

        /// <summary>
        /// Email của User
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
        }

        /// <summary>
        /// Giới tính của User
        /// </summary>
        public string Gender
        {
            get
            {
                return gender;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
        }

        /// <summary>
        /// Địa chỉ đường dẫn đến trang Profile của User
        /// </summary>
        public string Link
        {
            get
            {
                return link;
            }
        }

        public string Locale
        {
            get
            {
                return locale;
            }
        }

        public FacebookPage Location
        {
            get
            {
                return location;
            }
        }

        public string MiddleName
        {
            get
            {
                return middleName;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int TimeZone
        {
            get
            {
                return timeZone;
            }
        }

        public Picture Picture
        {
            get
            {
                return picture;
            }
        }
    }
}
