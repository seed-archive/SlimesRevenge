# Reference: Sword of the Stars: The Pit

A breakdown of the game the user names as the model for Slime's Revenge's run structure. Facts carry their source; anything the sources do not confirm is listed separately and must be checked by playing before a design decision rests on it. This is a reference for ideas, not a template to copy: Slime's Revenge takes mechanics in spirit, never names, content or look.

Researched on 2026-11-25. Sources are listed at the end.

## The game in short

- A 2D turn-based roguelike dungeon crawler by Kerberos Productions, set in their *Sword of the Stars* universe. Released in 2013 for Windows, macOS and Linux [W].
- Goal: descend to the bottom of the Pit and find a cure for a plague that has hit humanity; the story is told mostly through text and messages found on the way [W][R].
- Reviews: mixed at release (too safe, unfair difficulty); after years of updates seen as a solid roguelike with addictive "one more try" play [W][R][M].

## Structure of a run

- **Floors:** 30 in the original game, 40 with the expansions; deeper floors are harder, larger and more complex [W][R].
- **Generation:** every floor is procedurally generated; fog of war and a limited field of view hide what is not seen [W][R].
- **On a floor:** enemies, containers and locked ammo crates that need lockpicking, broken devices to repair [W][R]. Each floor has a hatch down to the next, monsters get tougher as you go [3].
- **Scenery changes with depth:** caves first, an alien facility deeper [R]. Slime's Revenge wants the same feeling of travel (vision: sewers → surface → town → troll cave).
- **Save and stash point every five floors** — reported by a player review, which also says items and experience can be dumped there for another character [M]. This matches the user's memory of "passing things from one character to the next", but only one source states it.

## Time and actions

- Turn-based: time is counted in rounds (Wikipedia) or "moments" (Giant Bomb), and every action spends time [W][R][G]. Giant Bomb's example: walking two steps passes one moment [G].
- Neither source says how many actions a character gets per turn, whether actions cost different amounts of time, or whether there is a speed stat.

## Survival pressure

- **Two meters, both lethal:** health and hunger; if either runs out, the character dies [W].
- **Hunger grows with time** [Rev]; starting rations run out fast and food has to be scrounged from containers [R]. Some floors in a row may have no food at all [Rev].
- **Ammunition is scarce:** players learn to count every bullet [M]; weapons and armour wear out (durability) [R].
- **Status effects** must be treated [R].

Why it works: time itself costs something, so wandering, retreating and waiting are never free. Slime's Revenge has the same lever built in — the body is both health and ammunition — and does not need a hunger meter on top.

## Character

- **Classes:** 3 in the original (Scout, Engineer, Marine), 12 with expansions; each starts with its own stats, gear and food. The Marine leans on melee might, the Engineer on brains (for example, spotting traps) [W].
- **Attributes:** might and brains are named by Wikipedia [W]; guides add finesse [Rev].
- **Experience:** every task, a kill included, gives experience; on a level-up the player spends points on stats and skills (lockpicking, deciphering, weapon skills, foraging) [W][R].

## Crafting and the meta-game

- Weapons, lockpicks, food and medicine are crafted from junk found in the dungeon [R].
- **Recipes are found as messages**; a Decipher skill decides how much of one is readable; **deciphered recipes stay known for future runs** — the game's meta-progression [R].
- Reviews call crafting deep but opaque: no hints, no recipe list [Rev].

## Not confirmed by the sources

Check these by playing before relying on them:

1. **"Four actions per turn."** No source describes a fixed number of actions per turn.
2. **A shotgun that fires all its pellets with one shell loaded.** No source covers shell loading or pellet counts.
3. **Ways to skip floors.** Not mentioned.
4. **Difficulty modes or an Iron Man switch.** Not mentioned; permadeath is implied by the reviews.

## What Slime's Revenge takes

| From The Pit | In Slime's Revenge | Where it is designed |
| --- | --- | --- |
| Long descent through generated floors, harder with depth | ~40 floors of generated rooms; now one generated dungeon for playtests | [vision.md](../vision.md), progression.md |
| Scenery that changes with depth | Sewers → surface → town → troll cave | [vision.md](../vision.md), levels.md |
| Save / stash point every few floors, passing things to the next character | Floor checkpoints and a way to hand something to the next slime (open) | progression.md |
| Time is a resource; scarce ammunition | The body is health and ammunition at once; every action and every unit counts | [vision.md](../vision.md) pillars *The body is the resource*, *Thrift is skill* |
| Fog of war, limited field of view | Walls block sight; vision range matters on Hard | [turn-structure.md](../turn-structure.md) |
| Knowledge that survives death (recipes) | Candidate: discovered liquid combinations stay known across runs | growth.md, progression.md |

## What Slime's Revenge does not take

- **Hunger as a second meter.** Volume already does this job; a second meter would hide the core trade-off.
- **Experience points and stat levels.** The vision rules out stat grinding: the slime grows by volume and affinity.
- **Opaque crafting.** Chemistry must be readable (pillar *Readable in one glance*); combinations are shown, not guessed.
- **Classes.** Not for now: the slime's "class" is the liquid it carries most of (affinity).

## Sources

- [W] Wikipedia, *Sword of the Stars: The Pit* — https://en.wikipedia.org/wiki/Sword_of_the_Stars:_The_Pit
- [R] RPGFan review (2019) — https://www.rpgfan.com/review/sword-of-the-stars-the-pit/
- [G] Giant Bomb wiki — https://giantbomb.com/wiki/Games/Sword_of_the_Stars_The_Pit
- [M] Metacritic critic and user reviews — https://www.metacritic.com/game/sword-of-the-stars-the-pit/user-reviews/
- [3] 3rd-strike.com review — https://3rd-strike.com/sword-of-the-stars-the-pit/
- [Rev] Other reviews and guides found in the search, not checked claim by claim: Gamer Living (https://gamerliving.net/review/sword-of-the-stars-the-pit-review/), GamesReviews.com (https://gamesreviews.com/pc/pc-reviews/07/sword-of-the-stars-the-pit-review/), Steam guides (https://steamcommunity.com/sharedfiles/filedetails/?id=131472299, https://steamcommunity.com/sharedfiles/filedetails/?id=863211390).
