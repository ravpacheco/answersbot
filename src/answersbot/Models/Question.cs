using Lime.Protocol;
using System;

namespace answersbot.Models
{
    public class Question
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public Document Content { get; set; }

        public Question(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString().Substring(0, 5);
        }
    }
}