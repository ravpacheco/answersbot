using answersbot.Models;
using answersbot.Resources;
using answersbot.Services;
using Lime.Protocol.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Lime.Protocol;
using Lime.Protocol.Serialization.Newtonsoft;
using Newtonsoft.Json;

namespace answersbot.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly QuestionService questionService;
        private readonly UserService userService;
        private readonly WebClientService webClientService;

        public MessagesController()
        {
            questionService = new QuestionService();
            userService = new UserService();
            webClientService = new WebClientService();
        }

        // POST api/messages
        public async Task<IHttpActionResult> Post(JObject jsonObject)
        {
            var message = JsonConvert.DeserializeObject<Message>(jsonObject.ToString(), JsonNetSerializer.Settings);

            var messageContent = message.Content.ToString();

            var user = await userService.GetUserAsync(new User { Node = message.From });

            await webClientService.SendMessageAsync("Oi. Recebi sua mensagem", Node.Parse("31992125857@0mn.io"));

            return Ok();

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
                    await questionService.AddQuestionAsync(new Question { Content = message.Content, UserId = user.Id });

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
                        var questionId = ExtractQuestionIdFromAnswer(messageContent);

                        //2.1 - Send to question's user owner this answer
                        await userService.UpdateUserAnswersAsync(user, new Answer { UserId = user.Id, QuestionId = questionId});

                        //2.2 - Send "ResetMessageByAnswer" message

                        //3 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Starting);
                    }

                    break;
            }

            Console.WriteLine("Received Message");
        }

        private string ExtractQuestionIdFromAnswer(string messageContent)
        {
            throw new NotImplementedException();
        }

        private async Task ChangeUserStateAsync(User user, Models.SessionState newState)
        {
            user.Session.State = newState;
            await userService.UpdateUserSessionAsync(user);
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
