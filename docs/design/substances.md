# Substances

**Status:** Draft (2026-11-26). The inventory describes the game today. Stacking, puddles, the 17-liquid roster, the two cycles, reactions in the body and the build rules are **Agreed**; the role of each liquid is **Open** (lore table); numbers are balance's (working). Detail: the design notes `lore/liquids-roles.md`, `lore/forum.md`, `lore/reactions.md`, `lore/alchemy.md`, `balance/liquids.md`, `balance/reactions.md`, `balance/mercury-build.md`, `balance/pus-build.md`.

The liquids the slime is made of: what each does, and how they relate to each other.

## Inventory (today)

| Liquid | Strike (`Power` / `Corrosion`) | Puddle and strike effect | Dominance perk (≥ 80% of volume) | Floor cost for AI |
| --- | --- | --- | --- | --- |
| Water | 1 / 0 | Wet; puts out Burning and Corroding | Fireproof, blocks and removes Burning | mild |
| Oil | 1 / 0 | Instability (−1 speed) + Flammable (fire ×2), 3 turns | Flammable forever | mild |
| Poison | 1 / 0 | Poisoned (1 HP per turn, 3 turns); not on Poisonous | — | damaging |
| Acid | 1 / 1 | Corroding (armor, then HP, 3 turns); a strike strips armor first | — | damaging |
| Blood | 1 / 0 | Nothing, except healing a creature with Vampirism | Regeneration (+1 Blood per turn) | mild |
| Lava | 1 / 0, ×2 on Flammable | Burning (1 HP per turn, 2 on Flammable, 3 turns); not on Fireproof | — | damaging |
| Mercury | 1 / 0 | — | Invisible | mild |

Every dominant liquid also gives **Retaliation**: whoever strikes the slime receives that liquid's effect. Details: [status-effects.md](../status-effects.md), [strike-vs-puddle.md](../strike-vs-puddle.md), [armor.md](../armor.md).

Relations that already exist in the code: Water ⊣ Lava (puts it out, blocks it), Oil → Lava (fire burns twice as hard on oil), Acid ⊣ armor.

## Stacking

**Agreed** (2026-11-25).

- Timed statuses stack in **strength, not in duration**. Every strike or puddle adds its own copy with its own timer; two copies hit twice as hard over the same turns.
- Example: an enemy that steps into two lava puddles in a row burns for 2 HP a turn, not 1 HP a turn for twice as long.
- **Why:** a longer burn would leave the retreating slime cornered against a wall before the enemy dies. A stronger burn ends the fight while the slime still has room.
- The code already works this way: [status-effects.md](../status-effects.md) (Накопление).
- **Flammable stacks additively** (Sulfur and Fat are the flammable liquids in the design). Copies multiplying each other (×2, ×4, ×8, as the code does today) is far too strong; the direction is additive stacking (each copy adds to the bonus). The numbers come from balance (pending).
- Burns stack by stepping through puddles in a row, not by two puddles on one cell (see below).

## Puddles

**Agreed** (2026-11-25).

- **One cell holds one puddle, never two.** (The code already refuses a second puddle on an occupied cell: `Floor.TryPlacePuddle`.)
- Stacking comes from a path of puddles: an enemy that walks through several puddles in a row collects one copy from each.

## The roster: 17 liquids

**Agreed** (2026-11-26).

- **10 combat liquids in two cycles of five.** Membership is lore's (`lore/forum.md`): Water, Lava, Aquavita, Mercury, Ichor; Vitriol, Alkali, Ambrosia, Acid, Poison.
- **7 outside the cycles**: Blood, Pus, Fat, Resin, Sulfur, Aphrodisiac, Ectoplasm. The lore page shows them next to the cycles even though they do not fit them.
- **Removed:** Oil and Tar. Fat stays of the flammable pair (it also slows: Instability), Resin of the sticky pair. Sulfur is flammable too and takes Oil's role (fire on the enemy burns harder, blue flame, no stinking smoke). Ink, spore tincture, the explosive mix, poison-balm, brine and strength potion are out; nothing is cut further.
- **Names:** Ambrosia, Ichor and Ectoplasm stay; the rejected alternatives are out. Oleg's view: take recognisable names from the genre, alchemy and folklore rather than invent them. **Open:** Phosphorus is a dry substance, not a liquid, and its place and sources are unsettled (Ichor from old bones in one note, no Ichor from skeletons in another).
- **Direction** (Oleg, to lore). **Four bloods:** red (Blood), golden (Ichor, the blood of gods, found in their children; skeletons give none), blue (Vitriol), black (Strzyga blood). Pus is not one of them.
- **Ectoplasm** (Agreed): magical. Physical damage passes through it, magical damage hits it harder. Physical: bites, blows, Stone and the material liquids; magical: Ichor, Ambrosia, Ectoplasm and the attacks of magical creatures. It drops from magical creatures.
- **Rarity and biomes** (Agreed): liquids have a rarity in the world (Aphrodisiac and Lava very rare); Lava lives underground, mostly with the dwarves.
- **At least 10 different liquids per run** (an achievement "10 different liquids in the slime"). After each "one of three" choice a liquid without a choice drops as well.

## Roles of a liquid

**Agreed (the principle):** every liquid can be held (affinity), struck with, struck on (the unit on the edge when the slime is bitten) and spat; a new liquid is designed in every role at once (strike, puddle, affinity) with a name from the world. The Aphrodisiac is the exception.

**Open:** the lore table of 17 liquids × roles (`lore/liquids-roles.md`) waits for Oleg. Decided rows: Mercury's affinity is defensive (below), Ectoplasm as above, Sulfur flammable, Aphrodisiac a single unit ([curios.md](curios.md)).

## Two cycles

**Agreed:** half the liquids deal damage, and each hits someone hard and someone not at all (immunity); the other half are statuses that hurt nobody. The 10 damage liquids form two wu-xing circles of five; each shows which liquids are incompatible (like Lava and Water) and which boost each other. The order within the circles is lore's (`lore/forum.md`).

## Elements in combat

**Agreed:** Water is good against a lava creature, and a lava creature is bad against Water. A fire creature hitting an all-Water slime takes a penalty or misses.

**Working** (balance): the suppressing liquid deals +1 damage (not ×2); an enemy whose element the slime's edge suppresses hits about 25 % of the time and loses 1 HP on a miss; a suppressed slime is always hit, ×1; an enemy takes no damage from its own element.

**Agreed:** a "win button" is fine. If a pure liquid wipes out one kind of enemy, the same level holds enemies it is weak against: balance is kept by the enemy mix, not by weakening the liquid.

## Reactions in the body

**Agreed.**

- Neighbouring units react at the end of the slime's turn. The joint nearest the edge reacts first; the row closes up and the new joint is checked again (4 Water + 4 Lava = −8).
- **Five loud reactions:** Water + Lava → Stone (both leave the body; Stone falls to the floor as an obstacle); Lava + Aquavita → flash, both vanish; Poison + Blood → Pus (volume kept); Alkali + Acid → Water; Resin + Lava → amber (only Resin vanishes and drops as amber; Lava stays). Other suppressing pairs react only in combat. Direction (Oleg): Ichor + Poison and Vitriol + Poison do not react.
- A dangerous joint is highlighted and can be fixed this turn by a swap (or a spit).
- **A full collapse kills:** a chain that eats the whole body kills the slime. Only a body made entirely of reacting pairs can do it; such a joint is highlighted as deadly. Poison + Blood never kills.
- **Working** (balance, `balance/reactions.md`): no chain limit; −2 per Water + Lava pair, one Stone per chain, Stone crumbles after 4 rounds; a boost gives +1 damage, at most one boost per turn. Not checked by the simulator.
- **Agreed:** three self-healing mechanics for 10 elements are too many; if Blood and Pus heal themselves, Ectoplasm must not. **Open:** Oleg's later proposal puts regeneration on Ectoplasm instead (easy to dodge, easy to kill).
- **Open:** useful alchemy in the body (two neighbours on purpose become one rarer unit; lore recipes in `lore/alchemy.md`).

## Builds

**Agreed** (2026-11-26).

- **Affinity:** see [volume.md](volume.md#affinity).
- **No extra sources for builds.** A map farms one liquid, the next one punishes it; together with hard resets ([route.md](route.md#resets-at-transitions)) the player changes build at least 2–3 times a run.
- **Pus build:** valid through the whole game. Pus accumulates without a limit. One ale plus 9 Pus is a legitimate run; the ale pays off in the final fight (alone on Easy, [curios.md](curios.md#the-final-pay-off)).
- **Mercury build:** Mercury is rare (about 15 in a run, 60 % of a full body of 25). A slime that gathers it becomes very strong, and that is kept. To hold the affinity the slime keeps its body under full; a lost unit of Mercury breaks the build. **Working** (balance, `balance/mercury-build.md`): the affinity is defensive — a bite takes half (rounded up), other liquids are knocked out before Mercury, the slime cannot be pushed.
- **When builds open** (Oleg's direction): Pus and Mercury can start at the very beginning; some builds only from mid-game; no new build is collected on the last map (at most the second-to-last).
- **Pus** (Agreed): from level 3 a poisonous centipede turns the slime's Blood into Pus, dead cargo to be dumped. Oleg's direction: Pus opens on maps 1–2, not on level 1. **Open:** Pus as a weapon after the undertaker, or the only liquid with a bad affinity (belching: skip a turn, leave Pus puddles around — balance working numbers in `balance/resets.md`).
- **Open:** a poison build needs a Poison source, or it turns into a Pus build.
