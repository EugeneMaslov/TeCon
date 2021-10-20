using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TeCon.ViewModels;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using TeCon.Models;

namespace TeCon.Views
{
    public partial class Login : ContentPage
    {
        public TestListViewModel ViewModel { get; private set; }
        public User User { get; private set; }
        public Login(User User)
        {
            InitializeComponent();
            this.User = User;
            ViewModel = new TestListViewModel() { Navigation = this.Navigation };
            BindingContext = ViewModel;
        }
    }
}