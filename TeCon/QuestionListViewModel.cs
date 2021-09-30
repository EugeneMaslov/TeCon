using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TeCon
{
    public class QuestionListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<QuestionViewModel> Question { get; set; }
        public ObservableCollection<VarientsViewModel> Varients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateQuestionCommand { protected set; get; }
        public ICommand DeleteQuestionCommand { protected set; get; }
        public ICommand SaveQuestionCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public ICommand CreateVarientCommand { protected set; get; }
        public ICommand DeleteVarientCommand { protected set; get; }
        public ICommand SaveVarientCommand { protected set; get; }

        VarientsViewModel selectedVarient;
        QuestionViewModel selectedQuestion;
        public INavigation Navigation { get; set; }

        public QuestionListViewModel()
        {
            Question = new ObservableCollection<QuestionViewModel>();
            CreateQuestionCommand = new Command(CreateQuestion);
            DeleteQuestionCommand = new Command(DeleteQuestion);
            SaveQuestionCommand = new Command(SaveQuestion);
            BackCommand = new Command(Back);
            Varients = new ObservableCollection<VarientsViewModel>();
            CreateVarientCommand = new Command(CreateVarient);
            DeleteVarientCommand = new Command(DeleteVarient);
            SaveVarientCommand = new Command(SaveVarient);
            BackCommand = new Command(Back);
        }
        public QuestionViewModel SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                QuestionViewModel tempQuestion = value;
                selectedQuestion = null;
                OnPropertyChanged("Selected question");
                Navigation.PushModalAsync(new PageQuestConst(tempQuestion));
            }
        }
        public VarientsViewModel SelectedVarient
        {
            get { return selectedVarient; }
            set
            {
                VarientsViewModel tempVarient = value;
                selectedVarient = null;
                OnPropertyChanged("Selected varient");
                Navigation.PushModalAsync(new PageVarient(tempVarient));
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private void CreateQuestion()
        {
            Navigation.PushModalAsync(new PageQuestConst(new QuestionViewModel(new VarientsViewModel()) { ListQuestViewModel = this }));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
        }
        private void SaveQuestion(object questionObject)
        {
            QuestionViewModel question = questionObject as QuestionViewModel;
            if (question != null && question.IsValid && !Question.Contains(question))
            {
                Question.Add(question);
            }
            Back();
        }
        private void DeleteQuestion(object questionObject)
        {
            QuestionViewModel question = questionObject as QuestionViewModel;
            if (question != null)
            {
                Question.Remove(question);
            }
            Back();
        }
        private void CreateVarient()
        {
            Navigation.PushModalAsync(new PageVarient(new VarientsViewModel() { ListQuestViewModel = this }));
        }
        private void SaveVarient(object varientObject)
        {
            VarientsViewModel varients = varientObject as VarientsViewModel;
            if (varients != null && varients.IsValid && !Varients.Contains(varients))
            {
                Varients.Add(varients);
            }
            Back();
        }
        private void DeleteVarient(object varientObject)
        {
            VarientsViewModel varient = varientObject as VarientsViewModel;
            if (varient != null)
            {
                Varients.Remove(varient);
            }
            Back();
        }
    }
}
