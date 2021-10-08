using System;
using System.Collections.Generic;
using System.Text;
using TeCon.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;
using TeCon.Views;
using System.Threading.Tasks;
using System.Linq;

namespace TeCon.ViewModels
{
    public class TestListViewModel : INotifyPropertyChanged
    {
        bool initialized = false;   // была ли начальная инициализация
        private bool isBusy;    // идет ли загрузка с сервера
        TestsService testsService = new TestsService();
        QuestionsService questionsService = new QuestionsService();

        public ObservableCollection<Test> Tests { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public ObservableCollection<Question> Varients { get; set; }

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

        Test selectedTest { get; set; }
        Question selectedQuestion { get; set; }
        Varient selectedVarient { get; set; }

        public INavigation Navigation { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public TestListViewModel()
        {
            Tests = new ObservableCollection<Test>();
            Questions = new ObservableCollection<Question>();
            IsBusy = false;
            CreateTestCommand = new Command(CreateTest);
            DeleteTestCommand = new Command(DeleteTest);
            SaveTestCommand = new Command(SaveTest);
            CreateQuestionCommand = new Command(CreateQuestion);
            DeleteQuestionCommand = new Command(DeleteQuestion);
            SaveQuestionCommand = new Command(SaveQuestion);
            //CreateVarientCommand = new Command(CreateVarient);
            //SaveVarientCommand = new Command(SaveVarient);
            //DeleteVarientCommand = new Command(DeleteVarient);
            BackCommand = new Command(Back);
        }
        public Test SelectedTest
        {
            get 
            {
                return selectedTest;
            }
            set
            {
                if (selectedTest != null)
                {
                    selectedTest = null;
                    OnPropertyChanged("SelectedTest");
                }
                else if (selectedTest != value)
                {
                    Test tempTest = new Test()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Questions = value.Questions,
                    };
                    selectedTest = tempTest;
                    OnPropertyChanged("SelectedTest");
                    Navigation.PushModalAsync(new Page1(tempTest, this));
                    Questions.Clear();
                }
            }
        }
        public Question SelectedQuestion
        {
            get
            {
                return selectedQuestion;
            }
            set
            {
                if (selectedQuestion != value)
                {
                    Question tempQuestion = new Question()
                    {
                        Id = value.Id,
                        OQuestion = value.OQuestion,
                        Varients = value.Varients,
                        TestId = value.Id
                    };
                    selectedQuestion = null;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageQuestConst(tempQuestion.TestId, tempQuestion, this));
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
            
            selectedTest = new Test();
            selectedTest.Name = "";
            selectedTest.Id = 0;
            Questions.Clear();
            Navigation.PushModalAsync(new Page1(selectedTest, this));
        }
        private void CreateQuestion()
        {
            Navigation.PushModalAsync(new PageQuestConst(0,new Question(), this));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
        }
        public async Task GetFriends()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Test> friends = await testsService.Get();

            // очищаем список
            //Tests.Clear();
            while (Tests.Any())
                Tests.RemoveAt(Tests.Count - 1);

            // добавляем загруженные данные
            foreach (Test f in friends)
                Tests.Add(f);
            IsBusy = false;
        }
        public async Task GetQuestions()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Question> questions = await questionsService.Get();
            // очищаем список
            //Questions.Clear();
            while (Questions.Any())
                Questions.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные
            foreach (Question f in questions)
            {
                if (selectedTest.Id == f.TestId)
                {
                    Questions.Add(f); //TODO
                }
            }
            IsBusy = false;
        }
        private async void SaveTest(object testObject)
        {
            Test friend = testObject as Test;
            friend.Questions = Questions.ToList();
            if (friend != null)
            {
                IsBusy = true;
                // редактирование
                if (friend.Id > 0)
                {
                    Test updatedFriend = await testsService.Update(friend);
                    // заменяем объект в списке на новый
                    if (updatedFriend != null)
                    {
                        int pos = Tests.IndexOf(updatedFriend);
                        Tests.RemoveAt(pos);
                        Tests.Insert(pos, updatedFriend);
                    }
                }
                // добавление
                else
                {
                    Test addedFriend = await testsService.Add(friend);
                    if (addedFriend != null)
                        Tests.Add(addedFriend);
                }
                IsBusy = false;
            }
            SelectedTest = null;
            Back();
        }
        private async void SaveQuestion(object testObject)
        {
            Question friend = testObject as Question;
            friend.TestId = selectedTest.Id; //TODO
            if (friend != null)
            {
                IsBusy = true;
                // редактирование
                if (friend.Id > 0)
                {
                    Question updatedFriend = await questionsService.Update(friend);
                    // заменяем объект в списке на новый
                    if (updatedFriend != null)
                    {
                        int pos = Questions.IndexOf(updatedFriend);
                        Questions.RemoveAt(pos);
                        Questions.Insert(pos, updatedFriend);
                    }
                }
                // добавление
                else
                {
                    Question addedFriend = await questionsService.Add(friend);
                    if (addedFriend != null)
                        Questions.Add(addedFriend);
                }
                IsBusy = false;
            }
            Back();
        }
        private async void DeleteTest(object friendObject)
        {
            Test friend = friendObject as Test;
            if (friend != null)
            {
                IsBusy = true;
                Test deletedFriend = await testsService.Delete(friend.Id);
                if (deletedFriend != null)
                {
                    Tests.Remove(deletedFriend);
                }
                IsBusy = false;
            }
            SelectedTest = null;
            Questions.Clear();
            Back();
        }
        private async void DeleteQuestion(object friendObject)
        {
            Question friend = friendObject as Question;
            if (friend != null)
            {
                IsBusy = true;
                Question deletedFriend = await questionsService.Delete(friend.Id);
                if (deletedFriend != null)
                {
                    Questions.Remove(deletedFriend);
                }
                IsBusy = false;
            }
            Back();
        }
    }
}
