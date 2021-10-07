using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using TeCon.ViewModels;

namespace TeCon.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string OQuestion { get; set; }
        public int? TestId { get; set; }
        public Test Test { get; set; }
        public IEnumerable<Varient> Varients { get; set; }
    }
}
