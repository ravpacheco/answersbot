using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using answersbot.Models;
using answersbot.Storage;
using System.Threading.Tasks;

namespace answersbot.Services
{
    public class QuestionService
    {
        public async Task<Question> GetRandomQuestion(User user)
        {
            // Get a Question that
            // 1. It is not mine question
            // 2. It is not a question that was answered by me
            var database = DataContext.Database();
            var questions = database.Questions.Where(q => q.UserId != user.Id && user.MyAnswers.FirstOrDefault(a => a.QuestionId == q.Id ) == null )?.ToList();

            if(questions == null || questions.Count == 0 )
            {
                return null;
            }

            Random rand = new Random();

            return questions[rand.Next(0, questions.Count)];
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            var database = DataContext.Database();

            database.Questions.Add(question);

            var user = database.Users.FirstOrDefault(u => u.Id == question.UserId);
            database.Users.Remove(user);

            user.MyQuestions.Add(question);
            database.Users.Add(user);

            return question;
        }
    }
}