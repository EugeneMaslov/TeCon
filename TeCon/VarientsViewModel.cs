using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TeCon
{

    public class VarientsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Varients varients;
        VarientsListViewModel lvm;

        public VarientsViewModel()
        {
            varients = new Varients();
        }

        public VarientsListViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
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

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Varient.Trim());
            }
        }

        public bool IsTrue
        {
            get { return varients.IsTrue; }
            set
            {
                if (varients.IsTrue != value)
                {
                    varients.IsTrue = value;
                    OnPropertyChanged("IsTrue");
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
