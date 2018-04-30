using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AllRound.Resources.Database;

namespace AllRound.Core.Currency
{
    public class Stones : ModuleBase<SocketCommandContext>
    {
        [Group("stone"), Alias("stones"), Summary("Group to manage stuff to do with stones")]
        public class StonesGroup : ModuleBase<SocketCommandContext>
        {
            [Command(""), Alias("me", "my"), Summary("Shows all your current stones")]
            public async Task Me(IUser User = null)
            {
                if (User == null)
                    await Context.Channel.SendMessageAsync($"{Context.User.Username}, you have {Data.Data.GetStones(Context.User.Id)} stones!");
                else
                    await Context.Channel.SendMessageAsync($"{User.Username}, you have {Data.Data.GetStones(User.Id)} stones!");
            }

            [Command("give"), Alias("gift"), Summary("Used to give people stones")]
            public async Task Give(IUser User = null, int Amount = 0)
            {
                //stones give @Sjustein 1000
                //group  cmd   user     amount

                //Checks
                //Does the user have permission?
                //Does the user have enough stones?
                if (User == null)
                {
                    //The executer has not mentioned a user
                    await Context.Channel.SendMessageAsync(":x: You didn't mention a user to give the stones to! Please use this syntax: a!stones give **<@user>** <amount>");
                    return;
                }

                //At this point, we made sure that a user has been pinged
                if (User.IsBot)
                {
                    await Context.Channel.SendMessageAsync($":x: Bots can't use this bot, so you can't give stones to a bot!");
                    return;
                }

                //At this point, we made sure that a user has been pinged AND that the user is not a bot
                if (Amount == 0)
                {
                    await Context.Channel.SendMessageAsync($":x: You need to specify a valid amount of stones that I need to give to {User.Username}! Please use this syntax: a!stones give {User.Mention} **<amount>**");
                    return;
                }

                //At this point, we made sure that a user has been pinged, that the user is not a bot AND that there is a valid amount of coins
                SocketGuildUser User1 = Context.User as SocketGuildUser;
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync($":x: You don't have admimistrator permissions in this discord server! Ask a moderator or the owner to execute this command!");
                    return;
                }

                //Execution
                //Calculations (games)
                //Telling the user what he has gotten
                await Context.Channel.SendMessageAsync($":tada: {User.Mention} you have received **{Amount}** stones from {Context.User.Username}!");


                //Saving the data
                //Save the data to the database
                //Save a file
                await Data.Data.SaveStones(User.Id, Amount);
            }

            [Command("reset"), Summary("resets the user's entire progress")]
            public async Task Reset(IUser User = null)
            {
                //Checks
                if (User == null)
                {
                    await Context.Channel.SendMessageAsync($":x: You need to tell me which user you want to reset the stones of! For example: a!stones reset {Context.User.Mention}");
                    return;
                }

                if (User.IsBot)
                {
                    await Context.Channel.SendMessageAsync(":x: Bots can't use this bot, so you also can't reset the progress of bots! :robot:");
                    return;
                }

                SocketGuildUser User1 = Context.User as SocketGuildUser;
                if (!User1.GuildPermissions.Administrator)
                {
                    await Context.Channel.SendMessageAsync($":x: You don't have admimistrator permissions in this discord server! Ask a moderator or the owner to execute this command!");
                    return;
                }

                //Execution
                await Context.Channel.SendMessageAsync($":skull: {User.Mention}, you have been reset by {Context.User.Username}! This means you have lost all your stones!");

                //Saving the data
                using (var DbContext = new SqliteDbContext())
                {
                    DbContext.Stones.RemoveRange(DbContext.Stones.Where(x => x.UserId == User.Id));
                    await DbContext.SaveChangesAsync();
                }
            }
        }
    }
}
