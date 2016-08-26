using answersbot.Models;
using Lime.Messaging.Contents;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Storage
{
    public class DataContext
    {
        public DataContext()
        {
            Users = new List<User>();
            Questions = new List<Question>();
        }

        private static DataContext _database;

        public List<User> Users { get; set; }
        public List<Question> Questions { get; set; }

        public static DataContext Database()
        {
            if(_database == null)
            {
                _database = new DataContext();
                _database.PopulateDataBase();
            }

            return _database;
        }
        
        private void PopulateDataBase()
        {
            CreateNewQuestion("5531993180234@0mn.io", "Por que a água da privada gira em sentidos diferentes no hemisfério norte e no sul?");
            CreateNewQuestion("5531992154087@0mn.io", "Por que kamikazes usam capacetes?");
            CreateNewQuestion("5531992125857@0mn.io", "Por que os filmes de batalha espaciais tem explosões tão barulhentas, se o som não se propaga no vácuo?");
            CreateNewQuestion("5531998271039@0mn.io", "Qual dos braços da poltrona do cinema é o da sua cadeira?");
        }


        private void CreateNewQuestion(string user, string question)
        {
            User newUser = new User();
            newUser.Node = Node.Parse(user);
            newUser.Session = new Models.Session { State = Models.SessionState.FirstAccess };

            Question newQuestion = new Question();
            newQuestion.Content = new PlainText() { Text = question };
            newQuestion.UserId = newUser.Id;

            newUser.MyQuestions.Add(newQuestion);

            _database.Users.Add(newUser);
            _database.Questions.Add(newQuestion);
        }


    }
}