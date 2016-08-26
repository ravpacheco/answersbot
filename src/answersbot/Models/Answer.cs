using System;
using System.Runtime.Serialization;
using Lime.Protocol;

namespace answersbot.Models
{
    public class Answer
    {
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
    }
}