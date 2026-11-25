# Behavior (AI)

Как мобы выбирают, кого гнать, от кого бежать и кого кусать.

Связанные доки: [creatures.md](creatures.md), [status-effects.md](status-effects.md), [architecture/dependency-inversion.md](architecture/dependency-inversion.md).

## Два слоя

| Слой | На кого | Зачем |
| --- | --- | --- |
| **Personality** (`CreaturePersonality`) | только **слайм** (если есть) | «Агрессия к игроку»: трусливый / пассивный / агрессивный |
| **Innate fear / hate** (`Fears*`, `Hates*`) | только **моб ↔ моб** | сюжетные стычки (кошка↔мышь, собака↔кошка), чтобы слайм мог стравить зверей и подобрать трупы |

Мобы **не дерутся друг с другом** вне fear/hate. Слайм фактически один против всех; исключения — только врождённые пары выше.

Отдельный статус «боится / ненавидит слаймов» **не нужен**: отношение к слайму задаёт только personality + маркер аггро (`MarkAggro` / `InCombat`). Cowardly — *как будто* у крысы был бы «FearsSlimes», но такого статуса в игре нет.

Personality focus — `CreatureTurnContext.Player` — **nullable**. Без игрока условия vision/adjacent к слайму ложны → граф даёт Idle/Wander → `CreatureHunt.Redirect` ведёт охоту/бегство моб↔моб. Доска не требует controlled creature (см. dependency-inversion).

## Personality → слайм

Графы: `CreaturePolicyGraphs`. Условия смотрят на `CreatureTurnContext.Player` (слайм; может быть `null`).

| Personality | Кто | К слайму | Аггро после контакта |
| --- | --- | --- | --- |
| **Aggressive** | Mushroom, Undine, Cactus, Mandragora, Eye (умолчание `Creature.Personality`); отставной Dog | Всегда враждебен: в vision — **Chase**, вплотную — **Attack**. Вне vision — **WanderOrIdle** (может постоять или отойти). | **Липкий.** Сама бежит на слайма; отошёл — догоняет и кусает до конца (отставной Dog со Speed 2 догоняет слайма со Speed 1). |
| **Passive** | Bat; отставные Cat, Scorpion | Игнор (**WanderOrIdle**), пока слайм не ударит → `MarkAggro`, дальше как Aggressive | **Липкий.** Отошёл — аггро **не** сбрасывается, моб продолжает гнать/бить. |
| **Cowardly** | отставной Rat | В vision — **Flee**. Вплотную **и** в бою — один **Attack**. Вне vision — WanderOrIdle | **Не липкий.** Слайм отошёл → `ClearAggro` → снова **Flee** (пока видит). |

Аналогия: Cowardly *как будто* «боится слайма», но статуса `FearsSlimes` нет. Aggressive *как будто* «ненавидит слаймов», статуса `HatesSlimes` тоже нет — только personality + `MarkAggro`.

Personality задаётся переопределением `Creature.Personality`; кто его не переопределяет — **Aggressive** (так все пять заглушек живого состава). Состав и отставные: [creatures.md](creatures.md).

Скорость: у живого состава и слайма **1**; у отставного Dog **Speed 2** — он догоняет, если слайм не за стеной / вне vision.

### Аггро и память

- `MarkAggro()` → `InCombat` («в бою со слаймом»).
- Игрок бьёт моба → `TurnManager.TryAttack` → `MarkAggro` (passive просыпается).
- Моб chase/атакует слайма → `CreatureHunt.AfterAct` / мили `Combat.Attack` → аггро + `RememberPursuit`.
- **Cowardly** граф: если не `(InCombat ∧ adjacent)` — каждый Decide делает `ClearAggro`, затем Flee при vision.
- **Passive / Aggressive** граф `ClearAggro` **не** вызывает — аггро держится, пока цель жива / не сбросили иначе.
- Вне зрения агроенный Aggressive/Passive идёт к `PursuitCell`, пока память не истечёт.

## Fear / hate → моб ↔ моб

Все пары fear/hate сейчас только у отставных зверей; в живом составе их нет, мобы друг с другом не дерутся.

Редирект только когда personality-граф вернул **Wander** или **Idle** (`CreatureHunt.Redirect`). Порядок: **страх (Flee)** → **добыча (Chase/Attack)** → преследование по `PursuitCell`.

| Черта | Владелец | Эффект |
| --- | --- | --- |
| `HatesRats` | Cat | охотится на Rat |
| `HatesCats` | Dog | охотится на Cat |
| `FearsCats` | Rat | убегает от Cat |
| `FearsDogs` | Cat | убегает от Dog |
| `FearsWater` | Cat | (вода / лужи — см. статусы; не про melee AI) |

Примеры сюжета: слайм стравливает собаку с кошкой → оба трупа → Devour без лишней траты эссенций.

Если Aggressive видит слайма, граф уже даёт Chase/Attack — hunt **не** перебивает это на охоту за кошкой. Hate срабатывает, когда слайма нет в зоне personality-реакции (нет vision / нет аггро / passive без аггро).

## Зрение и дуэль

- Vision: Chebyshev ≤ `VisionRange` (по умолчанию 5), без LOS-стен в текущей модели.
- Обнаружение: `CreatureMoves.CanSee` = в радиусе **и** `target.IsDetectedBy(observer)`. Условия графа `PlayerInVision` и `PlayerAdjacent` оба требуют обнаружения, так что необнаруженного слайма моб не видит, не гонит и не кусает, даже стоя вплотную.

### Невидимость (доминантная Mercury)

- Когда Mercury доминантна у слайма (≥ 80% объёма), пассив доминанты `Invisible` (`BlocksDetectionFrom`) скрывает слайма от всех мобов.
- Исключение: моб с аггро (`Aggroed`) видит слайма всегда. Аггро даёт удар слайма по мобу (`TurnManager.TryAttack` → `MarkAggro`) или собственная атака моба.
- Итог в бою: пока слайм не ударил моба, тот его игнорирует — нет Chase, нет Attack, только Wander/Idle. Ударенный Aggressive / Passive моб помнит аггро и дальше видит слайма; Cowardly сбрасывает аггро вне ближнего боя (`ClearAggro`): кусать уже не может, но ещё до `PursuitMemoryTurns` ходов идёт к последней клетке слайма (`PursuitCell` из `CreatureHunt.AfterAct` → `Redirect` → Chase).
- Субстанцию Mercury на ударе и в луже статусов не вешает; см. [status-effects.md](status-effects.md).
- Дуэль: карта `15×5`, `RunConfig.DuelSeparation = 7` — старт **за** vision. Пока слайм не войдёт в пятёрку клеток, Aggressive стоит/бродит; как только увидел — сокращает дистанцию и кусает.

## Сценарии (контракт тестов)

| Сценарий | Ожидание | Тест |
| --- | --- | --- |
| Собака в vision → Chase, не Wander | Behavior graph | `Dog_InVision_BehaviorGraphChoosesChase_NotWander` |
| Собака: слайм 7 → 6 Idle\|Wander → 5 Chase+сближение | Behavior graph + TurnManager | `Dog_ApproachFrom7_At6Idle_At5SeesAndCloses` |
| Собака: вплотную → слайм отошёл | догнала и укусила | `Dog_Adjacent_SlimeStepsAway_DogCatchesAndBites` |
| Крыса видит слайма | Flee, без аггро | `CowardlyRat_FleesSlimeOnSight_WithoutAggro` |
| Крыса вплотную + аггро, слайм отошёл | ClearAggro → Flee | `CowardlyRat_AdjacentFight_ThenSlimeStepsAway_DropsAggroAndFlees` |
| Кошка видит слайма | Idle, не двигается | `PassiveCat_IgnoresSlimeUntilHit_ThenChases` |
| Слайм ударил кошку, отошёл | аггро держится → Chase | `PassiveCat_AfterHit_KeepsAggroWhenSlimeStepsAway_AndChases` |
| Несколько chase подряд | pathfind не «замораживает» собаку | `TurnManager_RepeatedChasePathfinds_DoNotFreezeDog` |

Код: `Assets/Tests/Editor/Behavior/BehaviorRulesTests.cs` (+ смежные в `CreatureAiTests` / `CreatureSelectExecuteTests`).

Chase идёт через `GridPath` (A* Pathfinding Project).

### Выбор пути при равной длине (`GridPath.ShortestAstarPath`)

Цели — набор клеток (для Chase — свободные клетки вокруг цели, `ApproachCells`; для Flee — точка бегства). На **каждую** цель делается отдельный запрос `ABPath`; из найденных путей берётся лучший по правилам, по порядку:

1. Меньше клеток в пути (длина в шагах, а не стоимость A*).
2. Только Flee: больше выигрыш расстояния от угрозы после первого шага (Chebyshev).
3. Больше «раннее продвижение»: Chebyshev от старта до второй клетки пути (при пути в одну клетку — до первой).
4. Цель с меньшим `x`, затем меньшим `y`.

Внутри одного запроса путь выбирает сам пакет: сетка 8 соседей, диагональ дороже прямого шага (стоимость пакета, ≈ √2), штраф клетки = `Substance.FloorPriorityFor(walker) × 1000` за лужу; эвристика и порядок обхода — умолчания `AstarPath`, `GridPath` их не задаёт. Ходом моб проходит не больше `Speed` клеток этого пути (`TryWalk`).

Самописной «best toward» / кольцевой эвристики для шага **нет** — см. `.claude/rules/behavior-astar-architecture.md`. Temp `AstarPath` всегда `DestroyImmediate`, иначе в Play Mode следующий chase молча падает.

## Ключевой код

| Кусок | Роль |
| --- | --- |
| `CreaturePersonality` / `CreaturePolicyGraphs` | деревья Aggressive / Passive / Cowardly (Unity Behavior) |
| `CreatureBrain.Decide` | тик графа → `CreatureIntent`; перед тиком `CreatureTurnContext.Actor` |
| Conditions (`PlayerInVision` / `IsAggroed` / `PlayerAdjacent`) | читают `CreatureTurnContext.Actor` (+ Player). Доказательные тесты бинда: `BehaviorGraphBindOwnerProofTests` |
| `CreatureHunt.Redirect` / `AfterAct` | fear/hate + sticky pursuit к слайму |
| `CreatureMoves.TryChase` / `TryFlee` / `Attack` | исполнение |
| `Combat.Attack` (melee) | аггро + pursuit при ударе по слайму |
| `TurnManager.TryAttack` | аггро жертвы при ударе игрока |
