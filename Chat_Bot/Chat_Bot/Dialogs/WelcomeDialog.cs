using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Chat_Bot.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(PerformActionAsync);
            return Task.CompletedTask;
        }

        private async Task PerformActionAsync (IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.Equals("Hello"))
            {
                await context.PostAsync("Welcome to the PC Builder Chatbot assistance");
                context.Call(FormDialog.FromForm(Dialogs.PCFields.GetForm), MakeDialogComplete);
                await context.PostAsync("Place your order");
            }

            //else if (activity.Text.Equals("How are you"))
            //    await context.PostAsync("I am fine as always.");
            //else
            //    await context.PostAsync("I am unable to understand");

        }

        private IDialog<Dialogs.PCFields> MakeDialog(IDialogContext context, IAwaitable<object> result)
        {

            return Chain.From(() => FormDialog.FromForm(Dialogs.PCFields.GetForm));
        }

        private async Task MakeDialogComplete(IDialogContext context, IAwaitable<PCFields> result)
        {
            var dialogResult = await result;
            context.Done(dialogResult);
            //do something if result is true
        }
    }
}