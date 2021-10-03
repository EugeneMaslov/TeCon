using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using TeCon.ViewModels;

namespace TeCon.Models
{
    public class Question
    {
        public string OQuestion { get; set; }
        public ObservableCollection<VarientViewModel> Varients { get; set; }
        public VarientViewModel varientViewModel { get; set; }
    }
}
