using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQuestConst : ContentPage
    {
        public Test Test { get; private set; }
        public Question Question { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public PageQuestConst(Test model, Question question, TestListViewModel viewModel)
        {
            InitializeComponent();
            Test = model;
            ViewModel = viewModel;
            Question = question;
            this.BindingContext = this;
        }

        private void buttonNew_Clicked(object sender, EventArgs e)
        {
        }

        private void buttonBack_Clicked(object sender, EventArgs e)
        {
        }

        private void buttonEnd_Clicked(object sender, EventArgs e)
        {
        }
    }
}