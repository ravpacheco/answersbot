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
        public Task<Question> GetRandomQuestion(User user)
        {
            // Get a Question that
            // 1. It is not mine question
            var database = DataContext.Database();


            // 2. It is not a question that was answered by me
            return null;
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            var database = DataContext.Database();

            database.Questions.Add(question);

            var user = database.Users.First(u => u.Id == question.UserId);
            database.Users.Remove(user);

            user.MyQuestions.Add(question);
            database.Users.Add(user);

            return question;
        }
    }
}