using System;

namespace answersbot.Models
{
    public class Session
    {
        public SessionState State { get; set; }
        public string QuestionId { get; set; }
    }

    public enum SessionState
    {
        FirstAccess,
        Starting,
        Answering,
        Questioning,
    }
}