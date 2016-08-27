using System;
using System.Runtime.Serialization;
using Lime.Protocol;

namespace answersbot.Documents
{
    [DataContract]
    internal class Question : Document
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "isActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "answers")]
        public Answer[] Answers { get; set; }

        public Question() : base(MediaType.Parse("application/x-question"))
        {
            Id = Guid.NewGuid();
        }
    }
}