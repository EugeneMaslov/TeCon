using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TeCon.ViewModels;

namespace TeCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public TestViewModel ViewModel { get; private set; }
        public Page1(TestViewModel tvm)
        {
            InitializeComponent();
            ViewModel = tvm;
            this.BindingContext = ViewModel;
        }
    }
}