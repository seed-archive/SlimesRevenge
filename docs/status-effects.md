# Status Effects

Статус-эффекты общие для **всех** существ (животные и слайм). Они живут в `StatusQueue` (и частично как пассивы доминанта объёма) и пульсируют на **ходах владельца** (`Creature.RefreshStatuses`), а не в момент наложения.

Связанные доки: [strike-vs-puddle.md](strike-vs-puddle.md), [slime-dominance.md](slime-dominance.md), [Architecture/popup-cards.md](Architecture/popup-cards.md).

## Жизненные циклы

| База | Длительность | Смысл |
| --- | --- | --- |
| `OverTime` | обычно **3** хода (`DefaultDuration`), либо `Forever` | Игровые статусы с таймером или «вечные» от доминанты |
| `TickingHarm` | как `OverTime` | DoT: урон (или strip брони) на каждом пульсе хода |
| `BodyTrait` | `Forever` | Маркеры тела / доминанты |
| `InnateTrait` | `Forever` | Видовые черты; таймер и смена доминанты их не снимают |

Константы: `StatusEffect.Forever = -1`, `Flammable.DamageMultiplier = 2` (на **каждый** экземпляр), `Vampirism.HealAmount = 2`.

## Накопление (стак)

`StatusQueue.Add` не сливает одинаковые статусы и не обновляет таймер: каждый удар или лужа (оба через `Substance.Apply`) добавляет **новый** экземпляр со своим таймером (3 хода). Остановить добавление могут только `Blocks` и `CancelsWith`. Проверяют тесты `ThreeLavas_StackThreeBurningOnEnemy`, `TimedStatuses_Stack_AndFlammableMultiplies`, `TwoFlammable_QuadrupleBurningPulse` (`StatusTests`).

| Статус | N экземпляров дают |
| --- | --- |
| `Burning`, `Poisoned` | N пульсов по 1 HP за ход |
| `Corroding` | N пульсов за ход (каждый — strip брони или 1 HP) |
| `Flammable` | множитель `2^N`: удар Lava и каждый пульс Burning ×2, ×4, ×8… (`ModifyIncomingHarm` по очереди у каждого экземпляра) |
| `Instability` | `Speed − N`, не ниже 0 |
| `Wet` | эффекта нет; любой Wet гасит все Burning / Corroding |

Forever `Flammable` доминантной Oil у слайма множит только если в очереди нет timed `Flammable` (`VolumeDominance.ModifyIncomingHarm`).

Накопление задумано: статусы стакаются по силе, а не по длительности (решение дизайна, [design/substances.md](design/substances.md#stacking)). Множитель `Flammable` в коде растёт как 2^N; по дизайну он должен складываться (additive), числа — от баланса. Пока код не изменён, здесь описано текущее поведение.

Поиск `FindStatus<T>`: innate ≫ Forever (доминанта) ≫ оставшиеся ходы таймера.

---

## Откуда статусы берутся

Три независимых канала. Один и тот же `Substance.Apply` обслуживает **удар** и **лужу**; доминанта слайма — отдельный путь.

### 1. Удар (`Combat.Attack`)

1. Урон: `StripArmor(Corrosion)` → `Damage(Power)` (у слайма — единицы объёма).
2. Если цель **жива**: `substance.Apply(target)` → `NotifyReceivedSubstance` (статусы сами реагируют).
3. Снимок `CapturesSurvivedHitReaction` до урона → `OnOwnerSurvivedHit` (Retaliation).
4. Голый мили: `NotifyDealtMeleeHit` → `Poisonous` и т.п.
5. Сбитый tip: `NotifyStruckVolumeTip` → `Vampirism` при Blood.

Подробнее урон vs Apply: [strike-vs-puddle.md](strike-vs-puddle.md).

### 2. Наступание в лужу (`Floor.ApplyContact`)

- Только **не-слайм**. Слайм лужу не потребляет и статусов с пола не получает.
- Урона от Power/Corrosion **нет** — только `Substance.Apply`, затем лужа исчезает.
- Статус начинает тикать на ходах жертвы.

### 3. Доминанта слайма (`VolumeDominance`)

Когда одна субстанция ≥ **80%** объёма, `RefreshVolumeStatuses` синхронизирует доминанта:

| Механизм | Что даёт |
| --- | --- |
| `FindDominanceStatus` | Пассивы в `FindStatus` **без** обязательной записи в очередь (Fireproof, Forever Flammable, Retaliation) |
| `ApplyDominance` | Правки очереди (Blood → Regeneration; Water → снять Burning) |
| `DominanceBlocks` | Блок входящих статусов (Water блокирует Burning) |

Смена доминанта снимает статусы с `ClearedWhenDominanceChanges` (сейчас — `Regeneration`). Подробнее: [slime-dominance.md](slime-dominance.md).

### Сводка: субстанция → Apply / доминанта

| Субстанция | Apply (удар и лужа) | Доминанта слайма |
| --- | --- | --- |
| **Water** | `Wet` | `Fireproof` (Find); блок/снятие `Burning` |
| **Oil** | `Instability` + timed `Flammable` | Forever `Flammable` (Find); Retaliation (база) |
| **Poison** | `Poisoned` (если нет Poisonous) | только Retaliation |
| **Acid** | `Corroding` (corrosion 1) | только Retaliation |
| **Lava** | `Burning` (если нет Fireproof) | только Retaliation |
| **Blood** | heal через уже имеющийся Vampirism (`NotifyReceivedSubstance`), **статус не вешает** | `Regeneration` в очередь; Retaliation |
| **Mercury** | ничего (`OnApply` пуст) | `Invisible` (Find); Retaliation с пустым retort |

Базовый `Substance.FindDominanceStatus`: для **любого** доминанта — `Retaliation` с клоном этой субстанции как `Retort`.

---

## Каталог эффектов

Ниже: что делает статус и **кто его накладывает**. EN-лейблы в UI иногда совпадают с именами субстанций (`Poisoned` → «Poison», `Corroding` → «Acid»).

### Контактные / временные

#### Wet

- **Эффект:** «промокший»; сам по себе не бьёт по HP.
- **Взаимодействия:** взаимная отмена с `Burning` (`CancelsWith`). Входящий Wet при уже активных `Burning` / `Corroding` **гасит** их и **сам не вешается**.
- **Длительность:** 3 хода.
- **Накладывает:** Water — удар и лужа.

#### Burning

- **Эффект:** каждый ход владельца — 1 HP (броня как DR на пульс **не** действует). Каждый `Flammable` удваивает пульс (×2^N).
- **Взаимодействия:** гасится Wet; блокируется `Fireproof` и доминантной водой.
- **Длительность:** 3 хода.
- **Накладывает:** Lava — удар и лужа.

#### Corroding

- **Эффект:** каждый ход — пока есть броня, `StripArmor(Corrosion)`; иначе 1 HP без DR брони.
- **Взаимодействия:** Wet гасит (и наоборот по правилам CancelsWith).
- **Длительность:** 3 хода; Corrosion обычно 1 (с Acid).
- **Накладывает:** Acid — удар и лужа. На ударе Acid дополнительно один раз стрипает броню **до** Power-урона.

#### Poisoned

- **Эффект:** каждый ход — 1 HP без DR брони.
- **Взаимодействия:** `Poisonous` блокирует наложение.
- **Длительность:** 3 хода.
- **Накладывает:** Poison — удар и лужа; голый мили от существа с `Poisonous` (Scorpion).

#### Instability

- **Эффект:** `Speed − 1`, пока висит.
- **Длительность:** 3 хода.
- **Накладывает:** Oil — удар и лужа (вместе с timed Flammable).

#### Flammable

- **Эффект:** удваивает `Lava.StrikePower` и пульсы `Burning` (`Creature.ModifyIncomingHarm` ← `Flammable`). Экземпляры стакаются и множатся: два — ×4, три — ×8.
- **Длительность:**
  - **3 хода** — с удара/лужи Oil;
  - **Forever** — пока Oil доминантен у слайма (`FindDominanceStatus`, не обязательно в очереди).
- **Накладывает:** Oil (Apply + доминанта).

#### Digesting

- **Эффект:** таймер = число юнитов в трупе. Средние пульсы только тикают; **на последнем** — bulk-перенос вещества (сколько влезет в `Volume.Capacity`), лог unique substances, уничтожение трупа. Второй `Digesting` блокируется (`Blocks`).
- **Длительность:** `UnitCount` трупа на старте.
- **Накладывает:** `TurnManager.TryDevourCorpse` (`Digesting.CanBegin`: объём слайма ≥ объёма трупа).
- **Abort:** снятие статуса / softcore revive уничтожает meal **без** transfer (`SoftcoreRevive`).

### Доминанта / тело

#### Fireproof

- **Эффект:** `Blocks(Burning)` — огонь не накладывается.
- **Длительность:** Forever, пока Water доминантен (пассив Find).
- **Накладывает:** только доминантная Water у слайма. На животных с лужи/удара **не** вешается.

#### Regeneration

- **Эффект:** на пульсе хода — +1 `Blood` в объём, если есть место (`Volume.Capacity`).
- **Длительность:** Forever в очереди; снимается при смене доминанта (`ClearedWhenDominanceChanges`).
- **Накладывает:** `Blood.ApplyDominance` (вставка в начало очереди). Только слайм с доминантной кровью.

#### Invisible

- **Эффект:** `BlocksDetectionFrom` — мобы без аггро не видят слайма: нет Chase и Attack, даже вплотную. Моб с аггро (ударенный слаймом) видит его. Подробнее: [behavior.md](behavior.md#невидимость-доминантная-mercury).
- **Длительность:** Forever, пока Mercury доминантна (пассив Find).
- **Накладывает:** только доминантная Mercury у слайма.

#### Retaliation

- **Эффект:** при ударе по носителю (пока жив) на атакующего вызывается `Retort.Apply` — тот же пайплайн статусов, что у вещества-доминанта.
- **Длительность:** Forever как пассив Find, пока есть доминант.
- **Накладывает:** любой доминант слайма (`FindDominanceStatus`). На животных с лужи/удара отдельно не вешается — только через доминирующий объём слайма (или будущие явные `AddStatus`).

### Врождённые (innate)

Вешаются в `SeedInnateTraits` при Awake; Forever; смена доминанты и таймеры их не трогают.

AI fear/hate (моб↔моб) и personality к слайму: [behavior.md](behavior.md). `Hates*` / `Fears*` (кроме `FearsWater`) — охота и бегство между животными. К слайму статусов нет: Aggressive / Passive / Cowardly + `MarkAggro`. Cowardly ведёт себя *как* «боится слайма», но класса `FearsSlimes` нет.

| Статус | Эффект | Кто |
| --- | --- | --- |
| **FearsCats** | AI: кот — хищник | Rat |
| **FearsDogs** | AI: собака — хищник | Cat |
| **FearsWater** | pathfinding: Water как высокий floor cost (100) | Cat |
| **HatesRats** | AI: крыса — добыча | Cat |
| **HatesCats** | AI: кот — добыча | Dog |
| **Poisonous** | блок `Poisoned`; мили → `Poisoned`; Poison-лужи cost 0 | Scorpion |
| **Vampirism** | heal 2 HP при «питье» крови (лужа/retort Apply, или tip Blood в ударе) | Bat |

**Blood** вампиризм **не** вешает: `NotifyReceivedSubstance` → уже имеющийся `Vampirism`. Карточка: Applies пуста; Dominance — Regeneration + Retaliation.

### Vampirism и Blood

`Vampirism` лечит `HealAmount` (2, не выше `MaxHitPoints`) каждый раз, когда носитель получает Blood:

- **Удар Blood по носителю:** 1 урона, затем, если цель жива, `Blood.Apply` → +2. С 2+ HP так не убить (Bat: 3 → 2 → 3); с 1 HP удар убивает раньше лечения.
- **Retaliation доминантной Blood:** носитель бьёт слайма → retort `Blood.Apply` на атакующего → +2.
- **Лужа Blood:** +2.
- **Свой укус:** сбитый с слайма верхний юнит — Blood (`OnOwnerStruckVolumeTip`) → +2.

Убить носителя можно другими субстанциями (без лечения; Lava бьёт ×2 за каждый `Flammable`), DoT или ударом Blood, когда осталось 1 HP.

Слайм врождённых статусов не имеет — только объём и доминанта.

---

## Матрица «источник → статус»

| Статус | Удар субстанцией | Лужа | Доминанта слайма | Innate / другое |
| --- | --- | --- | --- | --- |
| Wet | Water | Water | — | — |
| Burning | Lava | Lava | снимается/блокируется Water | — |
| Corroding | Acid | Acid | — | — |
| Poisoned | Poison | Poison | — | мили Poisonous |
| Instability | Oil | Oil | — | — |
| Flammable | Oil (timed) | Oil (timed) | Oil Forever | — |
| Digesting | — | — | — | Devour corpse |
| Fireproof | — | — | Water | — |
| Regeneration | — | — | Blood (очередь) | — |
| Retaliation | — | — | любой доминант | — |
| Vampirism | — | — | — | Bat; Drink с Blood |
| Invisible | — | — | Mercury | — |
| Poisonous / fears / hates | — | — | — | таблица innate |

---

## Взаимные отмены и блоки (шпаргалка)

| Правило | Поведение |
| --- | --- |
| Wet ↔ Burning | взаимный `CancelsWith`: огонь и мокрота гасят друг друга |
| Corroding + входящий Wet | Corroding снимается, Wet не применяется |
| Fireproof | не пускает Burning |
| Water dominance | `DominanceBlocks(Burning)` + чистка Burning из очереди |
| Poisonous | не пускает Poisoned; сам раздаёт Poisoned мили |

Пульсы DoT (`Burning`, `Poisoned`, `Corroding`, `Regeneration`) идут в порядке FIFO очереди на старте хода владельца.

---

## UI и лог

- Карточка статуса: `StatusCard` (описание + таймер).
- Удар/лужа в action log: gains только если `AddStatus` принял эффект; клик → та же карточка.
- Иконки: `IconCatalog` / `Assets/Resources/Icons/status_*.png`.

Архитектура попапов: [Architecture/popup-cards.md](Architecture/popup-cards.md). Лог: [Architecture/action-log.md](Architecture/action-log.md).
