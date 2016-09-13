using FootballData;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InBytesBot
{
    [LuisModel("e0357190-4455-4506-8764-055b7e04a674", "a91e3e2044be4be99c291c54a153f3a6")]
    [Serializable]
    public class SimpleLUISDialog : LuisDialog<object>
    {
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            List<CardImage> images = new List<CardImage>();
            CardImage ci = new CardImage("http://intelligentlabs.co.uk/images/IntelligentLabs-White-Small.png");
            images.Add(ci);
            CardAction ca = new CardAction()
            {
                Title = "Visit Support",
                Type = "openUrl",
                Value = "http://www.intelligentlabs.co.uk"
            };
            ThumbnailCard tc = new ThumbnailCard()
            {
                Title = "Need help?",
                Subtitle = "Go to our main site support.",
                Images = images,
                Tap = ca
            };
            reply.Attachments.Add(tc.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        public static Championships champs = new Championships();
        [LuisIntent("TeamCount")]
        public async Task GetTeamCount(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"There are { champs.GetTeamCount() } teams.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have no idea what you are talking about.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("TopTeam")]
        public async Task BestTeam(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"{ champs.GetHighestRatedTeam()} is the best team in the championships.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("BottomTeam")]
        public async Task BottomTeam(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"{ champs.GetLowestRatedTeam()} is the worst team in the championships.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("RemoveGoodTeam")]
        public async Task RemoveGoodTeam(IDialogContext context, LuisResult result)
        {
            List<string> goodTeams = champs.GetTopThreeTeams();
            PromptOptions<string> options = new PromptOptions<string>("Select which of the top three teams to remove.",
                "Sorry please try again", "I give up on you", goodTeams, 2);
            PromptDialog.Choice<string>(context, RemoveGoodTeamAsync, options);
        }

        private async Task RemoveGoodTeamAsync(IDialogContext context, IAwaitable<string> result)
        {
            string team = await result;
            if (champs.DoesTeamExist(team))
            {
                champs.RemoveTeam(team);
                await context.PostAsync($"{team} has been removed.");
            }
            else
            {
                await context.PostAsync($"The team {team} was not found");
            }
            context.Wait(MessageReceived);
        }

        string teamName = "";
        [LuisIntent("RemoveTeam")]
        public async Task RemoveTeam(IDialogContext context, LuisResult result)
        {
            
            EntityRecommendation rec;
            if (result.TryFindEntity("TeamName", out rec))
            {
                teamName = rec.Entity;
                if (champs.DoesTeamExist(teamName))
                {
                    PromptDialog.Confirm(context, RemoveTeamAsync, $"Are you sure you want to delete {teamName}?");
                }
                else
                {
                    await context.PostAsync($"The team { teamName } was not found.");
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync("Sorry no team name was found");
                context.Wait(MessageReceived);
            }
        }

        private async Task RemoveTeamAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                champs.RemoveTeam(teamName);
                await context.PostAsync($"{teamName} has been removed.");
            }
            else
            {
                await context.PostAsync($"OK, we wont remove them.");
            }
            context.Wait(MessageReceived);
        }
    }
}