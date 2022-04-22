# RoR2-TPVoting
RoR2 mod - provides a voting system for the teleporter in your games

### How it works
Mod will lock players from starting teleporter until voting is passed. To prevent stalling, if half or majority players voted, countdown will begin after which teleporter will be unlocked. Players can vote by writting "r" in chat or by interacting with a portal or teleporter for the first time.  Adequate communication in the game is already made for the player to understand how voting works.

NOW WORKS WITH PORTALS! (void is intentionally excluded)

### Default Config Settings
| Setting                       | Default Value       |
| :---------------------------- | :-----------------: |
| PlayerIsReadyMessages         |       "r,rdy,ready" |
| IgnoredGameModes              |  "IgnoredGameModes" |
| MajorityVotesCountdownTime    |                  30 |
| UserAutoVoteOnDeath           |                true |

## More

Find my other mods here: https://thunderstore.io/package/Mordrog/

### Changelog
#### 1.2.3
- Update manifest + rebuild on new patch

#### 1.2.2
- Fix for Survivors of the Void changes
- Added option to turn off voting on selected game modes

#### 1.2.1
- Fixed issue where players would be auto kicked from dedicated servers if they do not have mod
- Added "outro" stage to ignored Stages for voting
- Added new config to let admin decide whenever players should auto vote on death

#### 1.2.0
- Fix for Anniversary Patch changes
- Make portals interaction work same as teleporter interaction
- Players can now vote when they interact with a portal or teleporter for the first time
- Added new Moon stage to ignored Stages for voting

#### 1.1.0
- Now works on portals only maps
- Chat messages should not appear for 1 player only

#### 1.0.2
- Included all stages without TP to ignored map list
- Fixed activating on interaction items when trying to start locked TP, which made fireworks and squids an actually usefull items. No more.
- Fixed players voting on death on disabled stages

#### 1.0.1
- Fixed checking for majority of players