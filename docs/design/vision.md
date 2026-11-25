# Vision

**Status:** Agreed (2026-11-25). Parts still marked *Proposed* are suggestions the later sections may change.

## Pitch

A turn-based tactics roguelike on a small grid, for phones. You are a slime, and your body is a jar of liquids. Every hit you throw and every trap you lay is paid for with your own body — and every enemy you kill can be drunk back in.

## Story

**Agreed** (details still open):

- Everyone has wronged the slime: in every game the weakest monster, the one you farm on the first level, is a rat or a slime.
- An alchemist (or a mage — both brew potions) lives in a tower and pours spent potions down a hole in the floor. It all flows into the cellar, mixes, and becomes a slime: you.
- The goal of the game: get through the dungeon and reach the alchemist. What happens then — a talk, revenge, eating something out of him — is not decided and does not block anything.

## The fantasy

You are small, soft and underestimated. You do not win by being stronger; you win by being *wetter*: pouring oil where the enemy will walk, setting it alight, letting the monsters fight each other and then drinking what is left. You end the run as a different slime from the one you started with — bigger, and made of something else.

## Pillars

Every mechanic must serve at least one pillar. A feature that serves none is cut.

1. **The body is the resource.** Volume is life, ammunition and identity at once. Spending is always a trade: a strike costs a unit of you.
2. **Chemistry over arithmetic.** Liquids react with each other and with the world (water puts out fire, oil makes fire worse, acid eats armor). A clever combination beats a big number.
3. **You are what you drink.** The liquid you carry most of shapes what you are (affinity). What you eat changes what you can do.
4. **Make the room fight for you.** Traps, puddles, enemies that hate each other. The best fight is one where the slime barely gets touched.
5. **Thrift is skill.** **Agreed.** Like a shotgun that fires three pellets whether you load one shell or three, a smart player gets the same effect for less of their body: the right puddle in the right cell, one spit that hits two enemies, an enemy lured into another's path. The actions and balance sections must leave room for such savings.
6. **Readable in one glance.** Small grids, few units, turn-based, every effect visible on the board. A turn is a decision, not a reflex.

## Run structure

**Agreed.**

- **The big idea:** a long descent through generated floors, in the spirit of *Sword of the Stars: The Pit* (the user's reference: about 40 floors). There are ways to skip floors and ways to pass something on from one character to the next. Designed in [progression.md](progression.md); not built now.
- **Now:** the arena (the duel, already in the game) and a generator of one dungeon, so the game can be played and tested end to end.
- **Difficulty grows by required mechanics:** every next level of the descent adds one mechanic the level cannot be passed without, starting from move and strike with Water — the [Mechanic ladder](progression.md#mechanic-ladder). This matches "Difficulty changes what the player has to do" below.

## Death and difficulty

**Agreed.**

- **Softcore is the main mode.** On defeat the slime reappears next to the spawning pool and loses nothing. The spawn has an unlimited Water pool; Iron Man has a reserve fountain of about 20 Water instead ([growth.md](growth.md#the-last-unit)).
- **Iron Man** is permadeath: a defeat ends the run. It is chosen when a new game starts and stays on for that whole game; it cannot be turned off later.
- **Difficulty changes what the player has to do.** Each difficulty level is defined by how many enemies at once a slime that just stands and trades hits ("face-tanks") survives — the table below is the target every level must meet.
- **The levers: practically every parameter differs between levels**:
  - enemy vision range;
  - the number of enemies;
  - enemy toughness (hit points);
  - any other parameter that turns out to matter.
- How much each lever adds per level is not decided: the balance analysis computes it so that each level meets the table.
- **The reference player** is a newcomer who walks up to a new mob and takes its hits to learn it. On Easy he survives them all and wins. On Hard a new tough mob must not be approached: one hit takes half his life.

| Level | Face-tanking survives | Dies to | What the player must do |
| --- | --- | --- | --- |
| Easy | 1–2 enemies at once | 3 | Walk into a room, beat everyone, collect the liquid, move on. |
| Normal | 1 enemy | 2 | Avoid fighting two at once; use terrain and puddles sometimes. |
| Hard | none | 1 | Use the mechanics: pull enemies one by one, manage their vision range, lay a slowing puddle while fleeing, set traps, spend the body sparingly. |

Open: enemies now come one at a time from spawners, with no packs ([enemies.md](enemies.md#no-groups)); how "enemies at once" in this table fits that is not decided yet.

This table is the first balance target: [balance.md](README.md) must make it true by numbers, and a playtest of a room with 1, 2 and 3 enemies must confirm it.

## Tone and setting

**Agreed** direction, details open:

- Between cartoon and grim, leaning to cartoon. Not horror: mechanics that read as creepy are out (for example, swallowing a living creature whole and digesting it while it is alive).
- The run starts in the cellar and the sewers under the tower, then the scenery changes so it never feels like one long dungeon: the surface and grass, a town, a troll's cave. The terrain art already allows several biomes.
- **Enemies are fantastic, not ordinary animals.** A rat, a cat and a dog as the first enemies do not sell the game: players choose a slime game for the unusual. The current monster art is to be redrawn; the slime itself reads as a jelly pudding and stays.

## Core loop

**Agreed:** the core loop is **swapping liquids and capitalising on them**. Every fight should net body: the slime comes out bigger, and able to replace at least one unit it has with a better one. A fight that nets nothing starts a downward spiral. Targets per difficulty level are in [growth.md](growth.md); the numbers come from balance (pending).

The steps below are *Proposed*, built from what the game already does plus the brief; they predate the spawner rule ([levels.md](levels.md)) and will be rewritten with it:

1. **Enter a room.** See the enemies, the liquids on the floor, the fountains.
2. **Plan.** Choose what to spend: a strike, a spit, a puddle as a trap, or simply moving into a better place.
3. **Fight.** Turn by turn; enemies react by their character (aggressive, passive, cowardly) and by whom they hate or fear.
4. **Kill and eat.** Corpses and their blood refill the volume and can change its mix.
5. **Drink and shape.** Fountains and puddles let you choose which liquid to grow; the dominant liquid gives an affinity.
6. **Leave stronger or different**, into the next room.

The loop has a built-in tension: spending makes you smaller and weaker, eating makes you bigger but takes turns, and the liquid you eat is not always the one you want.

## Platform and controls

- iOS and Android, landscape. Tap a cell for a context menu; swipe or tap an adjacent cell to move. Keyboard on desktop for development.
- Short sessions: *Proposed* one room in 1–3 minutes, a floor in 10–15.
- Pixel art in the 32 px Fantasy Dreamland style already used for the tiles and creatures.

## What the game is not

*Proposed*:

- Not real-time and not about reflexes.
- Not a stat-grinding RPG: no levels or experience points; growth is volume and affinity.
- Not a story-heavy game: the story frames the run, it does not stop it.
