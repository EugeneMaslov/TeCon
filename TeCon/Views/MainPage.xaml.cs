using System;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
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
            CheckLang();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                isNotConnection.IsVisible = false;
                await ViewModel.GetFriends();
                ViewModel.SelectedTest = null;
                base.OnAppearing();
            }
            else isNotConnection.IsVisible = true;
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
                buttonSwapLang.Text = "Change language";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                TESTS.Text = "Ваши тесты";
                CREATE_TEST.Text = "Создать новый тест";
                buttonToMain.Text = "Сменить аккаунт";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonSwapLang.Text = "Сменить язык";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                TESTS.Text = "Вашыя тэсты";
                CREATE_TEST.Text = "Стварыць новы тэст";
                buttonToMain.Text = "Змяніць акаўнт";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
                buttonSwapLang.Text = "Змяніць мову";
            }
        }
    }
}
