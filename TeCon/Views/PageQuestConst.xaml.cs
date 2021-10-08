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
        public int TestId { get; private set; }
        public Question Question { get; private set; }
        public TestListViewModel ViewModel { get; private set; }
        public PageQuestConst(int testId ,Question question, TestListViewModel viewModel)
        {
            InitializeComponent();
            TestId = testId;
            ViewModel = viewModel;
            Question = question;
            this.BindingContext = this;
        }
    }
}