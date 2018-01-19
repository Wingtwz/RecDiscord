using Discord.Commands;
using System.Threading.Tasks;

namespace RecDiscord.Modules
{
    [Name("Pruebas")]
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("ping"), Summary("Devuelve un pong!")]
        public async Task PongAsync()
        {
            await ReplyAsync($"pong!");
        }
    }
}
