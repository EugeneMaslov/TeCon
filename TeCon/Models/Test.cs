﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TeCon.ViewModels;

namespace TeCon.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public override bool Equals(object obj)
        {
            Test test = obj as Test;
            return this.Id == test.Id;
        }
    }
}
