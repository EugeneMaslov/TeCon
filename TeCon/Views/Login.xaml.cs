using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeCon.Views
{
    public partial class Login : ContentPage
    {
        public TestListViewModel ViewModel { get; private set; }
        public User User { get; private set; }
        public Login(User User, TestListViewModel viewModel)
        {
            InitializeComponent();
            this.User = User;
            if (viewModel == null)
            {
                ViewModel = new TestListViewModel() { Navigation = this.Navigation };
            }
            else ViewModel = viewModel;
            BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            CheckLang();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                isNotConnection.IsVisible = false;
            }
            else
            {
                isNotConnection.IsVisible = true;
                ViewModel.IsBusy = true;
            }
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                LOG_IN.Text = "Log in";
                LOG_IN_TEXT.Text = "Login:";
                PASSWORD.Text = "Password:";
                USER_NOT_FOUND.Text = "User not found!";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                buttonSave.Text = "Log in";
                buttonDeleteUser.Text = "Delete account";
                buttonAddUser.Text = "Create account";
                buttonSwapLang.Text = "Change language";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                LOG_IN.Text = "Вход";
                LOG_IN_TEXT.Text = "Логин:";
                PASSWORD.Text = "Пароль:";
                USER_NOT_FOUND.Text = "Пользователь не найден!";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonSave.Text = "Войти";
                buttonDeleteUser.Text = "Удалить аккаунт";
                buttonAddUser.Text = "Создать аккаунт";
                buttonSwapLang.Text = "Сменить язык";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                LOG_IN.Text = "Уваход";
                LOG_IN_TEXT.Text = "Лагін:";
                PASSWORD.Text = "Пароль:";
                USER_NOT_FOUND.Text = "Карыстальнік не знойдзены!";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
                buttonSave.Text = "Увайсці";
                buttonDeleteUser.Text = "Выдаліць карыстальніка";
                buttonAddUser.Text = "Стварыць акаўнт";
                buttonSwapLang.Text = "Змяніць мову";
            }
        }
    }
}