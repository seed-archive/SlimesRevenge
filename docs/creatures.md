# Существа

Поведение AI (personality к слайму, fear/hate между мобами): [behavior.md](behavior.md).

## Живой состав

Противники — `CreatureCatalog.Opponents`: все подклассы `Creature`, кроме слайма и отставных (`CreatureCatalog.IsRetired`). Кампания ставит по одному каждого вокруг центра карты (`WorldView`); дуэль по умолчанию — Bat (`RunConfig`).

| Название | Хит-поинты | Броня | Скорость | Субстанции | Personality | Врождённое |
| --- | ---: | ---: | ---: | --- | --- | --- |
| Slime | 0 (жизнь = объём) | 0 | 1 | 2× Water, 1× Oil, 1× Poison, 1× Acid, 1× Lava | — | — |
| Bat | 3 | 0 | 1 | 2× Blood | Passive | `Vampirism` |
| Mushroom | 5 | 0 | 1 | 1× Water | Aggressive | — |
| Undine | 5 | 0 | 1 | 1× Water | Aggressive | — |
| Cactus | 5 | 0 | 1 | 1× Water | Aggressive | — |
| Mandragora | 5 | 0 | 1 | 1× Water | Aggressive | — |
| Eye | 5 | 0 | 1 | 1× Water | Aggressive | — |

Mushroom, Undine, Cactus, Mandragora, Eye — заглушки: одинаковые числа, без черт и своих способностей. Personality у них не переопределена, поэтому действует умолчание `Creature.Personality` = **Aggressive**. Зрение у всех — `VisionRange` (по умолчанию 5).

Bat с 2+ HP не убить ударом Blood: удар снимает 1 HP, затем (цель жива) `Blood.Apply` → `Vampirism` лечит 2 (до `MaxHitPoints`). С 1 HP удар Blood убивает — лечение не успевает. Подробнее: [status-effects.md](status-effects.md#vampirism-и-blood).

## Отставные

Rat, Cat, Dog, Scorpion остаются в коде и тестах (ими проверяются fear/hate и personality), но в игру не выходят (`CreatureCatalog.IsRetired`).

| Название | Хит-поинты | Броня | Скорость | Субстанции | Personality | Врождённое |
| --- | ---: | ---: | ---: | --- | --- | --- |
| Rat | 3 | 0 | 1 | 1× Blood | Cowardly | `FearsCats` |
| Cat | 5 | 0 | 1 | 2× Blood | Passive | `HatesRats`, `FearsDogs`, `FearsWater` |
| Dog | 10 | 0 | 2 | 3× Blood | Aggressive | `HatesCats` |
| Scorpion | 3 | 1 | 1 | 1× Acid | Passive | `Poisonous` |

Пожирание трупа: слайм может начать, если его **заполненный** объём ≥ объёма трупа.

Объём = жизнь слайма (`IsAlive` при `UnitCount > 0`). Атака и лужа тратят 1 юнит; при последнем юните (`CanSpend == false`) эти пункты меню disabled — пустой substance picker не открывается.

Ближний бой (игрок и мобы): соседство = **Chebyshev 1** (8 клеток), единый API — [`GridStep.IsAdjacent`](../Assets/Scripts/Runtime/World/GridStep.cs) / `CreatureMoves.IsAdjacent`.

- **Карта:** на клетке рисуется только труп, умерший последним (лужа / живой моб могут быть рядом отдельно).
- **Меню Devour:** все трупы стека; у каждого `Name · N vol · cur/max turns` (объём и таймер гниения; max = MaxHitPoints); слишком большие — disabled. См. [cell-menu.md](cell-menu.md).

