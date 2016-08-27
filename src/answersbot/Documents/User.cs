using System;
using System.Linq;
using System.Runtime.Serialization;
using Lime.Protocol;
using Lime.Protocol.Serialization;

namespace answersbot.Documents
{
    [DataContract]
    internal class User : Document
    {
        static User()
        {
            TypeUtil.RegisterDocument<User>();
        }

        [DataMember(Name = "id")]
        public string Index { get; set; }

        [DataMember(Name = "name")]
        public string Identity { get; set; }

        [DataMember(Name = "questions")]
        public Question[] Questions { get; set; }

        [DataMember(Name = "currentQuestionId")]
        public Guid CurrentQuestionId { get; set; }

        [DataMember(Name = "currentQuestionUserIndex")]
        public string CurrentQuestionUserIndex { get; set; }

        public User() : base(MediaType.Parse("application/x-user"))
        {
        }

        public static string IndexFor(int index) => $"U{index}";

        public int GetExternalQuestionIndex(Question question)
        {
            var index = Questions.ToList().IndexOf(question);
            return index == -1 ? 1 : index + 2;
        }
    }
}