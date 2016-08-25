using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Document Content { get; set; }
    }
}