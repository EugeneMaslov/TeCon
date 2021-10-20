using System;
using TeCon.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    public partial class App : Application
    {
        public const string HEADER = "TeCon";
        public const string IsTrueText = "Правильный ответ";
        public App()
        {
            InitializeComponent();
            MainPage = new Login(new User());
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
