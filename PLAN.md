# Plan

Findings outside a task's scope, accepted debt and questions for the user. Rules: `.claude/rules/documentation.md` → "The plan file".

## Open

1. **Duplicated `Spawn<T>` test helpers.** Shared `Assets/Tests/Editor/Support/SpawnFixture.cs` and `TestCreatures.cs` exist, but 15 fixtures keep their own `private T Spawn<T>(Vector2Int)`: `PathfindingPursuitTests`, `EnemyHuntSimulationTests`, `CreatureAiTests`, `BehaviorRulesTests`, `CreatureBehaviorGraphTests`, `CreatureSelectExecuteTests`, `ActionLogTests`, `PuddleTests`, `PuddleCapacityTests`, `CorpseTests`, `BodyTraitTests`, `StatusTests`, `InnateTraitTests`, `StatusVolumeRefreshTests`, `CombatTests`. Risk: setup drift (`SetArmor`, starting fill) when creatures change. Fix: move each fixture to `SpawnFixture` when it is next edited; compare the test count before and after.
2. **Stale creature catalog.** `docs/creatures.md` still lists Rat / Cat / Dog / Scorpion, which are retired (`AGENTS.md`). Fix: list the current cast.
3. **README CI table is incomplete.** `README.md` → CI lists `test.yml` and `build.yml`; `.github/workflows/` also has `unity-editmode.yml` (the reusable EditMode job `test.yml` calls) and `activation.yml`. Fix: add both rows.
4. **The Dog sheet is a plush placeholder.** `scripts/draw_dog.py` → `Assets/{Art,Resources}/Animals/Dog.png` is a cute puppy (round eyes, tongue, collar); the Dog is an enemy and must look aggressive. Fix: redraw by the character rules in `.claude/skills/fdr-enemy-sheet/SKILL.md`. Weak spots noted by its author: north-row tail, 3-frame side trot, plain white bite arc, thin C15/C16 crumbs.

## Accepted

5. **`is Slime` instead of a capability flag** (`UsesVolumeHealth` and the like): clearer while volume-as-health exists only on the slime. 10 places: `Combat.cs:79`, `Floor.cs:51`, `Creature.cs:85,214,360`, `CreatureHunt.cs:157`, `ActionLog.cs:516`, `CreatureSpriteAnimator.cs:37`, `SoftcoreRevive.cs:44`, `Cards/CreatureCard.cs:53`. Revisit when a second creature gets volume-based health.

## Needs the user

6. **Dog is not in the live roster.** `CreatureCatalog` keeps Rat / Cat / Dog / Scorpion out of play, so the new Dog sheet is not visible in a duel or campaign. Question (2026-11-14): bring the Dog back into the roster? Waits on the user; until then the sheet is seen only in the editor.
