using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Protocol;
using Takenet.MessagingHub.Client.Extensions.Bucket;

namespace answersbot.Receivers
{
    internal class CloseQuestion : BaseMessageReceiver
    {
        public CloseQuestion(IBucketExtension bucket) : base(bucket)
        {
        }

        public override async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = new CancellationToken())
        {
            // Identify the user owner of the question
            var userIdentity = envelope.From.ToIdentity().ToString();
            var userIndex = await GetUserIndexForNodeAsync(userIdentity, cancellationToken);
            var user = await Bucket.GetAsync<User>(userIndex, cancellationToken);
            if (user != null)
            {
                // Identify the question
                int questionExternalIndex;
                if (int.TryParse(envelope.Content.ToString(), out questionExternalIndex))
                {
                    var questionIndex = questionExternalIndex - 2;
                    user.Questions = user.Questions ?? new Question[0];
                    var question = user.Questions.ElementAtOrDefault(questionIndex);
                    if (question != null)
                    {
                        // Update the question
                        question.IsActive = false;
                        // Save the user
                        await SaveUserAsync(user, cancellationToken);
                    }
                }
            }
        }
    }
}
