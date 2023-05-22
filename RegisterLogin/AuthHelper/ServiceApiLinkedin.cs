using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RegisterLogin.AuthHelper
{

    public class ServiceApiLinkedin : IDisposable
    {
        static string clientId = "867ff379z2qiuw";
        static string state = "fooobar";
        static string scope = "r_liteprofile%20r_emailaddress";
        static string responseType = "code";
        static string redirectUrl = "https://www.linkedin.com/developers/tools/oauth/redirect";
        static string callbackUrl = "com.companyname.registerlogin://";

        static string accessUrl = "https://www.linkedin.com/oauth/v2/accessToken";
        static string secret = "xiEN1vyiQiIIkYqm";
        static string grantType = "authorization_code";

        public async Task<WebAuthenticatorResult> Login()
        {
            // efectua el login de linkedin
            string LoginUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type={responseType}&client_id={clientId}&state={state}&scope={scope}&redirect_uri={redirectUrl}";

            WebAuthenticatorOptions Options = new WebAuthenticatorOptions();
            Options.PrefersEphemeralWebBrowserSession = true;
           

            return await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
            {
                Url=new Uri(redirectUrl),
                CallbackUrl=new Uri(callbackUrl),
                PrefersEphemeralWebBrowserSession=true
            });
        }

        public async Task<string> RequestAccessToken(string authCode)
        {
            HttpClient linClient = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", secret)
            });

            var response = await linClient.PostAsync(accessUrl, requestContent);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return null;
        }

        public async Task<string> RequestHandle(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/clientAwareMemberHandles?q=members&projection=(elements*(primary,type,handle~))");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return null; 
        }

        public async Task<string> ObtainProfile(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/me");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return null;
        }

        public async Task<string> ObtainProfilePhoto(string accessToken)
        {
            HttpClient linClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))");

            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            HttpResponseMessage response = await linClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}
