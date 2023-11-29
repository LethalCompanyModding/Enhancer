# Mod Manager (recommended)

This mod's releases are available on [Thunderstore](https://thunderstore.io/). It can be installed with ease using any mod manager 
that supports Thunderstore as a mod repository.

## Compatible Managers

### Thunderstore Mod Manager

1. Ensure Lethal Company is installed.
2. [Install ThunderStore Mod Manager from Overwolf](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager).
3. Search for and select 'Lethal Company' from the list of supported games.
4. Create / Select an existing game profile.
5. Open the `Mods>Online` tab, search for `Augmented Enhancer` and press `Download` -> `Download with dependencies`.
6. Launch the game using the `Start Modded` button in the top-right corner of the window.

### r2modman

1. Ensure Lethal Company is installed.
2. [Download the appropriate latest r2modman release from GitHub](https://github.com/ebkr/r2modmanPlus/releases).
3. Search for and select 'Lethal Company' from the list of supported games.
4. Create / Select an existing game profile.
5. Open the `Mods>Online` tab, search for `Augmented Enhancer` and press `Download` -> `Download with dependencies`.
6. Launch the game using the `Start Modded` button in the top-left corner of the window.

# Manual Installation (not recommended)

## Preamble

This mod uses BepInEx to modify the game code, like most Unity mods.
The release page for BepInEx can be found [on GitHub](https://github.com/BepInEx/BepInEx/releases).

## Installing BepInEx 5

1. Download the latest release of BepInEx `5.x` from [GitHub](https://github.com/BepInEx/BepInEx/releases).

   Note: ensure you choose the correct `.zip` archive for your architecture.

   - `x86`: 32-bit systems
   - `x64`: 64-bit systems
   - `unix`: N/A.
   
   Lethal Company will only run on Unix under Wine, so choose from other options based on your Wine configuration.

   If you didn't configure Wine yourself, `x64` is a good bet.

2. Install BepInEx to the game installation folder, i.e. the folder that contains the game executable `Lethal Company.exe`.
   1. Locate the game installation folder: right click on the game in Steam -> `manage` -> `browse local files`.
   2. Extract all files from the BepInEx `.zip` archive into this folder.
3. Run the game to allow BepInEx to perform first-time setup.
4. Wait to reach the main menu, then close the game.

## Installing the Mod

1. Create a new folder called `augmented-enhancer` inside `/BepInEx/plugins`.
2. Place the mod DLL into this folder.
   The filepath should end with `/BepInEx/plugins/augmented-enhancer/Enhancer.dll`.
3. Run the game again. The mod will create a default configuration file at `/BepInEx/config/lordfirespeed.enhancer.cfg`.
4. Wait to reach the main menu, then close the game. 
5. 5.Edit the values in the configuration file as you please, using the text editor of your choice.
