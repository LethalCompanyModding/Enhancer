# Single Player Enhancer Changelog

## v1.0.4

- Updated for newest game libs
- Silvy :3

## v1.0.3

- Updated for newest game libs

## v1.0.2

- Properly generate new config when empty (Reporter: HiJacked)

## v1.0.1

- Stop accidentally including CSync twice
- Rewrite patches to fix quota days not working anymore bug (Reporter: BlooFox)

## v1.0.0

- All changelogs will now use version numbers
- Implement CSync config syncing for all clients

## Update Aug-17-24#2

- Actually compiled for v60
- Re-added link to github repo

## Update Aug-17-24#1

- Compiled for v60
- Removed suit patches (finally)
- Updated project structure and made project available on Lethal Company Modding

## Update Apr-14-24#1

- Updated enemyPower to float to match zeekerss breaking method change

## Hotfix Dec-24-23#1

- Scrap protection now leaves any item in the ship alone, including equipment, oopsie. (Bug Rancher: JadedRoxie)

## Update Dec-23-23#1

- Patched scrap protection to properly remove equipment. (Bug Tamer: Karma)

## Update Dec-11-23#1

- Patched scrap protection to only run on hosts. This should fix any errors when clients tried to despawn network objects without permission. Oopsie. (Bug Assassin: Zoey ♥)

## Update Dec-10-23#1

- Cleaned up some code
- Verified compatibility with v45

## Update Dec-6-23#1

- Fixed a bug with the terminal remaining rendered even when the config was false in some cases.
- Improved compatibility with GameMaster

### Update Nov-21-23#1

- Scrap protection no long breaks things when failing a quota (Bug Smashers: Pinny/Toast)
- Improved compatibility with Bigger Lobby 2.2.2+ (Bug Annihilator: Bizzle)

## Update Nov-18-23#1

- Scrap Protection mode COINFLIP bug fixed to actually flip a coin (Bug Bonker: Vasanex)
- RPC added to properly inform clients of the company price each day
- Added Dat1Mew's lovely icon to thunderstore release

## Update Nov-17-23#1

- eScrapProtection, has a few simple options for protecting scrap when the party wipes

### Update Nov-15-23#1

- bUnlockSuits, Add Green and Hazard suit to ship
- Company buy prices are derived from level data so they stay they same after save/load
- Plugin moved to net472 to fix dependency errors
- Project updated to make compiling smoother

### Update Nov-13-23#1

- bEnabled, global toggle
- bAlwaysShowTerminal, show terminal without players
- bUseRandomPrices, randomly modifies company prices
- fTimeScale, modifies time on moons
- fMinCompanyBuyPCT, sets a floor for company prices
- fDoorTimer, modifies how long the hangar doors remain closed
- iQuotaDays, modifies how many days the players have to meet quota
- eThreatScannerType, adds a threat scanner to "scan" of specified type
