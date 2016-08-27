using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Protocol;
using Takenet.MessagingHub.Client.Extensions.Bucket;

namespace answersbot.Receivers
{
    internal class CancelAllQuestions : BaseMessageReceiver
    {
        public CancelAllQuestions(IBucketExtension bucket) : base(bucket)
        {
        }

        public override async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = new CancellationToken())
        {
            // Identity the current user
            var userIdentity = envelope.From.ToIdentity().ToString();
            var userIndex = await GetUserIndexForNodeAsync(userIdentity, cancellationToken);
            var user = await Bucket.GetAsync<User>(userIndex, cancellationToken);
            if (user != null)
            {
                user.Questions = user.Questions ?? new Question[0];

                // Mark all questions as inactive
                user.Questions.ForEach(q => q.IsActive = false);

                // Save the user
                await SaveUserAsync(user, cancellationToken);
            }
        }
    }
}
