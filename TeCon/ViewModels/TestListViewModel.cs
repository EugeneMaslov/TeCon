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
        public ObservableCollection<QuestionViewModel> Questions { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateTestCommand { protected set; get; }
        public ICommand DeleteTestCommand { protected set; get; }
        public ICommand SaveTestCommand { protected set; get; }
        public ICommand CreateQuestionCommand { protected set; get; }
        public ICommand DeleteQuestionCommand { protected set; get; }
        public ICommand SaveQuestionCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        TestViewModel selectedTest { get; set; }
        QuestionViewModel selectedQuestion { get; set; }

        public INavigation Navigation { get; set; }
        public TestListViewModel()
        {
            Tests = new ObservableCollection<TestViewModel>();
            Questions = new ObservableCollection<QuestionViewModel>();
            CreateTestCommand = new Command(CreateTest);
            DeleteTestCommand = new Command(DeleteTest);
            SaveTestCommand = new Command(SaveTest);
            CreateQuestionCommand = new Command(CreateQuestion);
            DeleteQuestionCommand = new Command(DeleteQuestion);
            SaveQuestionCommand = new Command(SaveQuestion);
            BackCommand = new Command(Back);
        }
        public TestViewModel SelectedTest
        {
            get 
            {
                return selectedTest;
            }
            set
            {
                if (selectedTest != value)
                {
                    TestViewModel tempTest = value;
                    selectedTest = tempTest;
                    tempTest.Questions = selectedTest.Questions;
                    OnPropertyChanged("SelectedTest");
                    Navigation.PushModalAsync(new Page1(tempTest));
                }
            }
        }
        public QuestionViewModel SelectedQuestion
        {
            get 
            {
                return selectedQuestion;
            }
            set
            {
                if (selectedQuestion != value)
                {
                    QuestionViewModel tempQuestion = value;
                    selectedQuestion = null;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageQuestConst(tempQuestion));
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
            Navigation.PushModalAsync(new Page1(new TestViewModel() { ListViewModel = this, Questions = new ObservableCollection<QuestionViewModel>() }));
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
            selectedTest = null;
            Back();
        }
        private void DeleteTest(object friendObject)
        {
            TestViewModel friend = friendObject as TestViewModel;
            if (friend != null)
            {
                selectedTest = null;
                Tests.Remove(friend);
            }
            Back();
        }
        private void CreateQuestion()
        {
            Navigation.PushModalAsync(new PageQuestConst(new QuestionViewModel() { ListViewModel = this }));
        }
        private void SaveQuestion(object questObject)
        {
            QuestionViewModel friend = questObject as QuestionViewModel;
            if (SelectedTest != null)
            {
                if (friend != null && friend.IsValid && !SelectedTest.Questions.Contains(friend))
                {
                    SelectedTest.Questions.Add(friend);
                }
            }
            else
            {
                SelectedTest = new TestViewModel();
                selectedTest.Questions = new ObservableCollection<QuestionViewModel>();
                if (friend != null && friend.IsValid && !SelectedTest.Questions.Contains(friend))
                {
                    SelectedTest.Questions.Add(friend);
                }
            }

            Back();
        }
        private void DeleteQuestion(object questObject)
        {
            QuestionViewModel friend = questObject as QuestionViewModel;
            if (friend != null)
            {
               SelectedTest.Questions.Remove(friend);
            }
            Back();
        }
    }
}
