# Progression

**Status:** Draft (2026-11-25) — mostly ideas. Only the sections and items marked **Agreed** are decided; everything under "Ideas" is recorded so it is not lost and will be worked out later.

What changes between runs and inside a run beyond one room: worlds, difficulty, what carries over, what can be handed to the next slime.

## Agreed

- **The long descent** through generated floors (~40) is the big idea; now: the arena and one generated dungeon ([vision.md](vision.md)).
- **Softcore is the main mode; Iron Man** (permadeath) is chosen at the start of a game and cannot be turned off ([vision.md](vision.md)).
- **Difficulty levels** Easy / Normal / Hard are defined by how many enemies a face-tanking slime survives. To get there they differ in practically every parameter: enemy vision range, number of enemies, enemy toughness (hit points), and any other parameter that turns out to matter. The amount per level comes from the balance analysis ([vision.md](vision.md)).
- **No separate survival meter.** No hunger or similar clock: spending the body to kill already is the survival pressure. (The reference game's hunger is deliberately not taken: [references/the-pit.md](references/the-pit.md).)
- **Meta-progression is wanted.** Something should carry over between runs; what exactly is open (ideas below).

## Mechanic ladder

**Agreed** (2026-11-25).

The game grows like a tower: one level at a time, each built on the one below it. Here a *level* is a stage of the descent, not a difficulty level (Easy / Normal / Hard).

- **Why.** The game needs progression. A slime that has every mechanic on level 1 leaves the player not knowing what to do and with no reason to go to level 2.
- **Level 1 — the minimum kit**, balanced first: the slime has Water only and a Water affinity from the start; it can move, strike with Water, drink and eat corpses. No other liquid, no puddles, no spit. Whether level 1 has fountains follows from the level-economy calculation (simulator + balance, pending): [growth.md](growth.md).
- **Every next level adds one new mechanic** that the level cannot be passed without. Candidates:
  - a new liquid;
  - poison against blood;
  - setting on fire;
  - putting fire out;
  - pouring puddles (not on level 1; it unlocks on a later level);
  - spit with range 1, 2 or 3 cells, the range set by balance.
- **Spit comes late on purpose:** against weak enemies it is far too strong.
- **Not decided yet:** the exact order of the mechanics, which level each unlocks on, and every number. They come from the balance analysis, which is not done yet ([balance.md](README.md)).

## Ideas

Not decided. Each needs a purpose, a cost and a balance check before it becomes a design.

### Worlds of one liquid

- Replayability through different **worlds**, each richer in one liquid: a water world, a lava world, and so on. Since the slime's body is made of what it drinks, a world shapes which builds are easy in it.
- At the **end of each world** the slime obtains something of that world — for example a *lava essence*.
- What essences are for is open: a currency, a key, a collection ("gather five essences, get a reward"), something given to someone.

### The slime's origin liquid

- **Locked first unit:** a run starts with one unit that cannot be spent — the slime's origin. A water-born slime gets a bonus to water builds; on the next run a lava-born slime gets a bonus to lava builds.
- **Alternative without locking:** the slime simply has an innate affinity to one liquid or element for the run.
- Either way the origin could be the meta-progression: origins unlocked by finishing worlds or collecting essences.

### Passing things on

- A **chest** in a safe place (the town, the sewers) that survives between characters.
- A **personal fountain**: pour into it the liquids that do not fit the current build, drink them back later or leave them to the next slime.
- The reference game's save-and-stash point every few floors is the model ([references/the-pit.md](references/the-pit.md)).

### Knowledge that survives death

- From the reference game: discovered things stay known in later runs. Candidate: liquid combinations and enemy weaknesses, once seen, are shown in the info cards from then on.

## Open questions

Recorded, not asked yet — they wait until volume, affinity and growth are designed:

1. What essences are for.
2. Origin liquid: locked first unit, or an innate affinity without locking.
3. Chest, personal fountain, or both; where they stand.
4. Mechanic ladder: the order of the mechanics, the level each unlocks on, spit range — from the balance analysis.
