using answersbot.Models;
using answersbot.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace answersbot.Services
{
    public class UserService
    {
        public async Task<User> AddUserAsync(User user)
        {
            var database = DataContext.Database();

            user.Session = new Session
            {
                State = SessionState.FirstAccess
            };

            database.Users.Add(user);

            return user;
        }

        public async Task<User> GetUserAsync(User user)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.First(u => u.Node.Name == user.Node.Name);

            if(userEntity == null)
            {
                return await AddUserAsync(user);
            }
            
            return userEntity;
        }

        public Task UpdateUserAsync(User user)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.First(u => u.Node.Name == user.Node.Name);
            database.Users.Remove(userEntity);

            if (user != null)
            {
                database.Users.Add(user);
            }

            return Task.CompletedTask;
        }

        public async Task MarkQuestionAsAnswerdAsync(User user, Question question)
        {
            var database = DataContext.Database();

            var userEntity = await GetUserAsync(user);

            var questionEntity = database.Questions.First(q => q.Id == question.Id);
            database.Questions.Remove(questionEntity);
        }
    }
}