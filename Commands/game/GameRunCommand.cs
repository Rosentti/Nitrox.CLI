using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using Nitrox.CLI.Helper;

namespace Nitrox.CLI.Commands;

public class GameRunCommand : NitroxCommand
{
    public override string Name => "run";

    public override string Description => "Run a game by name";

    public override List<Argument> Arguments => new List<Argument>() 
    {
        new Argument<string>("game", "Game to launch"),
    };
    public override List<Option> Options => new List<Option>() {
        OptionFactory.Create<bool>("skip-patching")
            .AddAlias("-s")
            .SetDescription("Should patching be skipped? (Note: Nitrox might not work)")
            .SetDefaultValue(false)
            .SetOptional(true)
            .Build()
    };

    public override void CommandExecuted() {
        var skip_patching = GetOption<bool>("skip-patching");
        var game = GetArgument<GameName>("game");
        GameStarting.StartMultiplayer(game.GameInfo);
    }
}