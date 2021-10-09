using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TeCon.ViewModels;
using TeCon.Models;
using Xamarin.Essentials;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Test Test { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public Page1(Test model, TestListViewModel viewModel)
        {
            InitializeComponent();
            Test = model;
            ViewModel = viewModel;
            this.BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await ViewModel.GetQuestions();
                ViewModel.SelectedQuestion = null;
                base.OnAppearing();
            }
        }
    }
}