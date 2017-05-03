# Roguelike-Game
KNOWN BUGS:
- Player dying on levels other than the first cause an error
  - likely due to the EndGame gameobject not being found
- Player does not level up
  - likely due to the object storing the players EXP being restarted upon starting a new level.
- Players health updates a frame late
- Escape Key does not work in opening the pause menu
