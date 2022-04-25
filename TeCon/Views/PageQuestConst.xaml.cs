using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQuestConst : ContentPage
    {
        public int TestId { get; private set; }
        public Question Question { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public PageQuestConst(int testId, Question question, TestListViewModel viewModel)
        {
            InitializeComponent();
            TestId = testId;
            ViewModel = viewModel;
            Question = question;
            this.BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CheckLang();
                isNotConnection.IsVisible = false;
                await ViewModel.GetVarients();
                base.OnAppearing();
            }
            else isNotConnection.IsVisible = true;
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                QUESTIONS.Text = "Questions of test";
                buttonNew.Text = "Create variant";
                buttonEnd1.Text = "Save question";
                buttonDelete1.Text = "Delete question";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                buttonBack1.Text = "Back";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                QUESTIONS.Text = "Вопросы теста";
                buttonNew.Text = "Создать вариант ответа";
                buttonEnd1.Text = "Сохранить вопрос";
                buttonDelete1.Text = "Удалить вопрос";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonBack1.Text = "Назад";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                QUESTIONS.Text = "Пытанні тэсту";
                buttonNew.Text = "Стварыць варыянт";
                buttonEnd1.Text = "Захаваць пытанне";
                buttonDelete1.Text = "Выдаліць пытанне";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Няма падключэння";
                buttonBack1.Text = "Назад";
            }
        }
    }
}