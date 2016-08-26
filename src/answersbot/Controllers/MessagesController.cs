using answersbot.Models;
using answersbot.Resources;
using answersbot.Services;
using Lime.Protocol;
using Lime.Protocol.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace answersbot.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly UserService userService;

        public MessagesController()
        {
            userService = new UserService();
        }

        // POST api/values
        public async Task Post(JObject jsonObject)
        {
            var envelopeSerializer = new EnvelopeSerializer();

            var message = (Message)envelopeSerializer.Deserialize(jsonObject.ToString());
            var messageContent = message.Content.ToString();

            var user = await userService.GetUserAsync(new User { Node = message.From });

            switch (user.Session.State)
            {
                case Models.SessionState.FirstAccess:
                    //Send a initial message and change user state to "Starting"

                    //1 - Send a initial message

                    //2 - change user state
                    await ChangeUserStateAsync(user, Models.SessionState.Starting);

                    break;
                case Models.SessionState.Starting:
                    //Handle to question or to answer action

                    if (UserWouldLikeQuestion(messageContent))
                    {
                        //1 - Send "SendQuestion" message

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Questioning);
                    }
                    else if (UserWouldLikeAnswer(messageContent))
                    {
                        //1 - Send a random question to user. Or if not exist send some default message

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Answering);
                    }
                    else
                    {
                        //1 - Send "FallbackMessage" message

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Starting);
                    }

                    break;
                case Models.SessionState.Questioning:
                    //Handle a new question

                    //1 - Save sent question

                    //2 - Send "ResetMessageByQuestion" message

                    //3 - change user state
                    await ChangeUserStateAsync(user, Models.SessionState.Starting);

                    break;
                case Models.SessionState.Answering:
                    //Handle: a new answer or more question

                    //1 - Save sent answer or skip current question
                    if (UserWouldLikeSkipCurrentQuestion(messageContent))
                    {
                        //1 - Send a random question to user. Or if not exist send some default message

                    }
                    else
                    {
                        //2.1 - Send to question's user owner this answer


                        //2.2 - Send "ResetMessageByAnswer" message

                        //3 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Starting);                   
                    }

                    break;
            }

            Console.WriteLine("Received Message");
        }

        private async Task ChangeUserStateAsync(User user, Models.SessionState newState)
        {
            user.Session.State = newState;
            await userService.UpdateUserAsync(user);
        }

        private bool UserWouldLikeQuestion(string message)
        {
            return Strings.QuestionActionValues.First(q => q.Equals(message.ToLowerInvariant())) != null;
        }

        private bool UserWouldLikeAnswer(string message)
        {
            return Strings.AnswerActionValues.First(q => q.Equals(message.ToLowerInvariant())) != null;
        }

        private bool UserWouldLikeSkipCurrentQuestion(string message)
        {
            return Strings.SkipQuestionActionValues.First(q => q.Equals(message.ToLowerInvariant())) != null;
        }
    }
}
