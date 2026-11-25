# Design guard

**Status:** Draft (2026-11-25). The purpose is **Agreed**; the first version's scope and acceptance criteria wait for the user's yes.

Code that watches the game design. It plays fights by the game's real rules, many times, and answers: **which builds work, on which difficulty they stop working, and why.** Design rules that are agreed become automatic checks, so a change to anything basic that breaks the design fails CI instead of going unnoticed.

## Why

- The design is too big for anyone's memory — the user's or an agent's context. Things will be forgotten.
- The parts depend on each other: change the strike damage or a liquid's effect, and builds three sections away may stop working. Nobody would notice by reading.
- Games narrow toward the top: on the first level most weapons work, on the hardest only a few do. Slime's Revenge must narrow on purpose, not by accident — for example, face-tanking on water and on lava both pass Easy, but on Hard water does not pass at all and the player has to find lava.

## What it answers

1. **Build × encounter → outcome.** For each build and each encounter: won or lost, how many turns, how much of the body was spent, what was left, what killed the slime.
2. **How far each build gets.** For each build, the hardest difficulty it still passes, and the reason it fails on the next one (which enemy, which status, which turn).
3. **Whether the agreed rules still hold.** Each agreed design rule is a check: green while the rule holds, red with the numbers when a change breaks it.

Later, once levels and growth exist: **the acquisition graph** — which liquids a slime can obtain where, so a build that only works with lava is checked against the places lava can be found.

## Model

| Term | Meaning |
| --- | --- |
| **Build** | The slime's volume: how many units of each liquid (for example 10 Water, 10 Lava, the starting mix), later plus affinity and origin. |
| **Encounter** | The enemies of one fight: kinds and counts, their start cells; later room shape, puddles, fountains. |
| **Policy** | How the simulated slime plays. *Face-tank*: walk to the nearest enemy and strike with its best liquid until it dies. Later: *kite* (strike and step back), *trap* (pour before the enemy steps), *pull* (wake one enemy at a time). |
| **Difficulty** | The encounters a level puts in a room and the enemy parameters it changes (vision range, hit points, …). Defined in balance; until then, encounters of 1, 2 and 3 enemies stand in for Easy / Normal / Hard (the face-tank table in [vision.md](vision.md)) — this stand-in varies only the number of enemies. |
| **Outcome** | Win or loss, turns, units spent and left, enemies killed, the cause of the slime's death. |

## How it works

- **Real rules, not a copy.** The simulator builds a board, spawns the slime and the enemies, and runs turns through the game's own code (turn order, combat, statuses, puddles, enemy AI through Unity Behavior and A*). When a rule changes in the game, the simulation changes with it.
- **Repeatable.** Randomness goes through a seeded generator: the same seed gives the same fight. A result is the summary of many seeds.
- **The simulated slime is a test player, not game AI.** Its policies live with the tools and the tests, never in the game; it moves through the same `GridPath` (A*) as everyone else.
- **Outputs:** a report table (build × encounter) written on every run and attached to CI like the coverage report; design rules as EditMode checks in the normal test run.

## Visualization

**Agreed** (2026-11-25): the results are shown as a **website**, so the user sees at a glance what shifted after a change. The same data can later become a **wiki for players** — which build works where — published the same way.

*Proposed* shape:

- **Data first.** The simulator writes its results as a data file (JSON): builds, encounters, outcomes, causes of death, the agreed rules and whether each holds. The site only draws that file; it computes nothing itself.
- **A static site, built in CI.** Plain HTML and charts generated from the data on every run — no server, no Unity needed to view it. Unity is not used for drawing: a page outside the game is easier to share and never risks ending up in the player's build.
- **Pages:**
  1. *Build matrix* — build × encounter, coloured by win rate; click a cell for the fight summary (turns, body spent, cause of death).
  2. *How far each build gets* — every build against Easy / Normal / Hard, with the reason it drops out.
  3. *What shifted* — the difference between this run and `dev`: rules that broke, builds that gained or lost a level, cells whose win rate moved by more than a threshold. This is the page to open on every pull request.
  4. *Design rules* — the list of agreed rules, green or red, each linked to its section of this document.
- **Where it lives.** Every CI run attaches the site as an artifact (like the coverage report); the version for `dev` is published as a web page. The repository is public, so that page is public too.
- **Player wiki, later.** A second, filtered build of the same site: only what a player should see (builds, enemies, liquids, what works where), without internal rules and numbers the design keeps hidden.

## First version

*Proposed* scope:

1. Run one encounter without a scene, deterministic for a given seed, by the real rules.
2. The face-tank policy.
3. Builds: the starting mix and one pure build per liquid (10 units of it).
4. Encounters: 1, 2 and 3 of each enemy in the roster, starting a few cells away.
5. A report: build × encounter → win rate over N seeds, average units left, most common cause of death; written as JSON to `Logs/` locally and uploaded by CI.
6. The site's first page, the build matrix, generated from that JSON and attached to CI. The "what shifted" page and publishing come in the next version, once there is a previous run to compare with.
7. One check that proves the machinery: the result for a fixed seed does not change between runs.

Not in the first version: other policies, rooms and walls, growth and acquisition, difficulty definitions, action points (they come with the turn-structure decisions).

## Acceptance criteria (first version)

1. A test runs a face-tank fight of a given build against a given encounter and seed without a scene, and two runs with the same inputs give the same outcome.
2. The report covers every build × encounter listed above and is produced by the normal EditMode test run, locally and in CI.
3. Changing a basic number (for example a liquid's strike power) visibly changes the report — checked once by hand when the tool is built.
4. The simulated slime uses only the actions a player has and moves only through `GridPath`.
5. CI attaches a page that shows the build matrix from the JSON, readable without Unity.

## Design rules as checks

Added one by one as rules are agreed in balance. The first candidates come from the face-tank table:

| Rule | Check |
| --- | --- |
| Easy: a face-tanking slime survives 2 enemies, dies to 3 | win rate ≥ the agreed threshold for 1–2, ≤ it for 3 |
| Normal: survives 1, dies to 2 | same, one enemy fewer |
| Hard: dies to 1 | win rate below the threshold for 1 |
| Some build passes Hard with the right policy | at least one build × policy wins every Hard encounter (needs the kite / trap policies) |
