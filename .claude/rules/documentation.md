# Documentation and decisions

Context ends; project knowledge must not. Anything whose loss would force someone to re-investigate, or would lead to a wrong decision, goes into `docs/`. `docs/` is committed with the code it describes.

## Where things live

| What | Where |
|---|---|
| What the project is, layout, commands, gotchas, where the docs are | `AGENTS.md` (written by hand) |
| How to build, play and test, for a person | `README.md` |
| Lasting knowledge about the game and the code | `docs/<topic>.md`, `docs/architecture/<topic>.md`, index in `docs/README.md` |
| Decisions a reader could take for a mistake | `docs/adr/NNNN-<title>.md` |
| Findings outside the task's scope, accepted debt, follow-ups, questions for the user | `PLAN.md` in the repository root |
| Rules for agents | `.claude/rules/` |
| Repeatable procedures for agents | `.claude/skills/<name>/SKILL.md` |

Do not create task-, plan- or status-files in `docs/`.

Write new notes in English, short sentences. Existing notes in Russian stay as they are; continue a note in its own language.

## Knowledge notes (`docs/<topic>.md`)

Record:

- How a part of the game actually behaves when it is not obvious from one file: turn order, status queues, dominance, pathfinding, what a library really does.
- Dependencies between systems and contracts with libraries and packages (Unity Behavior, A* Pathfinding Project, Localization): what they really do, quirks, limits.
- Results of investigations: what was checked, what was found, what was ruled out.
- Implementation details a future change must respect.

Do not record: a list of files opened or changed, a diary of steps, anything the code or `git log` already says plainly, secrets or personal data.

Update an existing note instead of creating a parallel one. A note describes the current state; delete statements that became false. Add every new note to `docs/README.md`. Describe what is merged into `dev`, not work in progress.

- **No history in notes.** Describe how it works now. Do not compare with how it worked before or write which PR changed it. That belongs in `git log` and ADRs.
- **Tunable values by name.** Name the constant, the ScriptableObject or the serialized field that holds a balance number; repeat the number only when the rule depends on it (a threshold the design is built around).
- **Secrets.** Never write a secret, a license or a signing key.

```md
<!-- Bad: history -->
Regeneration now grows on the pulse; before it grew on every volume refresh.

<!-- Good: current behaviour -->
Regeneration grows on `OnPulse`; the same dominant substance keeps its traits.
```

## Architecture decision records (`docs/adr/`)

Write an ADR when a decision is not obvious from the code and a future reader could take it for a mistake: an unusual structure, a rejected "obvious" alternative, a pinned Unity or package version, a vendored library, a constraint imposed by a tool or a store. Number sequentially, never reuse a number.

- **Only decisions that were actually made.** How a part of the game behaves is a knowledge note, not an ADR.
- **The title states the decision, not a comparison.** "A* Pathfinding Project is vendored under Assets", not "Vendoring vs UPM"; the rejected options go under Alternatives.

```md
# NNNN. <Decision in a short phrase>

Status: accepted | superseded by NNNN
Date: YYYY-MM-DD

## Context
What forced the decision. Constraints, facts, numbers.

## Decision
What we do.

## Alternatives
What else was considered and why it was rejected.

## Consequences
What becomes easier, harder, or must not be changed without revisiting this ADR.
```

Before changing code that an ADR covers, read the ADR. If the change contradicts it, write a new ADR that supersedes it (set the old one's status to `superseded by NNNN`); do not edit the decision of an accepted ADR.

## The plan file (`PLAN.md`)

Write the item into `PLAN.md` in the same turn you notice it, before you answer. Mentioning it in the reply is not a record: only the plan file is. A mistake of your own (a missed finding, a wrong claim, a broken build you let through) is recorded the same way.

While working on a task you notice things that are not part of it: a bug, a test that never runs, a wrong doc, a rule the code breaks, a missing file. They are out of scope, so you do not fix them now, and they are not ignored either. Write each one down, so it is picked up later.

- Sections: **Open** (to do), **Accepted** (debt kept on purpose, with the reason and when to revisit), **Needs the user** (questions and decisions).
- One item per finding: where (`file:line`), what is wrong and what it causes, the proposed fix if you have one, and what it waits for.
- An item you cannot judge or fix without more information or a decision goes under **Needs the user**, with the question in one sentence, and you tell the user about it in your report. Do not leave it for later silently.
- Every question you asked the user that is still unanswered also goes under **Needs the user**, with the date it was asked and what waits on it. Ask it again in your next report; remove it once answered, and write the answer where it belongs (a note, an ADR, the item it unblocks).
- Data loss (saves, serialized fields, scene references) is marked as such and named in the report every time until it is taken into work.
- Remove an item when it is done or becomes a GitHub issue or PR (link it). `PLAN.md` is not a status log or a history: what was merged is in git.
- `PLAN.md` is committed with the change that adds or removes an item.

## Before finishing a task

Check whether the task changed something a note, an ADR, `AGENTS.md`, `README.md` or a rule states. Update them in the same change, and add follow-ups to `PLAN.md` instead of fixing unrelated things.
