using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nitrox.CLI.Commands;
using Nitrox.CLI.Helper;

namespace Nitrox.CLI;

/// <summary>
/// Nitrox.CLI is a CLI for managing Nitrox. It allows you to:
/// - Set game paths
/// - Start SN (maybe BZ in the future?) clients
/// - Manage Nitrox servers running on the local machine (maybe remote access in the future)
/// - Add SN with Nitrox to Steam (Steam deck users will enjoy this)
/// </summary>
class Program
{
    public static RootCommand RootCommand;
    public static async Task<int> Main(string[] args)
    {
        RootCommand = new RootCommand("Commandline interface for managing Nitrox servers and clients");
        DefineAllCommands();

        return await RootCommand.InvokeAsync(args);
    }
    public static void DefineAllCommands() {
        AddCommand(new GamepathCommand());
        AddCommand(new GameCommand());
    }
    public static void AddCommand(NitroxCommand command) {
        RootCommand.AddCommand(command.UnderlyingCommand);
    }
}