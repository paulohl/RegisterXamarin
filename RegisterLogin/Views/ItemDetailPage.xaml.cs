using RegisterLogin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace RegisterLogin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}