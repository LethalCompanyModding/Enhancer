# Lethal Company SinglePlayer Enhancer

A Mod for Lethal Company to make the game more enjoyable alone or in small groups

## Maintainers

**Original Author**: Robyn

**Current Maintainer(s)**: Robyn

This mod has been dedicated to the [Lethal Company Modding community repo](https://github.com/LethalCompanyModding/Enhancer) and may be maintained by any willing community member with a github account.

## Configurable Settings

* Protect your scrap: Can keep scrap after a party wipe, either all of it or random amounts.
* Start with suits unlocked: Spawns the Green and Hazard suits at game start. (Still have to pay for the PJ suit)
* Always Show Terminal: keeps the ship terminal from closing on exit. Good for watching the output from "view monitor"
* Random Company Prices: by default the game uses a linear scaling price structure to have a proper risk/reward for holding on to your items after each run. If you're using this mod to extend the number of days per quota you must either set a minimum company price or use this option or the price will go negative. The random price will use in-game information when rolling such as the Company "mood" and how many days are left in the Quota. Despite the Sigurd
log file stating it is possible for the prices to be 120%, I found no evidence that it was
possible in the base game so this mod puts a 100% cap on the scaling formula.
* Minimum Company Prices: this settings keeps the company from offering prices below the configured amount. Will be enforced even when using random prices.
* Time Scale: this option can be tweaked to give the party more time on each moon. This can make the game drastically easier (or harder) because part of the monster spawning routine depends on how much time has passed.
* Hangar Door: the hangar door can be kept closed longer (or shorter if you want)
* Days Per Quota: The Quota can be increased (or decreased) from the default 3 days to whatever you want.
* Threat Scanner: The "scan" command on the terminal can have a threat scanner added to it for nervous players.

## FAQ

### How do I install the mod?

Use a mod manager like R2ModManager or GaleMM

### Is this mod compatible with X other mod

This mod is mostly compatible with other BepinEx mods.

### Will this mod work in online lobbies

Since version 1.0.0, Enhancer will use CSync to ensure all clients have the same config values. Enhancer will no longer support any client-only configurations, all clients must have it installed to connect to each other as CSync uses a network prefab.

### How do I change settings?

This mod stores all its config settings in BepinEx/Config/mom.llama.enhancer.cfg. I've left detailed descriptions of what each option does and recommendations to try out.

## Changelog

See CHANGELOG.md for a complete list of changes from patch to patch

## Downloading

See the link by the repo description to download this mod from the Thunderstore.

## How to contact me

Join <https://discord.gg/XeyYqRdRGC> (the LC Modding discord) search for Enhancer in the mod-releases section or @MamaLlama :]

## Other Credits

Enhanced Icon by Discord User Dat1Mew

Bug Testing Contributions by Discord Users:

* Pinny
* Vasanex
* Toast
* Sariol
* Zoey ♥
* Karma
* JadedRoxie
* BlooFox
