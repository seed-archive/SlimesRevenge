# Substances

**Status:** Draft (2026-11-25). The inventory describes the game today. The two cycles and enemy vulnerabilities are *Proposed*: the user asked for them, the links between liquids are not decided yet.

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

## Two cycles

*Proposed*, after the Chinese five-element system (*wu xing*), which has two circles over the same elements:

- **Strengthens (→):** A makes B stronger — together, B's effect is larger, or A in the volume boosts B's perk. Today: Oil → Lava.
- **Suppresses (⊣):** A weakens or cancels B — A's effect is stronger against B, A in a puddle neutralises B, a creature made of B takes extra from A. Today: Water ⊣ Lava.

Rules the cycles must obey (the design guard checks them, [design-guard.md](design-guard.md)):

1. Every liquid strengthens at least one other and suppresses at least one other.
2. Every liquid is suppressed by at least one other: there is no build without a counter.
3. No liquid suppresses everything: the relation is a circle, not a ladder (rock–paper–scissors, not "lava is best").
4. A relation reads from the world at a glance: water on fire, oil feeding fire, acid on metal.

### A first proposal for the links

To be discussed edge by edge; only Water ⊣ Lava and Oil → Lava exist today, Poison ⊣ Blood is the user's example.

| Liquid | Strengthens | Suppresses | Why it reads right |
| --- | --- | --- | --- |
| Water | Blood (life needs water) | Lava (quenches) | |
| Oil | Lava (fuel) | Water (floats on top, seals it) | |
| Lava | Acid (heat speeds the reaction) | Mercury (boils it off), Oil? | |
| Acid | Poison (a corrosive brew) | Mercury (eats metal), armor | |
| Poison | Mercury? | Blood (poisons the living) | user's example |
| Blood | Water? Poison? | Acid? | open |
| Mercury | Oil? | Poison? | open |

Seven liquids do not fall into one tidy circle of five. Options for the user: (a) one circle of seven; (b) two circles — the *elements* (Water, Oil, Lava, Acid, Mercury) and the *living* (Blood, Poison), linked in a few places — which is the "two circles" the user mentioned; (c) five-element circle and two liquids outside it with special roles.

## Enemy vulnerabilities

*Proposed*: an enemy's vulnerability comes from **what it is made of**, through the suppress cycle. A creature whose body is Blood is weak to Poison; a lava elemental is weak to Water; a metal construct is weak to Acid. One rule then covers every enemy, and the player can read a weakness from the enemy's look and its corpse.

What exists today: Flammable doubles fire damage; Fireproof blocks Burning; Poisonous blocks Poisoned; FearsWater makes water puddles a hard path cost. The bat is made of Blood; the five new enemies are placeholders made of one unit of Water.

## Open decisions

1. The shape: one circle of seven, two circles (elements and the living), or five plus two outside.
2. Each link in the table above.
3. What "strengthens" means in numbers: ×2 effect, +1 power, a longer status, a bigger dominance perk.
4. Enemy vulnerability from body liquid through the suppress cycle — yes or no.
