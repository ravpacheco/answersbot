using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Models
{
    public class Session
    {
        public SessionState State { get; set; }
        public Guid QuestionId { get; set; }
    }

    public enum SessionState
    {
        FirstAccess,
        Starting,
        Answering,
        Questioning,
    }
}