# Softcore revive

What happens when the slime is defeated, by game mode.

## Modes

`GameSettings.Mode` (`GameMode.Hardcore` by default, `GameMode.Softcore`). Nothing in the game sets `Softcore` yet: no menu option or run config selects it, so in a normal run every defeat is a game over. Tests set it directly.

## Defeat flow

`TurnManager` handles a defeat once (`IsGameOver` guards repeats):

1. `IsGameOver = true`, the player creature `Die()`s, `PlayerDefeated` fires.
2. In `Softcore`, if `TrySoftcoreContinue` is bound and returns `true`, `IsGameOver` goes back to `false` and the run continues on the same board.
3. Otherwise (`Hardcore`, nothing bound, or the revive failed) the `GameOverScreen` is shown and the run returns to the main menu.

`WorldView` binds `TrySoftcoreContinue` to `SoftcoreRevive.TryContinue(turnManager, slime, Map)` in both the campaign and the duel bootstrap, before `TurnManager.Bind`.

## What a revive does (`SoftcoreRevive.TryContinue`)

1. Returns `false` for a null player.
2. Aborts a `Digesting` meal without granting its reward, then clears all statuses.
3. Replaces the volume with `SpawnPoolWater` units of water. Whatever the slime held before is lost.
4. Reactivates the creature's `GameObject` if it was disabled; a `Slime` gets `SetMaxHitPoints(0)` (its life is its volume, not hit points).
5. Recomputes volume statuses (`RefreshVolumeStatuses`) and returns `IsAlive`.

The slime stays on the cell where it died. The design places it next to a spawning pool; that pool does not exist yet (`TODO(SpawnPool)` in `SoftcoreRevive`).

## Tests

`DigestionDevourTests` (revive during a meal) and `StatusTickOrderTests` (revive after a status tick) assert `SpawnPoolWater` units, all water.
