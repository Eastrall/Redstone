using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Hosting;
using Redstone.CLI.Registries;
using System.Threading.Tasks;

namespace Redstone.CLI
{
    [Command(UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    [Subcommand(typeof(RegistriesCommand))]
    public class Program
    {
        static Task<int> Main(string[] args)
            => new HostBuilder().RunCommandLineApplicationAsync<Program>(args);

        public void OnExecute(CommandLineApplication application)
        {
            application.ShowHelp();
        }
    }
}
