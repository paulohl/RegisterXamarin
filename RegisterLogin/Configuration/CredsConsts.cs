namespace RegisterLogin.Configuration
{
    public static class CredsConsts
    {
        /*
         * Google Creds
         * Passed from google-services.json file in individual projects[android & iOS]
         */

        //Linkedin Consts
        public static string linkedin_ClientId = @"867ff379z2qiuw";
        public static string linkedin_ClientSecret = @"xiEN1vyiQiIIkYqm";
        public static string linkedin_Scopes = @"r_liteprofile r_emailaddress";

        public static string linkedin_AuthorizeUriString = @"https://www.linkedin.com/uas/oauth2/authorization";
        public static string linkedin_RedirectUriString = @"https://www.linkedin.com/developers/tools/oauth/redirect://com.companyname.registerlogin://";
        public static string linkedin_AccesstokenUri = @"https://www.linkedin.com/uas/oauth2/accessToken";

        public static string linkedin_ProfileRequestUri = @"https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))&oauth2_access_token=";

    }
}
