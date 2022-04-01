using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeCon.Models;
using TeCon.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeCon.ViewModels
{
    public class TestListViewModel : INotifyPropertyChanged
    {
        #region Initialization
        bool initialized = false;   // была ли начальная инициализация
        private bool isBusy;    // идет ли загрузка с сервера
        private bool isVerify; // верификация
        private bool isNull = false;
        TestsService testsService = new TestsService();
        QuestionsService questionsService = new QuestionsService();
        VarientsService varientsService = new VarientsService();
        LoginService loginService = new LoginService();

        public string TestCode { get; set; }
        public ObservableCollection<Test> Tests { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
        public ObservableCollection<Varient> Varients { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public User User { get; set; }

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
        public ICommand UserInCommand { protected set; get; }
        public ICommand UserOutCommand { protected set; get; }
        public ICommand UserCreateCommand { protected set; get; }
        public ICommand RegistrCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public ICommand CopyCommand { protected set; get; }

        public ICommand LanguageCommand { protected set; get; }

        Test selectedTest { get; set; }
        string selectedLanguage = "English"; 
        Question selectedQuestion { get; set; }
        Question postSelectedQuestion { get; set; }
        Varient selectedVarient { get; set; }
        User activeUser { get; set; }
        public INavigation Navigation { get; set; }
        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
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
        public bool IsVerify
        {
            get { return isVerify; }
            set
            {
                isVerify = value;
                OnPropertyChanged("IsVerify");
            }
        }

        public TestListViewModel()
        {
            Tests = new ObservableCollection<Test>();
            Questions = new ObservableCollection<Question>();
            Varients = new ObservableCollection<Varient>();
            User = new User();
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
            UserInCommand = new Command(UserIn);
            UserOutCommand = new Command(UserOut);
            RegistrCommand = new Command(Registr);
            LoginCommand = new Command(NewLogin);
            UserCreateCommand = new Command(UserCreate);
            BackCommand = new Command(Back);
            CopyCommand = new Command(CopyMethod);
            LanguageCommand = new Command(LanguageMethod);
            Languages = new ObservableCollection<string>() { "English", "Русский (Россия)" };
        }
        #endregion
        #region Localization
        protected void LanguageMethod()
        {
            Navigation.PushModalAsync(new Language(this));
        }
        public string SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
            set
            {
                if (selectedLanguage != null)
                {
                    selectedLanguage = value;
                    Navigation.PopModalAsync();
                }
                else selectedLanguage = "English";
                OnPropertyChanged("SelectedLanguage");
            }
        }
        #endregion
        #region LoadedRegion & SelectedObjects
        public User ActiveUser
        {
            get { return activeUser; }
            set
            {
                if (value != null)
                {
                    this.activeUser = value;
                }
                else activeUser = null;
                OnPropertyChanged("ActiveUser");
                Navigation.PushModalAsync(new MainPage(this));
            }
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
                        Code = value.Code
                    };
                    TestCode = value.Code;
                    selectedTest = tempTest;
                    OnPropertyChanged("SelectedTest");
                    OnPropertyChanged("TestCode");
                }
                else if (selectedTest != null && value == null)
                {
                    selectedTest = null;
                    TestCode = null;
                    OnPropertyChanged("SelectedTest");
                    OnPropertyChanged("TestCode");
                }
                else if (selectedTest != value)
                {
                    Test tempTest = new Test()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Questions = value.Questions,
                        Code = value.Code
                    };
                    TestCode = value.Code;
                    OnPropertyChanged("TestCode");
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
        #endregion
        #region Creating and Logging
        private void NewLogin()
        {
            Navigation.PushModalAsync(new Login(new User(), this));
        }
        private async void UserIn(object User)
        {
            this.User = User as User;
            if (await CheckUser())
            {
                IsVerify = false;
                ActiveUser = this.User;
                await Navigation.PushModalAsync(new MainPage(this));
            }
            else IsVerify = true;
        }
        private async void UserOut(object userObject)
        {
            User user = userObject as User;
            user = await GetUser(user);
            if (user != null)
            {
                IsBusy = true;
                User deletedFriend = await loginService.Delete(user.Id);
                if (deletedFriend != null)
                {
                    User = null;
                    ActiveUser = null;
                }
                IsBusy = false;
            }
            await Navigation.PushModalAsync(new Login(new User(), this));
        }
        private void Registr()
        {
            Navigation.PushModalAsync(new Register(this, new User()));
        }
        private async void UserCreate(object userObject)
        {
            User user = userObject as User;
            if (user != null)
            {
                if ((user.Login != null && user.Email != null && user.Password != null) && (user.Login != "" && user.Email != "" && user.Password != ""))
                {
                    IsBusy = true;
                    isNull = false;
                    // редактирование
                    if (user.Id > 0)
                    {
                        //TODO
                    }
                    // добавление
                    else
                    {

                        user.Password = GetHash(user.Password);
                        User addedFriend = await loginService.Add(user);
                        if (addedFriend != null)
                            ActiveUser = addedFriend;
                    }
                    IsBusy = false;
                    Later();
                }
                else
                {
                    isNull = true;
                }
            }
        }
        private void Later()
        {
            Navigation.PushModalAsync(new MainPage(this));
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
            Navigation.PushModalAsync(new PageQuestConst(0, selectedQuestion, this));
        }
        private void CreateVarient()
        {
            Navigation.PushModalAsync(new PageVarient(0, new Varient(), this));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
        }
        public async Task<bool> CheckUser()
        {
            IsBusy = true;
            IEnumerable<User> users = await loginService.Get();
            List<User> inter = users.ToList();
            foreach (User user in inter)
            {
                if (User.Equals(user))
                {
                    User = user;
                    IsBusy = false;
                    return true;
                }
            }
            IsBusy = false;
            return false;
        }
        public async Task<User> GetUser(User GettedUser)
        {
            IsBusy = true;
            IEnumerable<User> users = await loginService.Get();
            List<User> inter = users.ToList();
            foreach (User user in inter)
            {
                if (GettedUser.Equals(user))
                {
                    GettedUser = user;
                    IsBusy = false;
                    return GettedUser;
                }
            }
            IsBusy = false;
            return null;
        }
        #endregion
        #region Getting on Server
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
                if (f.UserId == activeUser.Id)
                {
                    Tests.Add(f);
                }
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
        #endregion
        #region Saving
        public void CopyMethod()
        {
            Clipboard.SetTextAsync(TestCode);
        }
        static string Generate()
        {
            int L = 35;
            char[] A = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
                'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            char[] generatedChar = new char[L];
            int x;
            Random r = new Random();
            for (int i = 0; i < L; i++)
            {
                x = r.Next(A.Length - 1);
                generatedChar[i] = A[x];
            }
            string generated = "";
            for (int i = 0; i < generatedChar.Length; i++)
            {
                generated += generatedChar[i];
            }
            return generated;
        }
        private async void SaveTest(object testObject)
        {
            Test friend = testObject as Test;
            if (friend.Code == null)
            {
                friend.Code = Generate();
            }
            TestCode = friend.Code;
            friend.Questions = Questions.ToList();
            if (friend.Name == "")
                friend.Name = "Неназванный тест";
            if (friend != null && activeUser != null)
            {
                friend.UserId = activeUser.Id;
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
                OnPropertyChanged("TestCode");
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
                friend.UserId = activeUser.Id;
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
        #endregion
        #region Deleting
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
        #endregion
    }
}