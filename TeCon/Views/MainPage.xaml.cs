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
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public TestListViewModel ViewModel { get; private set; }
        public MainPage(TestListViewModel viewModel)
        { 
            InitializeComponent();
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                isNotConnection.IsVisible = false;
                await ViewModel.GetFriends();
                ViewModel.SelectedTest = null;
                base.OnAppearing();
            }
            else isNotConnection.IsVisible = true;
            CheckLang();
        }
        private async void testList_Refreshing(object sender, EventArgs e)
        {
            await ViewModel.GetFriends();
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                TESTS.Text = "Your tests";
                CREATE_TEST.Text = "Create new test";
                buttonToMain.Text = "Change account";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                buttonSwapLang.Text = "Swap language";
            }
            else if (ViewModel.SelectedLanguage == "Русский (Россия)")
            {
                TESTS.Text = "Ваши тесты";
                CREATE_TEST.Text = "Создать новый тест";
                buttonToMain.Text = "Сменить аккаунт";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonSwapLang.Text = "Сменить язык";
            }
        }
    }
}
