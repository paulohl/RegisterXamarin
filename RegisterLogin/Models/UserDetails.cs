using Newtonsoft.Json;
using System;

namespace RegisterLogin.Models
{
    //Retrieval User Data
    public class UserDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Profile { get; set; }

        public Uri ImageUri { get; set; }

        public GoogleUser googleUserData { get; set; }
    }

    #region Google

    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
    }
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);
        void Logout();
    }

    #endregion

    #region Linkedin

    public class FirstName
    {
        public Localized localized { get; set; }
        public PreferredLocale preferredLocale { get; set; }
    }

    public class LastName
    {
        public Localized localized { get; set; }
        public PreferredLocale preferredLocale { get; set; }
    }

    public class Localized
    {
        public string en_US { get; set; }
    }

    public class PreferredLocale
    {
        public string country { get; set; }
        public string language { get; set; }
    }

    public class LinkedinModelParser
    {
        public FirstName firstName { get; set; }
        public LastName lastName { get; set; }
        public string id { get; set; }
    }

    #region EmailResponse

    public class EmailResponseModel
    {
        public EmailResponseElementModel[] elements { get; set; }
    }

    public class EmailResponseElementModel
    {
        public string handle { get; set; }
        public string type { get; set; }
        public bool primary { get; set; }

        [JsonProperty("handle~")]
        public EmailResponseAddressModel handle2 { get; set; }
    }

    public class EmailResponseAddressModel
    {
        public string emailAddress { get; set; }
    }

    public class ProfileModel
    {
        public string localizedFirstName { get; set; }

        public string localizedLastName { get; set; }
    }

    #endregion

    #endregion
}
