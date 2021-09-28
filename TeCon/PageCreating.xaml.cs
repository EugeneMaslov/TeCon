using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        public void buttonQuest_Clicked(object sender, EventArgs e)
        {

        }

        public async void buttonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}