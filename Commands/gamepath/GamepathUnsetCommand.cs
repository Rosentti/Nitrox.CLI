using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text.Json;
using Nitrox.CLI.Helper;
using NitroxModel;
using NitroxModel.Helper;

namespace Nitrox.CLI.Commands;

public class GamepathUnsetCommand : NitroxCommand
{
    public override string Name => "unset";

    public override string Description => "Remove a set game path.";

    public override List<Argument> Arguments => new List<Argument>() {
        new Argument<string>("game", "Game to set the game path for")
    };

    public override void CommandExecuted() {
        var game = GetArgument<GameName>("game");
        NitroxUser.PreferredGamePath = null;
        Console.WriteLine($"Unset {game.GameInfo.FullName}'s path.");
    }
}