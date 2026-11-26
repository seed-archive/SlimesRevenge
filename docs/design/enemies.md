# Enemies

**Status:** Draft (2026-11-26). Items marked **Agreed** are decided; the bestiary and the guardian roster are lore and balance drafts (**Open**). Today's roster in the code: [creatures.md](../creatures.md). Detail: the design notes `lore/bestiary.md`, `lore/names.md`, `lore/guardians-structure.md`, `lore/unique-guardians.md`, `lore/locations.md`, `balance/bestiary-numbers.md`, `balance/ranged.md`.

## No groups

**Agreed** (2026-11-25). There are no enemy groups or packs (the game is not XCOM). Enemies are roaming singles from spawners. The slime can aggro several at a time (by running through the map, or by standing where more than one sees it); luring them one by one is the counter ([levels.md](levels.md)). Enemies met in a corridor count as singles.

## Fast enemies

- **Agreed:** a fast enemy may move and bite in one turn (a dash).
- **Working, pending balance:** such enemies have low hit points, so the slime can kill one in a turn and step back. This is Oleg's assumption; balance checks it.
- Their speed and hit points come from balance (pending).

## What enemies are

**Agreed.**

- **Unusual, not ordinary animals.** Players pick a slime game for the unusual; the current mobs are to be redrawn. Interesting enemies per biome, not the ones we happen to have art for.
- **Every next enemy is the reason for a new mechanic** (an eye with spit, a leech with poison, a fire creature to put out).
- **The slime is the only slime:** no slime cousins, as enemies or in the story.
- **Nothing is animated but the slime and the golem.** No living flasks or homunculi. The golem is one, in the finale.
- **A mimic is only a chest** ([progression.md](progression.md#secrets-and-quick-endings)).
- **Mechanical enemies only with the dwarves** (maybe in a town).
- Swollen corpses, centipedes and beetles, the fire salamander, sirens, the sulfur beetle: yes. Oleg also: plants and mushrooms as many as needed; most likely no fighting fish (an idea).
- **One-word names** that read in English and Russian, from the genre and myths (Bat, Leech, Mold, Scolopendra, Automaton, Siren, Banshee, Skeleton, Ghoul, Mimic…). The list: `lore/names.md`.

## Enemy types

**Open** (Oleg's idea and direction; lore types in `lore/bestiary.md`).

- **A type is what you can take from it:** the living are made of Blood and give Blood; undead bones give nothing; plants give Resin or Acid; mushrooms something like Acid.
- **Enemies against builds:** for each of the 10 combat liquids at least one enemy is weak to it, one is immune (not invulnerable) and about three are normal. As many types as possible; **every level has at least one enemy of each type** present on its map.
- Lore's types: Living, Insects, Acid beetles, Crustaceans, Plants, Mushrooms, Fire, Water, Dwarf mechanisms, Undead bones, Undead corpses, Mimic, Magical. Each has an element; it is immune to its element and weak to the liquid that suppresses it.
- **Open:** who the magical creatures are (fairies, noon wraiths, wisps are candidates; noon wraiths walk the mill fields by day).

## The bestiary

**Open** (lore with balance numbers). About ten species per map, rotated 3–5 types per level: for example Bat, Leech, Mold, Undine, Vine, Scolopendra, Toadstool, Automaton, Scarab and the Sentinel on map 1. Full table: `lore/bestiary.md`; numbers: `balance/bestiary-numbers.md`.

## Ranged enemies

**Agreed:** few shooters, and they are heavy. Nobody else has a ranged attack.

- **Ballista** (Oleg's direction): not a mob but a static trap, like a tower in tower defense — a dwarf crossbow in a gallery that fires a bolt when the slime enters its line. Mercury on the edge stops the bolt; Ectoplasm lets it pass through.
- Lore's other shooters: the Toad (spits acid, maps 2 and 4) and the Skeleton archer (maps 3 and 5); crossbowmen of the squad on Hard.
- **Working** (balance, `balance/ranged.md`): a shot instead of a step or bite, damage = the law's bite − 1; at most one shooter per level, none on levels 1–2 or at guardians; the Ballista on map 1 levels 3–4, 2 units off the edge at up to 4 cells in a straight line (Easy 1).

## Guardians and bosses

**Agreed:** the last level of a place has a guardian. **A unique mob is a guardian with its own name that guards something** (needs heavy balancing).

**Open** (Oleg's direction to lore):

- **A boss on every map, a guardian at every location.** Location guardians are static, like towers in tower defense, standing at the exit (the sluice the slime dives through).
- **Three ways past a guardian:** fight (risk, but loot: what it guards plus what it is made of), bargain (give what it asks), or a secret bypass (no fight, but part of the volume is lost in advance). An arena may or may not appear in a run.
- **Bosses:** the Dwarf Guardian (mine), Ligeia (village; a siren on an island in the reservoir behind the dam, guards the dam's stone), the Strzyga (graveyard; fights herself, then turns into a huge bat), the Rat King (tunnels; spawns 1-HP rats that are Living and give Blood), the golem and the alchemist (finale).
- Lore examples of bargains: Pan in the grove takes dwarf ale and gives Mercury; Ligeia takes amber and opens the dam without a fight.
- Numbers (which body passes, toughness and loot, the price of a bypass): balance, one report with the guardians.
- Hunters and the squad: [story.md](story.md#hunters-and-the-alchemists-squad).
