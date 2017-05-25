using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace RecDiscord
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            string token;

            client.Log += Log;

            client.MessageReceived += MessageReceived;

            using (var stream = new FileStream("token", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    token = reader.ReadToEnd();
                }
            }

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("pong!");
            }
            else
            {
                var data = message.Content.Split(' ');

                switch (data[0])
                {
                    case "!echo":
                        await message.Channel.SendMessageAsync($"{message.Author.Username} ha dicho: " +
                            $"{message.Content.Substring(6)}");
                        break;

                    case "!recon":
                        await JoinChannel(message);
                        break;
                }

            }
        }

        [Command("join")]
        public async Task JoinChannel(SocketMessage message, IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument.");
                return;
            }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();
        }
    }
}