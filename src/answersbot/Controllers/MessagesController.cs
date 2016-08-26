using answersbot.Models;
using answersbot.Resources;
using answersbot.Services;
using Lime.Protocol.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Lime.Messaging.Contents;
using Lime.Protocol;
using Lime.Protocol.Serialization.Newtonsoft;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SessionState = answersbot.Models.SessionState;
using System.Threading;

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
            Console.WriteLine($"Message Received: {jsonObject}");

            var message = JsonConvert.DeserializeObject<Message>(jsonObject.ToString(), JsonNetSerializer.Settings);

            var plainText = (message.Content as PlainDocument)?.Value ?? (message.Content as PlainText)?.Text;
            if (plainText == null)
            {
                await webClientService.SendMessageAsync(Strings.FallbackMessage, message.From);
                return Ok();
            }

            var messageContent = plainText;

            var user = await userService.GetUserAsync(new User { Node = message.From });

            if(Strings.KeepAnsweringActionValue == messageContent.ToLower())
            {
                return Ok();
            }

            if (Strings.StartActionValues.Contains(messageContent.ToLower()) && user.Session.State != SessionState.Answering)
            {
                await ChangeUserStateAsync(user, Models.SessionState.FirstAccess);
            }

            if (SponsorCommand(messageContent))
            {
                var to = messageContent.Split(' ')[1];

                var mediaLink = new MediaLink
                {
                    PreviewUri = new Uri(@"https://takenethmgomni.blob.core.windows.net/media-db/vivo_tn.jpg"),
                    PreviewType = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg),
                    Title = "Patrocinado",
                    Size = 200,
                    Text = "[Patrocinado] Pergunta patrocinada!",
                    Type = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg),
                    Uri = new Uri(@"https://takenethmgomni.blob.core.windows.net/media-db/vivo.jpg")
                };

                await webClientService.SendMessageAsync(mediaLink, Node.Parse(to));

                Thread.Sleep(100);

                var select = new Select
                {
                    Text = $"Como você avalia os serviços prestados pela VIVO ?",
                    Options = new[]
                            {
                                new SelectOption
                                {
                                    Text = "Excelente",
                                    Value = new PlainText { Text = "Excelente" }
                                },
                                new SelectOption
                                {
                                    Text = "Médio",
                                    Value = new PlainText { Text = "Médio" }
                                },
                                new SelectOption
                                {
                                    Text = "Precisa melhorar",
                                    Value = new PlainText { Text = "Precisa melhorar" }
                                }
                            }

                };

                await webClientService.SendMessageAsync(select, Node.Parse(to));

                //2 - change user state
                await ChangeUserStateAsync(user, Models.SessionState.Answering, "sponsor");

                return Ok();
            }

            var questionId = ExtractQuestionIdFromAnswer(messageContent);
            if (!string.IsNullOrEmpty(questionId))
            {
                var question = await questionService.GetQuestionByIdAsync(questionId);
                await SendInitialMenuAsync(string.Format(Strings.ResetMessageByClosing, question.Content), message.From);

                questionService.CloseQuestion(questionId);

                await ChangeUserStateAsync(user, Models.SessionState.Starting);
                return Ok();
            }

            switch (user.Session.State)
            {
                case Models.SessionState.FirstAccess:
                    //Send a initial message and change user state to "Starting"

                    //1 - Send a initial message
                    await SendInitialMenuAsync(Strings.FirstMessage, message.From);

                    //2 - change user state
                    await ChangeUserStateAsync(user, Models.SessionState.Starting);

                    break;
                case Models.SessionState.Starting:
                    //Handle to question or to answer action

                    if (UserWouldLikeQuestion(messageContent))
                    {
                        //1 - Send "SendQuestion" message
                        await webClientService.SendMessageAsync(Strings.SendQuestion, message.From);

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Questioning);
                    }
                    else if (UserWouldLikeAnswer(messageContent))
                    {
                        //1 - Send a random question to user. Or if not exist send some default message
                        var question = await SendRandomQuestionOrDefaultMessageAsync(user, message);

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Answering, question.Id);
                    }
                    else
                    {
                        //Handle a new question
                        await RegisterQuestionAync(user, message);
                    }

                    break;
                    
                case Models.SessionState.Questioning:
                    //Handle a new question
                    await RegisterQuestionAync(user, message);

                    break;
                case Models.SessionState.Answering:
                    //Handle: a new answer or more question

                    //1 - Save sent answer or skip current question
                    if (UserWouldLikeSkipCurrentQuestion(messageContent))
                    {
                        //1 - Send a random question to user. Or if not exist send some default message
                        var question = await SendRandomQuestionOrDefaultMessageAsync(user, message);

                        //2 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Answering, question.Id);
                    }
                    else
                    {
                        if (user.Session.QuestionId == "sponsor")
                        {
                            //2.2 - Send "ResetMessageByAnswer" message
                            await SendInitialMenuAsync(Strings.ResetMessageByAnswer, message.From);

                            //3 - change user state
                            await ChangeUserStateAsync(user, Models.SessionState.Starting);
                            break;
                        }

                        var question = await questionService.GetQuestionByIdAsync(user.Session.QuestionId);
                        var questionUser = userService.GetUserByIdAsync(question.UserId);

                        //2.1 - Send to question's user owner this answer
                        await userService.UpdateUserAnswersAsync(user, new Answer { UserId = user.Id, QuestionId = question.Id });
                        var select = new Select
                        {
                            Text = $"Pergunta: {question.Content}\nResposta: {messageContent}",
                            Options = new[]
                            {
                                new SelectOption
                                {
                                    Text = Strings.KeepAnsweringActionText,
                                    Value = new PlainText { Text = Strings.KeepAnsweringActionValue }
                                },
                                new SelectOption
                                {
                                    Text = Strings.QuestionClosedActionText,
                                    Value = new PlainText { Text = $"{Strings.QuestionClosedActionValue} #{question.Id}#" }
                                }
                            }

                        };
                        await webClientService.SendMessageAsync(select, questionUser.Node);

                        //2.2 - Send "ResetMessageByAnswer" message
                        await SendInitialMenuAsync(Strings.ResetMessageByAnswer, message.From);

                        //3 - change user state
                        await ChangeUserStateAsync(user, Models.SessionState.Starting);
                    }

                    break;
            }

            return Ok();
        }

        private async Task SendInitialMenuAsync(string text, Node to)
        {
            var select = new Select
            {
                Text = text,
                Options = new[]
                {
                    new SelectOption
                    {
                        Order = 1,
                        Text = Strings.QuestionActionText
                    },
                    new SelectOption
                    {
                        Order = 2,
                        Text = Strings.AnswerActionText
                    }
                }
            };
            await webClientService.SendMessageAsync(select, to);
        }

        private async Task<Question> SendRandomQuestionOrDefaultMessageAsync(User user, Message message)
        {
            var randomQuestion = await questionService.GetRandomQuestion(user);
            if (randomQuestion != null)
            {
                var select = new Select
                {
                    Text = randomQuestion.Content.ToString(),
                    Options = new[]
                    {
                        new SelectOption
                        {
                            Order = 1,
                            Text = Strings.SendAnotherQuestionActionText
                        }
                    }
                };
                await webClientService.SendMessageAsync(select, message.From);
            }
            else
            {
                await webClientService.SendMessageAsync(Strings.NoQuestionsAvailable, message.From);
            }

            return randomQuestion;
        }

        private bool SponsorCommand(string messageContent)
        {
            return messageContent.Contains("/patrocinado/");
        }

        private string ExtractQuestionIdFromAnswer(string messageContent)
        {
            var pattern = new Regex(@"#(?<questionId>.+)#");
            var match = pattern.Match(messageContent);
            var questionId = match.Groups["questionId"].Value;
            return questionId;
        }

        private async Task ChangeUserStateAsync(User user, Models.SessionState newState, string questionId = null)
        {
            user.Session.State = newState;
            user.Session.QuestionId = questionId;
            await userService.UpdateUserSessionAsync(user);
        }

        private bool UserWouldLikeQuestion(string message)
        {
            return Strings.QuestionActionValues.FirstOrDefault(q => q.Equals(message.ToLowerInvariant())) != null;
        }

        private bool UserWouldLikeAnswer(string message)
        {
            return Strings.AnswerActionValues.FirstOrDefault(q => q.Equals(message.ToLowerInvariant())) != null;
        }

        private bool UserWouldLikeSkipCurrentQuestion(string message)
        {
            return Strings.SkipQuestionActionValues.FirstOrDefault(q => q.Equals(message.ToLowerInvariant())) != null;
        }

        private async Task RegisterQuestionAync(User user, Message message)
        {
            //1 - Save sent question
            await questionService.AddQuestionAsync(new Question { Content = message.Content, UserId = user.Id });

            //2 - Send "ResetMessageByQuestion" message
            await SendInitialMenuAsync(Strings.ResetMessageByQuestion, message.From);

            //3 - change user state
            await ChangeUserStateAsync(user, Models.SessionState.Starting);
        }
    }
}
