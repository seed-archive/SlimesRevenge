# Turn structure

**Status:** Draft (2026-11-25). "Today" describes the code; *Proposed* parts and the open decisions wait for the user.

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

*Proposed*:

1. **Keep strict turns: the slime, then everyone else.** It is the easiest to read on a phone and every move is a decision (pillar *Readable in one glance*). No real-time timers, no undo.
2. **One action per turn for the slime**, with one exception below (open decision 1).
3. **Speed stays "cells per move"** for now. Fast enemies (Speed 2) are what makes fleeing dangerous; the counter is slowing them (Instability from oil). This gives the Hard level its "slow them and get away" play without new rules.
4. **Walls block sight** once rooms have walls (open decision 3). Without it, "pull enemies one by one" and "manage their vision" — the Hard level's core skills — are impossible.
5. **The enemy order is shown.** A small number or an arrow on each enemy tells which one moves next, so the player can plan around it.

## Open decisions

1. **Puddle while fleeing.** If pouring a puddle costs a whole turn, a fleeing slime loses a step and the pursuer catches it, so the "slowing puddle while running away" tactic does not work. Options: (a) a *trail* action — move one cell and leave a puddle on the cell you left, costing one unit and one turn (recommended); (b) pouring stays a full turn and slowing is only for traps prepared in advance.
2. **Slow enemies.** Should some enemies act only every second turn (a slow mushroom), on top of cells-per-move speed? Options: (a) yes, an "acts every N turns" property (recommended, gives a clear weak-but-tanky archetype); (b) no, speed only.
3. **Line of sight.** Do walls block vision? Options: (a) yes (recommended, needed for the Hard level); (b) no, vision stays a plain square.
