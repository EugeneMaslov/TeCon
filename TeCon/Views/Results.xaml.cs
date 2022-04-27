using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Results : ContentPage
    {
        public TestListViewModel ViewModel { get; private set; }
        public static string Pass { get; set; }
        public static string Resulting { get; set; }
        public Results(TestListViewModel viewModel)
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
                base.OnAppearing();
            }
            else isNotConnection.IsVisible = true;
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                RESULTS.Text = "Results: ";
                Pass = "Pass: ";
                Resulting = "; result: ";
                BACK.Text = "Back";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                RESULTS.Text = "Результаты: ";
                Pass = "Прошёл: ";
                Resulting = "; результат: ";
                BACK.Text = "Назад";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                RESULTS.Text = "Вынікі: ";
                Pass = "Прайшоў: ";
                Resulting = "; вынік: ";
                BACK.Text = "Назад";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
            }
        }
    }
}