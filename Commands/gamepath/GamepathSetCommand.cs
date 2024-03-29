using System;
using System.Collections.Generic;
using System.CommandLine;
using Nitrox.CLI.Helper;
using NitroxModel;
using NitroxModel.Helper;

namespace Nitrox.CLI.Commands;

public class GamepathSetCommand : NitroxCommand
{
    public override string Name => "set";

    public override string Description => "Set a game's path";

    public override List<Argument> Arguments => new List<Argument>() {
        new Argument<string>("game", "Game to set the game path for"),
        new Argument<string>("path", "The install location of the game")
    };

    public override void CommandExecuted() {
        var game = GetArgument<GameName>("game");
        var path = GetArgument<string>("path");
        NitroxUser.GamePath = path;
        GamePathUtil.SetPreferredGamePath(game.GameInfo, path);
        Console.WriteLine($"Set {game.GameInfo.FullName}'s path to {path}");
    }
}