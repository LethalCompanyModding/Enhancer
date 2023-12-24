# Lethal Company SinglePlayer Enhancer
A Mod for Lethal Company to make the game more enjoyable alone or in small groups

## Configurable Settings
*  Protect your scrap: Can keep scrap after a party wipe, either all of it or random amounts.
*  Start with suits unlocked: Spawns the Green and Hazard suits at game start. (Still have to pay for the PJ suit)
*  Always Show Terminal: keeps the ship terminal from closing on exit. Good for watching the output from "view monitor"
*  Random Company Prices: by default the game uses a linear scaling price structure to have a proper risk/reward for holding on to your items after each run. If you're using this mod to extend the number of days per quota you must either set a minimum company price or use this option or the price will go negative. The random price will use in-game information when rolling such as the Company "mood" and how many days are left in the Quota. Despite the Sigurd
log file stating it is possible for the prices to be 120%, I found no evidence that it was
possible in the base game so this mod puts a 100% cap on the scaling formula.
*  Minimum Company Prices: this settings keeps the company from offering prices below the configured amount. Will be enforced even when using random prices.
*  Time Scale: this option can be tweaked to give the party more time on each moon. This can make the game drastically easier (or harder) because part of the monster spawning routine depends on how much time has passed.
*  Hangar Door: the hangar door can be kept closed longer (or shorter if you want)
*  Days Per Quota: The Quota can be increased (or decreased) from the default 3 days to whatever you want.
*  Threat Scanner: The "scan" command on the terminal can have a threat scanner added to it for nervous players.

## FAQ

### How do I install the mod?
See Docs/Installing.txt and follow the instructions

### Is this mod compatible with X other mod
This mod is mostly compatible with other BepinEx mods.

### Will this mod work in online lobbies
Features in the config files have been marked to indicate whether they require you to be host or not in a multiplayer game. For the best experience have all players use the same modlist with the same configuration. Please use the included config option "bEnabled" set to false when playing online with other users that aren't using the mod.

### How do I change settings?
This mod stores all its config settings in BepinEx/Config/mom.llama.enhancer.cfg. I've left detailed descriptions of what each option does and recommendations to try out.

## Changelog

See Changes.md for a complete list of changes from patch to patch

## Downloading

Download from the releases section and check out the tags section for tagged versions of interest that weren't given a stand alone release

## How to contact me
Join discord.gg/lcmod (the LC Modding discord) search for Enhancer in the mod-releases section or @MamaLlama :]

## Other Credits

Enhanced Icon by Discord User Dat1Mew

Bug Testing Contributions by Discord Users:
*  Pinny
*  Vasanex
*  Toast
*  Sariol
*  Zoey ♥
*  Karma
*  JadedRoxie