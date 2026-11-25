# Slime appearance

How the slime's sprite follows what its body is made of.

## Entry points

- `SlimeAppearance.FromVolume(Volume)` picks a `SlimeLook` for a volume.
- `Slime.ApplyAppearance()` runs in `Awake` and every `LateUpdate`; it redraws only when the look changed or `Slime.Face(cell)` turned the slime (it resets `appearanceStarted`).
- `SlimeSprites.Walk(sheetMember, facing)` slices the walk frames for one facing out of a slime sheet.

## Choosing the look

- Each `Substance` names its `Look` and an `AppearanceTieBreak` (lower wins).
- The look is the plurality: the substance look with the most units in the volume. A tie goes to the lower `AppearanceTieBreak`: water → poison → acid → oil → blood → lava → mercury.
- An empty volume (or none) looks like water.
- This is not dominance: dominance (`slime-dominance.md`) needs a share of the volume and drives statuses; the look only needs the most units.

## Sprites

- `Slime` holds one sheet sprite per look in serialized fields (`waterSprite`, `oilSprite`, `poisonSprite`, `acidSprite`, `bloodSprite`, `lavaSprite`, `mercurySprite`), set in the `Game` scene; sheets live in `Assets/Art/Slimes/`. A missing `mercurySprite` falls back to `waterSprite`.
- Sheets use the FDR layout (`FdrSheetLayout`): `CellsPerRow` cells per row, rows south / west / east / north, walk in columns `WalkColumn`…`WalkColumn + WalkFrameCount - 1`.
- `SlimeSprites.Walk` builds the frames with `Sprite.Create` from the sheet's texture, using the sheet sprite's width as the cell size and its pixels per unit. Frames are cached per (texture, facing) for the whole session.
- With more than one frame, `Slime` plays them through `CreatureSpriteAnimator.Play`; otherwise it shows the sheet sprite itself.

## Tests

- `SlimeSprites.ResetCacheForTests` (editor only) destroys the cached frames and clears the cache; the sheet assets are untouched. Tests call it in `SetUp` and `TearDown` (`SlimeAppearanceTests`, `SlimeTests`). A frame taken before a reset is dead afterwards.
- The sprites of `Mercury.png` are named `Water_N` (its `.meta` is a copy of `Water.png.meta`), so find sprites by sheet path, not by name.
