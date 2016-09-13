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
    public class CardsDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(ActivityReceivedAsync);
        }

        private async Task ActivityReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = activity.CreateReply();
            reply.Attachments = new List<Attachment>();

            if (activity.Text.StartsWith("give me"))
            {
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = "http://intelligentlabs.co.uk/images/about/qualitycontrol.png",
                    ContentType = "image/png",
                    Name = "QualityControl.png"
                });

            }
            else if (activity.Text.StartsWith("who are you"))
            {
                HeroCard hc = new HeroCard()
                {
                    Title = "Who am I?",
                    Subtitle = "I'm the bot!"
                };
                List<CardImage> images = new List<CardImage>();
                CardImage ci = new CardImage("http://intelligentlabs.co.uk/images/about/qualitycontrol.png");
                images.Add(ci);
                hc.Images = images;
                reply.Attachments.Add(hc.ToAttachment());

            }
            else if (activity.Text.StartsWith("help"))
            {
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
            }
            else if (activity.Text.StartsWith("login"))
            {
                List<CardAction> buttons = new List<CardAction>();
                CardAction ca = new CardAction()
                {
                    Title = "Sign In",
                    Type = "signin",
                    Value = "https://www.facebook.com/dialog/oauth?client_id=306586879700216&redirect_uri=https://www.facebook.com/connect/login_success.html"
                };
                buttons.Add(ca);
                SigninCard card = new SigninCard()
                {
                    Text = "You need to sign in to use this",
                    Buttons = buttons
                };
                reply.Attachments.Add(card.ToAttachment());
            }

            await context.PostAsync(reply);
            context.Wait(ActivityReceivedAsync);
        }
    }
}