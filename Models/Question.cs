using System;
using System.Collections.Generic;

namespace Cousera.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string? QuestionDescription { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
