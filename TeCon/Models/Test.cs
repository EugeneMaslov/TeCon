using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TeCon.ViewModels;

namespace TeCon.Models
{
    public class Test
    {
        public string Name { get; set; }
        public ObservableCollection<QuestionViewModel> Questions { get; set; }
        public QuestionViewModel questionViewModel { get; set; }
    }
}
