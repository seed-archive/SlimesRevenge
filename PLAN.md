# Plan

Findings outside a task's scope, accepted debt and questions for the user. Rules: `.claude/rules/documentation.md` → "The plan file".

## Open

1. **Duplicated `Spawn<T>` test helpers.** Shared `Assets/Tests/Editor/Support/SpawnFixture.cs` and `TestCreatures.cs` exist, but 15 fixtures keep their own `private T Spawn<T>(Vector2Int)`: `PathfindingPursuitTests`, `EnemyHuntSimulationTests`, `CreatureAiTests`, `BehaviorRulesTests`, `CreatureBehaviorGraphTests`, `CreatureSelectExecuteTests`, `ActionLogTests`, `PuddleTests`, `PuddleCapacityTests`, `CorpseTests`, `BodyTraitTests`, `StatusTests`, `InnateTraitTests`, `StatusVolumeRefreshTests`, `CombatTests`. Risk: setup drift (`SetArmor`, starting fill) when creatures change. Fix: move each fixture to `SpawnFixture` when it is next edited; compare the test count before and after.
2. **Stale creature catalog.** `docs/creatures.md` still lists Rat / Cat / Dog / Scorpion, which are retired (`AGENTS.md`). Fix: list the current cast.
3. **README CI table is incomplete.** `README.md` → CI lists `test.yml` and `build.yml`; `.github/workflows/` also has `unity-editmode.yml` (the reusable EditMode job `test.yml` calls) and `activation.yml`. Fix: add both rows.

## Accepted

4. **`is Slime` instead of a capability flag** (`UsesVolumeHealth` and the like): clearer while volume-as-health exists only on the slime. 10 places: `Combat.cs:79`, `Floor.cs:51`, `Creature.cs:85,214,360`, `CreatureHunt.cs:157`, `ActionLog.cs:516`, `CreatureSpriteAnimator.cs:37`, `SoftcoreRevive.cs:44`, `Cards/CreatureCard.cs:53`. Revisit when a second creature gets volume-based health.

## Needs the user

5. **How new creature art is made** (2026-11-14). The sprite skill says "generate with an image tool, never pixel-draw by hand"; the current agent has no image generator, only drawing pixels from code (like `scripts/generate_*.py`). Question: keep image generation as the only route (the user generates, the agent downscales), or allow code-drawn sheets? Either way the sheet pipeline helpers (`scripts/downscale_fdr_sheet.py`, `scripts/fdr_sheet_grid_guide.png`) are lost and must be written anew first (how: the `fdr-enemy-sheet` skill).
