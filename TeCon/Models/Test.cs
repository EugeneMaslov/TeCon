using System.Collections.Generic;

namespace TeCon.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public int UserId { get; set; }
        public override bool Equals(object obj)
        {
            Test test = obj as Test;
            return this.Id == test.Id;
        }
    }
}
