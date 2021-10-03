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
        public ICommand CreateQuestionCommand { protected set; get; }
        public ICommand DeleteQuestionCommand { protected set; get; }
        public ICommand SaveQuestionCommand { protected set; get; }
        public ICommand CreateVarientCommand { protected set; get; }
        public ICommand DeleteVarientCommand { protected set; get; }
        public ICommand SaveVarientCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        TestViewModel selectedTest { get; set; }
        QuestionViewModel selectedQuestion { get; set; }
        VarientViewModel selectedVarient { get; set; }

        public INavigation Navigation { get; set; }
        public TestListViewModel()
        {
            Tests = new ObservableCollection<TestViewModel>();
            CreateTestCommand = new Command(CreateTest);
            DeleteTestCommand = new Command(DeleteTest);
            SaveTestCommand = new Command(SaveTest);
            CreateQuestionCommand = new Command(CreateQuestion);
            DeleteQuestionCommand = new Command(DeleteQuestion);
            SaveQuestionCommand = new Command(SaveQuestion);
            CreateVarientCommand = new Command(CreateVarient);
            SaveVarientCommand = new Command(SaveVarient);
            DeleteVarientCommand = new Command(DeleteVarient);
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
                    if (tempTest != null)
                    {
                        if (selectedTest != null)
                        {
                            if (selectedTest.Questions != null)
                            {
                                tempTest.Questions = selectedTest.Questions;
                            }
                        }
                    }
                    selectedTest = tempTest;
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
                    if (tempQuestion != null)
                    {
                        if (selectedQuestion != null)
                        {
                            if (selectedQuestion.Varients != null)
                            {
                                tempQuestion.Varients = selectedQuestion.Varients;
                            }
                        }
                    }
                    selectedQuestion = tempQuestion;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageQuestConst(tempQuestion));
                }
            }
        }
        public VarientViewModel SelectedVarient
        {
            get
            {
                return selectedVarient;
            }
            set
            {
                if (selectedVarient != value)
                {
                    VarientViewModel tempVarient = value;
                    selectedVarient = null;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageVarient(tempVarient));
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
            SelectedTest = null;
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
            Navigation.PushModalAsync(new PageQuestConst(new QuestionViewModel() { ListViewModel = this, Varients = new ObservableCollection<VarientViewModel>() }));
        }
        private void SaveQuestion(object questObject)
        {
            QuestionViewModel friend = questObject as QuestionViewModel;
            if (selectedTest != null)
            {
                if (friend != null && friend.IsValid && !selectedTest.Questions.Contains(friend))
                {
                    SelectedTest.Questions.Add(friend);
                }
            }
            else
            {
                selectedTest = new TestViewModel() { Questions = new ObservableCollection<QuestionViewModel>() };
                if (friend != null && friend.IsValid && !SelectedTest.Questions.Contains(friend))
                {
                    SelectedTest.Questions.Add(friend);
                }
            }
            SelectedQuestion = null;
            Back();
        }
        private void DeleteQuestion(object questObject)
        {
            QuestionViewModel friend = questObject as QuestionViewModel;
            if (friend != null)
            {
               SelectedQuestion = null;
               SelectedTest.Questions.Remove(friend);
            }
            Back();
        }
        private void CreateVarient()
        {
            Navigation.PushModalAsync(new PageVarient(new VarientViewModel() { ListViewModel = this }));
        }
        private void SaveVarient(object varientObject)
        {
            VarientViewModel friend = varientObject as VarientViewModel;
            if (selectedQuestion != null)
            {
                if (friend != null && friend.IsValid && !selectedQuestion.Varients.Contains(friend))
                {
                    SelectedQuestion.Varients.Add(friend);
                }
            }
            else
            {
                selectedQuestion = new QuestionViewModel() { Varients = new ObservableCollection<VarientViewModel>() };
                if (friend != null && friend.IsValid && !selectedQuestion.Varients.Contains(friend))
                {
                    SelectedQuestion.Varients.Add(friend);
                }
            }
            Back();
        }
        private void DeleteVarient(object varientObject)
        {
            VarientViewModel friend = varientObject as VarientViewModel;
            if (friend != null)
            {
                selectedQuestion.Varients.Remove(friend);
            }
            Back();
        }
    }
}
