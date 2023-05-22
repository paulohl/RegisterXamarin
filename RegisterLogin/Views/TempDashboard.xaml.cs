using RegisterLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterLogin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TempDashboard : ContentPage
    {
        public TempDashboard(UserDetails userDetails)
        {
            InitializeComponent();

            txtName.Text = userDetails.Name;
            txtDetails.Text = userDetails.Email;
            imgUser.Source = userDetails.ImageUri;
        }
    }
}