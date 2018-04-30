using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord.Commands;

using Newtonsoft.Json;

using AllRound.Resources.Settings;
using AllRound.Resources.Datatypes;

namespace AllRound.Core.Moderation
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("reload"), Summary("Reload the settings.json file while the bot is running")]
        public async Task Reload()
        {
            //Checks
            if (Context.User.Id != ESettings.Owner)
            {
                await Context.Channel.SendMessageAsync(":x: You are not the owner. Ask the bot owner to execute this command!");
                return;
            }
            
            string SettingsLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.0", @"Data\Settings.json");
            if (!File.Exists(SettingsLocation))
            {
                await Context.Channel.SendMessageAsync(":x: The file is not found in the given location. The expected location can be found in the log!");
                Console.WriteLine(SettingsLocation);
                return;
            }

            //Excution
            string JSON = "";
            using (var Stream = new FileStream(SettingsLocation, FileMode.Open, FileAccess.Read))
            using (var ReadSettings = new StreamReader(Stream))
            {
                JSON = ReadSettings.ReadToEnd();
            }

            Setting Settings = JsonConvert.DeserializeObject<Setting>(JSON);

            //Save
            ESettings.Banned = Settings.banned;
            ESettings.Log = Settings.log;
            ESettings.Owner = Settings.owner;
            ESettings.Token = Settings.token;
            ESettings.Version = Settings.version;

            await Context.Channel.SendMessageAsync(":white_check_mark: All the settings were updated successfully!");
        }
    }
}
