# Volume

**Status:** Draft (2026-11-26). Body order, the affinity threshold and the capacity target are **Agreed**; how capacity grows is **Open** (balance working budget). Detail: the design notes `balance/capacity.md`, `lore/capacity.md`, `balance/liquids.md`.

The slime's body: volume is life and ammunition.

## Body order

**Agreed** (2026-11-25).

- The body is ordered from the edge to the centre.
- Hits and spending take from the edge.
- The skill is to keep the valuable liquid deep.
- **Swap:** a new action swaps any two adjacent units of the body. It costs one action, not a whole turn ([actions.md](actions.md#swap)).
- Reordering the body by pouring puddles and drinking them back is not the intended way.
- **Agreed:** in the final fight a blow takes 2 units from the edge, so valuables go in the middle, not the deepest centre.

## Affinity

**Agreed** (2026-11-26).

- A liquid that makes up **at least 60 % of the slime's current volume** (not of its capacity) gives its affinity. The code still uses 80 % ([slime-dominance.md](../slime-dominance.md)); it changes when the design is built.
- The affinity structure must be worked out for every liquid ([substances.md](substances.md#roles-of-a-liquid)). Affinity changes action points: heavy liquids slow the slime ([turn-structure.md](turn-structure.md#action-points)).

## Capacity over a run

**Agreed** (2026-11-26).

- Start: 10 units (Hard 6). By the alchemist the slime has grown a little: **maximum about 25**. This is a balance condition.
- **Open** (Oleg's proposals): +1 capacity per transition between locations; buying capacity at a trader (one of three liquids or +1 capacity; at the smith +1 capacity or a unit of ale); events that take capacity away (visible in advance, avoidable). The result Oleg sketched: the final fight with about 20 capacity and 5 unique liquids instead of 25.
- **Working** (balance, `balance/capacity.md`): capacity = start + 1 per transition inside a map (except entering a guardian level, 10 a run) + one of "+1 capacity or a curio" at each of 5 choices − at most 2 avoidable events; 25 / 22 / 21 at most on Easy / Normal / Hard. The river refills the body to full every level.

## The body is the wallet

**Agreed** (2026-11-26): the slime pays traders with units of its body ([route.md](route.md#stops)).
