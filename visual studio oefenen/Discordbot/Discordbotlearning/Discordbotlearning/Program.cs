using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Discordbotlearning
{
    internal class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            string token = "NjkzNDkyMjQzNjc1MzQ5MTI0.Xn93AQ.-dbP7V-doDIiiMDEbYTZ9rf4SbU";

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            ulong id = 693492243675349124;
            var chnl = _client.GetChannel(id) as IMessageChannel;
            await chnl.SendMessageAsync("YO YO YO");

            // Block this task until the program is closed.

            await Task.Delay(-1);
        }

        public async Task Announce(DiscordSocketClient _client) // 1
        {
            ulong id = 693492243675349124; // 3
            var chnl = _client.GetChannel(id) as IMessageChannel; // 4
            await chnl.SendMessageAsync("Announcement!"); // 5
        }
    }
}