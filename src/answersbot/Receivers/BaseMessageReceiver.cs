using System;
using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Messaging.Contents;
using Lime.Protocol;
using Takenet.MessagingHub.Client.Extensions.Bucket;
using Takenet.MessagingHub.Client.Listener;

namespace answersbot.Receivers
{
    internal abstract class BaseMessageReceiver : IMessageReceiver
    {
        protected IBucketExtension Bucket { get; }

        protected BaseMessageReceiver(IBucketExtension bucket)
        {
            Bucket = bucket;
        }

        public abstract Task ReceiveAsync(Message envelope,
            CancellationToken cancellationToken = new CancellationToken());

        protected async Task<string> GetUserIndexForNodeAsync(string userIdentity, CancellationToken cancellationToken)
        {
            var userIndex =
                (await Bucket.GetAsync<PlainText>(userIdentity, cancellationToken))?.Text ??
                GenerateNewUserIndex();
            return userIndex;
        }

        protected async Task<int> GetUsersCountAsync(CancellationToken cancellationToken)
        {
            int userCount;
            int.TryParse((await Bucket.GetAsync<PlainText>(nameof(userCount), cancellationToken))?.Text, out userCount);
            return userCount;
        }

        protected string GenerateNewUserIndex()
        {
            int userCount;
            lock (Bucket)
            {
                int.TryParse(Bucket.GetAsync<PlainText>(nameof(userCount)).Result?.Text, out userCount);
                userCount = userCount + 1;
                var newUserCountValue = new PlainText { Text = userCount.ToString() };
                Bucket.SetAsync(nameof(userCount), newUserCountValue, TimeSpan.FromDays(short.MaxValue)).Wait();
            }
            var userIndex = User.IndexFor(userCount);
            return userIndex;
        }

        protected async Task SaveUserAsync(User user, CancellationToken cancellationToken)
        {
            var userIdForNode = new PlainText { Text = user.Index };
            await Bucket.SetAsync(user.Identity, userIdForNode, TimeSpan.FromDays(short.MaxValue), cancellationToken);
            await Bucket.SetAsync(user.Index, user, TimeSpan.FromDays(short.MaxValue), cancellationToken);
        }
    }
}