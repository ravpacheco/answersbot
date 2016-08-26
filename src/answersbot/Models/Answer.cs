using System;
using System.Runtime.Serialization;
using Lime.Protocol;

namespace answersbot.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
    }
}