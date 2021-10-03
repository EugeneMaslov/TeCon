using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using TeCon.Models;

namespace TeCon.ViewModels
{
    public class VarientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        TestListViewModel testListViewModel;
        public Varient Varient { get; private set; }
        public VarientViewModel()
        {
            Varient = new Varient();
        }
        public TestListViewModel ListViewModel
        {
            get { return testListViewModel; }
            set
            {
                if (testListViewModel != value)
                {
                    testListViewModel = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string OVarient
        {
            get { return Varient.OVarient; }
            set
            {
                if (Varient.OVarient != value)
                {
                    Varient.OVarient = value;
                    OnPropertyChanged("OVarient");
                }
            }
        }
        public bool IsTrue
        {
            get { return Varient.IsTrue; }
            set
            {
                if (Varient.IsTrue != value)
                {
                    Varient.IsTrue = value;
                    OnPropertyChanged("IsTrue");
                }
            }
        }
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(OVarient.Trim());
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
