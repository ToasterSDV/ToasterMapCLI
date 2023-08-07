# ToasterMapCLI

ToasterMapCLI is a very basic command-line tbin to tmx utility designed for Stardew Valley and written in C#.

## Usage

1. Build it. If there is one, get the latest release from [the releases page](/releases) otherwise, refer to [Building](#Building)
2. Obtain the `xTile.dll` file from your Stardew Valley install and place it beside the downloaded file.
2. Run it with `./ToasterMapCLI path/to/map.tbin` you may specify a tmx path as a second argument, otherwise it will default to the same folder the tbin is in.

## Building

1. Run `./release.bat <win/osx/linux>` and find the compiled binary in `bin/Release/net7.0/<os>/publish/ToasterMapCLI[.exe]`

## Credits

1. `targets/find-game-folder.targets` is taken from [SMAPI](https://github.com/Pathoschild/SMAPI)
1. Pathoschild, for making the assembly finding code and helping me debug heavily!