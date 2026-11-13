---
name: editmode-tests
description: Run Slime's Revenge EditMode tests in Unity batchmode — the full suite or a focused filter — and read the result from the NUnit XML. Use before claiming tests pass, before a PR, and while changing AI, pathfinding, combat or statuses.
---

# EditMode tests

## 1. The project must be closed

Batchmode cannot open a project an editor already holds. If `Temp/UnityLockfile` exists, run `./scripts/stop.sh` and wait for `STOPPED:`.

## 2. Run

The run imports and compiles first; the first run on a fresh `Library/` takes minutes. Run it in the background and wait for it to exit.

```bash
UNITY="${UNITY_EDITOR:-/Applications/Unity/Hub/Editor/6000.5.10f1/Unity.app/Contents/MacOS/Unity}"
"$UNITY" -batchmode -nographics \
  -projectPath "$(pwd)" \
  -runTests -testPlatform EditMode \
  -testResults "$(pwd)/Logs/editmode-results.xml" \
  -logFile "$(pwd)/Logs/editmode.log"
```

Focused run: add `-testFilter "<prefix>"` and write to `Logs/focused-results.xml` / `Logs/focused.log`. Prefixes by area:

| Filter | Covers |
| --- | --- |
| `SlimesRevenge.Tests.BehaviorRulesTests` | Duel approach / personality contract |
| `SlimesRevenge.Tests.BehaviorGraphBindOwnerProofTests` | Behavior bind / Actor |
| `SlimesRevenge.Tests.EnemyHuntSimulationTests` | Corridor / maze / DoT doors |
| `SlimesRevenge.Tests.GridPathTests` | A* grid paths |
| `SlimesRevenge.Tests.PathfindingPursuitTests` | Pursuit memory, puddle penalties |

## 3. Read the result

The exit code alone is not the result. Read the summary line of the XML:

```bash
grep -m1 -o '<test-run[^>]*>' Logs/editmode-results.xml
grep -o '<test-case [^>]*result="Failed"[^>]*>' Logs/editmode-results.xml | sed -E 's/.*fullname="([^"]*)".*/\1/'
```

- No XML: the run did not reach the tests (compile error, license, locked project). Find the cause in the log: `grep -n 'error CS\|Exception\|License' Logs/editmode.log`.
- Report `total`, `passed`, `failed`, `skipped` and the results file. For a failure, show its message and stack from the XML (`<failure>` under that `test-case`).
- Compare `total` with the previous run when the change touched tests, helpers or assembly definitions; a drop is a failure until explained (`testing` rule).
