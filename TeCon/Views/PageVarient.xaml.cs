using System;
using System.Collections.Generic;
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
    }
}