# Repository boundaries

This repository is self-contained. Everything an agent needs to work here is in this repository.

- Other systems (stores, Unity services, the GameCI images) are external dependencies, described by contract only: service name, package id and version, secret name, bundle id.
- Do not read, edit, run or link files of other repositories, and do not assume they are checked out next to this one.
- No relative paths that leave the repository (`../<other repo>/...`) in code comments, docs, rules or the plan.
- Files an external tool writes outside the repository (an image generator's output, the Asset Store cache) are copied in, never referenced by their outside path.
