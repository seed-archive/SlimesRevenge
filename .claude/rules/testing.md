---
paths:
  - "Assets/Tests/**"
  - "Assets/Scripts/**/*.cs"
---

# Testing

Tests are NUnit EditMode tests in `Assets/Tests/Editor` (assembly `SlimesRevenge.Editor.Tests`), mirrored by feature folder. How to run them: the `editmode-tests` skill.

- Derive test cases from the acceptance criteria, not from the code you just wrote.
- A test proves a criterion only if it fails when the criterion is broken. For a bug fix, write the test first and see it fail on the old code; say so in the report.
- Test behaviour through the public API of the unit (a creature's statuses and volume after a turn, a session's board, a path's cells, an action log's entries), not private fields or reflection.
- Every test destroys what it created, also when it fails: `GameObject`s and components with `Object.DestroyImmediate` in `[TearDown]`, and any static or singleton state (`AstarPath.active`, `CreatureTurnContext`) reset there. A test does not depend on the order of other tests.
- Use the shared helpers in `Support/` (`SpawnFixture`, `TestCreatures`) and `TestSupport/SessionFactory` for new fixtures instead of a new private `Spawn<T>`.
- Randomness goes through `IRng`: tests pass `FixedRng` (or a seeded `SystemRng`), never `UnityEngine.Random`.
- No fixed waits (`Thread.Sleep`, `yield return new WaitForSeconds`) in tests: wait for a state, or test the logic without a frame loop.
- Mob↔mob hunts are tested board-only (`SessionFactory.Board`, `player: null`), never with a hidden slime (`dependency-inversion` rule).
- Do not weaken a check to get green: no `[Ignore]`, `[Explicit]`, raised `[Timeout]`, broader asserts or mocked-out code under test. A test that cannot run in batchmode is reported, not deleted.
- When a change touches test infrastructure, assembly definitions or shared helpers, compare the total test count (`total` in the results XML) before and after; a drop is a failure until explained.
- To watch a test fail on the old code, put the old code back with git (`git stash` of the fix, or `git checkout dev -- <file>`) and restore it the same way, never with a second hand edit or `sed`; then check the diff before committing.
- A claim that tests passed names the results file (`Logs/*-results.xml`) of that run. Never truncate the only evidence of a run with `head`/`tail`.
- A change to build configuration (`ProjectSettings/`, `Packages/manifest.json`, assembly definitions, `scripts/build.sh`, Editor build code) is verified with a real build of each affected platform (`./scripts/build.sh Android`, `./scripts/build.sh iOS`), not only with EditMode tests. Name the builds in the report.
