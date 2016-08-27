using System;
using System.Runtime.Serialization;
using Lime.Protocol;

namespace answersbot.Documents
{
    [DataContract]
    internal class Answer : Document
    {
        [DataMember(Name = "timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "userIdentity")]
        public string UserIdentity { get; set; }

        public Answer() : base(MediaType.Parse("application/x-answer"))
        {
        }
    }
}