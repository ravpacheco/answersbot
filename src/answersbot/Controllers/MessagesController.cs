using answersbot.Models;
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
            
            var message = envelopeSerializer.Deserialize(jsonObject.ToString());

            var user = await userService.GetUserAsync(new User { Node = message.From });

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
