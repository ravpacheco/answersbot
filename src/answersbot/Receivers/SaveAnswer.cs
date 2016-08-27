using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using answersbot.Documents;
using Lime.Messaging.Contents;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Extensions.Bucket;
using Takenet.MessagingHub.Client.Sender;

namespace answersbot.Receivers
{
    internal class SaveAnswer : BaseMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private readonly Settings _settings;

        public SaveAnswer(IMessagingHubSender sender, IBucketExtension bucket, Settings settings) : base(bucket)
        {
            _sender = sender;
            _settings = settings;
        }

        public override async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = new CancellationToken())
        {
            // Identity the current user
            var userIdentity = envelope.From.ToIdentity().ToString();
            var userIndex = await GetUserIndexForNodeAsync(userIdentity, cancellationToken);
            var user = await Bucket.GetAsync<User>(userIndex, cancellationToken);

            // Identify the user owner of the question
            var questionUser = await Bucket.GetAsync<User>(user.CurrentQuestionUserIndex, cancellationToken);

            // Identify the question
            var question = questionUser.Questions.FirstOrDefault(q => q.Id == user.CurrentQuestionId);
            if (question != null)
            {
                // Save the answer into the question
                var answer = new Answer
                {
                    Timestamp = DateTimeOffset.Now,
                    UserIdentity = user.Identity,
                    Text = envelope.Content.ToString()
                };
                question.Answers = (question.Answers ?? new Answer[0]).Concat(new[] {answer}).ToArray();


                // Save the user
                await SaveUserAsync(user, cancellationToken);

                // Send the answer to the owner of the question
                var closureOptions = (Select) _settings.ClosureOptions.ToDocument();
                closureOptions.Text = string.Format(closureOptions.Text, question.Text, answer.Text);
                closureOptions.Options.First().Order = questionUser.GetExternalQuestionIndex(question);
                await _sender.SendMessageAsync(closureOptions, Node.Parse(questionUser.Identity), cancellationToken);
            }
        }
    }
}
