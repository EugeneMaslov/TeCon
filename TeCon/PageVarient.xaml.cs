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
    public partial class PageVarient : ContentPage
    {
        public PageVarient()
        {
            InitializeComponent();
        }

        private async void buttonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void buttonEndVariant_Clicked(object sender, EventArgs e)
        {
            Varients varients = new Varients(textBox1.Text, isTrue.On);
        }
    }
}