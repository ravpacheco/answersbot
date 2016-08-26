using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using answersbot.Models;
using answersbot.Storage;

namespace answersbot.Services
{
    public class QuestionService
    {
        public async Task<Question> GetRandomQuestion(User user)
        {
            // Get a Question that
            // 1. It is not mine question
            var database = DataContext.Database();
            database


            // 2. It is not a question that was answered by me

        }
    }
}