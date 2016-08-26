using answersbot.Models;
using answersbot.Services;
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
            CreateNewQuestion("225323721@telegram.gw.msging.net", "Qual o melhor site para comprar eletrônicos?");
            CreateNewQuestion("5531992154087@0mn.io", "Por que kamikazes usam capacetes?");
            CreateNewQuestion("5531992125857@0mn.io", "É confiável comprar um iphone 6 no mercado livre por 1.969 reais?");
            CreateNewQuestion("5531998271039@0mn.io", "Qual dos braços da poltrona do cinema é o da sua cadeira?");

            CreateNewQuestion("1414594858555994@messenger.gw.msging.net", "O que você faria se descobrisse que seu amigo(a) é homossexual?");
            CreateNewQuestion("5531992125857@0mn.io", "O que você faria se descobrisse que seu amigo(a) usa Crocs?");
            CreateNewQuestion("5531992154087@0mn.io", "Como falar à um colega de trabalho que ele tem cc?");
            CreateNewQuestion("5531992125857@0mn.io", "Por que quando ligamos para um número errado nunca dá ocupado?");
            CreateNewQuestion("5531998271039@0mn.io", "Por que a fila do lado sempre parece andar mais rápido?");

            CreateNewQuestion("1414594858555994@messenger.gw.msging.net", "Qual a melhor raça de cachorro para crianças?");
            CreateNewQuestion("5531992154087@0mn.io", "Qual o melhor bar em Belo Horizonte para comemorar aniversário?");
            CreateNewQuestion("5531992125857@0mn.io", "Qual o melhor lugar para morar em Belo Horizonte?");
            CreateNewQuestion("5531998271039@0mn.io", "O que é melhor? Gato ou Cachorro?");

            CreateNewQuestion("225323721@telegram.gw.msging.net", "Qual o maior time de Minas Gerais? Atlético ou Cruzeiro?");
            CreateNewQuestion("5531992154087@0mn.io", "Na sua humilde opinião, a Letícia fala muito?");
            CreateNewQuestion("5531992125857@0mn.io", "Por que quando ligamos para um número errado nunca dá ocupado?");
            CreateNewQuestion("5531998271039@0mn.io", "Qual o último livro que você leu? Você o recomendaria?");

            CreateNewQuestion("1414594858555994@messenger.gw.msging.net", "Qual o último filme que você assistiu? Você o recomendaria?");
            CreateNewQuestion("5531992154087@0mn.io", "Qual seu estilo de música preferido?");
        }

        private void CreateNewQuestion(string user, string question)
        {
            User newUser = new User();
            newUser.Node = Node.Parse(user);
            newUser.Session = new Models.Session { State = Models.SessionState.FirstAccess };

            var userService = new UserService();
            var questionService = new QuestionService();

            newUser = userService.GetUser(newUser);

            Question newQuestion = new Question();
            newQuestion.Content = new PlainText() { Text = question };
            newQuestion.UserId = newUser.Id;

            questionService.AddQuestion(newQuestion);

        }





    }
}