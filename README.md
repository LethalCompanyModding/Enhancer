# Lethal Company SinglePlayer Enhancer
A Mod for Lethal Company to make the game more enjoyable alone or in small groups

## Configurable Settings
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
I dunno, I've tried getting in contact with other modders but there aren't many of us yet and I don't know what version of Bepin they're built against so I can't make any promises.

### Will this mod work in online lobbies
Maybe. If it will work at all, it will require all players to run it with the same settings. I don't have anyone to test it with at the moment so I have no networking implemented to share the host's settings. A lot of what this mod touches is already sync'd via the vanilla game but not ALL of it so all players that want to use the mod should communicate beforehand and agree on what settings to use. The host's settings for most things will take priority, for example, Company price, Quota days, and probably TimeScale will all be in the host's control. The door timer is not sync'd as far as I am able to figure so all clients should use
the same door timer setting to avoid breaking things. Do not use this mod in lobbies as a client unless you know the host is using it or it may cause errors in your game. Use the bEnabled setting in the config option to turn the mod off and restart the game before joining public lobbies. This mod is not for cheating in public lobbies so most of it won't work without the host running it.

## Changelog

See Changes.md for a complete list of changes from patch to patch

## Downloading

Download from the releases section and check out the tags section for tagged versions of interest that weren't given a stand alone release