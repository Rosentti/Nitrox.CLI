using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace Nitrox.CLI.Helper;
public class OptionFactory<T> {
    private Option<T> opt;
    
    internal OptionFactory(string name) {
        opt = new Option<T>($"--{name}");
    }
    public OptionFactory<T> SetDescription(string description) {
        this.opt.Description = description;
        return this;
    }
    public OptionFactory<T> SetOptional(bool optional) {
        if (optional == false) {
            opt.Arity = ArgumentArity.OneOrMore;
        } else {
            opt.Arity = ArgumentArity.ZeroOrMore;
        }
        return this;
    }
    public OptionFactory<T> SetDefaultValue(T defaultValue) {
        opt.SetDefaultValue(defaultValue);
        return this;
    }
    public OptionFactory<T> AddAlias(string alias) {
        opt.AddAlias(alias);
        return this;
    }
    public Option<T> Build() {
        return this.opt;
    }
    
}
public class OptionFactory {
    public static OptionFactory<T> Create<T>(string name) {
        return new OptionFactory<T>(name);
    }
}
