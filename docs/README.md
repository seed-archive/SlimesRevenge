# Docs

Knowledge notes about the game and the code.

## Game

- [creatures.md](creatures.md) — the cast: hit points, armor, substances, devouring corpses.
- [behavior.md](behavior.md) — mob AI: personality toward the slime, fear/hate between mobs.
- [status-effects.md](status-effects.md) — status catalog: strike, puddle, dominance, innate.
- [slime-dominance.md](slime-dominance.md) — dominant substance (≥ 80% of volume) and its statuses.
- [strike-vs-puddle.md](strike-vs-puddle.md) — what a strike applies versus a floor puddle.
- [puddles.md](puddles.md) — floor puddles.
- [armor.md](armor.md) — armor, corrosion, damage reduction.
- [cell-menu.md](cell-menu.md) — the cell action menu.

## Architecture

- [architecture/dependency-inversion.md](architecture/dependency-inversion.md) — statuses apply via hooks; the board does not need a controlled player.
- [architecture/action-log.md](architecture/action-log.md) — the action log.
- [architecture/popup-cards.md](architecture/popup-cards.md) — popup cards.
