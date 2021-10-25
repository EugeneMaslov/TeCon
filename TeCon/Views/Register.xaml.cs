using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeCon.Models;
using TeCon.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeCon.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
		public User User { get; private set; }
		public TestListViewModel ViewModel { get; private set; }
		public Register (TestListViewModel viewModel, User user)
		{
			InitializeComponent ();
			User = user;
			ViewModel = viewModel;
			this.BindingContext = this;
		}
		protected override async void OnAppearing()
		{
			if (Connectivity.NetworkAccess == NetworkAccess.Internet)
			{
				isNotConnection.IsVisible = false;
			}
			else
			{
				isNotConnection.IsVisible = true;
				ViewModel.IsBusy = true;
			}
		}
	}
}