# Balance

**Status:** Draft (2026-11-25). The numbers below are a **working version** from the balance analysis, not final. The formulas and the spreadsheet are not written yet.

**Agreed:** balance computes two levels at once, not only level 1 ([progression.md](progression.md#mechanic-ladder)).

## Level 1 — working numbers

Columns are the difficulty levels Easy / Normal / Hard.

| Parameter | Easy | Normal | Hard |
| --- | ---: | ---: | ---: |
| Slime capacity (units) | 10 | 7 | 6 |
| Enemy hit points | 2 | 2 | 3 |
| Enemy bite | 1 | 1 | 3 |
| Enemy vision (cells) | 2 | 3 | 4 |

Same on every difficulty: corpse 4; 1–2 enemies per room; 1 flask. The number of enemies is a difficulty lever ([vision.md](vision.md#death-and-difficulty)), but on level 1 the working version keeps it equal.

The working numbers may diverge once there are 20 levels.

### Why not 4 HP on Hard

**Agreed** (2026-11-25): enemy HP 4 on Hard is twice too much. With 6 volume and blows traded in turn, 2 units are left and 4 must be refilled — and level 1 has only Water. A Water puddle in a doorway gives at most Wet; an Oil puddle would work far better (Oil was later removed; Fat and Sulfur took its roles), but level 1 has only Water. The working number on Hard is 3. The first-fight rule behind it: [growth.md](growth.md#the-first-fight).

Targets these numbers serve: the face-tank table ([vision.md](vision.md#death-and-difficulty)) and net body per fight ([growth.md](growth.md#every-fight-nets-body)).
