# Augmented Enhancer
A mod for Lethal Company that adds additional configuration options to fit the player's preferences. 

## Configurable Features

### Scrap Protection
The mod allows configuring the following in the event that no players survive a mission:
- The approximate proportion of secured scrap items lost

<details>
<summary><strong>Details</strong></summary>

Supply the proportion of items that you would like to (approximately) keep. For example:

- `0.0` $\rightarrow$ 0% chance each scrap item is kept $\rightarrow$ all scrap lost (Vanilla behaviour)
- `0.5` $\rightarrow$ 50% chance each scrap item is kept $\rightarrow$ approximately half scrap lost
- `1.0` $\rightarrow$ 100% chance each scrap item is kept $\rightarrow$ no scrap is lost

</details>

### Suit Unlocker
Enabling this option unlocks the Green and Hazard suits from game start. 
The PJs suit are not unlocked by default and remain available via the Company store.

### Always Show Terminal
Enabling this option will prevent the ship terminal from closing on exit. 
Ideal for watching the output from `view monitor`.

### Minimum Company Buying Factor
The minimum company buying factor can be configured.

Prevents the company from offering buying factors below. Should be enforced regardless of the 
[randomizer](#company-buying-factor-randomizer)'s enabledness.

### Company Buying Factor Randomizer
Enabling this option will randomize the company buying factor after each run.

<details>
<summary><strong>Details</strong></summary>
By default, the company buying factor is inversely proportional to the remaining days on the quota, so that there is a 
risk/reward tradeoff for holding on to your items after each run. 

This option exists because it can be very discouraging to lose many runs' worth of scrap when playing 
with longer-than-usual quota assignment durations.

The random price will use in-game information when rolling such as 
- The Company "mood" 
- How many days are left on the quota assignment

Depending on the quota deadline duration, the company buying factor may be negative at the start of each assignment.
Use the [minimum buying factor](#minimum-company-buying-factor) feature to mitigate negative buying factors.

Despite the Sigurd log file stating that the company bought at 120%, @Crunchepillar found no evidence that it was
possible in the base game so this mod caps the buying factor at `1.0`.
</details>

### Time Scale
How quickly time passes on moons can be configured.

This can make the game drastically easier (or harder) because part of the monster spawning routine depends on 
how much time has passed.

### Hangar Door Close Duration
The time it takes for the hangar doors to re-open due to lack of power can be configured.

### Threat Scanner
The `scan` command on the terminal can be configured to provide additional information about the environment's 
danger level due to enemy threats.

### Quota Settings
- Assignment Duration (Days Per Quota): The quota assignment duration can be configured.

## FAQ

### How do I install the mod?
See [Installation](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/blob/main/docs/Installation.md)

### Is this mod compatible with X?
This mod is mostly compatible with other BepInEx mods.

See [Compatability](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/blob/main/docs/Compatability.md)
for details and exceptions.

If you encounter issues, please [open an issue on GitHub](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/issues),
making sure to provide information about other mods you have installed 

### Will this mod work in online lobbies?
Features in the config files have been marked to indicate whether they require the host to have them configured
for them to take effect in a multiplayer game. 

Configuration values marked with `Host Required: No` control client-side features, 
which will take effect regardless of the host's configuration.

For the best experience, all players should use the same mods (down to their versions) with the same configuration. 

Please set the `bEnabled` option to `false` (and disable other installed mods) when playing multiplayer with 
players that do not have mods installed.

### How do I change settings?
All this mod's configuration is contained within one file, `/BepInEx/config/lordfirespeed.enhancer.cfg`.

If an option's description is too confusing or not well-enough explained, 
please [open an issue on GitHub](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/issues).

## Changelog

See the [Changelog](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/blob/main/docs/Changelog.md)

## Releases

Releases are published on [GitHub](https://github.com/Lordfirespeed/Lethal-Company-Augmented-Enhancer/releases) 
and [Thunderstore](https://thunderstore.io).

## Acknowledgements

### References

- Forked from [Crunchepillar/Lethal-Company-Enhancer](https://github.com/Crunchepillar/Lethal-Company-Enhancer)
- CI/CD configuration from [BepInEx](https://github.com/BepInEx/BepInEx/tree/master)

### Contributions

Enhanced Icon by Discord User Dat1Mew

Bug Testing Contributions by Discord Users:
*  Pinny
*  Vasanex
*  Toast