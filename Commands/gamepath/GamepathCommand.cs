using System;
using System.Collections.Generic;
using System.CommandLine;
using Nitrox.CLI.Helper;

namespace Nitrox.CLI.Commands;

public class GamepathCommand : NitroxCommand
{
    public override string Name => "gamepath";

    public override string Description => "Commands to manage Nitrox game paths";

    public override List<Argument> Arguments => new List<Argument>();
    public override List<NitroxCommand> Subcommands => new List<NitroxCommand>()
    {
        new GamepathSetCommand(),
        new GamepathGetCommand()
    };
    public override bool? CategorizingCommand => true;

    public override void CommandExecuted() {}
}