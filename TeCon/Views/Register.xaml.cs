using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public User User { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public Register(TestListViewModel viewModel, User user)
        {
            InitializeComponent();
            User = user;
            ViewModel = viewModel;
            this.BindingContext = this;
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
                REGISTR.Text = "Registration";
                LOGIN.Text = "Login:";
                EMAIL.Text = "E-mail:";
                PASSWORD.Text = "Password:";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                NOTHING_FIELD.Text = "Fields can't be empty";
                buttonSave.Text = "Save and log in";
                buttonDeleteUser.Text = "Delete account";
                buttonBack.Text = "Back";
                buttonSwapLang.Text = "Change language";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                REGISTR.Text = "Регистрация";
                LOGIN.Text = "Логин:";
                EMAIL.Text = "E-mail:";
                PASSWORD.Text = "Пароль:";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                NOTHING_FIELD.Text = "Поля не могут быть пустыми!";
                buttonSave.Text = "Сохранить и войти";
                buttonDeleteUser.Text = "Удалить аккаунт";
                buttonBack.Text = "Назад";
                buttonSwapLang.Text = "Сменить язык";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                REGISTR.Text = "Рэгістрацыя";
                LOGIN.Text = "Лагін:";
                EMAIL.Text = "E-mail:";
                PASSWORD.Text = "Пароль:";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
                buttonBack.Text = "Назад";
                NOTHING_FIELD.Text = "Бланкі не могуць быць пустымі!";
                buttonSave.Text = "Захаваць і ўвайсці";
                buttonDeleteUser.Text = "Выдаліць акаўнт";
                buttonSwapLang.Text = "Змяніць мову";
            }
        }
    }
}