using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Extensions.Bucket;
using Takenet.MessagingHub.Client.Sender;

namespace answersbot.Receivers
{
    internal class SendRandomQuestion : BaseMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private readonly Settings _settings;

        public SendRandomQuestion(IMessagingHubSender sender, IBucketExtension bucket, Settings settings) : base(bucket)
        {
            _sender = sender;
            _settings = settings;
        }

        public override async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = new CancellationToken())
        {
            // Identity the current user
            var userIdentity = envelope.From.ToIdentity().ToString();
            var userIndex = await GetUserIndexForNodeAsync(userIdentity, cancellationToken);
            var user = await Bucket.GetAsync<User>(userIndex, cancellationToken) ?? new User();
            user.Index = userIndex;
            user.Identity = userIdentity;

            // Select a random question from a random user
            var random = new Random((int)DateTime.Now.Ticks);
            var userCount = await GetUsersCountAsync(cancellationToken);
            var randomUser = user;
            while (randomUser == user)
            {
                var randomUserId = User.IndexFor(random.Next(userCount));
                randomUser = await Bucket.GetAsync<User>(randomUserId, cancellationToken);
            }
            var questions = randomUser.Questions.Where(q => q.IsActive).ToArray();
            var question = questions.Length > 0 ? questions[random.Next(questions.Length)] : null;

            // Set the question as current question for the current user
            if (question != null)
            {
                user.CurrentQuestionId = question.Id;
                user.CurrentQuestionUserIndex = user.Index;
                await SaveUserAsync(user, cancellationToken);

                // Send the random question to the current user
                await _sender.SendMessageAsync(question, envelope.From, cancellationToken);
            }
            else
            {
                // Send fallback message
                await _sender.SendMessageAsync(_settings.Congratulations, envelope.From, cancellationToken);
            }
        }
    }
}
