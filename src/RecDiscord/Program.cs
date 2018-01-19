using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RecDiscord
{
    public class Program
    {
        private DiscordSocketClient client;
        private CommandHandler commands;

        public static AppSettings Settings;

        public static void Main(string[] args)
                    => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            ReadSettings();

            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            });

            client.Log += (l) => Console.Out.WriteLineAsync(l.ToString());
            
            await client.LoginAsync(TokenType.Bot, Settings.Token);
            await client.StartAsync();

            commands = new CommandHandler();
            await commands.InstallAsync(client);

            await Task.Delay(-1);
        }

        public void ReadSettings()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("settings.json");
                var json = sr.ReadToEnd();
                Settings = JsonConvert.DeserializeObject<AppSettings>(json);
            }
            catch (Exception ex) when (ex is IOException || ex is JsonReaderException)
            {
                // TODO: Create default settings (w/o token)
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        public static void WriteSettings()
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter("settings.json");
                var json = JsonConvert.SerializeObject(Settings);
                sw.Write(json);
            }
            catch (IOException) { }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}
