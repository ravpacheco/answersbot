﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
    }
}