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

    public class VarientsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<VarientsViewModel> Varients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateVarientCommand { protected set; get; }
        public ICommand DeleteVarientCommand { protected set; get; }
        public ICommand SaveVarientCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        VarientsViewModel selectedVarient;

        public INavigation Navigation { get; set; }

        public VarientsListViewModel()
        {
            Varients = new ObservableCollection<VarientsViewModel>();
            CreateVarientCommand = new Command(CreateVarient);
            DeleteVarientCommand = new Command(DeleteVarient);
            SaveVarientCommand = new Command(SaveVarient);
            BackCommand = new Command(Back);
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

        private void CreateVarient()
        {
            Navigation.PushModalAsync(new PageVarient(new VarientsViewModel() { ListViewModel = this }));
        }
        private void Back()
        {
            Navigation.PopModalAsync();
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
