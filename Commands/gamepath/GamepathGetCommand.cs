using System;
using System.Collections.Generic;
using System.CommandLine;
using Nitrox.CLI.Helper;
using NitroxModel;
using NitroxModel.Helper;

namespace Nitrox.CLI.Commands;

public class GamepathGetCommand : NitroxCommand
{
    public override string Name => "get";

    public override string Description => "Get a game's path";

    public override List<Argument> Arguments => new List<Argument>() {
        new Argument<string>("game", "Game to get the game path of")
    };

    public override void CommandExecuted() {
        var game = GetArgument<GameName>("game");
        string? preferred = GamePathUtil.GetPreferredGamePath(game.GameInfo);
        var preferredExists = !string.IsNullOrEmpty(preferred);
        Console.WriteLine($"Preferred {(preferredExists ? "(used) " : "")}game path: {preferred}");

        if (game.Name == "SubnauticaZero") {
            Console.WriteLine($"Discovered {(!preferredExists ? "(used) " : "")}game path: {NitroxUser.GamePath_BZ}");
        } else {
            Console.WriteLine($"Discovered {(!preferredExists ? "(used) " : "")}game path: {NitroxUser.GamePath}");
        }
    }
}