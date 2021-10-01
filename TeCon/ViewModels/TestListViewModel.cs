using System;
using System.Collections.Generic;
using System.Text;
using TeCon.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;
using TeCon.Views;

namespace TeCon.ViewModels
{
    public class TestListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TestViewModel> Tests { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateTestCommand { protected set; get; }
        public ICommand DeleteTestCommand { protected set; get; }
        public ICommand SaveTestCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        TestViewModel selectedTest;

        public INavigation Navigation { get; set; }
        public TestListViewModel()
        {
            Tests = new ObservableCollection<TestViewModel>();
            CreateTestCommand = new Command(CreateTest);
            DeleteTestCommand = new Command(DeleteTest);
            SaveTestCommand = new Command(SaveTest);
            BackCommand = new Command(Back);
        }
        public TestViewModel SelectedTest
        {
            get { return selectedTest; }
            set
            {
                if (selectedTest != value)
                {
                    TestViewModel tempTest = value;
                    selectedTest = null;
                    OnPropertyChanged("SelectedTest");
                    Navigation.PushModalAsync(new Page1(tempTest));
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private void CreateTest()
        {
            Navigation.PushModalAsync(new Page1(new TestViewModel() { ListViewModel = this }));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
        }
        private void SaveTest(object testObject)
        {
           TestViewModel friend = testObject as TestViewModel;
            if (friend != null && friend.IsValid && !Tests.Contains(friend))
            {
                Tests.Add(friend);
            }
            Back();
        }
        private void DeleteTest(object friendObject)
        {
            TestViewModel friend = friendObject as TestViewModel;
            if (friend != null)
            {
                Tests.Remove(friend);
            }
            Back();
        }
    }
}
