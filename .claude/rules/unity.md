---
paths:
  - "Assets/**"
  - "ProjectSettings/**"
  - "Packages/**"
---

# Unity project files

- **Every asset has its `.meta`.** A new, moved or renamed file or folder under `Assets/` is committed together with its `.meta`; a deleted one loses its `.meta` in the same commit. Move and rename assets so the `.meta` moves with them (`git mv` both, or through the Editor) — a new `.meta` is a new GUID and breaks every reference in scenes and prefabs.
- **Never change a GUID** in a `.meta`, and never copy a `.meta` to another asset.
- Scenes (`.unity`), prefabs and `.asset` files are YAML written by the Editor. Prefer changing them through the Editor or Editor code; a hand edit is small, keeps `fileID`s and GUIDs, and is checked by opening the scene in Play Mode.
- Only commit `ProjectSettings/` changes the task needs. Editor-local state (`UserSettings/`, `Library/`, `Temp/`, `Logs/`, `Build/`) is never committed (`.gitignore`).
- Runtime code lives in `Assets/Scripts/Runtime` (one assembly, `SlimesRevenge.Runtime`); Editor-only code in `Assets/Scripts/Editor`, never `UnityEditor` in Runtime without `#if UNITY_EDITOR`.
- A renamed serialized field keeps `[FormerlySerializedAs("oldName")]`; a removed one is checked against the scenes and prefabs that set it.
- C# is formatted with CSharpier: `./scripts/format.sh` before committing (the pre-commit hook in `.githooks/` does it for staged files once `git config core.hooksPath .githooks` is set). The vendor tree `Assets/AstarPathfindingProject/` is excluded and never formatted.
- Runtime-loaded sprites go under `Assets/Resources/`; pixel-art textures import with `filterMode: Point` (`0` in the `.meta`) and `spritePixelsToUnits: 32`.
