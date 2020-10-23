# RoR2-TPVoting
RoR2 mod - provides a voting system for the teleporter in your games

### How it works
Mod will lock players from starting teleporter until voting is passed. To prevent stalling, if half or majority players voted, countdown will begin after which teleporter will be unlocked. Players can vote by writting "r" in chat.  Adequate communication in the game is already made for the player to understand how voting works.

NOW WORKS WITH PORTALS! (void is intentionally excluded)

### Default Config Settings
| Setting                       | Default Value       |
| :---------------------------- | :-----------------: |
| PlayerIsReadyMessages         |       "r,rdy,ready" |
| MajorityVotesCountdownTime    |                  30 |

## More

Find my other mods here: https://thunderstore.io/package/Mordrog/

### Changelog
#### 1.1.0
- Now works on portals only maps
- Chat messages should not appear for 1 player only
#### 1.0.2
- Included all stages without TP to ignored map list
- Fixed activating on interaction items when trying to start locked TP, which made fireworks and squids an actually usefull items. No more.
- Fixed players voting on death on disabled stages
#### 1.0.1
- Fixed checking for majority of players