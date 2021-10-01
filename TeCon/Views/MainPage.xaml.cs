using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TeCon.ViewModels;

namespace TeCon.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        { 
            InitializeComponent();
            BindingContext = new TestListViewModel() { Navigation = this.Navigation };
        }
    }
}
