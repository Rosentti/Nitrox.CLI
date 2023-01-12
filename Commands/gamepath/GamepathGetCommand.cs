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
        var game = GetArgument<GameName>("game");
        var preferredExists = !string.IsNullOrEmpty(NitroxUser.PreferredGamePath);
        Console.WriteLine($"Preferred {(preferredExists ? "(used) " : "")}game path: {NitroxUser.PreferredGamePath}");
        Console.WriteLine($"Discovered {(!preferredExists ? "(used) " : "")}game path: {NitroxUser.GamePath}");

    }
}