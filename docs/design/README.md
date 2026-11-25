# Game design document

The design of Slime's Revenge, from the idea down to formulas. It is written before the code: a mechanic is designed and balanced here, then built. The implementation notes in `docs/` (statuses, puddles, armor, AI) describe what the code does today; when design and code disagree, the section says so.

## How to read a section

Every section opens with a status line:

| Status | Meaning |
| --- | --- |
| **Not started** | The section is listed so it is not forgotten. Nothing is decided. |
| **Draft** | Written, open for discussion. Decisions marked *Proposed* are suggestions, not agreements. |
| **Agreed** | The user approved it. Code may be built from it. |
| **Implemented** | Agreed and in the game; links to the implementation notes. |

Open decisions are listed at the end of each section and repeated in the agent's question list until answered.

## Sections

Written in this order: each layer depends on the one above it.

| # | Section | Covers | Status |
| --- | --- | --- | --- |
| 1 | [vision.md](vision.md) | The idea: pitch, story, pillars, run structure, death and difficulty, tone, core loop | Agreed |
| 2 | [turn-structure.md](turn-structure.md) | Turns, initiative, speed, what one action costs, vision | Agreed |
| 3 | volume.md | The slime's body: units, capacity, volume = life = ammo, layered vs proportional body, how size grows | Not started |
| 4 | [substances.md](substances.md) | Every liquid today; stacking (strength, not duration; Flammable additive); one puddle per cell; the two cycles (strengthens / suppresses, after *wu xing*); enemy vulnerability from body liquid | Draft — stacking and puddles Agreed; 4 open decisions |
| 5 | actions.md | Everything the slime does: move, strike, spit (ranged), drop a puddle / set a trap, collect, devour, wait | Not started |
| 6 | affinity.md | Slime affinity (one liquid outweighs the others: thresholds, tiers, perks) and enemy affinities | Not started |
| 7 | [growth.md](growth.md) | Gaining volume: net body per fight, eating corpses and blood, collecting puddles, fountains (flasks and tanks, rate, which liquids), growing capacity | Draft — net-body targets, the last unit (spawn pool / reserve fountain), drink/eat in the level-1 kit, fountain kinds Agreed; numbers from balance (pending) |
| 8 | combat.md | Damage, armor, death, corpses, decay, the kill → eat loop | Not started |
| 9 | statuses.md | Every status as a design (purpose, counterplay); today's catalog is [status-effects.md](../status-effects.md) | Not started |
| 10 | [enemies.md](enemies.md) | The roster: role, abilities, affinity, behavior archetype, what each drops; a template for new enemies | Draft — no groups, fast enemies Agreed |
| 11 | [levels.md](levels.md) | The room generator: room shapes, corridors, spawners and placement of enemies, fountains and hazards, difficulty curve; first target is one dungeon for playtests | Draft — spawners, not packs Agreed |
| 12 | [progression.md](progression.md) | The mechanic ladder (one new required mechanic per level), the long descent (~40 floors), difficulty levels and Iron Man, worlds of one liquid, essences, origin liquid, chest and personal fountain, what carries over | Draft — Mechanic ladder Agreed, the rest mostly ideas |
| 13 | balance.md | Formulas and numbers in one place, the balance spreadsheet, how a change is checked | Not started |
| 14 | art-brief.md | The image-generator spec: sheet layout, palette, per-creature brief template, so another model can draw every asset | Not started |
| 15 | tools.md | Game-design tools used here and why (spreadsheets, flow and economy modelling, level editors) | Not started |
| 16 | [design-guard.md](design-guard.md) | Code that plays fights by the real rules and checks which builds work on which difficulty and why; agreed design rules as CI checks | Draft — first version waits for approval |

## References

Breakdowns of games the design borrows ideas from (mechanics in spirit, never content or look):

- [references/the-pit.md](references/the-pit.md) — *Sword of the Stars: The Pit*: run structure, time, survival pressure, meta-progression.

## What exists in the game today

So the design starts from facts, not from memory (checked against the code on 2026-11-25):

- **Slime:** volume of up to `Volume.Capacity` units of liquid; volume is life (`IsAlive` while units > 0). Starts with 2 Water, Oil, Poison, Acid, Lava. Speed 1.
- **Liquids:** Water, Oil, Poison, Acid, Blood, Lava, Mercury. Each has a strike effect and a puddle effect; a liquid at ≥ 80% of the volume gives a dominance perk ([slime-dominance.md](../slime-dominance.md), [status-effects.md](../status-effects.md)).
- **Actions:** move one cell (8 directions); Attack an adjacent creature by spending one unit of a chosen liquid; Mess — pour one unit as a puddle on the own cell; Collect a puddle from the own cell; Devour a corpse (takes as many turns as the corpse has units) ([cell-menu.md](../cell-menu.md)).
- **Not in the game:** ranged spit, traps beyond a puddle, fountains, capacity growth, a room generator (maps are a fixed 10×10 grass field and a 15×5 duel strip).
- **Enemies:** Bat (Vampirism, 2 Blood) and five placeholders — Mushroom, Undine, Cactus, Mandragora, Eye — each 5 HP, 1 Water, no abilities. Rat, Cat, Dog, Scorpion are retired from the roster but kept in code and tests ([creatures.md](../creatures.md), [behavior.md](../behavior.md)).
- **Modes:** campaign and duel; Hardcore only in practice ([softcore-revive.md](../softcore-revive.md)).
