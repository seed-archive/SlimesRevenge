# Level generator

**Status:** Draft (2026-11-26). A specification, not built: the game has a grid, cells, puddles and paths, but no generator. Every requirement carries its status: **Agreed** (Oleg decided), **Working** (balance or coordinator working value), **Open** (Oleg's proposal or direction, or a lore draft). Detail: the design notes `lore/transitions.md`, `lore/locations.md`, `lore/guardians-structure.md`, `lore/unique-guardians.md`, `lore/variety.md`, `balance/working-values.md`, `balance/graph20-ladder-spec.md`, `balance/level-size.md`, `balance/mercury-build.md`, `balance/easter-eggs.md`, `balance/water-level.md`, `balance/ranged.md`, `balance/hunters.md`, `balance/squad.md`, `balance/past-slimes.md`, `balance/variety.md`.

What one level is made of and how it is put together from rooms. The route above the level (maps, stops, resets) is in the route section of this document (`route.md`).

## Inputs

- The run seed, the map (1–5), the level (1–20), the difficulty.
- The run's draws from the variety pools (below), made once at the start of the run.

## What a level is

- **Agreed:** the world is rooms, corridors and doors. No "descents" with finds along them. Walls block sight.
- **Working** (balance): a level is a pass from the entrance to the exit, with no way back. A chain of rectangular rooms joined by one-cell doorways and short corridors (corridor length 2).
- **Open** (lore): the river is the road. The slime surfaces below an obstacle on the river, goes around it on land through the rooms and dives in again at the sluice upstream. Rooms stand along the river; the river runs beside them or is broken; which rooms it crosses is the generator's choice. The three typical levels of a map share one obstacle kind, the unique first level has its own. Exceptions: on the graveyard the river runs below in a ravine and the levels are crypts in the bluff; under the tower the route is vertical (steam lifts, the still's coil, a pipe).
- **Agreed:** the fast way between locations is the river (a lake, a waterfall, …); a "bend" is not a rule.

## Size

- **Agreed:** each level is bigger than the one before. Room size and enemy vision are difficulty levers.
- **Working** (coordinator): a level takes at most 15 minutes (≤ 7 rooms); levels grow in content and difficulty rather than length.
- **Working** (balance, `balance/working-values.md`):

| Parameter | Value |
| --- | --- |
| Rooms | min(4 + level, 7) |
| Room side | 5–7; the upper bound grows by 1 every 3 levels, up to 11; Hard +2 on both bounds |
| Corridor | 2 cells |
| Enemies per room | 1–2 on levels 1–4, 1–3 from level 5; the same on every difficulty |
| Enemy vision | 2 / 3 / 4 (Easy / Normal / Hard) |

- **Working** (balance rule): a room's side is at least 2 × vision + 1, so there is a place in it where an enemy does not see the slime and luring one at a time is possible.

## Enemies

- **Agreed:** no packs; enemies roam singly ([enemies.md](enemies.md#no-groups)). Running through the level aggroes many; luring from the edge works.
- **Working** (balance): only the Rat King spawns enemies (1 rat every 2 rounds, at most 4 alive, in the boss room). Other enemies are placed at generation.
- **Open** (Oleg's direction): every level has at least one enemy of each type present on its map. **Working** (balance): 5 elements per level; circle I plus the Living on maps 1–2, circle II on maps 3–5. Lore rotates 3–5 types per level (`lore/bestiary.md`).
- **Working** (balance): at most one shooter per level, none on levels 1–2 or on guardian levels.
- **Working** (balance, Hard only): a hunter on levels 5–7, 9–11, 13–15, 17–19 enters at the exit on round 10 or when the slime enters the second-to-last room. The squad waits on level 3 in the second-to-last room, which cannot be bypassed.

## The exit and its guardian

- **Agreed:** the last level of a place has a guardian.
- **Open** (Oleg's direction): a boss on every map, a guardian at every location. Lore: the guardian stands at the exit sluice, one on each non-boss level, static like a tower in tower defense; one kind per map — Ballista, Scarecrow, Bell, Gargoyle, Athanor (`lore/guardians-structure.md`).
- **Working** (balance): the Ballista's line is straight, up to 4 cells, and always crosses the path to the exit; one per level on levels 3–4.

## Optional places

### Guardian arenas

- **Open** (Oleg's direction): small separate locations with a unique, named guardian. An arena may or may not appear in a run; the slime can pass through it or around it. Three ways: fight (loot), bargain (bring what it asks), or a hidden bypass that costs part of the volume in advance.
- **Working** (balance placeholders): each arena pool appears in 50 % of runs; the bypass costs 20 % of the current body.

### Ligeia's island

- **Open:** Ligeia sits on an island in the middle of the reservoir behind the dam (boss of map 2). How the generator places "an island in the centre" is not yet clear to Oleg. Lore's proposal: a lake room about 7×7, the island in the centre reached by a ford one cell wide; 3×3 land is better for a fight than one cell.

### Secret rooms and chests

- **Agreed:** a couple of chests per run with some chance, in a secret room that is either "not very secret" (visible entrance) or truly hidden. A chest is a mimic with chance 25 % / 50 % / 75 % (Easy / Normal / Hard). The mimic fight is in a small closed room: no running out, no kiting.
- **Working** (balance): one chest room per map from map 2, with a 60 % chance (about 3 a run); half with a visible entrance, half hidden. The mimic room is 3×3 and closed, the mimic in the centre.

### The secret place

- **Agreed:** one secret location per game, not in every run, at a different place each time, with an unnoticeable entrance (for example the dwarf smithy).
- **Working** (balance): 70 % of runs (80 % a location, 20 % an empty pantry); on levels 2–3, 6–7, 10–11, 14–15 or 18–19; a dead end off the path to the exit.

### Variety pools

- **Open** (Oleg's direction): not everything in one run. Each pool draws one option (or nothing) at the start of the run; both options of one pool are never seen in one run. Pools by map: `lore/variety.md` (mine secret: smithy / pantry / chest / nothing; grove arena: Pan / Hive / nothing; graveyard boss: Strzyga / vampire; …).
- **Working** (balance): an option unseen in the player's past runs weighs twice as much.

## Special levels

- **Water level at the dam** (Open, Oleg's proposal; numbers working, `balance/water-level.md`): half a level of 3 rooms between levels 7 and 8; cells where Water is gathered without limit; every enemy fully immune to Water; a whirlpool of 2–3 cells.
- **Stealth level after a full reset** (Open, fallback in balance): liquids worth 1.5 × capacity in puddles of 2; while the body is under half its capacity, enemies see the slime only when adjacent.
- **Dry levels** (Working, balance): levels 6–7 have no river; 3 drip cells of 2 Water each, in corridors on the path.

## Surface exits, day and night

- **Agreed:** a short way up to the surface per stretch; the sun dries the slime on the surface.
- **Open** (Oleg's proposal): the graveyard is always night, the village always day; by day a Mercury or Lava slime can go out, at night a Water one. Lore: map 1 the quarry floor (day), map 2 the village (day), map 3 the graveyard (night), map 4 a night square, map 5 the tower yard (day).
- **Working** (balance): the sun covers the 2 middle rooms of the chain on surface levels 3, 10, 14 and 20.

## Things placed on the floor

- **Water** (Working, balance): the river gives 6 units per level at the exit (from level 8 alternating with the run's chosen liquids); 1 flask of 1 Water on levels 1–2; no flasks after that.
- **Past-slime puddles** (Agreed: on the level where the slime died; Working: at most one per level, on the death cell).
- **Coins** (Working, balance `balance/prices.md`): about 2 per map.
- **Stone and amber** (Agreed: Water + Lava drops Stone, Resin + Lava drops amber, as obstacles). **Working** (balance): one per chain, on a free neighbouring cell towards the nearest enemy; it crumbles after 4 rounds; it must never close the path to the exit.

## Themes per map

**Open** (lore): the room kinds each map draws from.

| Map | Room kinds |
| --- | --- |
| 1. Quarry and mines | quarry settling pond, gallery, timbering, dwarf pantry, underground waterfall with a cage |
| 2. Village | mill, dry riverbed, cellar, vegetable garden, barn, grove, spillway |
| 3. Graveyard | crypt, mausoleum, chapel with a bell, catacombs, crossing |
| 4. Tunnels | brick vault, sluice, laundry drain, distillery drain, apothecary cellar |
| 5. Under the tower | distilling cellar, laboratory, ingredient store, greenhouse, gallery |

## Assembly (proposed order)

1. Draw the run's pools (once per run).
2. Pick the level's parameters from the table above and the map's room kinds.
3. Lay the chain of rooms from the entrance to the exit sluice, with the river along it (or the map's exception).
4. Put the guardian at the exit; put the boss room on a boss level.
5. Attach optional places as dead ends off the path: the secret place, a chest room, an arena with its hidden bypass.
6. Place enemies by the map's types and the per-level limits; place the Ballista line across the path.
7. Place floor items: the river's Water at the exit, flasks, drip cells, past-slime puddles, coins.
8. Check: the exit is reachable; every room is big enough for its vision; a drop can never close the path to the exit.

## Open

- Which size rule wins: "every level bigger" (Agreed) or "≤ 15 minutes, ≤ 7 rooms" (Working).
- Whether the location guardian stands on every non-boss level.
- Ligeia's island: size and approach.
- Day and night per map, and which levels have a surface exit.
- Guardian arena frequency and the bypass cost (balance, next report).
- Chest count: Oleg's "a couple per run" against balance's one room per map at 60 %.
- The stealth level is a fallback; whether a full reset happens at all.
- The level economy (enemies, corpses, river, flasks) is checked by the simulator, not by this note.
