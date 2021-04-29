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

    [Serializable] // converts data into stream of bytes
    [LuisModel("a6262e6f-8642-47b3-94e2-230fd01d09b7", "62a4660245174573899ae7b55df2a286", domain: "westeurope.api.cognitive.microsoft.com", Staging = false)] // Retrives the Luis model created
    public class Luis: LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")] // None intent indentified
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I did not understood what you said."); //Output for the user
            context.Wait(MessageReceived);
        }

        [LuisIntent("Welcome")] // Welcome intent indentified
        public async Task Greeting (IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello! Welcome to the PC Builder Chatbot assistance, feel free to ask any questions about a specific build such as gaming, work etc. Or feel free to start building your required specification"); //Output for the user
            context.Wait(MessageReceived);
        }

        [LuisIntent("Order")] // Order intent indentified
        public async Task Order(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("You would like to to choose a PC specification build"); //Output for the user
            context.Call(FormDialog.FromForm(Dialogs.PCFields.GetForm), MakeDialogComplete); //calls the PC form flow
            await context.PostAsync("Please press any key and press enter to bring up the PC Builder");
        }

        [LuisIntent("Gaming")] // Gaming intent indentified
        public async Task GamingHelp(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync("The suggested build for a gaming build is a processor above I5+, a large RAM and also a top end graphics card such as an RTX3070"); //Output for the user
            context.Wait(MessageReceived);
        }

        [LuisIntent("Work")] // Work intent indentified
        public async Task WorkHelp(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("There are no specific requirements for a work computer, you could have the most cheapest CPU and lowest RAM but depending on the type of work a higher processing power may be needed e.g coding."); //Output for the user
            context.Wait(MessageReceived);
        }

        [LuisIntent("LowCost")] // LowCost intent indentified
        public async Task Cheap(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("For a very cheap and low budget build, I would recommend a I3 processor, 4 GB RAM, 1 TB hard drive and a basic graphics card."); //Output for the user
            context.Wait(MessageReceived);
        }

        [LuisIntent("CPUhelp")] // LowCost intent indentified
        public async Task CPU(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("The more expensive CPUs such as I7 or I9 have more processing power which means more clocks per second and thus faster proccessing for aspects such as multi-tasking."); //Output for the user
            context.Wait(MessageReceived);
        }


        private IDialog<Dialogs.PCFields> MakeDialog(IDialogContext context, IAwaitable<object> result) //function to call PCfields
        {

            return Chain.From(() => FormDialog.FromForm(Dialogs.PCFields.GetForm)); //call the form flow
        }

        private async Task MakeDialogComplete(IDialogContext context, IAwaitable<PCFields> result)
        {
            var dialogResult = await result;
            context.Done(dialogResult);
            //do something if result is true
        }
    }
}
