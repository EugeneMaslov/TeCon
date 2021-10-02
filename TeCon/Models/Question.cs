using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace TeCon.Models
{
    public class Question
    {
        public string OQuestion { get; set; }
        public List<Varient> Varients { get; set; }
    }
}
