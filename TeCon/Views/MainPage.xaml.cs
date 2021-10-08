using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TeCon.ViewModels;
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace TeCon.Views
{
    public partial class MainPage : ContentPage
    {
        TestListViewModel ViewModel;
        public MainPage()
        { 
            InitializeComponent();
            ViewModel = new TestListViewModel() { Navigation = this.Navigation };
            BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            await ViewModel.GetFriends();
            base.OnAppearing();
        }
    }
}
