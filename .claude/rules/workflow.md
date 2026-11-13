# Task workflow

1. **Start** — understand the task from the user. Write the acceptance criteria before writing code; if the task has none, propose them and get them confirmed. If the requirement is ambiguous, ask; do not guess.
2. **Branch** — before the first edit, switch to a branch for the task created from the base branch **`dev`** (releases go to `main`). Without a given name use `wip-<short-topic>`.
3. **Implement** — the smallest change that satisfies the criteria. Follow the rules in `.claude/rules/` and the matching note in `docs/` (`AGENTS.md` → domain docs).
4. **Verify** — run the verification gates: the focused EditMode filter for the area while working, the full EditMode suite before a PR (`editmode-tests` skill), `./scripts/format.sh check`. A visible change is also checked in Play Mode (`./scripts/play.sh --duel` / `--campaign`). Tests must check the acceptance criteria, not the implementation you just wrote (`testing` rule).
5. **Simplify** — KISS / DRY / YAGNI pass over the whole diff (`kiss-dry-yagni` rule). Re-run the affected checks.
6. **Review** — the change is reviewed by someone who did not write it (the user, or a separate subagent given the requirement, the criteria and the diff base). Fix or answer every finding. Merge only after approval. The AI / combat / status checklist in `AGENTS.md` is part of the review.
7. **Document** — `docs/` notes, ADRs, `AGENTS.md`, `README.md` (`documentation` rule).
8. **Report** — what changed, the checks run with their outcome, what is left, and every question to the user that is still unanswered (from "Needs the user" in `PLAN.md`, asked again). Follow-ups go to `PLAN.md`, not into code.

Problems noticed outside the task's scope are not fixed silently and not dropped: report them and add them to `PLAN.md`. A question to the user is not dropped either when it stays unanswered: it goes under "Needs the user" in `PLAN.md` with its date and is asked again (`documentation` rule, "The plan file").

## Definition of Done

- Every acceptance criterion is met and has a test or a recorded manual check (Play Mode, screenshot) that proves it.
- The project compiles, `./scripts/format.sh check` passes, and the relevant EditMode tests were **run** and **passed** in this session, with the command and the results file named in the report.
- No new `[Ignore]`, `[Explicit]`, `Assert.Pass`/`Assert.Inconclusive` shortcuts, raised `[Timeout]`, `#pragma warning disable` or `// csharpier-ignore` introduced to get green.
- Every new or moved asset is committed together with its `.meta` file (`unity` rule).
- KISS / DRY / YAGNI pass done.
- Saved data and serialized fields stay loadable: a renamed serialized field keeps `[FormerlySerializedAs]`, and a changed save format is stated in the report.
- Documentation updated.

## Reporting

Keep three statements apart: **test written**, **test run**, **test passed**. "Verified" means a check ran and passed in this session. When a check was not run, say so and why. When a check failed, show the failing output.

## Git

- Never write code on `dev`, `main` or someone else's branch. Check `git branch --show-current` right before every commit and push; if it is not the task's branch, stop.
- Commit messages follow conventional commits (`fix:`, `feat:`, `feat!:` / `BREAKING CHANGE:`, `chore:`, `docs:`, `test:`, `ci:`), one logical change per commit. Art, vendor libraries and scenes go into their own commits, not mixed with gameplay code.
- A pull request targets `dev`. Its description says what changes and why, how it was verified (commands and results), and what is not verified.
- Do not force-push shared branches and do not rewrite pushed history. Tags are not deleted without the user's yes.
- Pull requests are merged with **squash** (one commit per PR on `dev`).
- Work in the repository's main checkout on a task branch: no git worktrees, temporary clones or side checkouts. Uncommitted changes found there are shown to the user, not routed around.
- After a merge, delete the task branch locally. A squash-merged branch is not an ancestor of `dev`, so check by the pull request (`gh pr list --state merged --head <branch>`), then `git branch -D <branch>`. Remote branches are deleted only with the user's yes.
