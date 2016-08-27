using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Protocol;
using Takenet.MessagingHub.Client.Extensions.Bucket;

namespace answersbot.Receivers
{
    internal class SaveQuestion : BaseMessageReceiver
    {
        public SaveQuestion(IBucketExtension bucket) : base(bucket)
        {
        }

        public override async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = new CancellationToken())
        {
            // Identity the current user
            var userIdentity = envelope.From.ToIdentity().ToString();
            var userIndex = await GetUserIndexForNodeAsync(userIdentity, cancellationToken);
            var user = await Bucket.GetAsync<User>(userIndex, cancellationToken) ?? new User();
            user.Index = userIndex;
            user.Identity = userIdentity;

            // Add the question
            var question = new Question
            {
                Text = envelope.Content.ToString(),
                IsActive = true
            };
            user.Questions = (user.Questions ?? new Question[0]).Concat(new[] { question }).ToArray();

            // Save the user
            await SaveUserAsync(user, cancellationToken);
        }
    }
}
