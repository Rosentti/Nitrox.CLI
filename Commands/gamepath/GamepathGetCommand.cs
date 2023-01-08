using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text.Json;
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
        var game = GetArgument<string>("game");
        switch (game) {
            case "subnautica":
            case "sn":
                Console.WriteLine($"Preferred game path: {NitroxUser.PreferredGamePath}");
                Console.WriteLine($"Actual (used) game path: {NitroxUser.GamePath}");
                break;
            case "subnauticabelowzero":
            case "belowzero":
            case "bz":
                CommandError("Subnautica: Below Zero is not supported in Nitrox yet.");
                break;
            default:
                CommandError($"I didn't understand {game}");
                break;
        }
    }
}