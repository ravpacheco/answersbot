using Lime.Protocol;
using Lime.Protocol.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace answersbot.Controllers
{
    public class MessagesController : ApiController
    {

        // POST api/values
        public void Post(JObject jsonObject)
        {
            var envelopeSerializer = new EnvelopeSerializer();
            
            var message = envelopeSerializer.Deserialize(jsonObject.ToString());

            Console.WriteLine("Received Message");
        }

    }
}
