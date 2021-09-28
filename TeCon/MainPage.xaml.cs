using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TeCon
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        { 
            InitializeComponent();
        }

        public async void button1_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Page1());
        }
    }
}
