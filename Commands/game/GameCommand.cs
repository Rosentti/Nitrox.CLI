using System;
using System.Collections.Generic;
using System.CommandLine;
using Nitrox.CLI.Helper;

namespace Nitrox.CLI.Commands;

public class GameCommand : NitroxCommand
{
    public override string Name => "game";

    public override string Description => "Commands to manage and launch games supported by Nitrox.";

    public override List<Argument> Arguments => new List<Argument>();
    public override List<NitroxCommand> Subcommands => new List<NitroxCommand>()
    {
        new GameRunCommand()
    };
    public override bool? CategorizingCommand => true;

    public override void CommandExecuted() {}
}