
using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            CheckLang();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                isNotConnection.IsVisible = false;
                await ViewModel.GetQuestions();
                ViewModel.SelectedQuestion = null;
                base.OnAppearing();
            }
            else isNotConnection.IsVisible = true;
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                NAME_TEST_TEXT.Text = "Name of test";
                CODE_TEST_TEXT.Text = "Code of test";
                buttonCopy.Text = "Copy";
                buttonSave.Text = "Save";
                buttonDelete.Text = "Delete test";
                buttonQuest.Text = "Create question";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                buttonBack.Text = "Back";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                NAME_TEST_TEXT.Text = "Название теста";
                CODE_TEST_TEXT.Text = "Код теста";
                buttonCopy.Text = "Скопировать";
                buttonSave.Text = "Сохранить";
                buttonDelete.Text = "Удалить тест";
                buttonQuest.Text = "Создать вопрос";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonBack.Text = "Назад";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                NAME_TEST_TEXT.Text = "Назва тэста";
                CODE_TEST_TEXT.Text = "Код тэста";
                buttonCopy.Text = "Скапіяваць";
                buttonSave.Text = "Захаваць";
                buttonDelete.Text = "Выдаліць тэст";
                buttonQuest.Text = "Стварыць пытанне";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
                buttonBack.Text = "Назад";
            }
        }
    }
}