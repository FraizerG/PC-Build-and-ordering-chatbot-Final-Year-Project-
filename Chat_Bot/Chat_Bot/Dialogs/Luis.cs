using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Chat_Bot.Dialogs
{

    [Serializable]
    [LuisModel("a6262e6f-8642-47b3-94e2-230fd01d09b7", "62a4660245174573899ae7b55df2a286", domain: "westeurope.api.cognitive.microsoft.com", Staging = false)]
    public class Luis: LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Default Intent");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Welcome")]
        public async Task Greeting (IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello Welcome to the PC Builder Chatbot assistance");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Order")]
        public async Task Order(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("You would like to to choose a PC specification build");
            context.Call(FormDialog.FromForm(Dialogs.PCFields.GetForm), MakeDialogComplete);
            await context.PostAsync("Please press any key and press enter to bring up the PC Builder");
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
