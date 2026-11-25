# Growth

**Status:** Draft (2026-11-25). The items below are **Agreed**; every number comes from balance (pending). The rest of the section (eating rates, collecting puddles, growing capacity) is not written yet.

How the slime gains body: drinking, eating corpses, fountains.

## Every fight nets body

**Agreed** (2026-11-25).

- The core loop is swapping liquids and capitalising on them ([vision.md](vision.md#core-loop)).
- Every fight should net body, and let the slime replace at least one unit it already has.
- A fight that nets nothing starts a downward spiral.
- Open: the Hard target allows 0, while a 0 fight starts the spiral; how the two fit is not decided yet.

| Difficulty | Net body per fight (target) |
| --- | --- |
| Easy | +1…+2 units |
| Normal | +1 unit |
| Hard | 0…+1 unit |

Targets, not rules of the code. Exact numbers come from balance (pending).

## The last unit

**Agreed** (2026-11-25).

- When the body is down to one unit, the fountains are empty and enemies remain, the slime still has to go and kill them itself. The rule that makes this possible comes from balance (pending).
- **Softcore:** an unlimited Water pool at the spawn.
- **Iron Man:** no unlimited Water; a reserve fountain of about 20 Water instead (number from balance, pending).

## Drinking and eating

**Agreed** (2026-11-25). Drinking and eating corpses belong to the level-1 kit: move, strike, drink, eat ([progression.md](progression.md#mechanic-ladder)). Eating is the Devour action. Open: whether drinking from a fountain is the same action as Collect (taking a puddle) or a new one.

## Fountains

**Agreed** (2026-11-25).

- **Small flasks** in the alchemist's lab: one drink each.
- **Large tanks** (for example a body in formaldehyde): several drinks.
- A fountain holds one liquid type or several.
- Whether level 1 has fountains, and how many, follows from the level-economy calculation (simulator + balance, pending).
