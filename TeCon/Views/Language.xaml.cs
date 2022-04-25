using TeCon.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Language : ContentPage
    {
        public TestListViewModel ViewModel { get; private set; }
        public Language(TestListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }
        protected override async void OnAppearing()
        {
            CheckLang();
        }
        private void CheckLang()
        {
            if (ViewModel.SelectedLanguage == "English")
            {
                Lang.Text = "Language:";
                buttonBack.Text = "Back";
            }
            else if (ViewModel.SelectedLanguage == "Русский")
            {
                Lang.Text = "Выберите язык:";
                buttonBack.Text = "Назад";
            }
            else if (ViewModel.SelectedLanguage == "Беларуская")
            {
                Lang.Text = "Абярыце мову:";
                buttonBack.Text = "Назад";
            }
        }
    }
}