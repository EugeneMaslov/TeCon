using TeCon.Models;
using Xamarin.Forms;

namespace TeCon.Views
{
    public partial class App : Application
    {
        public const string HEADER = "Te";
        public const string HEADER1 = "st";
        public const string HEADER2 = "Con";
        public const string HEADER3 = "structor";
        public const string IsTrueText = "Правильный ответ";
        public App()
        {
            InitializeComponent();
            MainPage = new Login(new User(), null);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
