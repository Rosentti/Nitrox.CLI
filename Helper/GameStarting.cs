using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Nitrox.CLI.Patching;
using NitroxModel;
using NitroxModel.Helper;
using NitroxModel.Platforms.Store;
using NitroxModel.Platforms.Store.Interfaces;

namespace Nitrox.CLI.Helper;
public static class GameStarting {
    private static NitroxEntryPatch? nitroxEntryPatch;
    public static void StartMultiplayer(GameInfo game)
        {
            string gamepath = "";

            string? preferred = GamePathUtil.GetPreferredGamePath(game);
            bool isBelowZero = game.Name == "SubnauticaZero";
            
            if (!string.IsNullOrEmpty(preferred)) {
                gamepath = preferred;
                if (game.Name == "SubnauticaZero") {
                    NitroxUser.GamePath_BZ = gamepath;
                } else {
                    NitroxUser.GamePath = gamepath;
                }
            } else {
                if (game.Name == "SubnauticaZero") {
                    gamepath = NitroxUser.GamePath_BZ;
                } else {
                    gamepath = NitroxUser.GamePath;
                }
            }

            
            
            if (string.IsNullOrEmpty(gamepath))
            {
                throw new Exception($"Location of {game.FullName} is unknown. ");
            }

            if (PirateDetection.HasTriggered)
            {
                throw new Exception("Aarrr! Nitrox walked the plank :(");
            }

#if RELEASE
            if (Process.GetProcessesByName(game.Name).Length > 0)
            {
                throw new Exception($"An instance of {game.FullName} is already running");
            }
#endif

            // TODO: The launcher should override FileRead win32 API for the Subnautica process to give it the modified Assembly-CSharp from memory 
            string initDllName;
            if (isBelowZero) {
                initDllName = "NitroxPatcher-BelowZero.dll";
            } else {
                initDllName = "NitroxPatcher-Subnautica.dll";
            }

            try
            {
                File.Copy(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "lib", initDllName),
                    Path.Combine(gamepath, $"{game.Name}_Data", "Managed", initDllName),
                    true
                );
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Unable to move initialization dll to Managed folder. Still attempting to launch because it might exist from previous runs. {ex.Message}");
                Console.ResetColor();
            }

            nitroxEntryPatch = new NitroxEntryPatch(() => Path.Combine(gamepath, $"{game.Name}_Data"), isBelowZero);
            
            // Try inject Nitrox into Subnautica code.
            if (nitroxEntryPatch?.IsApplied == true)
            {
                nitroxEntryPatch.Remove();
            }

            if (nitroxEntryPatch == null)
            {
                throw new Exception("Nitrox was blocked by another program");
            }
            nitroxEntryPatch.Remove();
            nitroxEntryPatch.Apply();

            if (IsQModInstalled(gamepath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Seems like QModManager is Installed");
                Console.ResetColor();
            }

            StartGame(game, gamepath);
        }

        private static void StartGame(GameInfo game, string gamePath)
        {
            IGamePlatform platform = GamePlatforms.GetPlatformByGameDir(gamePath);
            switch (platform.Platform) {
                case NitroxModel.Discovery.Models.Platform.STEAM:
                    var processes = Process.GetProcessesByName("steam");
                    if (processes.Length == 0) {
                        throw new Exception("Steam is not running or couldn't be found. Please launch steam and try again.");
                    }

                    ProcessStartInfo startInfo = new ProcessStartInfo() { 
                        FileName = "/usr/bin/steam", 
                        Arguments = $"-applaunch {game.SteamAppId} -nitrox {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}" }; 
                    Process proc = new Process() { StartInfo = startInfo, };
                    // we need to symlink the Nitrox folder in the wine prefixes appdata into a user folder
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                        var protonPrefixPath = System.IO.Path.Combine(gamePath, "..", "..", "compatdata", game.SteamAppId.ToString(), "pfx");
                        var appdataRoaming = System.IO.Path.Combine(protonPrefixPath, "drive_c", "users", "steamuser", "AppData", "Roaming");
                        var localShare = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        var localShareNitrox = Path.Combine(localShare, "Nitrox");
                        var appdataRoamingNitrox = Path.Combine(appdataRoaming, "Nitrox");
                        System.IO.Directory.CreateDirectory(appdataRoaming);
                        System.IO.Directory.CreateDirectory(localShareNitrox);
                        if (!Directory.Exists(appdataRoamingNitrox)) {
                            System.IO.File.CreateSymbolicLink(Path.Combine(appdataRoaming, "Nitrox"), localShareNitrox);
                        }
                    }
                    proc.Start();
                    Console.WriteLine($"Started {game.FullName}");
                    return;
                default:
                    throw new Exception($"Unsupported platform {platform.Name}");
            }
        }
        internal static bool IsQModInstalled(string subnauticaBasePath)
        {
            string subnauticaQModManagerPath = Path.Combine(subnauticaBasePath, "Bepinex/plugins/QModManager");
            return Directory.Exists(subnauticaQModManagerPath);
        }
        
}