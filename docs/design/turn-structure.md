# Turn structure

**Status:** Agreed (2026-11-25). "Today" describes the code; numbers marked *to be set by the design guard* are starting values, not final.

How time passes: who acts when, what one action costs, how fast things move and how far they see. Every other section counts in turns, so this one comes first.

## Today

### Order of a round

1. **The slime acts** — exactly one action, then the round goes on. Moving one cell, Attack, Mess (pour a puddle), Collect, starting to Devour and Wait each cost the whole turn.
2. **Every other creature takes its turn**, one after another in a fixed order (the order they were placed on the board):
   1. its statuses pulse (damage over time, timers count down); if that kills it, it becomes a corpse and does not act;
   2. it chooses one intent — Attack, Chase, Flee, Wander or Idle (by personality and fear/hate, [behavior.md](../behavior.md)) — and carries it out;
   3. if the slime is dead after that, the round stops (defeat).
3. **Environment, then the slime's statuses.** Corpses on the floor age by one turn (a corpse dropped this round skips one tick); then the slime's statuses pulse — including Digesting.

Statuses always pulse at the start of their owner's turn, never at the moment they are applied: poison applied by the slime hurts the enemy at the start of that enemy's next turn.

### Movement and speed

- 8 directions; a step to any of the 8 neighbours costs the same.
- `Speed` = how many cells a creature may walk in one move along an A* path. The slime has 1, most enemies 1; statuses change it (Instability −1).
- Attacking takes the whole turn; a creature does not move and attack in one turn.

### Vision

- `VisionRange` 5, measured as Chebyshev distance (a square around the creature). Walls do not block sight: the maps have no walls yet.
- Enemies that do not see the slime wander or stand; an aggroed enemy remembers where it last saw the slime and goes there.

### Eating takes time

Devour starts a Digesting status whose length equals the corpse's units. The slime keeps acting while it digests; the liquid arrives all at once on the last turn. A second corpse cannot be started meanwhile.

## Design

**Agreed** (2026-11-25):

1. **Strict turns: the slime, then everyone else.** No real-time timers, no undo. This is how the game already works.
2. **The slime gets several actions per round (action points).** One action per round makes a fleeing puddle impossible: pouring costs the step the pursuer needs to catch up. With several actions the slime can step, pour behind itself and step again before the enemies move. How many, and whether the number can grow, is open (below). The reference game's time model is analysed in [references/the-pit.md](references/the-pit.md); a fixed "four actions" rule is not confirmed there.
3. **Enemies keep one move per round, measured in cells.** Speed stays "cells per move": slow enemies walk 1 cell, fast ones 2. There is no "acts every N turns" property.
4. **Slow enemies die to effects, not to strikes.** The intended play: pour lava where the slow enemy will step, then walk away while it burns. On Hard this is a tactic the player must use; balance must make a slow enemy too dangerous to face-tank and easy to burn, poison or dissolve.
5. **Fast enemies are escaped by slowing them** — Oil's Instability (−1 speed), as the game does today.
6. **The enemy order is shown on the board.** Each enemy shows when it moves next, so the player can plan around it.
7. **Walls block line of sight.** Vision is a square of `VisionRange` cut by walls. Pulling enemies one by one and hiding from their sight are the Hard level's core skills.
8. **A fast enemy may move and bite in one turn (a dash)** — with low hit points to match: [enemies.md](enemies.md#fast-enemies). Today no creature moves and attacks in one turn.

### Action points

**Agreed** (2026-11-25):

- **Starting value: 2 action points per round** — *to be set by the design guard*: the simulator ([design-guard.md](design-guard.md)) runs the face-tank table with different values and the number that makes it true is kept.
- **Every action costs 1 point.** Move one cell, strike, pour, collect, start devouring, wait, and later spit.
- **Volume does not change action points.** A smaller slime is not quicker: enemies get both stronger abilities and more health as the game goes on, so the slime keeps its volume as health and cannot trade it for speed.
- **Affinity changes action points.** Heavy liquids slow the slime: a Lava or Mercury affinity costs points. A light liquid may add one (Water is a candidate, not decided). The exact numbers are *to be set by the design guard*; the user's example ("heavy liquids slow it down to 3") assumes a base of 4, so the base and the modifiers are tuned together.
- **No pick-ups that add points.** The world is rooms, corridors and doors; there is no "descent" with finds along it.
