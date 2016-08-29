using FootballData;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InBytesBot
{
    [Serializable]
    public class SimpleDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(ActivityReceivedAsync);
        }

        private async Task ActivityReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            Championships champs = new Championships();
            if (activity.Text.Contains("how many teams"))
            {

                await context.PostAsync($"There are { champs.GetTeamCount() } teams.");
            }
            else if (activity.Text.StartsWith("who") || activity.Text.StartsWith("which team"))
            { 
                if (activity.Text.Contains("best") || activity.Text.Contains("highest") || activity.Text.Contains("greatest"))
                {
                    await context.PostAsync($"The highest ranked team is { champs.GetHighestRatedTeam() }");
                }
                else if (activity.Text.Contains("worst") || activity.Text.Contains("lowest"))
                {
                    await context.PostAsync($"The lowest ranked team is { champs.GetLowestRatedTeam() }");
                }
            }
            else
            {
                await context.PostAsync("Sorry but my responses are limited. Please ask the right questsions.");
            }
            context.Wait(ActivityReceivedAsync);
        }
    }
}
