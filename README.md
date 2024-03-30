# Nitrox.CLI
A command-line tool to manage Nitrox game instances on Linux.
## Setting up (For developers)
Setting up Nitrox.CLI for development purposes is quite simple. You will need:
 - Nitrox-BZ source code
 - Nitrox.CLI source code (this repo)

Start by cloning the Nitrox-BZ source code. Run the BuildTool.
Then, clone this repository into the Nitrox source code, so that the Nitrox.CLI folder is in the same folder as all the other Nitrox projects. 
Then go ahead and cd into Nitrox.CLI, and run `dotnet run -- game run SN` or `dotnet run -- game run BZ` for Subnautica and Below Zero respectively