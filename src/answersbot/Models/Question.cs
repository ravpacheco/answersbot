using Lime.Protocol;
using System;

namespace answersbot.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Document Content { get; set; }
    }
}