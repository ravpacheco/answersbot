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

            var userEntity = database.Users.FirstOrDefault(u => u.Node.Name == user.Node.Name);

            if(userEntity == null)
            {
                return await AddUserAsync(user);
            }
            
            return userEntity;
        }

        public Task UpdateUserSessionAsync(User user)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.FirstOrDefault(u => u.Node.Name == user.Node.Name);
            database.Users.Remove(userEntity);

            if (user.Session != null) { 
                userEntity.Session = user.Session;
                database.Users.Add(userEntity);
            }

            return Task.CompletedTask;
        }

        public Task UpdateUserAnswersAsync(User user, Answer answer)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.FirstOrDefault(u => u.Node.Name == user.Node.Name);
            database.Users.Remove(userEntity);

            if (answer != null)
            {
                userEntity.MyAnswers.Add(answer);
                database.Users.Add(userEntity);
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



        public User AddUser(User user)
        {
            var database = DataContext.Database();

            user.Session = new Session
            {
                State = SessionState.FirstAccess
            };

            database.Users.Add(user);

            return user;
        }

        public User GetUser(User user)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.FirstOrDefault(u => u.Node.Name == user.Node.Name);

            if (userEntity == null)
            {
                return AddUser(user);
            }

            return userEntity;
        }

        public void UpdateUserSession(User user)
        {
            var database = DataContext.Database();

            var userEntity = database.Users.First(u => u.Node.Name == user.Node.Name);
            database.Users.Remove(userEntity);

            if (user.Session != null)
            {
                userEntity.Session = user.Session;
                database.Users.Add(userEntity);
            }
        }

    }
}