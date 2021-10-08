using System;
using System.Collections.Generic;
using System.Text;

namespace TeCon.Models
{
    public class Varient
    {
        public int Id { get; set; }
        public string OVarient { get; set; }
        public bool IsTrue { get; set; }
        public int QuestionId { get; set; }
        public override bool Equals(object obj)
        {
            Varient varient = obj as Varient;
            return this.Id == varient.Id;
        }
    }
}
