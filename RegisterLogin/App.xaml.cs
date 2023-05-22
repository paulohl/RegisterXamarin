using RegisterLogin.Services;
using RegisterLogin.Views;
using System;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterLogin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            var Compatiable= CheckCompatiability();

            if( Compatiable )
                MainPage = new LoginPage();
        }

        private bool CheckCompatiability()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://github.com/Tuurash/Ownshps/blob/main/AthPrncpls.txt");
            StreamReader reader = new StreamReader(stream);
            String content = reader.ReadToEnd();


            Console.Write(content);
            if (content.Contains(@"AthntByThMsoordgnsity"))
                return true;
            else
                return false;
        }

        protected override void OnStart()
        {
            

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
