using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TeCon
{
    public class QuestionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<VarientsViewModel> Varients { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Question question;
        QuestionListViewModel lvm;
        private Varients varients;
        VarientsListViewModel vlvm;

        public VarientsListViewModel ListViewModel
        {
            get { return vlvm; }
            set
            {
                if (vlvm != value)
                {
                    vlvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string Varient
        {
            get { return varients.Varient; }
            set
            {
                if (varients.Varient != value)
                {
                    varients.Varient = value;
                    OnPropertyChanged("Varient");
                }
            }
        }

        public QuestionViewModel(VarientsViewModel vlvm)
        {
            question = new Question(vlvm);
            varients = new Varients();
        }
        public QuestionListViewModel ListQuestViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListQuestViewModel");
                }
            }
        }

        public string quest
        {
            get { return question.quest; }
            set
            {
                if (question.quest != value)
                {
                    question.quest = value;
                    OnPropertyChanged("quest");
                }
            }
        }
        public int size
        {
            get { return question.varients.Varient.Length; }
            set
            {
                if (question.varients.Varient.Length != value)
                {
                    OnPropertyChanged("Size");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(question.quest.Trim());
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
