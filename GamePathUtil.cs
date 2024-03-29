using System;
using System.IO;
using NitroxModel;

namespace Nitrox.CLI;

internal static class GamePathUtil {
    public static string BasePath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static string? GetPreferredGamePath(GameInfo gi) {
        string filePath = Path.Combine(BasePath, $"nitroxpreferredgamepath_{gi.Name}.txt");
        if (!File.Exists(filePath)) {
            return null;
        }

        return File.ReadAllText(filePath);
    }

    public static void SetPreferredGamePath(GameInfo gi, string? newPath) {
        string filePath = Path.Combine(BasePath, $"nitroxpreferredgamepath_{gi.Name}.txt");
        if (newPath == null) {
            File.Delete(filePath);
        } 

        File.WriteAllText(filePath, newPath);
    }
}