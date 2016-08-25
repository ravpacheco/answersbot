﻿using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Models
{
    public class User
    {
        public Node Node { get; set; }
        public Guid Id { get; set; }
        public Session Session { get; set; }
        public List<Question> MyQuestions { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}