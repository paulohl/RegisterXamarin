using Acr.UserDialogs;
using Newtonsoft.Json;
using RegisterLogin.Models;
using RegisterLogin.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterLogin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {


        string UserType = string.Empty;

        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var content = "";
            string content_URL = "";
            HttpClient client = new HttpClient();
            var RestURL = "https://projectplacermobile.azurewebsites.net/api/SYSINDS/SELECT?tYPE=LIST&prefixText=";
            client.BaseAddress = new Uri(RestURL);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(RestURL);
            content = await response.Content.ReadAsStringAsync();
            content_URL = content.Replace("{\"Code\":200,\"Status\":\"OK\",\"Message\":\"Success\",\"Data\":", "");
            content = content_URL.Replace("]}", "]");
            var Items = JsonConvert.DeserializeObject<List<SysIndsIndustry>>(content);
            IndustryPicker.ItemsSource = (System.Collections.IList)Items;

        }


        private async void registerButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(FirstName.Text) ||
                string.IsNullOrEmpty(LastName.Text) ||
                string.IsNullOrEmpty(EmailFields.Text) ||
                string.IsNullOrEmpty(PasswordField.Text) ||
                string.IsNullOrEmpty(UserType) ||
                IndustryPicker.SelectedIndex < 0 ||
                IndustryPicker.SelectedItem is null)
            {
                await DisplayAlert("Input Error!", "Please fill the form. All fields are required to register with Project Placer", "OK");
                return;
            }

            if (CompanyInfo.IsVisible)
            {
                if (string.IsNullOrEmpty(CompanyName.Text))
                {
                    await DisplayAlert("Input Error!", "Your Company Name is required when registering as a Company on Project Placer", "OK");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(PasswordField.Text) && PasswordField.Text != ConfirmPasswordField.Text)
            {
                await DisplayAlert("Input Error!", "Password and Confirm password didn't match.", "OK");

                PasswordField.Text = string.Empty;
                ConfirmPasswordField.Text = string.Empty;

                return;
            }

            var response = await RegisterUser();

            if (response != null)
            {

                await DisplayAlert("Successfull", "User has been registered successfully.", "OK");
                Application.Current.MainPage = new AppShell();
            }
        }

        private async Task<AspNetUser1> RegisterUser()
        {
            try
            {

                var register = new RegisterModel
                {
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Email = EmailFields.Text,
                    Password = PasswordField.Text,
                    Industry = (int)(IndustryPicker.SelectedItem as SysIndsIndustry).IndsId,
                    UserType = UserType,
                    //CompanyName = CompanyName.Text,
                    //CompanyUrl = CompanyURL.Text
                };

                var json = JsonConvert.SerializeObject(register);

                UserDialogs.Instance.ShowLoading("Signing Up...", MaskType.Gradient);

                using (var httpClient = new HttpClient())
                {

                    var response = await httpClient.PostAsync("https://projectplacermobile.azurewebsites.net/api/user/register", new StringContent(json, Encoding.UTF8, "application/json"));

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        await DisplayAlert("System Error!", "Unable to sign up due to Server Error. Please try again later.", "OK");

                        return null;
                    }

                    var data = await response.Content.ReadAsStringAsync();


                    // Saving user details (JSON string)
                    Xamarin.Essentials.Preferences.Set("user_details", data);



                    // Parsing User details from JSON string to Object
                    var user = JsonConvert.DeserializeObject<AspNetUser1>(data);


                    return user;


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");

                return null;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }


        private void loginButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async Task<List<SysIndsIndustry>> GetIndustriesFromServer()
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Loading Industries...");

                var httpClient = new HttpClient();

                var jsonResponse = await httpClient.GetStringAsync("https://projectplacermobile.azurewebsites.net/api/industries");

                if (string.IsNullOrEmpty(jsonResponse))
                    return new List<SysIndsIndustry>();

                Preferences.Set("Industries", jsonResponse);

                var data = JsonConvert.DeserializeObject<List<SysIndsIndustry>>(jsonResponse);

                return data;

            }
            catch
            {
                return new List<SysIndsIndustry>();
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        private void CompanyChkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ResourceChkBox.IsChecked = !e.Value;
            UserType = e.Value ? "C" : "R";

            if (UserType == "C")
            {
                CompanyInfo.IsVisible = true;
            }
            else
            {
                CompanyInfo.IsVisible = false;
            }

        }

        private void ResourceChkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CompanyChkBox.IsChecked = !e.Value;
            UserType = e.Value ? "R" : "C";

            if (UserType == "C")
            {
                CompanyInfo.IsVisible = true;
            }
            else
            {
                CompanyInfo.IsVisible = false;
            }


        }

    }
}