using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.CommandLine;
using System.Reflection;
using System.CommandLine.Invocation;
using System.Linq;
using System.Runtime.Versioning;

namespace Nitrox.CLI.Helper;

/// <summary>
/// Wrapper around System.CommandLine.Command
/// </summary>
public abstract class NitroxCommand {
    /// <summary>
    /// The name of the command.
    /// </summary>
    public abstract string Name { get; }
    /// <summary>
    /// The description of the command.
    /// </summary>
    public abstract string Description { get; }
    /// <summary>
    /// Arguments to add to the command.
    /// </summary>
    public virtual List<Argument>? Arguments { get; }
    private Dictionary<string, Argument> argNameToArg = new Dictionary<string, Argument>();
    public virtual List<Option>? Options { get; }
    private Dictionary<string, Option> optionNameToOption = new Dictionary<string, Option>();
    /// <summary>
    /// Subcommands of this command.
    /// </summary>
    public virtual List<NitroxCommand>? Subcommands { get;  }
    /// <summary>
    /// Is this command a categorizing command <br/>
    /// Set to true if you need users to add a subcommand  <br/>
    /// If this is set to true, CommandExecuted will not fire.  <br/>
    /// The default is false.
    /// </summary>
    public virtual bool? CategorizingCommand { get; }
    /// <summary>
    /// The System.CommandLine command for this NitroxCommand
    /// </summary>
    public Command UnderlyingCommand;

    public NitroxCommand() {
        this.UnderlyingCommand = new Command(this.Name, this.Description);

        if (this.Arguments != null)
        { 
            foreach (var item in this.Arguments)
            {
                this.UnderlyingCommand.AddArgument(item);
                this.argNameToArg.Add(item.Name, item);
            }
        }

        if (this.Options != null)
        { 
            foreach (var item in this.Options)
            {
                this.UnderlyingCommand.AddOption(item);
                this.optionNameToOption.Add(item.Name, item);
            }
        }
        
        if (this.Subcommands != null) {
            foreach (var item in this.Subcommands)
            {
                this.UnderlyingCommand.AddCommand(item.UnderlyingCommand);
            }
        }

        // ensure this is always set
        if (this.CategorizingCommand == null) {
            this.CategorizingCommand = false;
        }
        
        if ((bool)this.CategorizingCommand) {
            this.UnderlyingCommand.SetHandler(() =>
            {
                CommandError("Missing subcommand");
            });
        } else {
            this.UnderlyingCommand.SetHandler(internalHandler);
        }
        
    }
    private InvocationContext invocationContext;
    private void internalHandler(InvocationContext ctx) {
        invocationContext = ctx;
        this.CommandExecuted();
    }
    /// <summary>
    /// The code that will be executed when this command is ran.
    /// </summary>
    public virtual void CommandExecuted() {
        throw new NotImplementedException("This command has not been implemented.");
    }
    /// <summary>
    /// Gets the value to an argument
    /// </summary>
    /// <typeparam name="T">The type of data to return</typeparam>
    /// <param name="name">The name of the argument</param>
    /// <returns></returns>
    
    public T GetArgument<T>(string name) {
        argNameToArg.TryGetValue(name, out Argument? arg);
        if (arg == null) {
            throw new Exception($"Argument {name} does not exist or wasn't defined. ");
        }
        var argVal = invocationContext.ParseResult.GetValueForArgument(arg);
        if (typeof(T).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomParsing<,>)))
        {
            // we have to hope that the target class implements the interface properly
            // GetInterfaceMap does not work with generic interfaces
            var parseMethod = typeof(T).GetMethod("Parse");
            return (T)parseMethod.Invoke(null, new object[] { argVal });
        }
        return (T)argVal;
    }
    /// <summary>
    /// Gets the value to an option
    /// </summary>
    /// <typeparam name="T">The type of data to return</typeparam>
    /// <param name="name">The name of the option</param>
    /// <returns></returns>
    public T GetOption<T>(string name) {
        optionNameToOption.TryGetValue(name, out Option? option);
        if (option == null) {
            Console.WriteLine(optionNameToOption.Values.ToList().First().Name);
            throw new Exception($"Option {name} does not exist or wasn't defined. ");
        }
        
        return invocationContext.ParseResult.GetValueForOption<T>((Option<T>)option);
    }
    /// <summary>
    /// Print an error message, help and then quit the application.
    /// </summary>
    /// <param name="error">The error message to print</param>
    public void CommandError(string error) {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();

        this.UnderlyingCommand.Invoke("--help");

        Environment.Exit(1);
    }
}


