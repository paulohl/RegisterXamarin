using Newtonsoft.Json;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using RegisterLogin.Configuration;
using RegisterLogin.Models;
using RegisterLogin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RegisterLogin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        //IGoogleClientManager _googleService = CrossGoogleClient.Current;
        //IOAuth2Service _oAuth2Service;
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
//        public ObservableCollection<AuthNetwork> AuthenticationNetworks { get; set; } = new ObservableCollection<AuthNetwork>()
//        {
//            new AuthNetwork()
//            {
//                Name = "Facebook",
//                Icon = "ic_fb",
//                Foreground = "#FFFFFF",
//                Background = "#4768AD"
//            },
//             new AuthNetwork()
//            {
//                Name = "Instagram",
//                Icon = "ic_ig",
//                Foreground = "#FFFFFF",
//                Background = "#DD2A7B"
//            },
//              new AuthNetwork()
//            {
//                Name = "Google",
//                Icon = "ic_google",
//                Foreground = "#000000",
//                Background ="#F8F8F8"
//            }
//        };
//        async Task LoginAsync(AuthNetwork authNetwork)
//        {
//            switch (authNetwork.Name)
//            {
//                case "Google":
//                    await LoginGoogleAsync(authNetwork);
//                    break;
//            }
//        }
//        async Task LoginGoogleAsync(AuthNetwork authNetwork)
//        {
//            try
//            {
//                //if (!string.IsNullOrEmpty(_googleService.ActiveToken))
//                //{
//                    //Always require user authentication
//                    _googleService.Logout();
//                //}

//                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
//                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
//                {
//                    switch (e.Status)
//                    {
//                        case GoogleActionStatus.Completed:
//#if DEBUG
//                            var googleUserString = JsonConvert.SerializeObject(e.Data);
//                            Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
//#endif

//                            var socialLoginData = new NetworkAuthData
//                            {
//                                Id = e.Data.Id,
//                                Logo = authNetwork.Icon,
//                                Foreground = authNetwork.Foreground,
//                                Background = authNetwork.Background,
//                                Picture = e.Data.Picture.AbsoluteUri,
//                                Name = e.Data.Name,
//                            };

//                            await App.Current.MainPage.Navigation.PushModalAsync(new AboutPage());
//                            break;
//                        case GoogleActionStatus.Canceled:
//                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
//                            break;
//                        case GoogleActionStatus.Error:
//                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
//                            break;
//                        case GoogleActionStatus.Unauthorized:
//                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
//                            break;
//                    }

//                    _googleService.OnLogin -= userLoginDelegate;
//                };

//                _googleService.OnLogin += userLoginDelegate;

//                await _googleService.LoginAsync();
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.ToString());
//            }
//        }
    }
}
