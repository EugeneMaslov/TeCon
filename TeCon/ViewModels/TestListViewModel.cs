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
        VarientsService varientsService = new VarientsService();

        public ObservableCollection<Test> Tests { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public ObservableCollection<Varient> Varients { get; set; }

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
        Question postSelectedQuestion { get; set; }
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
            Varients = new ObservableCollection<Varient>();
            IsBusy = false;
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
        public Test SelectedTest
        {
            get 
            {
                return selectedTest;
            }
            set
            {
                if (selectedTest != null && selectedTest.Id == 0 && value != null)
                {
                    Test tempTest = new Test()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Questions = value.Questions,
                    };
                    selectedTest = tempTest;
                    OnPropertyChanged("SelectedTest");
                }
                else if (selectedTest != null && value == null)
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
                if (selectedQuestion != null && selectedQuestion.Id == 0 && value != null)
                {
                    Question tempQuestion = new Question()
                    {
                        Id = value.Id,
                        OQuestion = value.OQuestion,
                        Varients = value.Varients,
                        TestId = value.Id
                    };
                    selectedQuestion = tempQuestion;
                    OnPropertyChanged("SelectedQuestion");
                }
                else if (selectedQuestion != null && value == null)
                {
                    selectedQuestion = null;
                    OnPropertyChanged("SelectedQuestion");
                }
                else if (selectedQuestion != value)
                {
                    Question tempQuestion = new Question()
                    {
                        Id = value.Id,
                        OQuestion = value.OQuestion,
                        Varients = value.Varients,
                        TestId = value.Id
                    };
                    selectedQuestion = tempQuestion;
                    OnPropertyChanged("SelectedQuestion");
                    Navigation.PushModalAsync(new PageQuestConst(tempQuestion.TestId, tempQuestion, this));
                }
            }
        }
        public Varient SelectedVarient
        {
            get { return selectedVarient; }
            set
            {
                if (selectedVarient != value)
                {
                    Varient tempVarient = new Varient()
                    {
                        Id = value.Id,
                        IsTrue = value.IsTrue,
                        OVarient = value.OVarient,
                        QuestionId = value.QuestionId
                    };
                    selectedVarient = null;
                    OnPropertyChanged("SelectedVarient");
                    Navigation.PushModalAsync(new PageVarient(tempVarient.QuestionId, tempVarient, this));
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
            Questions.Clear();
            Navigation.PushModalAsync(new Page1(selectedTest, this));
        }
        private void CreateQuestion()
        {
            selectedQuestion = new Question();
            selectedQuestion.OQuestion = "";
            Varients.Clear();
            Navigation.PushModalAsync(new PageQuestConst(0,selectedQuestion, this));
        }
        private void CreateVarient()
        {
            Navigation.PushModalAsync(new PageVarient(0, new Varient(), this));
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
            Tests.Clear();
            while (Tests.Any())
                Tests.RemoveAt(Tests.Count - 1);

            // добавляем загруженные данные
            foreach (Test f in friends)
            {
                Tests.Add(f);
            }    
                
            IsBusy = false;
        }
        public async Task GetQuestions()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Question> questions = await questionsService.Get();
            // очищаем список
            Questions.Clear();
            while (Questions.Any())
                Questions.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные
            foreach (Question f in questions)
            {
                if (selectedTest.Id == f.TestId)
                {
                    Questions.Add(f); 
                }
            }
            IsBusy = false;
        }
        public async Task GetVarients()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Varient> varients = await varientsService.Get();
            // очищаем список
            Varients.Clear();
            while (Questions.Any())
                Questions.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные

            if (selectedQuestion != null)
            {
                foreach (Varient f in varients)
                {
                    if (SelectedQuestion.Id == f.QuestionId)
                    {
                        Varients.Add(f);
                    }
                }
            }
            else
            {
                foreach (Varient f in varients)
                {
                    if (postSelectedQuestion.Id == f.QuestionId)
                    {
                        Varients.Add(f);
                    }
                }

            }

            IsBusy = false;
        }
        private async void SaveTest(object testObject)
        {
            Test friend = testObject as Test;
            friend.Questions = Questions.ToList();
            if (friend.Name == "")
                friend.Name = "Неназванный тест";
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
            Back();
        }
        private async Task SaveTestNotBack(object testObject)
        {
            Test friend = testObject as Test;
            if (friend != null)
            {
                if (friend.Name == "")
                    friend.Name = "Неназванный тест";
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
                        SelectedTest = updatedFriend;
                    }
                }
                // добавление
                else
                {
                    Test addedFriend = await testsService.Add(friend);
                    if (addedFriend != null)
                    {
                        Tests.Add(addedFriend);
                        SelectedTest = addedFriend;
                    }
                }
                IsBusy = false;
            }
        }
        private async void SaveQuestion(object testObject)
        {
            Question friend = testObject as Question;
            if (friend.OQuestion == "")
                friend.OQuestion = "Неназванный вопрос";
            friend.Varients = Varients.ToList();
            if (selectedTest.Id == 0)
            {
                await SaveTestNotBack(selectedTest);
            }
            friend.TestId = selectedTest.Id; 
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
                        await GetQuestions();
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
        private async Task SaveQuestionNotBack(object testObject)
        {
            Question friend = testObject as Question;
            friend.Varients = Varients.ToList();
            if (selectedTest.Id == 0)
            {
                await SaveTestNotBack(selectedTest);
            }
            friend.TestId = selectedTest.Id;
            if (friend != null)
            {
                if (friend.OQuestion == "")
                    friend.OQuestion = "Неназванный вопрос";
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
                        SelectedQuestion = updatedFriend;
                    }
                }
                // добавление
                else
                {
                    Question addedFriend = await questionsService.Add(friend);
                    if (addedFriend != null)
                    {
                        Questions.Add(addedFriend);
                        SelectedQuestion = addedFriend;
                    }
                }
                IsBusy = false;
            }
        }
        private async void SaveVarient(object varientObject)
        {
            Varient friend = varientObject as Varient;
            bool isNewQuestion = false;
            if (selectedQuestion.Id == 0)
            {
                isNewQuestion = true;
                await SaveQuestionNotBack(selectedQuestion);
            }
            friend.QuestionId = selectedQuestion.Id; 
            if (friend != null)
            {
                if (friend.OVarient == null)
                    friend.OVarient = "Неназванный вариант";
                IsBusy = true;
                // редактирование
                if (friend.Id > 0)
                {
                    Varient updatedFriend = await varientsService.Update(friend);
                    //заменяем объект в списке на новый
                    if (updatedFriend != null)
                    {
                        int pos = Varients.IndexOf(updatedFriend);
                        Varients.RemoveAt(pos);
                        Varients.Insert(pos, updatedFriend);
                    }
                }
                // добавление
                else
                {
                    Varient addedFriend = await varientsService.Add(friend);
                    if (addedFriend != null)
                        Varients.Add(addedFriend);
                }
                if (isNewQuestion)
                {
                    postSelectedQuestion = selectedQuestion;
                    SaveQuestion(postSelectedQuestion);
                }
                IsBusy = false;
                Back();
            }
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
            SelectedQuestion = null;
            Varients.Clear();
            Back();
        }
        private async void DeleteVarient(object friendObject)
        {
            Varient friend = friendObject as Varient;
            if (friend != null)
            {
                IsBusy = true;
                Varient deletedFriend = await varientsService.Delete(friend.Id);
                if (deletedFriend != null)
                {
                    Varients.Remove(deletedFriend);
                }
                IsBusy = false;
            }
            Back();
        }
    }
}
