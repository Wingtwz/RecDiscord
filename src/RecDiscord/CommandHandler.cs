using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace RecDiscord
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService cmdSrv;

        public async Task InstallAsync(DiscordSocketClient c)
        {
            client = c;
            cmdSrv = new CommandService();

            await cmdSrv.AddModulesAsync(Assembly.GetEntryAssembly());

            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var message = s as SocketUserMessage;

            if (message == null)
                return;

            var context = new SocketCommandContext(client, message);
            int argPos = 0;

            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var result = await cmdSrv.ExecuteAsync(context, argPos);

                //if (!result.IsSuccess)
                //    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}