# KISS, DRY, YAGNI

Applies to every file in the repository. Apply it while writing, then run it as a separate pass over the full diff before a task is finished and again before any commit. Do not finish or commit a change that fails the pass.

## KISS

Ship the smallest change that fixes the current case. Do not add a manager, service, interface, helper or abstraction for a single call site.

```csharp
// Bad — a class whose only method forwards one call
public sealed class DamageService
{
    public int Apply(Creature target, Substance substance) => Combat.Harm(target, substance);
}

// Good — call the method
var dealt = Combat.Harm(target, substance);
```

- Prefer direct readable code over many private methods. A private helper needs a domain meaning, not just to hide two lines.
- Avoid helper chains where one helper only delegates to another.
- Prefer explicit `if` branches for game rules that are decisions; prefer `StatusEffect` hooks for rules that belong to a status (`dependency-inversion` rule).
- Do not introduce broad generic types or interfaces when one concrete type is enough.
- Do not add a `MonoBehaviour` for logic that needs no scene object; plain C# classes are easier to test in EditMode.

## DRY

Extract shared logic only when the same behavior already exists in two production places. Do not extract for a hypothetical second caller.

An abstraction (helper, base class, interface, ScriptableObject catalog, shared utility) is allowed only when at least one is true:

- The same logic is used in 2+ production places.
- The logic is complex enough that naming it materially improves readability.
- It protects a boundary: a library contract (Unity Behavior, A* Pathfinding Project), the save format, or a rule owned by a status or substance.
- It is required to test a non-trivial branch.

## YAGNI

Do not add placeholders, unused serialized fields, inspector toggles nobody sets, future-creature branches, TODO-only branches, empty overrides or flags nobody reads. An unsupported case fails explicitly (an exception or a logged error with the reason), not silently. Leave an existing enum value in place when removing it would break serialized scenes, prefabs or saves, and do not produce it from new code if the game no longer uses it.

## The simplification pass

During a task the change usually touches only the code it needs. The pass looks wider:

1. Read the full diff against `dev` (`git diff dev...` plus uncommitted changes) plus the classes and methods the diff touches.
2. Look for: new code that duplicates something that already exists; helpers with one caller; parameters, fields, branches or types nothing reads; code made obsolete by the change; neighbouring code in the touched flow that can now be simpler.
3. Make the reductions that stay inside the task's flow. Record larger cleanups outside the flow in `PLAN.md` instead of doing them.
4. Re-run the checks the reductions affect. A simplification that was not re-verified is not done.

Expect the pass to remove 5–10% of new code. If it removes nothing, say what you checked.
