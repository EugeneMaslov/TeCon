using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageVarient : ContentPage
    {
        public int QuestionId { get; private set; }
        public Varient Varient { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public PageVarient(int questionId, Varient varient, TestListViewModel viewModel)
        {
            InitializeComponent();
            QuestionId = questionId;
            ViewModel = viewModel;
            Varient = varient;
            this.BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            CheckLang();
        }

        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                VARIANT_TEXT.Text = "Variant";
                buttonEndVariant.Text = "Save variant";
                DeleteVariant.Text = "Delete variant";
                LOADING.Text = "Loading...";
                INTERNET_ERROR.Text = "Something wrong. Check your internet access";
                buttonBack.Text = "Back";
            }
            else if (ViewModel.SelectedLanguage == "Русский (Россия)")
            {
                VARIANT_TEXT.Text = "Вариант ответа";
                buttonEndVariant.Text = "Сохранить вариант";
                DeleteVariant.Text = "Удалить вариант";
                LOADING.Text = "Загрузка...";
                INTERNET_ERROR.Text = "Нет подключения";
                buttonBack.Text = "Назад";
            }
        }
    }
}