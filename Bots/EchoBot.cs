// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using InsuranceBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InsuranceBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly EmbeddingGenerator _embeddingGenerator;
        public EchoBot(EmbeddingGenerator embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {            
            var userQuery = turnContext.Activity.Text;

            var similarQuestionAnswer = await _embeddingGenerator.FindMostSimilarQuestionAsync(userQuery);

            string replyText;
            if (similarQuestionAnswer != null)
            {
                replyText = $"Answer: {similarQuestionAnswer.Answer}";
            }
            else
            {
                replyText = "Sorry, I couldn't find this answers.";
            }
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Welcome!";

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText), cancellationToken);
                }
            }
        }
    }
}
