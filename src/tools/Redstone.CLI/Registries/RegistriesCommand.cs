using McMaster.Extensions.CommandLineUtils;

namespace Redstone.CLI.Registries
{
    [Command("registries", 
        Description = "Command that allows manipulation on Minecraft registries reports.", 
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    [Subcommand(typeof(RegistriesConverterCommand))]
    public class RegistriesCommand
    {
        public void OnExecute(CommandLineApplication application)
        {
            application.ShowHelp();
        }
    }
}
