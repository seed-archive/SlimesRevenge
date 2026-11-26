# CI

GitHub Actions in `.github/workflows/`. The base branch is `dev`; releases go to `main`.

**CI on pull requests is temporarily off** (Oleg's decision, 2026-11-26): docs need no CI and the game has nothing to test yet. `test.yml` runs only by hand; `build.yml` builds Android and iOS on a pull request to `main`. Before every PR run `./scripts/format.sh check` and the EditMode tests locally. To restore: put back the `push`/`pull_request` triggers kept as a comment in `test.yml`, the `test` job and `needs: test` in `build.yml`, and the required checks `test / Test` and `Format` on `dev`. Restore when gameplay code lands.

## Workflows

| Workflow | Trigger | Jobs |
| --- | --- | --- |
| `test.yml` | manual only (temporarily) | `test` (EditMode, via `unity-editmode.yml`), `format` (CSharpier) |
| `build.yml` | PR to `main`; manual | `build` for Android and iOS (the `test` job is temporarily off) |
| `unity-editmode.yml` | `workflow_call` only | EditMode tests in GameCI |
| `activation.yml` | manual | requests a Unity manual activation file |

`dev` is protected, with no required checks while CI is off (normally `test / Test` and `Format`, `strict` off; admins can bypass). A direct push to `dev` is rejected for non-admins; admins bypass the protection, so agents still never push to `dev` (`workflow` rule).

Mobile builds run only from `build.yml`, so work on `dev` never builds Android or iOS. A concurrency group per workflow and ref cancels the older run when a new push arrives.

## EditMode job (`unity-editmode.yml`)

- Runs `game-ci/unity-test-runner` with the editor image `customImage` and `UNITY_VERSION`. Both must match `ProjectSettings/ProjectVersion.txt` (`versions` rule).
- The test runner action is pinned to a commit SHA, not a tag. The later v4 build uses the license-file method, which Unity rejects for the Personal license ("TimeStamp validation failed"). Do not bump it without a working activation.
- Caches `Library/` keyed by the hash of `Assets/`, `Packages/` and `ProjectSettings/`; any asset change gives a new key and restores the closest older one.
- Uploads the NUnit results as the `Test-results` artifact, even when tests fail.
- Measures code coverage in the same run (the runner always passes `-enableCodeCoverage`; it takes effect because `com.unity.testtools.codecoverage` is in the manifest — without the package the `Coverage` artifact is empty). `coverageOptions` limits it to the `SlimesRevenge.Runtime` and `SlimesRevenge.Editor` assemblies, so tests and the vendored A* do not count. The HTML report, `Summary.json` and badges are uploaded as the `Coverage` artifact. Coverage is reported, not enforced: no threshold fails the job.
- Secrets: `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`. A fork PR has no secrets, so its EditMode job cannot activate Unity.

## Format job (`test.yml`)

- Installs .NET SDK 8 (`actions/setup-dotnet`) and runs `./scripts/format.sh check`.
- `format.sh` restores CSharpier from `.config/dotnet-tools.json` and checks `Assets/Scripts` and `Assets/Tests`. `.csharpierignore` excludes the vendor tree `Assets/AstarPathfindingProject/`.
- The job fails on any file CSharpier would change. Fix locally with `./scripts/format.sh`; the pre-commit hook (`.githooks/pre-commit`, enabled with `git config core.hooksPath .githooks`) formats staged `.cs` files on every commit.
- CSharpier's version moves through Renovate. A new version can reformat untouched files; format the whole tree in the same PR that bumps it.

## Build job (`build.yml`)

- `game-ci/unity-builder` per platform (`Android`, `iOS`), with its own `Library/` cache per platform.
- Android builds an APK (`androidExportType: androidPackage`); iOS produces an Xcode project, not an `.ipa`.
- Artifacts: `slimesrevenge-Android`, `slimesrevenge-iOS`.

## Locally

The Mac editor here has no Android or iOS modules. `./scripts/build.sh OSX` checks that player scripts compile outside the editor; mobile builds are checked only in CI.
