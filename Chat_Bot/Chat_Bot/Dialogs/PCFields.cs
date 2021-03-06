using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;

namespace Chat_Bot.Dialogs
{
    public enum PCManufactor // Public enums were created to store the values of each components
    {
        Acer, Lenovo, Asus, Microsoft, Samsung
    }

    public enum OperatingSystem // Public enums were created to store the values of each components
    {
        Windows10, Windows8, Windows7, Linux, UNIX
    }

    public enum RamSize // Public enums were created to store the values of each components
    {
        GB4, GB8, GB16, GB32
    }

    public enum CPU// Public enums were created to store the values of each components
    {
        I3, I5, I7, I9, AMD5, AMD9
    }
    public enum GraphicsCard// Public enums were created to store the values of each components
    {
        GTX1660Super, AMD5600XT, RTX3070, RRTX2080TI
    }
    public enum StorageType// Public enums were created to store the values of each components
    {
        HDD1TB, HDD5TB, SSD256GB, SSD1TB
    }

    [Serializable]
    public class Costs
    {
        public decimal Cost { get; set; }
        public decimal Cost2 { get; set; }
        
        public decimal Cost3 { get; set; }

        public decimal Cost4 { get; set; }

        public decimal Total { get; set; }
    }

    [Serializable]
    public class PCFields
    {
        public Costs costs = new Costs();


        [Describe(description: "Company", title: "PC Brand", subTitle: "These are the brands of PCs we only offer")] // Gives information about each components
        public PCManufactor? PCManufactor; // Assigns the enum to a variable to build the form in
        [Describe(description: "Operating Systems", subTitle: "This comes free with the PC")] // Gives information about each components
        public OperatingSystem? OperatingSystem; // Assigns the enum to a variable to build the form in
        [Describe(description: "RAM Size", subTitle: "How big of a RAM size would you like? The bigger more tasks can be perfomed.")] // Gives information about each components
        public RamSize? RAM; // Assigns the enum to a variable to build the form in
        [Describe(description: "CPU", subTitle: "What CPU would you like?")] // Gives information about each components
        public CPU? Core; // Assigns the enum to a variable to build the form in
        [Describe(description: "GPU", subTitle: "What graphics card would you like?")] // Gives information about each components
        public GraphicsCard? GPU; // Assigns the enum to a variable to build the form in
        [Describe(description: "Storage Type", subTitle: "What storage type would you like? SSD is faster but more expensive than HDD")] // Gives information about each components
        public StorageType? Storage; // Assigns the enum to a variable to build the form in


        [Describe(description: "your First Name:")] // Input for first name
        public string firstName;

        [Describe(description: "your last Name:")] // input for last name name
        public string secondName;

        [Describe(description: "your Email Address:")]//input for the email address
        [Pattern(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")] // a pattern regex that only accepts a certain format of email address
        public string email;
        public static IForm<PCFields> GetForm()
        {
            Price[] prices = null; // make prices list empty
            using (var httpClient = new HttpClient()) // create an http client
            {
                using (var response = httpClient.GetAsync("https://localhost:44391/api/values")) // retrieve the local host prices values 
                {
                    string apiResponse = response.Result.Content.ReadAsStringAsync().Result; //read the json file as a string
                    prices = JsonConvert.DeserializeObject<Price[]>(apiResponse); // Use a JSON converter and retrieve the file into the prices list
                }
            }
            //Price[] prices = JsonConvert.DeserializeObject<Price[]>(File.ReadAllText(@"C:\Users\fraiz\source\repos\Chat_Bot\Chat_Bot\results.json"));

            OnCompletionAsyncDelegate<PCFields> onFormCompletion = async (context, state) =>
            {

                await context.PostAsync($"Final Cost is £{state.costs.Total}");//final cost defined
                await context.PostAsync("We have your PC Build configuration ready to be built! An email should be sent confirming this."); //final message
                var smtpClient = new SmtpClient("smtp.office365.com") //set up smtp client
                {
                    Port = 587,
                    Credentials = new NetworkCredential("rg015981@student.reading.ac.uk", "Graizer1"), //credentials to send the email from
                    EnableSsl = true,
                };

                smtpClient.Send("rg015981@student.reading.ac.uk", state.email, "PC Build", $" Dear {state.firstName} {state.secondName},\n We have your PC Build configuration ready to be built!\n The specification required is as below:\n Manufactor: {state.PCManufactor}\n Operating System: {state.OperatingSystem}\n RAM Size: {state.RAM}\n CPU: {state.Core}\n Graphics Card: {state.GPU}\n Storage Size: {state.Storage}\n Total Cost: £{state.costs.Total}\nThanks for using my service!");
            };

            

            

            return new FormBuilder<PCFields>()
                .Message("Please choose your required PC build specification:")
                .Field(nameof(firstName))
                .Field(nameof(secondName))
                .Field(nameof(PCManufactor))
                .Field(nameof(OperatingSystem))
                .Field(nameof(RAM))
                .Confirm(async (state) =>
                {
                    decimal cost = 0;
                    switch (state.RAM)
                    {
                        case RamSize.GB4: cost = Convert.ToDecimal(prices[0].price); break;
                        case RamSize.GB8: cost = Convert.ToDecimal(prices[1].price); break;
                        case RamSize.GB16: cost = Convert.ToDecimal(prices[2].price); break;
                        case RamSize.GB32: cost = Convert.ToDecimal(prices[3].price); break;
                    }

                    state.costs.Cost = cost;
                    return new PromptAttribute($"Minimum Cost for this RAM will be £{cost}. Is this good with you? ");

                })
                .Field(nameof(Core))
                .Confirm(async (state) =>
                {
                    decimal cost2 = 0;
                    switch (state.Core)
                    {
                        case CPU.I3: cost2 = Convert.ToDecimal(prices[4].price); break;
                        case CPU.I5: cost2 = Convert.ToDecimal(prices[5].price); break;
                        case CPU.I7: cost2 = Convert.ToDecimal(prices[6].price); break;
                        case CPU.I9: cost2 = Convert.ToDecimal(prices[7].price); break;
                        case CPU.AMD5: cost2 = Convert.ToDecimal(prices[8].price); break;
                        case CPU.AMD9: cost2 = Convert.ToDecimal(prices[9].price); break;

                    }
                    state.costs.Cost2 = cost2;
                    return new PromptAttribute($"Minimum Cost for this processor will be £{cost2}. Is this good with you? ");
                })
                .Field(nameof(GPU))
                .Confirm(async (state) =>
                {
                    decimal cost3 = 0;
                    switch (state.GPU)
                    {
                        case GraphicsCard.AMD5600XT: cost3 = Convert.ToDecimal(prices[10].price); break;
                        case GraphicsCard.GTX1660Super: cost3 = Convert.ToDecimal(prices[11].price); break;
                        case GraphicsCard.RRTX2080TI: cost3 = Convert.ToDecimal(prices[12].price); break;
                        case GraphicsCard.RTX3070: cost3 = Convert.ToDecimal(prices[13].price); break; ;
                    }
                    state.costs.Cost3 = cost3;

                    return new PromptAttribute($"Minimum Cost for this Graphics card will be £{cost3}. Is this good with you? ");
                })
                .Field(nameof(Storage))
                .Confirm(async (state) =>
                {
                    decimal cost4 = 0;
                    switch (state.Storage)
                    {
                        case StorageType.HDD1TB: cost4 = Convert.ToDecimal(prices[14].price); break;
                        case StorageType.HDD5TB: cost4 = Convert.ToDecimal(prices[15].price); break;
                        case StorageType.SSD256GB: cost4 = Convert.ToDecimal(prices[16].price); break;
                        case StorageType.SSD1TB: cost4 = Convert.ToDecimal(prices[17].price); break;

                    }
                    state.costs.Cost4 = cost4;
                    state.costs.Total = state.costs.Cost + state.costs.Cost2 + state.costs.Cost3 + state.costs.Cost4; 
                    return new PromptAttribute($"Minimum Cost for this Storage will be £{cost4}. Is this good with you? ");
                })
                .Field(nameof(email))
                .OnCompletion(onFormCompletion)
                .Build();
        }

    }
}