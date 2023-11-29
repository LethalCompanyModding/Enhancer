# Augmented Enhancer Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

### Changed

- Forked from https://github.com/Crunchepillar/Lethal-Company-Enhancer/
- Project restructured
- Project renamed
- Project icon replaced

### Added

- CI/CD build setup
- Automatic publish to Thunderstore via GitHub actions

## [0.0.5] - 2023-11-21

### Fixed

- Scrap protection no long breaks things when failing a quota (Bug Smashers: Pinny/Toast)
- Improved compatability with Bigger Lobby 2.2.2+ (Bug Annihilator: Bizzle)

## [0.0.4] - 2023-11-18

### Added

- Added Dat1Mew's lovely icon to Thunderstore release

### Fixed

- Scrap Protection mode COINFLIP bug fixed to actually flip a coin (Bug Bonker: Vasanex)
- RPC added to properly inform clients of the company price each day

## [0.0.3] - 2023-11-17

### Added

- `eScrapProtection` configured value: has a few simple options for protecting scrap when the party wipes

## [0.0.2] - 2023-11-15

### Changed

- Company buy prices are derived from level data so they stay they same after save/load
- Plugin moved to net472 to fix dependency errors
- Project updated to make compiling smoother

### Added

- `bUnlockSuits` configured value: Add Green and Hazard suit to ship

## [0.0.1] - 2023-11-13

### Added

- `bEnabled` configured value: global toggle
- `bAlwaysShowTerminal` configured value: show terminal without players
- `bUseRandomPrices` configured value: randomly modifies company prices
- `fTimeScale` configured value: modifies time on moons
- `fMinCompanyBuyPCT` configured value: sets a floor for company prices
- `fDoorTimer` configured value: modifies how long the hangar doors remain closed
- `iQuotaDays` configured value: modifies how many days the players have to meet quota
- `eThreatScannerType` configured value: adds a threat scanner to "scan" of specified type