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
using TeCon.Services;
using TeCon.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeCon.ViewModels
{
    public class TestListViewModel : INotifyPropertyChanged
    {
        #region Initialization
        private bool initialized;   // была ли начальная инициализация
        private bool isBusy;    // идет ли загрузка с сервера
        private bool isVerify; // верификация
        private bool isNull;
        protected string selectedLanguage = "English";
        private TestsService testsService;
        private QuestionsService questionsService;
        private VarientsService varientsService;
        private LoginService loginService;

        public string TestCode { get; set; }
        public ObservableCollection<Result> InstantResults { get; set; }
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


        Question selectedQuestion { get; set; }
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
            initialized = false;
            testsService = new TestsService();
            questionsService = new QuestionsService();
            varientsService = new VarientsService();
            loginService = new LoginService();
            Tests = new ObservableCollection<Test>();
            Questions = new ObservableCollection<Question>();
            Varients = new ObservableCollection<Varient>();
            InstantResults = new ObservableCollection<Result>();
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
            Languages = new ObservableCollection<string>() { "English", "Русский", "Беларуская" };
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
                    if (value.Code == null)
                    {
                        value.Code = Generate(SelectedTest.Name);
                    }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
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
            if (this.User.Login != null && this.User.Password != null)
            {
                if (await CheckUser())
                {
                    IsVerify = false;
                    ActiveUser = this.User;
                    await Navigation.PushModalAsync(new MainPage(this));
                }
                else IsVerify = true;
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
            if (userObject is User user)
            {
                if ((user.Login != null && user.Email != null && user.Password != null) && (user.Login != "" && user.Email != "" && user.Password != ""))
                {
                    IsBusy = true;
                    isNull = false;
                    bool laters = false;
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
                        {
                            ActiveUser = addedFriend;
                            laters = true;
                        }
                    }
                    IsBusy = false;
                    if (laters)
                    {
                        Later();
                    }
                    else isNull = true;
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
            selectedTest = new Test
            {
                Name = ""
            };
            Questions.Clear();
            Navigation.PushModalAsync(new Page1(selectedTest, this));
        }
        private void CreateQuestion()
        {
            selectedQuestion = new Question
            {
                OQuestion = ""
            };
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
            if (User != null)
            {
                User user = await loginService.Get(User.Login);
                if (user != null)
                {
                    if (User.Equals(user))
                    {
                        User = user;
                        IsBusy = false;
                        return true;
                    }
                    else
                    {
                        IsBusy = false;
                        return false;
                    }
                }
            }
            IsBusy = false;
            return false;
        }
        public async Task<User> GetUser(User GettedUser)
        {
            IsBusy = true;
            if (GettedUser != null)
            {
                User user = await loginService.Get(GettedUser.Login);
                if (user != null)
                {
                    if (GettedUser.Equals(user))
                    {
                        GettedUser = user;
                        IsBusy = false;
                        return GettedUser;
                    }
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

            // очищаем список
            Tests.Clear();
            while (Tests.Any())
                Tests.RemoveAt(Tests.Count - 1);

            // добавляем загруженные данные
            IEnumerable<Test> friends = await testsService.GetTestByUserId(activeUser.Id);
            foreach (Test item in friends)
            {
                if (!Tests.Contains(item))
                {
                    Tests.Add(item);
                }
            }
            IsBusy = false;
        }
        public async Task GetQuestions()
        {
            if (initialized == true) return;
            IsBusy = true;
            IEnumerable<Question> questions = await questionsService.GetQuestByTestId(selectedTest.Id);

            // очищаем список
            Questions.Clear();
            while (Questions.Any())
                Questions.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные
            foreach (Question item in questions)
            {
                Questions.Add(item);
            }
            IsBusy = false;
        }
        public async Task GetVarients()
        {
            if (initialized == true) return;
            IsBusy = true;

            IEnumerable<Varient> varients = new Collection<Varient>();
            if (SelectedQuestion != null)
            {
                varients = await varientsService.GetVarientByQuestId(SelectedQuestion.Id);
            }

            // очищаем список
            Varients.Clear();
            while (Varients.Any())
                Varients.RemoveAt(Questions.Count - 1);

            // добавляем загруженные данные
            foreach (Varient item in varients)
            {
                Varients.Add(item);
            }
            IsBusy = false;
        }
        #endregion
        #region Saving
        public void CopyMethod()
        {
            Clipboard.SetTextAsync(TestCode);
        }
        static string Generate(string name)
        {
            int L = 35;
            char[] G = name.ToCharArray().Where(p => p != ' ').ToArray();
            char[] A = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
                'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            char[] generatedChar = new char[L];
            int x;
            Random r = new Random();
            Random k = new Random();
            int temp;
            for (int i = 0; i < L; i++)
            {
                temp = k.Next(2);
                x = A.Length / 2;
                generatedChar[i] = A[A.Length / 2];
                if (temp == 0)
                {
                    x = r.Next(A.Length);
                    generatedChar[i] = A[x];
                }
                else if (temp == 1)
                {
                    x = r.Next(G.Length);
                    generatedChar[i] = G[x];
                }
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
            Test friend;
            if (selectedTest == null)
            {
                friend = testObject as Test;
            }
            else
            {
                friend = testObject as Test;
                friend.Id = selectedTest.Id;
                friend.UserId = selectedTest.UserId;
            }
            IsBusy = true;
            if (friend.Code == null)
            {
                friend.Code = Generate(friend.Name);
            }
            TestCode = friend.Code;
            friend.Questions = Questions.ToList();
            if (friend.Name == "")
                friend.Name = "Неназванный тест";
            if (friend != null && activeUser != null)
            {
                friend.UserId = activeUser.Id;
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
            if (testObject is Test friend)
            {
                if (friend.Code == null)
                {
                    friend.Code = Generate(friend.Name);
                }
                if (friend.Name == "")
                    friend.Name = "Неназванный тест";
                friend.UserId = activeUser.Id;
                friend.Questions = Questions.ToList();
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
                OnPropertyChanged("TestCode");
            }
        }
        private async void SaveQuestion(object testObject)
        {
            Question friend;
            if (selectedQuestion == null)
            {
                friend = testObject as Question;
            }
            else
            {
                friend = testObject as Question;
                friend.Id = selectedQuestion.Id;
                friend.TestId = selectedQuestion.TestId;
            }
            if (friend.OQuestion == "")
                friend.OQuestion = "Неназванный вопрос";
            friend.Varients = Varients.ToList();
            selectedTest.Questions = Questions.ToList();
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
                    {
                        Questions.Add(addedFriend);
                    }
                }
                IsBusy = false;
            }
            Back();
        }
        private async Task SaveQuestionNotBack(object testObject)
        {
            if (testObject is Question friend)
            {
                if (selectedTest.Id == 0)
                {
                    await SaveTestNotBack(selectedTest);
                }
                friend.TestId = selectedTest.Id;
                friend.Varients = Varients.ToList();
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
            if (varientObject is Varient friend)
            {
                if (selectedQuestion.Id == 0)
                {
                    await SaveQuestionNotBack(selectedQuestion);
                }
                friend.QuestionId = selectedQuestion.Id;
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
                IsBusy = false;
                Back();
            }
        }
        #endregion
        #region Deleting
        private async void DeleteTest(object friendObject)
        {
            if (friendObject is Test friend)
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
            if (friendObject is Question friend)
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
            if (friendObject is Varient friend)
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