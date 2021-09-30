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
    public partial class PageQuestConst : ContentPage
    {
        public QuestionViewModel ViewModel { get; private set; }
        public PageQuestConst(QuestionViewModel vm)
        {
            InitializeComponent();
            BindingContext = new VarientsListViewModel() { Navigation = this.Navigation };
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }

        private void buttonNew_Clicked(object sender, EventArgs e)
        {
        }

        private async void buttonBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void buttonEnd_Clicked(object sender, EventArgs e)
        {

        }
    }
}