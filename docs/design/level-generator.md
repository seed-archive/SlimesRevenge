# Level generator

**Status:** Draft (2026-11-26). A specification, not built: the game has a grid, cells, puddles and paths, but no generator. Every requirement carries its status: **Agreed** (Oleg decided), **Working** (a balance or coordinator working value), **Open** (Oleg's proposal or direction, or a lore draft). Conflicts between sources are listed under [Open](#open), not resolved here.

What one level is made of and how it is put together from rooms. The route above the level (maps, stops, resets) is in [route.md](route.md).

## Inputs

- The run seed, the map (1–5), the level (1–20), the difficulty.
- The run's draws from the variety pools ([below](#variety-pools)), made once at the start of the run.

## What a level is

- **Agreed:** the world is rooms, corridors and doors. No "descents" with finds along them. Walls block sight.
- **Working** (balance): a level is a pass from the entrance to the exit, with no way back: a chain of rectangular rooms joined by one-cell doorways and corridors of 2 cells.
- **Agreed:** the fast way between locations is the river (a lake, a waterfall, …); a "bend" is not a rule.
- **Open** (lore): the river is the road. The slime surfaces below an obstacle on the river, goes around it on land through the rooms and dives in again at the sluice upstream. Rooms stand along the river; the river runs beside them or is broken; which rooms it crosses is the generator's choice. The three typical levels of a map share one obstacle kind, the unique first level has its own. Exceptions: on the graveyard the river runs below in a ravine and the levels are crypts in the bluff; under the tower the route is vertical (steam lifts, the still's coil, a pipe).

## Size

- **Agreed:** each level is bigger than the one before. Room size and enemy vision are difficulty levers; the number of enemies is a difficulty lever too.
- Fallback (coordinator): a level takes at most 15 minutes (≤ 7 rooms); levels grow in content and difficulty rather than length.
- **Working** (balance):

| Parameter | Value |
| --- | --- |
| Rooms | 4 + ⌈level / 2⌉ (5 on level 1, 14 on level 20); fallback min(4 + level, 7) |
| Room side | Easy 5–7, Normal 6–8, Hard 7–9; the upper bound grows by 1 every 3 levels, up to 11 (Hard 13) |
| Corridor | 2 cells |
| Enemies per room | levels 1–4: Easy 1, Normal 1–2, Hard 1–2; from level 5: Easy 1–2, Normal 1–3, Hard 2–3 |
| Enemy vision | 2 / 3 / 4 (Easy / Normal / Hard) |

- **Agreed:** every level is bigger than the one before (coordinator, 2026-11-26: Oleg's rule stands). Fallback (balance with lore): growth by content inside ≤ 7 rooms — room sides +1 every 3 levels, more enemies per room, new kinds; the rooms table above then caps at 7.
- **Working** (balance rule): a room's side is at least 2 × vision + 1, so an enemy in the centre does not see the whole room and luring one at a time is possible; the sides above follow it. Fallback: the same sides on every difficulty.
- **Agreed:** the number of enemies is a difficulty lever, like every other parameter (coordinator, 2026-11-26: Oleg's rule stands). Fallback (balance): the same count on every difficulty, with difficulty from enemy HP, bite and corpse, slime capacity and vision.

## Enemies

- **Agreed:** no packs; enemies roam singly ([enemies.md](enemies.md#no-groups)). Running through the level aggroes many; luring them from the edge works.
- **Agreed:** enemies roam as singles from spawners (coordinator, 2026-11-26: the rule stands). **Working** (balance): from level 3, 1 spawner per level releases one roaming enemy every 15 slime rounds, at most 2 alive on top of the placed enemies; the Rat King and the Hive spawn 1 every 2 rounds, at most 4 alive. Fallback (balance with lore): enemies placed when the level is generated, only bosses and guardians spawn.
- **Open** (Oleg's direction): every level has at least one enemy of each category (Living, Undead, Mushrooms, Plants, …). **Working** (balance): 5 elements per level, circle I plus the Living on maps 1–2, circle II on maps 3–5; lore rotates 3–5 kinds per level. Balance has fewer categories per level than Oleg's direction asks for.
- **Working** (balance): at most one shooter per level, none on levels 1–2 and none on boss levels (4, 8, 12, 16, 20).
- **Working** (balance, Hard only): a hunter on levels 5–7, 9–11, 13–15 and 17–19 enters at the exit on round 10, or when the slime enters the second-to-last room. The squad waits on level 3 in the second-to-last room, which cannot be bypassed.

## The exit and its guardian

- **Agreed:** the last level of a place has a guardian. A unique mob is a guardian with its own name that guards something.
- **Open** (Oleg's direction): a boss on every map and a guardian at every location. Three ways past a location guardian: come with the right body, break it for loot, or go around it and lose volume. Lore: the guardian stands at the exit sluice, one on each non-boss level, static like a tower in tower defense; one kind per map (Ballista, Scarecrow, Bell, Gargoyle, Athanor). Lore's fallback: ordinary guard mobs instead of static guardians.
- **Working** (balance): the Ballista is a trap on map 1, levels 3–4, one per level. Its line is straight, up to 4 cells, and always crosses the path to the exit.
- **Working** (coordinator, 2026-11-26): the Ballista is both — a static trap that is the location guardian of map 1.

## Boss levels

- The boss stands on the last level of each map (4, 8, 12, 16, 20).
- **Ligeia** is the boss of map 2 (level 8), not an optional arena (**Working**, coordinator, 2026-11-26). Her arena offers three ways: fight, a deal with amber, or a bypass by the spillway.
- **Ligeia's island.** **Open:** Ligeia sits on an island in the middle of the reservoir behind the dam. How the generator places "an island in the centre" is not yet clear to Oleg. Lore's proposal: a lake room of about 7×7 with the island in the centre. It is reached by a ford one cell wide, or through the water (spit, planks, the whirlpool). An island of one cell is lore's base version; 3×3 is better for a fight. The amber bargain needs free cells on the island, where the slime makes amber from Resin and Lava.

## Optional places

### Guardian arenas

- **Open** (Oleg's direction): small separate locations with a unique, named guardian. An arena may or may not appear in a run; it sits on the path with a hidden bypass beside it, so the slime can pass through it or around it. Three ways: fight (loot), bargain (bring what the guardian asks), or take the hidden bypass, losing part of the volume in advance. Lore's bypasses: a narrow adit at the dwarf guardian, the dam spillway at Ligeia, thorns at Pan.
- **Working** (balance placeholders): each arena pool appears in 50 % of runs; the bypass costs 20 % of the current body.

### Secret rooms and chests

- **Agreed:** a couple of chests per run, with some chance; for example in a secret room that is either "not very secret" (the entrance is visible) or truly hidden. A chest is a mimic with a chance of 25 % / 50 % / 75 % (Easy / Normal / Hard). The mimic fight is in a small closed room: no running out, no kiting.
- **Working** (balance): one chest room per map from map 2, with a 60 % chance (about 3 a run); half with a visible entrance, half hidden. The mimic room is 3×3 and closed, with the mimic in the centre.

### The secret place

- **Agreed:** one secret location per game, not in every run, at a different place each time, with an unnoticeable entrance (for example the dwarf smithy).
- **Working** (balance): one per run, in 70 % of runs (80 % a location, 20 % an empty pantry); on levels 2–3, 6–7, 10–11, 14–15 or 18–19; a dead end off the path to the exit. The variety pools on every map are a different thing (arenas and optional rooms) and stay. Fallback: a secret place on every map.

### Variety pools

- **Open** (Oleg's direction): not everything in one run. Each pool draws one option, or nothing, at the start of the run; both options of one pool are never seen in the same run. Lore's pools: mine secret: smithy / dwarf pantry / chest / nothing; grove arena: Pan / Hive / nothing; graveyard boss: Striga / vampire; one secret option on each map.
- **Working** (balance): an option the player has not seen in past runs weighs twice as much.

## Special levels

- **Water level at the dam** (Open, Oleg's proposal; numbers Working): half a level of 3 rooms between levels 7 and 8, with no hunter and no free +1 capacity. On its cells Water is gathered without limit. Every enemy is fully immune to Water; the sirens' song pulls the slime 1 cell towards them. A whirlpool of 2–3 cells by the spillway drifts the slime towards the gate. The level's resource is the miller's skeleton (Oleg: Phosphorus from fish bones). The exit goes through the rotten lake: Water turns to Poison, at most half the body.
- **Stealth level after a full reset** (Open, a fallback in balance): after the slit in the tower's pipe, the stealth part falls on the cell stop and level 17. The smithy-exit slit is also a fallback; the main option there is the quench tub. Liquids worth 1.5 × capacity lie in puddles of 2. While the body is under half its capacity, enemies see the slime only when adjacent.
- **Dry levels** (Open, Oleg's idea of a dried-up location with rain dripping from the ceiling; numbers Working): levels 6–7 have no river. Instead there are 3 drip cells of 2 Water each; balance places them variously in random rooms, ditches and cellars, or corridors on the path.

## Surface exits, day and night

- **Agreed:** one short way up to the surface per stretch; the sun dries the slime on the surface.
- **Open** (Oleg's proposal): the graveyard is always at night, the village always by day; by day a Mercury or Lava slime can go out, at night a Water one. Lore: map 1 the quarry floor (day), map 2 the village (day), map 3 the graveyard (night), map 4 a night square, map 5 the tower yard (day).
- **Working** (balance with lore, 2026-11-26):
  - The village (map 2) is by day: on the whole level the sun dries Water, Aquavita and Ectoplasm, 1 unit off the edge every 2 slime rounds. Shade does not dry: cellars, ditches, the mill race, under footbridges.
  - The graveyard (map 3) is at night: the damp cold cools Lava, 1 unit every 2 rounds (it turns to Stone and drops).
  - The tunnels and the underground have no sky. The tower yard is at night (coordinator, 2026-11-26: keeps "by day only a Mercury or Lava slime goes out"); fallback: by day.
  - Day exits elsewhere: the sun dries every round in surface rooms.
  - Fallbacks: no shade in the village (it dries everywhere); the village at night.

## Things placed on the floor

- **Water** (Working, balance): the river's stock lies on the flow cells at the entrance and the exit, 6 units per level (from level 8 alternating with the run's chosen liquids); 1 flask of 1 Water on levels 1–2; no flasks after that.
- **The spawn pool and the Iron Man reserve fountain** ([growth.md](growth.md#the-last-unit)): placed at the slime's spawning point.
- **Past-slime puddles** (Agreed: on the level where the slime died). **Working** (balance): at most one per level and difficulty, collected once a run, on the death cell (a wall or river cell moves it to the nearest free cell); up to 3 dated traces without liquid. Lore: in the room where the slime died; a nameless crypt on the graveyard holds up to 5 puddles.
- **Coins** (Open, lore's "coins in the belly"; Working: about 2 per map).
- **Stone and amber.** **Agreed:** Water + Lava drops Stone, an obstacle on the floor; Resin + Lava drops amber. **Working** (balance): one per chain, on a free neighbouring cell towards the nearest enemy; amber blocks like Stone; both crumble after 4 rounds and must never close the path to the exit.

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
4. Put the guardian at the exit; on a boss level, the boss room.
5. Put an arena on the path with its hidden bypass beside it; attach the secret place and a chest room as dead ends off the path.
6. Place enemies by the map's categories and the per-level limits; place the Ballista line across the path.
7. Place the floor items: the river's stock, flasks, drip cells, the spawn pool, past-slime puddles, coins.
8. Check that the exit is reachable and that every room is big enough for its vision.

## Open

- **Categories per level:** Oleg's "at least one of each" against balance's fewer.
- **Guardians:** whether a location guardian stands on every non-boss level.
- **Ligeia:** the island's size and approach.
- **Secrets:** the dwarf pantry full of liquids (lore) or empty (balance).
- **Day and night:** which surface exits exist on maps 1, 4 and 5.
- **Arenas:** frequency and the bypass cost (balance's next report).
- **Not covered yet:** how a hidden entrance is found or shown; hazards and their placement; the difficulty curve across levels.
- The level economy (enemies, corpses, river, flasks) is checked by the simulator, not by this note.
