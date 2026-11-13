# Versions and dependencies

- UPM package versions in `Packages/manifest.json` are changed by Renovate (`renovate.json`, base branch `dev`), not by hand — except when the user asks to update to the latest: then every affected package is bumped by hand, in one go, and that round does not mix with Renovate.
- A major waits for approval in the Dependency Dashboard; approving it is a decision, not a routine merge. A manual version change needs the user's explicit decision and says so in the commit.
- Packages are added and removed through Unity (Package Manager, or the `unity-package-management` skill), so `Packages/packages-lock.json` stays consistent; never hand-edit the lock file.
- **The Unity Editor version is changed by hand, together in every place that names it:** `ProjectSettings/ProjectVersion.txt`, `.github/workflows/*.yml` (`UNITY_VERSION`, `customImage`), `scripts/play.sh`, `scripts/build.sh`, `README.md`, `AGENTS.md`. An Editor upgrade is its own commit, verified with the full EditMode suite and both mobile builds.
- **Vendored libraries** under `Assets/` (A* Pathfinding Project, ElvGames art) are not managed by Renovate. They are updated only on the user's decision, in a separate commit, without local edits inside the vendor tree; a local fix to vendor code is an ADR.
- A version that is deliberately held back has a Renovate limit (`allowedVersions`) with a `description`, and an ADR that pins it.
- Tools: CSharpier comes from `.config/dotnet-tools.json` (`dotnet tool restore`); its version moves like a package, through Renovate.
