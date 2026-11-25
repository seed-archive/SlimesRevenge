# Vision

**Status:** Draft. Written from the game as it exists and from the user's brief (2026-11-25). Everything marked *Proposed* waits for the user's yes; the open decisions are at the end.

## Pitch

A turn-based tactics game on a small grid, for phones. You are a slime, and your body is a jar of liquids. Every hit you throw, every trap you lay, every step you take through danger is paid for with your own body — and every enemy you kill can be drunk back in.

## The fantasy

You are small, soft and underestimated. You do not win by being stronger; you win by being *wetter*: pouring oil where the enemy will walk, setting it alight, letting the monsters fight each other and then drinking what is left. You end the run as a different slime from the one you started with — bigger, and made of something else.

## Pillars

Every mechanic must serve at least one pillar. A feature that serves none is cut.

1. **The body is the resource.** Volume is life, ammunition and identity at once. Spending is always a trade: a strike costs a unit of you.
2. **Chemistry over arithmetic.** Liquids react with each other and with the world (water puts out fire, oil makes fire worse, acid eats armor). A clever combination beats a big number.
3. **You are what you drink.** The liquid you carry most of shapes what you are (affinity). What you eat changes what you can do.
4. **Make the room fight for you.** Traps, puddles, enemies that hate each other. The best fight is one where the slime barely gets touched.
5. **Readable in one glance.** Small grids, few units, turn-based, every effect visible on the board. A turn is a decision, not a reflex.

## Core loop

*Proposed*, built from what the game already does plus the brief:

1. **Enter a room.** See the enemies, the liquids on the floor, the fountains.
2. **Plan.** Choose what to spend: a strike, a spit, a puddle as a trap, or simply moving into a better place.
3. **Fight.** Turn by turn; enemies react by their character (aggressive, passive, cowardly) and by whom they hate or fear.
4. **Kill and eat.** Corpses and their blood refill the volume and can change its mix.
5. **Drink and shape.** Fountains and puddles let you choose which liquid to grow; the dominant liquid gives an affinity.
6. **Leave stronger or different**, into the next room.

The loop has a built-in tension: spending makes you smaller and weaker, eating makes you bigger but takes turns, and the liquid you eat is not always the one you want.

## Platform and controls

- iOS and Android, landscape. Tap a cell for a context menu; swipe or tap an adjacent cell to move. Keyboard on desktop for development.
- Short sessions: *Proposed* one room in 1–3 minutes, a run in 15–30.
- Pixel art in the 32 px Fantasy Dreamland style already used for the tiles and creatures.

## What the game is not

*Proposed*:

- Not real-time and not about reflexes.
- Not a stat-grinding RPG: no levels or experience points; growth is volume and affinity.
- Not a story-heavy game: the story frames the run, it does not stop it.

## Open decisions

1. **Run structure.** A roguelike run (generated rooms, death ends the run) or a handcrafted campaign of levels, or both? The room generator in the brief points to a roguelike.
2. **The revenge.** Who wronged the slime and why does it want revenge? This sets the enemies, the places and the final goal.
3. **Death.** Hardcore (a defeat ends the run) as the main mode and Softcore as an easier option, or the other way round?
4. **Tone.** Dark and grim, or cute and funny? The current art and creatures (bats, mushrooms, mandragoras) fit both.
