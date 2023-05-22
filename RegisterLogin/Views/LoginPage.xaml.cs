using Acr.UserDialogs;
using Newtonsoft.Json;
using RegisterLogin.Configuration;
using RegisterLogin.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UserDetails = RegisterLogin.Models.UserDetails;

namespace RegisterLogin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region Declaration_obj_AppStoragePrefs

        IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        UserDetails user = new UserDetails();

        public static string ExistingLinkedinToken
        {
            get => Preferences.Get("ExistingLinkedinToken", Convert.ToString("", CultureInfo.InvariantCulture));
            set { Preferences.Set("ExistingLinkedinToken", Convert.ToString(value, CultureInfo.InvariantCulture)); }
        }

        public static string ExistingLTokenExpiry
        {
            get => Preferences.Get("ExistingLTokenExpiry", Convert.ToString("", CultureInfo.InvariantCulture));
            set { Preferences.Set("ExistingLTokenExpiry", Convert.ToString(value, CultureInfo.InvariantCulture)); }
        }

        #endregion

        #region ServerAuth_EmailPass

        public LoginPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<RegisterLogin.App, string>((App)Application.Current, "Acknowledged", (sender, arg) =>
            {
                var aa = arg;
            });
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(UsernameField.Text) || string.IsNullOrEmpty(PasswordField.Text))
            {

                await DisplayAlert("Alert", "Please provide valid credentials to login.", "OK");
                return;
            }

            try
            {

                var login = new LoginModel
                {
                    UserName = UsernameField.Text,
                    PasswordHash = PasswordField.Text

                };

                Transporter.email = UsernameField.Text;
                Transporter.picture = "yankees.jpg";

                var json = JsonConvert.SerializeObject(login);

                UserDialogs.Instance.ShowLoading("Signing In...", MaskType.Gradient);

                using (var httpClient = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender1, cert, chain, sslPolicyErrors) => true;

                    //var response = await httpClient.PostAsync("https://projectplacermobile.azurewebsites.net/Token", new StringContent(json, Encoding.UTF8, "application/json"));

                    //var response = await httpClient.PostAsync("https://localhost:44327/api/AdmUSER/SELECT?tYPE=USERALL&nUSR_ID=0&email=patrick.depirro@ivstech.com&mSGC_CONTACT_EMAIL=&pFXTEXT=&fLTRITEMS=&email=", new StringContent(json, Encoding.UTF8, "application/json"));

                    UserDialogs.Instance.HideLoading();

                    var response = await httpClient.GetAsync("https://projectplacermobile.azurewebsites.net/api/AdmUSER/SELECT?tYPE=USERALL&nUSR_ID=0&email=" + login.UserName + "&mSGC_CONTACT_EMAIL=&pFXTEXT=&fLTRITEMS=&email=");

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await DisplayAlert("Login Error!", "Invalid Username or Password", "OK");

                        return;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        await DisplayAlert("System Error!", "Unable to login due to Server Error. Please try again later.", "OK");

                        return;
                    }

                    var data = await response.Content.ReadAsStringAsync();


                    // Saving user details (JSON string)
                    Xamarin.Essentials.Preferences.Set("user_details", data);

                    // Getting user detils (JSON string)
                    var data1 = Xamarin.Essentials.Preferences.Get("user_details", string.Empty);

                    // Parsing User details from JSON string to Object
                    var user = JsonConvert.DeserializeObject<RegisterLogin.Models.LoginModel>(data1);


                    Application.Current.MainPage = new AppShell();


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

            Application.Current.MainPage = new AppShell();
        }

        private void signupButton_Clicked(object sender, EventArgs e) => Application.Current.MainPage = new RegisterPage();

        #endregion

        #region GoogleAuth

        public void btnGoogleAuth_Clicked(object sender, EventArgs e)
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
            _googleManager.Login(OnLoginComplete);
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                UserDetails userDetails = new UserDetails()
                {
                    Name = googleUser.Name,
                    Email = googleUser.Email,
                    googleUserData = googleUser,
                    ImageUri = googleUser.Picture,
                };

                await App.Current.MainPage.Navigation.PushModalAsync(new TempDashboard(userDetails));
            }
            else
                await DisplayAlert("Message", message, "Ok");
        }

        private async void GoogleLogout() => await Task.Run(() => _googleManager.Logout());

        #endregion

        #region LinkedinAuth

        private async void btnLinkedinAuth_Clicked(object sender, EventArgs e) => await ProcessLinkedinAuth();

        #region Native_Active_Current

        private async Task ProcessLinkedinAuth()
        {
            var LinkedinAuthenticator = new OAuth2Authenticator(
                clientId: CredsConsts.linkedin_ClientId,
                clientSecret: CredsConsts.linkedin_ClientSecret,
                scope: CredsConsts.linkedin_Scopes,
                authorizeUrl: new Uri(CredsConsts.linkedin_AuthorizeUriString),
                redirectUrl: new Uri(CredsConsts.linkedin_RedirectUriString),
                accessTokenUrl: new Uri(CredsConsts.linkedin_AccesstokenUri),
                null,
                false);

            var page = new AuthenticatorPage(LinkedinAuthenticator);
            LinkedinAuthenticator.Completed += OnLinkedInAuthenticationComplete;
            await Navigation.PushModalAsync(page);
            //await page.Navigation.PushModalAsync();
        }

        private async void OnLinkedInAuthenticationComplete(object sender, AuthenticatorCompletedEventArgs e)
        {
            string AuthCode = "";
            var creds = e.Account.Properties.ToArray();
            AuthCode = creds[0].Value;

            TimeSpan time = TimeSpan.FromSeconds(double.Parse(creds[1].Value));
            ExistingLTokenExpiry = (DateTime.Now + time).ToString();

            if (!e.IsAuthenticated)
                return;
            try
            {
                await Navigation.PopModalAsync();

                var request = new OAuth2Request(
                    "GET",
                    new Uri(CredsConsts.linkedin_ProfileRequestUri + AuthCode),
                    null, e.Account);

                request.AccessTokenParameterName = "oauth2_access_token";
                var linkedInResponse = await request.GetResponseAsync();
                var json = linkedInResponse.GetResponseText();

                user = GetUserFromLinkedInJson(json);

                if (user == null) return;
                else
                    await Navigation.PushModalAsync(new TempDashboard(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private UserDetails GetUserFromLinkedInJson(string json)
        {
            try
            {
                LinkedinModelParser FetchedUserData = Newtonsoft.Json.JsonConvert.DeserializeObject<LinkedinModelParser>(json);

                if (FetchedUserData != null)
                {
                    return new UserDetails
                    {
                        Id = FetchedUserData.id.Trim(),
                        Name = (FetchedUserData.firstName.localized.en_US.ToString()).Trim()
                             + " " + (FetchedUserData.lastName.localized.en_US.ToString()).Trim(),
                    };
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #endregion
    }
}