# Actions

**Status:** Draft (2026-11-25). Gather and Swap are **Agreed**, the last-drop strike is **Working**; the full action list (move, strike, spit, pour, devour, wait) is not written yet. Costs: [turn-structure.md](turn-structure.md#action-points).

## Gather

**Agreed** (2026-11-25). Drink and Collect are one action: **gather liquid from the environment** — a puddle, a flask on a table, a tank. The liquid adds to the body ([growth.md](growth.md)).

## Swap

**Agreed** (2026-11-25). Swap any two adjacent units of the body, for one action ([volume.md](volume.md#body-order)).

## Last-drop strike

**Working** (2026-11-25), proposed by the coordinator from balance to meet Oleg's last-unit decision ([growth.md](growth.md#the-last-unit)).

- When the slime has one unit left, it can still strike an adjacent enemy for 1 damage without spending that unit.
- Why: once the body is down to one unit and the fountains are empty, the slime must still be able to kill the remaining enemies itself. Balance shows Hard on levels 1–2 is nearly impossible without it (level 1: 32% without it → 82% with it).
- Code today: not built. `Volume.CanSpend` forbids spending the last unit, so a 1-unit slime cannot attack at all ([creatures.md](../creatures.md)).
