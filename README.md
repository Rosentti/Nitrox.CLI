# Nitrox.CLI
A command-line tool to manage Nitrox servers. Primarily meant for Linux users but should work on Windows as well. 
## Usage
Nitrox.CLI can start Nitrox clients, manage servers and saves
## Setting up (For developers)
Setting up Nitrox.CLI for development purposes is quite simple. You will need:
 - Nitrox source code
 - Nitrox.CLI source code (this repo)

Start by cloning the Nitrox source code. Ensure that it builds, and that you have ran the BuildTool.
Then, clone this repository into the Nitrox source code, so that the Nitrox.CLI folder is in the same folder as all the other Nitrox projects. 
It should look a little something like this:

![image](https://user-images.githubusercontent.com/32398752/208914199-569a5dee-14c8-4014-8bad-4fa22a2304e8.png)

After these steps, Nitrox.CLI is ready to use. Just hit compile in your favourite IDE or `cd` into the source dir and run `dotnet run`.

## Contibuting
Feel free to contribute features you think would be useful, but keep in mind this project was made primarily with rapid prototyping in mind.

This also means that you should *not* include this project in commits to Nitrox's main repo. 
