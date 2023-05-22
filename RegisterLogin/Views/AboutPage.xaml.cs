using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterLogin.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void rfrshPage_Refreshing(object sender, EventArgs e)
        {

            await Task.Delay(3000);
            rfrshPage.IsRefreshing = false;

        }
    }
}