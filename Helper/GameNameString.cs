using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Runtime.Versioning;
using Nitrox.CLI.Helper;
using NitroxModel;
public class GameName : ICustomParsing<GameName, string> { 
    public string Name { get; set; }
    public GameInfo GameInfo;
    public GameName(string name) {
        this.Name = ConvertToUniversal(name);
        switch (this.Name)
        {
            case "SN":
                this.GameInfo = GameInfo.Subnautica;
                break;
            case "BZ":
                this.GameInfo = GameInfo.SubnauticaBelowZero;
                break;
            default:
                throw new Exception($"Failed to create GameInfo with {this.Name}");
        }
    }
    public static bool IsValid(string name) {
        try {
            ConvertToUniversal(name);
            return true;
        } catch (Exception)
        {
            return false;
        }
    }
    public static GameName Parse(string name) {
        return new GameName(ConvertToUniversal(name));
    }
    /// <summary>
    /// Returns "SN" or "BZ" depending on the input.
    /// </summary>
    /// <param name="longName"></param>
    /// <returns></returns>
    public static string ConvertToUniversal(string longName) {
        switch (longName.ToLower()) {
            case "sn":
            case "subnautica":
                return "SN";
            case "bz":
            case "subnauticabelowzero":
            case "subnautica: below zero":
            case "belowzero":
                return "BZ";
            default:
                throw new Exception($"Unsupported game {longName}");
        }
    }
}