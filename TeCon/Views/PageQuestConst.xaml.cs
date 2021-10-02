using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeCon.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQuestConst : ContentPage
    {
        public QuestionViewModel ViewModel { get; private set; }
        public PageQuestConst(QuestionViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }

        private void buttonNew_Clicked(object sender, EventArgs e)
        {
        }

        private void buttonBack_Clicked(object sender, EventArgs e)
        {
        }

        private void buttonEnd_Clicked(object sender, EventArgs e)
        {
        }
    }
}