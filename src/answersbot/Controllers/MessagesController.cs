using answersbot.Models;
using answersbot.Services;
using Lime.Protocol.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace answersbot.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly UserService userService;
        private readonly WebClientService webClientService;

        public MessagesController()
        {
            userService = new UserService();
            webClientService = new WebClientService();
        }

        // POST api/values
        public async Task Post(JObject jsonObject)
        {
            var envelopeSerializer = new EnvelopeSerializer();
            
            var message = envelopeSerializer.Deserialize(jsonObject.ToString());

            var user = await userService.GetUserAsync(new User { Node = message.From });

            await webClientService.SendMessageAsync("Oi. Recebi sua mensagem");

            switch (user.Session.State)
            {
                case Models.SessionState.FirstAccess:
                    //Send a initial message and change user state to "Starting"
                    break;
                case Models.SessionState.Starting:
                    //Handle to question or to answer action
                    break;
                case Models.SessionState.Questioning:
                    //Handle a new question
                    break;
                case Models.SessionState.Answering:
                    //Handle: a new answer or more question
                    break;
            }
            
            Console.WriteLine("Received Message");
        }

    }
}
