using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text.Json;
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
        var game = GetArgument<string>("game");
        var path = GetArgument<string>("path");
        switch (game) {
            case "subnautica":
            case "sn":
                NitroxUser.GamePath = path;
                NitroxUser.PreferredGamePath = path;
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