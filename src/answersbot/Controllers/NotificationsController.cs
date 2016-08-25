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
    public class NotificationsController : ApiController
    {
        // POST api/values
        public void Post([FromBody]JObject jsonObject)
        {
            var envelopeSerializer = new EnvelopeSerializer();

            var notification = envelopeSerializer.Deserialize(jsonObject.ToString());

            Console.WriteLine("Received Notification");
        }

    }
}
