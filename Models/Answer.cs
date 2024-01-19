using System;
using System.Collections.Generic;

namespace Cousera.Models
{
    public partial class Answer
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? QuestionId { get; set; }

        public virtual Question? Question { get; set; }
    }
}
