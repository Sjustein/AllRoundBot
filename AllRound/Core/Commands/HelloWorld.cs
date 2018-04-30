using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;
using System.Reflection;
using System.IO;

namespace AllRound.Core.Commands
{
    public class HelloWorld : ModuleBase<SocketCommandContext>
    {
        [Command("hello"), Alias("helloworld", "world"), Summary("Hello world command")]
        public async Task Sjustein()
        {
            await Context.Channel.SendMessageAsync("Hello worldtestset");
        }

        [Command("embed"), Summary("Embed test command")]
        public async Task Embed([Remainder]string Input = "None")
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor("Test embed", Context.User.GetAvatarUrl());
            Embed.WithColor(40, 200, 150);
            Embed.WithFooter("The footer of the embed", Context.Guild.Owner.GetAvatarUrl());
            Embed.WithDescription("This is a **dummy** description, with a cool link.\n" +
                              "[This is my favourite website](https://www.sjustein.com/)");
            Embed.AddInlineField("User input:", Input);

            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
}