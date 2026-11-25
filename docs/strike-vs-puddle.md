# Удар vs лужа

Общий пайплайн:

| Путь | Урон (`StrikePower` / `Power`) + `Corrosion` | `Substance.Apply` (статус) |
| --- | --- | --- |
| **Удар** (`Combat.Attack`) | Да: `StripArmor(Corrosion)`, затем `Power` vs оставшийся DR | Да, если цель жива |
| **Лужа** (`Floor.ApplyContact`) | Нет | Да (лужа исчезает; слайм лужу не трогает) |

Статус с лужи/удара начинает тикать на **ходах жертвы** (`RefreshStatuses`), не в момент контакта.

## По субстанциям

| Субстанция | Удар | Apply (удар и лужа одинаково) |
| --- | --- | --- |
| Water | `Power` 1, `Corrosion` 0 | `Wet`; гасит `Burning`/`Corroding` через `CancelsWith` |
| Oil | `Power` 1 | `Instability` + timed `Flammable` |
| Poison | `Power` 1 | `Poisoned` (не на `Poisonous`) |
| Acid | `Power` 1 + `Corrosion` 1 | `Corroding` (пульсы: броня → HP); удар: сначала strip, потом Power vs DR |
| Blood | `Power` 1 | `NotifyReceivedSubstance` → `Vampirism` если есть (+2 HP после урона, если цель жива: носителя с 2+ HP ударом Blood не убить) |
| Lava | `Power` 1, ×2 за каждый `Flammable` (×2^N) | `Burning` (пульсы в HP, броню не трогает) |
| Mercury | `Power` 1 | ничего |

Каждое попадание вешает новый экземпляр статуса — они стакаются по силе, так задумано: [status-effects.md](status-effects.md#накопление-стак).

Броня: см. [armor.md](armor.md).
