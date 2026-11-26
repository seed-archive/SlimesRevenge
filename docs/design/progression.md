# Progression

**Status:** Draft (2026-11-26) — partly ideas. Only the sections and items marked **Agreed** are decided; everything under "Ideas" is recorded so it is not lost and will be worked out later.

What changes between runs and inside a run beyond one room: worlds, difficulty, what carries over, what can be handed to the next slime.

## Agreed

- **The run** is 20 levels on 5 maps ([route.md](route.md)); now: the arena and one generated dungeon ([vision.md](vision.md)).
- **Softcore is the main mode; Iron Man** (permadeath) is chosen at the start of a game and cannot be turned off ([vision.md](vision.md)).
- **Difficulty levels** Easy / Normal / Hard are defined by how many enemies a face-tanking slime survives. To get there they differ in practically every parameter: enemy vision range, number of enemies, enemy toughness (hit points), and any other parameter that turns out to matter. The amount per level comes from the balance analysis ([vision.md](vision.md)).
- **No separate survival meter.** No hunger or similar clock: spending the body to kill already is the survival pressure. (The reference game's hunger is deliberately not taken: [references/the-pit.md](references/the-pit.md).)
- **Meta-progression is wanted.** Something should carry over between runs; what exactly is open (ideas below).

## Mechanic ladder

**Agreed** (2026-11-25).

The game grows like a tower: one level at a time, each built on the one below it. Here a *level* is a stage of the run, not a difficulty level (Easy / Normal / Hard).

- **Why.** The game needs progression. A slime that has every mechanic on level 1 leaves the player not knowing what to do and with no reason to go to level 2.
- **Level 1 — the minimum kit**, balanced first: the slime has Water only and a Water affinity from the start; it can move, strike with Water, drink and eat corpses. No other liquid, no puddles, no spit. Fountains on level 1: working number 1 flask ([balance.md](balance.md)), final from the level-economy calculation (pending): [growth.md](growth.md).
- **Every next level adds one new mechanic** that the level cannot be passed without. Candidates:
  - a new liquid;
  - poison against blood;
  - setting on fire;
  - putting fire out;
  - pouring puddles (not on level 1; it unlocks on a later level);
  - spit with range 1, 2 or 3 cells, the range set by balance — taken: level 2.
- **Spit is the level-2 mechanic** (Agreed 2026-11-25): one new mechanic per level, and spit is the first after the level-1 kit.
- **Balance computes two levels at once**, not only level 1 (Agreed 2026-11-25; [balance.md](balance.md)).
- **Not decided yet:** the exact order of the mechanics after spit, which level each unlocks on, and every number. They come from the balance analysis, which is not done yet ([balance.md](balance.md)).

## Past slimes

**Agreed:** a past slime lies as a puddle on the level where it died; one unit of its affinity liquid can be collected from it. **Working** (balance, `balance/past-slimes.md`): one liquid puddle per level (the last death on that level and difficulty), 1 unit, once a run; up to 3 dated traces without liquid.

**Rejected:** a fountain of inheritance in the cellar. Lava or Mercury from the last run on level 1 makes the first levels mindless; like *The Pit*, inheritance must be limited to the level it belongs to.

## Secrets

**Agreed.**

- **A secret location** that does not always appear and may or may not give a bonus. Example: a dwarf smithy where the whole body can be swapped to Lava for free. One per game, a different place each time, with an unnoticeable entrance.
- **Easter eggs are a little unbalanced but never break the game:** the bonus is comfort and speed for what is reachable anyway, not new power. A big bonus is fine if the next map takes it away (the principle: the counterweight takes exactly the advantage that would break the next map).
- **Secret rooms with chests:** a couple of chests with some chance, in a secret room that is either "not very secret" (the entrance is visible) or truly hidden. A chest may be a **mimic**: Easy 25 %, Normal 50 %, Hard 75 %; on Hard a real chest must give something very good (maybe Mercury). The mimic fight is in a small closed room: no running out, no kiting.
- **Working** (lore and balance, `lore/easter-eggs.md`, `balance/easter-eggs.md`): the list of secret places with chances; nothing triggers by accident.

## Achievements

**Agreed:** "10 different liquids in the slime"; "Puke over the map" — a puddle on every cell of a level, or simply 100 cells with puddles on a map.

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
4. Mechanic ladder: the order of the mechanics after spit, the level each unlocks on, spit range — from the balance analysis.
