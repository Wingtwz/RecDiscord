using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace RecDiscord.Modules
{
    [Name("Soporte")]
    public class Help : ModuleBase<SocketCommandContext>
    {
        private CommandService service;

        public Help(CommandService service)
        {
            this.service = service;
        }

        [Command("help"), Summary("Muestra el listado de comandos")]
        public async Task HelpAsync()
        {
            string prefix = "!";
            var builder = new EmbedBuilder()
            {
                Color = new Color(50, 255, 50),
                Description = Format.Bold("Comandos disponibles:")
            };

            foreach (var module in service.Modules)
            {
                string description = string.Empty;

                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);

                    if (result.IsSuccess)
                    {
                        var remarks = string.IsNullOrWhiteSpace(cmd.Remarks) ? string.Empty : " " + cmd.Remarks;

                        description += $"{prefix}{cmd.Name}{remarks} - {cmd.Summary}\n";
                    }
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync(string.Empty, false, builder.Build());
        }

        /*[Command("help"), Summary("Muestra la ayuda de un comando")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            string prefix = "!";
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Remarks: {cmd.Remarks}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }*/
    }
}