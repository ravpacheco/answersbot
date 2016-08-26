using Lime.Protocol;
using System;

namespace answersbot.Models
{
    public class Question
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public Document Content { get; set; }

        public Question()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 5);
        }
    }
}