# Game design document

The design of Slime's Revenge, from the idea down to formulas. It is written before the code: a mechanic is designed and balanced here, then built. The implementation notes in `docs/` (statuses, puddles, armor, AI) describe what the code does today; when design and code disagree, the section says so.

## How to read a section

Every section opens with a status line:

| Status | Meaning |
| --- | --- |
| **Not started** | The section is listed so it is not forgotten. Nothing is decided. |
| **Draft** | Written, open for discussion. Decisions marked *Proposed* are suggestions, not agreements. |
| **Working** | A provisional rule or number used by balance until the user agrees or replaces it. |
| **Agreed** | The user approved it. Code may be built from it. |
| **Implemented** | Agreed and in the game; links to the implementation notes. |

Open decisions are listed at the end of each section and repeated in the agent's question list until answered.

## Sections

Written in this order: each layer depends on the one above it.

| # | Section | Covers | Status |
| --- | --- | --- | --- |
| 1 | [vision.md](vision.md) | The idea: pitch, story, pillars, run structure, death and difficulty, tone, core loop | Agreed |
| 2 | [turn-structure.md](turn-structure.md) | Turns, initiative, speed, what one action costs, vision | Agreed |
| 3 | [volume.md](volume.md) | The slime's body: units, capacity, volume = life = ammo, layered vs proportional body, how size grows | Draft — body order and swap Agreed |
| 4 | [substances.md](substances.md) | Every liquid today; stacking (strength, not duration; Flammable additive); one puddle per cell; the two cycles (strengthens / suppresses, after *wu xing*); enemy vulnerability from body liquid | Draft — stacking and puddles Agreed; 4 open decisions |
| 5 | [actions.md](actions.md) | Everything the slime does: move, strike, the last-drop strike, spit (ranged), drop a puddle / set a trap, gather (drink / collect), swap, devour, wait | Draft — Gather (drink = collect) and Swap Agreed; last-drop strike Working |
| 6 | affinity.md | Slime affinity (one liquid outweighs the others: thresholds, tiers, perks) and enemy affinities | Not started |
| 7 | [growth.md](growth.md) | Gaining volume: net body per fight, eating corpses and blood, collecting puddles, fountains (flasks and tanks, rate, which liquids), growing capacity | Draft — net-body targets, swapping liquids, the last unit (spawn pool / reserve fountain), drink/eat in the level-1 kit, fountain kinds Agreed; numbers from balance (pending) |
| 8 | combat.md | Damage, armor, death, corpses, decay, the kill → eat loop | Not started |
| 9 | statuses.md | Every status as a design (purpose, counterplay); today's catalog is [status-effects.md](../status-effects.md) | Not started |
| 10 | [enemies.md](enemies.md) | The roster: role, abilities, affinity, behavior archetype, what each drops; a template for new enemies | Draft — no groups and the dash Agreed; fast enemies' low HP working |
| 11 | [levels.md](levels.md) | The room generator: room shapes, corridors, spawners and placement of enemies, fountains and hazards, difficulty curve; first target is one dungeon for playtests | Draft — spawners, room size and vision, level size grows Agreed |
| 12 | [progression.md](progression.md) | The mechanic ladder (one new required mechanic per level; spit on level 2), the long descent (~40 floors), difficulty levels and Iron Man, worlds of one liquid, essences, origin liquid, chest and personal fountain, what carries over | Draft — Mechanic ladder Agreed, the rest mostly ideas |
| 13 | [balance.md](balance.md) | Formulas and numbers in one place, the balance spreadsheet, how a change is checked | Draft — level-1 working numbers (not final) |
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
