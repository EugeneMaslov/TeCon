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

        public ObservableCollection<Test> Tests { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public int Id { get; set; }
        public int QuestId { get; set; }
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
        public ICommand BackAndClearQuestionCommand { protected set; get; }
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
            BackAndClearQuestionCommand = new Command(BackAndClearQuestion);
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
                if (selectedTest != value)
                {
                    Test tempTest = new Test()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Questions = value.Questions
                    };
                    Id = Tests.IndexOf(value);
                    selectedTest = null;
                    OnPropertyChanged("SelectedTest");
                    Navigation.PushModalAsync(new Page1(tempTest, this));
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
                        Varients = value.Varients
                    };
                    QuestId = Questions.IndexOf(value);
                    selectedQuestion = null;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageQuestConst(Tests[Id], tempQuestion, this));
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
            Navigation.PushModalAsync(new Page1(new Test(), this));
        }
        private void CreateQuestion()
        {
            if (Tests[Id] != null)
            {
                Navigation.PushModalAsync(new PageQuestConst(Tests[Id], new Question(), this));
            }
            else Navigation.PushModalAsync(new PageQuestConst(new Test(), new Question(), this));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
        }
        private void BackAndClearQuestion()
        {
            Questions.Clear();
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
            initialized = true;
        }
        public void GetQuestions()
        {
            // очищаем список
            Questions.Clear();
            while (Questions.Any())
                Questions.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные
            if (Tests[Id].Questions != null)
            {
                foreach (Question f in Tests[Id].Questions)
                    Questions.Add(f);
            }
        }
        private async void SaveTest(object testObject)
        {
            Test friend = testObject as Test;
            if (friend != null)
            {
                friend.Questions = Questions;
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
            Back();
        }
        private void SaveQuestion(object questionObject)
        {
            Question friend = questionObject as Question;
            if (QuestId != 0)
            {
                if (Questions[QuestId].Id >= 0)
                {
                    Questions.Remove(Questions[QuestId]);
                    Questions.Insert(QuestId, friend);
                }
            }
            else if (friend != null && !Questions.Contains(friend))
            {
                Questions.Add(friend);
            }
            Back();
        }
        private async void DeleteTest(object friendObject)
        {
            Test friend = friendObject as Test;
            Questions.Clear();
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
            Back();
        }
        private void DeleteQuestion(object questionObject)
        {
            Question friend = questionObject as Question;
            if (friend != null)
            {
                Questions.Remove(friend);
            }
            Back();
        }
    }
}
