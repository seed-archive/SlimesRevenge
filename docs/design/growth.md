# Growth

**Status:** Draft (2026-11-25). The items below are **Agreed**; every number comes from balance (pending). The rest of the section (eating rates, collecting puddles, growing capacity) is not written yet.

How the slime gains body: drinking, eating corpses, fountains.

## Every fight nets body

**Agreed** (2026-11-25).

- The core loop is swapping liquids and capitalising on them ([vision.md](vision.md#core-loop)).
- Every fight should net body, and let the slime replace at least one unit it already has.
- A fight that nets nothing starts a downward spiral.
- On Hard a single fight may net under +1 (for example 0), but **on average** a Hard fight nets slightly above 0 (for example +1…+3 over 3 enemies is fine; +4 is too generous).

| Difficulty | Net body per fight (target) |
| --- | --- |
| Easy | +1…+2 units |
| Normal | +1 unit |
| Hard | slightly above 0 on average |

Targets, not rules of the code. Exact numbers come from balance (pending).

## The first fight

**Agreed** (2026-11-25).

- The slime's starting volume may shrink with difficulty. Then the first fight is the hardest: the player needs a way to net at least +1 in the very first fight.
- Oleg's example: start at 6, an enemy worth 3 volume: 6 − 3 + 3 = 6 — at best zero.
- Enemy hit points on Hard are tuned against this: see [balance.md](balance.md#why-not-4-hp-on-hard).

## Swapping liquids

**Agreed** (2026-11-25).

- Swapping does not need puddles: hits free room in the body, the slime drinks new liquids and arranges its body ([volume.md](volume.md)). This works from level 1.
- Later, once pouring arrives, a full slime can also swap by pouring a puddle and drinking the new liquid. Puddles stay a separate, later mechanic; spit is level 2.
- Liquid is never lost: what the slime pours stays on the floor until someone takes it.
- The slime's own puddle gives it no buff and no debuff. Anyone else who steps in gets the effect, mostly a debuff.
- Code today ([puddles.md](../puddles.md)): the slime takes no status from any puddle, not only its own; a non-slime that steps in takes the effect and the puddle disappears.

## The last unit

**Agreed** (2026-11-25).

- When the body is down to one unit, the fountains are empty and enemies remain, the slime still has to go and kill them itself. The rule that makes this possible comes from balance (pending).
- **Softcore:** an unlimited Water pool at the slime's spawning pool (not the enemies' spawners).
- **Iron Man:** no unlimited Water; a reserve fountain of about 20 Water instead (number from balance, pending).

## Drinking and eating

**Agreed** (2026-11-25). Drinking and eating corpses belong to the level-1 kit: move, strike, drink, eat ([progression.md](progression.md#mechanic-ladder)). Eating is the Devour action. Drinking is Gather ([actions.md](actions.md#gather)).

## Fountains

**Agreed** (2026-11-25).

- **Small flasks** in the alchemist's lab: one drink each.
- **Large tanks** (for example a body in formaldehyde): several drinks.
- A fountain holds one liquid type or several.
- Fountains on level 1: the working number is 1 flask ([balance.md](balance.md)); final numbers from the level-economy calculation (simulator + balance, pending).
