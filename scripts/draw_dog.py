#!/usr/bin/env python3
"""Draws the Dog creature sheet (512x208, FDR enemy layout) pixel by pixel.

Run from the repository root:  python3 -I scripts/draw_dog.py
Writes Assets/Art/Animals/Dog.png and Assets/Resources/Animals/Dog.png (same sheet),
and a 4x preview over grass to scripts/out/Dog_preview.png. Deterministic.
"""
import os
import site
import sys

try:
    from PIL import Image
except ModuleNotFoundError:  # -I hides the user site-packages; Pillow lives there
    sys.path.append(site.getusersitepackages())
    from PIL import Image

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SHEETS = [os.path.join(ROOT, "Assets", d, "Animals", "Dog.png") for d in ("Art", "Resources")]
PREVIEW = os.path.join(ROOT, "scripts", "out", "Dog_preview.png")
SHEET_W, SHEET_H = 512, 208

# ---------------------------------------------------------------- palette
PAL = {
    "W": (255, 255, 255),  # eye glint, teeth, bite FX, white stamps
    "C": (250, 236, 204),  # cream: muzzle, chest, paws, tail tip
    "c": (212, 180, 140),  # cream shadow
    "H": (246, 196, 120),  # fur highlight
    "F": (222, 150, 76),   # fur base
    "f": (176, 104, 50),   # fur shadow 1
    "d": (122, 68, 40),    # fur shadow 2 (ears, far legs)
    "o": (84, 42, 30),     # outline: dark fur tone, not black (as on the pack sheets)
    "P": (244, 128, 140),  # tongue
    "R": (214, 48, 56),    # collar
    "r": (132, 26, 44),    # collar shadow, mouth interior
    "n": (20, 14, 18),     # eyes, nose
}
PALETTE_ORDER = "WCcHFfdoPRrn"
N4 = ((1, 0), (-1, 0), (0, 1), (0, -1))


class Group:
    """A dog part: fill pixels from small ASCII maps that share one 1 px outline."""

    def __init__(self):
        self.p = {}

    def add(self, art, ox, oy):
        for j, row in enumerate(art):
            for i, ch in enumerate(row):
                if ch != ".":
                    self.p[(ox + i, oy + j)] = ch
        return self

    def px(self, x, y, ch):
        self.p[(x, y)] = ch
        return self

    def onto(self, cell, outline=True, dx=0, dy=0):
        pts = {(x + dx, y + dy): c for (x, y), c in self.p.items()}
        if outline:
            for (x, y) in pts:
                for ax, ay in N4:
                    q = (x + ax, y + ay)
                    if q not in pts:
                        cell[q] = "o"
        cell.update(pts)
        return cell


def mirror(cell, w=32):
    return {(w - 1 - x, y): c for (x, y), c in cell.items()}


def sym(rows):
    """Left half -> full symmetric rows."""
    return [r + r[::-1] for r in rows]


def patch(art, edits):
    """Return a copy of art with {(col,row): ch} replaced."""
    rows = [list(r) for r in art]
    for (cx, cy), ch in edits.items():
        rows[cy][cx] = ch
    return ["".join(r) for r in rows]


# ================================================================ SOUTH
S_HEAD = [
    "....FFFFFF....",
    "..FFHHHHHHFF..",
    ".fFHHFFFFFFFf.",
    "dfFFFFFFFFFFfd",
    "ddFWnFFFFnWFdd",
    "ddFnnCCCCnnFdd",
    "ddFFCCnnCCFFdd",
    ".dfFCCCCCCFfd.",
    "...ffccccff...",
]
S_HEAD_OPEN = S_HEAD[:7] + [
    ".dfFWrrrrWFfd.",
    "...frPPPPrf...",
    "....cCCCCc....",
]
S_HEAD_HALF = S_HEAD[:7] + [
    ".dfFCrrrrCFfd.",
    "...ffcPPcff...",
]
S_BODY = [
    "FFFFFFFFFF",
    "FFFFFFFFFF",
    "FRRRHHRRRF",
    "FFCCCCCCFF",
    "fFCCCCCCFf",
    "ffcCCCCcff",
]


def s_face(art, eyes):
    if eyes == "open":
        return art
    if eyes == "squint":  # hurt: > <
        return patch(art, {(3, 4): "F", (4, 4): "F", (9, 4): "F", (10, 4): "F"})
    if eyes == "angry":  # slanted brows over the eyes
        return patch(art, {(3, 3): "o", (3, 4): "o", (4, 4): "o", (10, 3): "o",
                           (10, 4): "o", (9, 4): "o", (4, 3): "F", (9, 3): "F"})
    raise ValueError(eyes)


def south(leg=0, dy=0, head_dy=0, eyes="open", mouth="closed", tail=0,
          fx=0, splay=0):
    cell = {}
    hx, hy = 9, 6
    # tail (behind everything): 0 none, 1 right high, 2 right low, -1/-2 left
    if tail:
        t = Group()
        if abs(tail) == 1:
            t.px(21, 17, "F").px(22, 16, "F").px(23, 15, "C")
        else:
            t.px(21, 17, "F").px(22, 17, "C")
        tc = t.onto({}, dy=dy)
        cell.update(tc if tail > 0 else mirror(tc))
    # body + front legs
    b = Group().add(S_BODY, 11, 14)
    if leg == 0:      # viewer-left paw planted, right paw lifted, right hip shows
        b.add(["FFf", "CCc"], 11, 20).add(["cCC"], 18, 20)
        b.add(["f", "c"], 21, 19)
    elif leg == 2:
        b.add(["CCc"], 11, 20).add(["fFF", "cCC"], 18, 20)
        b.add(["f", "c"], 10, 19)
    elif leg == 1:    # passing: both paws level
        b.add(["FFf", "CCc"], 11, 20).add(["fFF", "cCC"], 18, 20)
    elif leg == 3:    # crouch / splayed
        b.add(["CCc"], 11 - splay, 20).add(["cCC"], 18 + splay, 20)
    b.onto(cell, dy=dy)
    # head
    art = {"closed": S_HEAD, "open": S_HEAD_OPEN, "half": S_HEAD_HALF}[mouth]
    Group().add(s_face(art, eyes), hx, hy).onto(cell, dy=dy + head_dy)
    # bite FX (white fang arc under the jaws)
    if fx == 2:
        Group().add([
            "WW........WW",
            "WWW......WWW",
            ".WWWWCCWWWW.",
            "..CWWWWWWC..",
            "....CCCC....",
        ], 10, 26).onto(cell, outline=False)
    elif fx == 1:
        Group().add([
            "W..........W",
            "............",
            ".W........W.",
            "...W.CC.W...",
        ], 10, 25).onto(cell, outline=False)
    return cell


def south_frames():
    w1 = south(leg=0, tail=1)
    w2 = south(leg=1, dy=-1, tail=2)
    w3 = south(leg=2, tail=-1)
    w4 = south(leg=1, dy=-1, tail=-2)
    a5 = south(leg=3, dy=-2, head_dy=2, eyes="angry", tail=1)
    a6 = south(leg=1, dy=3, head_dy=1, eyes="angry", mouth="open", tail=-1, fx=2)
    a7 = south(leg=1, dy=1, eyes="angry", mouth="half", tail=2, fx=1)
    h10 = south(leg=3, splay=1, dy=-1, eyes="squint", mouth="half", tail=-2)
    h11 = south(leg=0, eyes="squint", tail=1)
    return [w1, w2, w3, w4, a5, a6, a7, w2, None, h10, h11, w2]


# ================================================================ NORTH
N_HEAD = [   # back of the skull; 'o' lines split the hanging ears from it
    "....FFFFFF....",
    "..FFHHHHHHFF..",
    ".dFHHFFFFFFFd.",
    "ddFFFFFFFFFFdd",
    "ddoFFFFFFFFodd",
    "ddoffffffffodd",
    ".dd........dd.",
]
N_BODY = [   # collar, shoulders, wider rump
    "..RRRRRRRR..",
    ".FFHHHHHHFF.",
    ".FFFFFFFFFF.",
    "FFFFFFFFFFFF",
    "FFFFFFFFFFFF",
    "fFFFFFFFFFFf",
    "ffFFFFFFFFff",
    ".ffffffffff.",
]


def north(leg=0, dy=0, head_dy=0, tail=0, fx=0, flare=0):
    cell = {}
    b = Group().add(N_BODY, 10, 12)
    if leg == 0:      # same feet as the south row, seen from behind
        b.add(["ccc"], 11, 20).add(["FFf", "CCc"], 18, 20)
    elif leg == 2:
        b.add(["fFF", "cCC"], 11, 20).add(["ccc"], 18, 20)
    elif leg == 1:
        b.add(["fFF", "cCC"], 11, 20).add(["FFf", "CCc"], 18, 20)
    elif leg == 3:    # crouch
        b.add(["ccc"], 11, 20).add(["ccc"], 18, 20)
    elif leg == 4:    # hind legs kicked back during the lunge
        b.add(["fFF", "fFF", "cCC"], 11, 20).add(["FFf", "FFf", "CCc"], 18, 20)
    b.onto(cell, dy=dy)
    # tail lies up along the back, no outline (soft brown edging instead):
    # -1 / 0 / +1 = cream tip leaning, 3 = tucked down between the legs (hurt)
    if tail == 3:
        for x in (15, 16):
            cell[(x, 18 + dy)] = "f"
            cell[(x, 19 + dy)] = "H"
            cell[(x, 20 + dy)] = "C"
            cell[(x, 21 + dy)] = "o"
    else:
        for y in (14, 15, 16, 17):
            lean = tail if y < 16 else 0
            cell[(14 + lean, y + dy)] = "f"
            cell[(17 + lean, y + dy)] = "f"
            for x in (15, 16):
                cell[(x + lean, y + dy)] = "C" if y < 16 else "H"
        cell[(15 + tail, 13 + dy)] = "f"
        cell[(16 + tail, 13 + dy)] = "f"
        cell[(15, 18 + dy)] = "f"
        cell[(16, 18 + dy)] = "f"
    g = Group().add(N_HEAD, 9, 5)
    if flare:  # ears thrown outwards
        for y in (8, 9, 10):
            g.px(8, y, "d").px(23, y, "d")
    g.onto(cell, dy=dy + head_dy)
    if fx == 2:
        Group().add([
            "..CWWWWWWC..",
            ".WWWW..WWWW.",
            "WW........WW",
        ], 10, 0).onto(cell, outline=False)
    elif fx == 1:
        Group().add([
            "...W.CC.W...",
            ".W........W.",
            "W..........W",
        ], 10, 0).onto(cell, outline=False)
    return cell


def north_frames():
    w1 = north(leg=0, tail=-1)
    w2 = north(leg=1, dy=-1, tail=0)
    w3 = north(leg=2, tail=1)
    w4 = north(leg=1, dy=-1, tail=0)
    a5 = north(leg=3, dy=2, head_dy=1, tail=0)
    a6 = north(leg=4, dy=-2, head_dy=-1, tail=1, fx=2, flare=1)
    a7 = north(leg=1, dy=-1, tail=-1, fx=1)
    h10 = north(leg=3, dy=1, tail=3, flare=1)
    h11 = north(leg=0, tail=3)
    return [w1, w2, w3, w4, a5, a6, a7, w2, None, h10, h11, w2]


# ================================================================ WEST
W_HEAD = [
    "....FFFFF..",
    "...FHHHHFF.",
    "..FFHFFddFF",
    "..FFWFdffdF",
    ".CFFnFdffdF",
    "nCCCFFdfddF",
    "CCCCCFFddF.",
    ".ccccfFFF..",
]
W_HEAD_OPEN = [
    "....FFFFF..",
    ".CCFHHHHFF.",
    "nCCCHFFddFF",
    "CCCCoFdffdF",
    "W.WrnFdffdF",
    "..rrrFdfddF",
    ".PPPrFFddF.",
    "CWCCcfFFF..",
    ".ccc.......",
]
W_HEAD_HALF = [
    "....FFFFF..",
    "...FHHHHFF.",
    "..FFHFFddFF",
    ".CFFoFdffdF",
    "nCCCnFdffdF",
    "CCCCFFdfddF",
    ".rrrrFFddF.",
    ".CCCcfFFF..",
]
W_BODY = [
    "..FHHHHF.",
    ".FFFFFFFF",
    "FFFFFFFFF",
    "FFFFFFFFf",
    "fFFFFFFff",
    ".fccccff.",
]
BX = 2  # the profile sits slightly right in the west cell


def w_leg(g, base, lean, near=True, lift=0):
    """A 2 px wide leg hanging from y=18. lean: -1 forward, 0, +1 back."""
    a, b, paw, paw2 = ("F", "f", "C", "c") if near else ("d", "d", "f", "d")
    rows = 4 - lift
    for i in range(rows):
        x = base + (lean if i > 0 else 0)
        if i == rows - 1:
            g.px(x, 18 + i, paw).px(x + 1, 18 + i, paw2)
        else:
            g.px(x, 18 + i, a).px(x + 1, 18 + i, b)


def west(leg=0, dx=0, dy=0, head=(0, 0), eyes="open", mouth="closed", tail=0,
         fx=0, crouch=0):
    cell = {}
    ox = BX + dx
    # tail: 0 up, 1 up curled, 2 straight back, 3 down (hurt)
    t = Group()
    if tail == 0:
        t.add(["..C", ".FC", "FF.", "Ff."], 22, 8)
    elif tail == 1:
        t.add([".CC", ".FC", "FF.", "F.."], 22, 9)
    elif tail == 2:
        t.add(["FFCC", "ff.."], 22, 12)
    else:
        t.add(["F..", "fF.", ".fC"], 23, 14)
    t.onto(cell, dx=ox, dy=dy + crouch)
    # far legs (dark), then body + near legs
    far = Group()
    b = Group().add(W_BODY, 14, 12 + crouch)
    if leg == 0:      # near pair planted and spread, far pair lifted under the belly
        w_leg(far, 16, 0, near=False, lift=1)
        w_leg(far, 19, 0, near=False, lift=1)
        w_leg(b, 14, -1)
        w_leg(b, 21, 1)
    elif leg == 2:    # far pair planted and spread, near pair lifted and gathered
        w_leg(far, 14, -1, near=False)
        w_leg(far, 21, 1, near=False)
        w_leg(b, 15, 0, lift=1)
        w_leg(b, 20, 0, lift=1)
    elif leg == 1:    # passing: legs under the body
        w_leg(far, 16, 0, near=False)
        w_leg(far, 19, 0, near=False)
        w_leg(b, 14, 0)
        w_leg(b, 21, 0)
    elif leg == 4:    # lunge: front reaching, hind pushing
        w_leg(far, 15, -1, near=False, lift=1)
        w_leg(far, 20, 1, near=False)
        w_leg(b, 14, -1, lift=1)
        w_leg(b, 21, 1)
    elif leg == 3:    # crouch
        w_leg(far, 16, 0, near=False, lift=1)
        w_leg(far, 19, 0, near=False, lift=1)
        w_leg(b, 14, -1)
        w_leg(b, 21, 0)
    far.onto(cell, dx=ox, dy=dy)
    b.onto(cell, dx=ox, dy=dy)
    # collar
    for y in (13, 14, 15):
        cell[(16 + ox, y + dy + crouch)] = "R"
    cell[(16 + ox, 16 + dy + crouch)] = "r"
    # head
    art = {"closed": W_HEAD, "open": W_HEAD_OPEN, "half": W_HEAD_HALF}[mouth]
    hy = 7 if mouth != "open" else 6
    if eyes == "squint":
        art = patch(art, {(4, 3): "F", (3, 4): "n", (4, 4): "n"})
    elif eyes == "angry" and mouth == "closed":
        art = patch(art, {(4, 3): "o", (3, 2): "o"})
    Group().add(art, 5, hy).onto(cell, dx=ox + head[0], dy=dy + head[1])
    if fx == 2:
        Group().add([
            "...WW",
            "..WWW",
            ".CWW.",
            "CWW..",
            "CWC..",
            "CWC..",
            "CWW..",
            ".CWW.",
            "..WWW",
            "...WW",
        ], 0, 8).onto(cell, outline=False)
    elif fx == 1:
        Group().add([
            "...W.",
            ".....",
            ".W...",
            "W....",
            "c....",
            "W....",
            ".W...",
            ".....",
            "...W.",
        ], 0, 8).onto(cell, outline=False)
    return cell


def west_frames():
    w1 = west(leg=0, tail=0)
    w2 = west(leg=1, dy=-1, tail=1)
    w3 = west(leg=2, tail=0)
    w4 = west(leg=1, dy=-1, tail=1)
    a5 = west(leg=3, dx=2, head=(0, 2), eyes="angry", tail=0, crouch=1)
    a6 = west(leg=4, dx=-3, head=(-1, 0), mouth="open", tail=2, fx=2)
    a7 = west(leg=1, dx=-1, head=(-1, 0), mouth="half", tail=2, fx=1)
    h10 = west(leg=3, dx=2, dy=-1, head=(1, 0), eyes="squint", mouth="half", tail=3)
    h11 = west(leg=0, eyes="squint", tail=3)
    return [w1, w2, w3, w4, a5, a6, a7, w2, None, h10, h11, w2]


# ================================================================ damage / dead
def white(cell):
    return {p: "W" for p in cell}


def h01(x, y, s):
    n = (x * 73856093) ^ (y * 19349663) ^ (s * 83492791)
    n &= 0xFFFFFFFF
    n = ((n ^ (n >> 13)) * 1274126177) & 0xFFFFFFFF
    n ^= n >> 16
    return (n & 0xFFFF) / 65536.0


def dissolve(mask, stage, seed):
    """White stamp breaking into irregular crumbs that drift up. stage 1..3."""
    xs = [p[0] for p in mask]
    ys = [p[1] for p in mask]
    cx = sum(xs) / len(xs)
    cy = sum(ys) / len(ys)
    seeds = {}
    for gy in range(-1, 12):
        for gx in range(-1, 12):
            sx = gx * 3 + 1 + round(h01(gx, gy, seed) * 2 - 1)
            sy = gy * 3 + 1 + round(h01(gx, gy, seed + 9) * 2 - 1)
            seeds[(gx, gy)] = (sx, sy)
    keep, crack, spread, lift = {
        1: (1.00, 0.30, 0.08, 0.5),
        2: (0.96, 0.22, 0.16, 1.4),
        3: (0.82, 0.22, 0.26, 2.4),
    }[stage]
    out = set()
    for (x, y) in mask:
        near = sorted(((sx - x) ** 2 + (sy - y) ** 2, k) for k, (sx, sy) in seeds.items())[:2]
        d1, k = near[0]
        d2 = near[1][0]
        a, b = h01(k[0], k[1], seed + 31), h01(k[0], k[1], seed + 47)
        if a > keep:                                   # this crumb is already gone
            continue
        if d2 ** 0.5 - d1 ** 0.5 < crack:              # crack between two crumbs
            continue
        sx, sy = seeds[k]
        if stage > 1:                                  # crumbs shrink to 2-3 px blocks
            w = (3 if b > 0.45 else 2) if stage == 2 else (2 if b > 0.4 else 1)
            h = (3 if a * 7 % 1 > 0.6 else 2) if stage == 2 else 2
            ex, ey = x - sx + 1, y - sy + 1
            if not (0 <= ex < w and 0 <= ey < h):
                continue
            if stage == 3 and (ex, ey) == (1, 1) and b > 0.7:
                continue
        sx, sy = seeds[k]
        q = (x + round((sx - cx) * spread), y + round((sy - cy) * spread - lift))
        if 0 <= q[0] < 32 and 0 <= q[1] < 32:
            out.add(q)
    return out


# ================================================================ battle 64x64
B_HEAD_L = [
    ".........FFFF",
    "......FFFHHHH",
    "....FFFHHHHHH",
    "..ffFFHHHFFFF",
    ".dffFFFFFFFFF",
    "ddfdFFFFFFFFF",
    "dffdFFFnnFFFF",
    "dffdFFnWnnFFC",
    "dffdFFnnnnFCC",
    "dffdFFFnnFCCC",
    "ddfdfFFFFCCnn",
    "dddd.fFFCCCnn",
    ".dd..ffFCoCCo",
    "......ffcCooC",
    ".......ffcCCC",
    ".........fccc",
]
B_MOUTH_OPEN_L = [  # replaces rows 13.. when panting
    "......ffcCorr",
    ".......ffcoPP",
    ".........fcPP",
    "...........PP",
]
B_BODY_L = [
    ".......FFFFFF",
    "......fFFFFFF",
    ".....fFFFCCCC",
    "....fFFFCCCCC",
    "...fFFFFCCCCC",
    "..fFFFFCCCCCC",
    ".fFFFFFCCCCCC",
    ".fFFFfFFCCCCC",
    "fFFFFdFFFCCCC",
    "fFFFFdFFFFCCc",
    "fFFFfdFFFFccd",
    "fFFffdfFFFfdd",
    "fffff.fFFFfdd",
    "CCCCc.CCCCc..",
    "cCCcc.cCCcc..",
]
B_COLLAR_L = [
    ".....fRRRRRRR",
    "......rrRRRHH",
    "...........HH",
]


def battle(beat, eyes="open"):
    """beat 0..3: rest, inhale, peak, exhale."""
    head_dy = (0, -2, -1, 1)[beat]
    chest_dy = (0, -1, -1, 0)[beat]
    pant = beat in (1, 2)
    wag = beat % 2
    cell = {}
    # tail behind the right haunch
    t = Group()
    if wag == 0:
        t.add(["...CC", "..FCC", ".FFF.", "FFF..", "FF..."], 43, 33)
    else:
        t.add(["..CC.", "..FCC", ".FFF.", "FFF..", "FF..."], 44, 32)
    t.onto(cell)
    # body (static floor) + breathing collar
    b = Group().add(sym(B_BODY_L), 19, 29)
    b.add(sym(B_COLLAR_L), 19, 31 + chest_dy)
    b.onto(cell)
    # head
    rows = list(B_HEAD_L)
    if pant:
        rows = rows[:13] + B_MOUTH_OPEN_L
    if eyes == "half":
        rows = patch(rows, {(7, 6): "F", (8, 6): "F", (6, 7): "f", (7, 7): "f",
                            (8, 7): "f", (9, 7): "f"})
    elif eyes == "closed":
        rows = patch(rows, {(7, 6): "F", (8, 6): "F", (6, 7): "F", (7, 7): "F",
                            (8, 7): "F", (9, 7): "F", (6, 8): "o", (7, 8): "F",
                            (8, 8): "F", (9, 8): "o", (7, 9): "o", (8, 9): "o"})
    Group().add(sym(rows), 19, 15).onto(cell, dy=head_dy)
    return cell


def battle_frames():
    eyes = ["open", "open", "open", "open", "closed", "half", "open", "open"]
    return [battle(i % 4, eyes[i]) for i in range(8)]


# ================================================================ footer
FONT = {
    "A": ["#####", "#...#", "#####", "#...#", "#...#"],
    "B": ["####.", "#...#", "####.", "#...#", "####."],
    "C": ["#####", "#....", "#....", "#....", "#####"],
    "D": ["####.", "#...#", "#...#", "#...#", "####."],
    "E": ["#####", "#....", "###..", "#....", "#####"],
    "F": ["#####", "#....", "###..", "#....", "#...."],
    "G": ["#####", "#....", "#.###", "#...#", "#####"],
    "I": ["#", "#", "#", "#", "#"],
    "K": ["#...#", "#..#.", "###..", "#..#.", "#...#"],
    "L": ["#....", "#....", "#....", "#....", "#####"],
    "M": ["#...#", "##.##", "#.#.#", "#...#", "#...#"],
    "N": ["#...#", "##..#", "#.#.#", "#..##", "#...#"],
    "O": ["#####", "#...#", "#...#", "#...#", "#####"],
    "P": ["#####", "#...#", "#####", "#....", "#...."],
    "R": ["#####", "#...#", "####.", "#...#", "#...#"],
    "S": ["#####", "#....", "#####", "....#", "#####"],
    "T": ["#####", "..#..", "..#..", "..#..", "..#.."],
    "W": ["#...#", "#...#", "#.#.#", "##.##", "#...#"],
    "X": ["#...#", ".#.#.", "..#..", ".#.#.", "#...#"],
    "Y": ["#...#", ".#.#.", "..#..", "..#..", "..#.."],
    "2": ["#####", "....#", "#####", "#....", "#####"],
    "3": ["#####", "....#", ".####", "....#", "#####"],
    "4": ["#...#", "#...#", "#####", "....#", "....#"],
    "6": ["#####", "#....", "#####", "#...#", "#####"],
    "8": ["#####", "#...#", "#####", "#...#", "#####"],
    "-": ["...", "...", "###", "...", "..."],
    " ": ["...", "...", "...", "...", "..."],
}


def text_width(s):
    return sum(len(FONT[ch][0]) + 1 for ch in s) - 1


def draw_text(px, s, cx, y):
    x = round(cx - text_width(s) / 2)
    glyph = set()
    for ch in s:
        g = FONT[ch]
        for j, row in enumerate(g):
            for i, c in enumerate(row):
                if c == "#":
                    glyph.add((x + i, y + j))
        x += len(g[0]) + 1
    for (gx, gy) in glyph:  # 1 px drop shadow, as on the pack sheets
        if (gx, gy + 1) not in glyph:
            px[gx, gy + 1] = (0, 0, 0, 255)
    for (gx, gy) in glyph:
        px[gx, gy] = (255, 255, 255, 255)


# ================================================================ sheet
def blit(px, cell, ox, oy, size, alpha=255, rgb=None):
    for (x, y), ch in cell.items():
        if not (0 <= x < size and 0 <= y < size):
            raise ValueError("pixel (%d,%d) outside its %d px cell at %d,%d" % (x, y, size, ox, oy))
        col = rgb if rgb else PAL[ch]
        px[ox + x, oy + y] = col + (alpha,)


def build():
    im = Image.new("RGBA", (SHEET_W, SHEET_H), (0, 0, 0, 0))
    px = im.load()
    west_f = west_frames()
    rows = [
        south_frames(),
        west_f,
        [mirror(c) if c is not None else None for c in west_f],
        north_frames(),
    ]
    for r, frames in enumerate(rows):
        oy = r * 32
        for c, cell in enumerate(frames):
            if cell is not None:
                blit(px, cell, c * 32, oy, 32)
        stamp = set(frames[0])
        blit(px, white(frames[0]), 8 * 32, oy, 32)    # C9
        blit(px, white(frames[0]), 12 * 32, oy, 32)   # C13
        for stage, alpha in ((1, 150), (2, 90), (3, 60)):
            pts = dissolve(stamp, stage, seed=r * 17 + 3)
            blit(px, {p: "W" for p in pts}, (12 + stage) * 32, oy, 32, alpha=alpha)
    for b, cell in enumerate(battle_frames()):
        blit(px, cell, b * 64, 128, 64)
    # footer: flush 4x4 swatches + labels
    bar_x = 256 - len(PALETTE_ORDER) * 2
    for i, ch in enumerate(PALETTE_ORDER):
        for yy in range(194, 198):
            for xx in range(4):
                px[bar_x + i * 4 + xx, yy] = PAL[ch] + (255,)
    draw_text(px, "ENEMY SPRITE 32X32 FRAMES", 110, 193)
    draw_text(px, "WALK - ATTACK - DAMAGE - DEAD", 110, 200)
    draw_text(px, "COLOR PALETTE", 256, 200)
    draw_text(px, "BATTLE SPRITE 64X64 FRAMES", 414, 193)
    draw_text(px, "8 FRAMES ANIMATION", 414, 200)
    return im


def main():
    im = build()
    assert im.size == (SHEET_W, SHEET_H)
    for sheet in SHEETS:
        im.save(sheet)
    bg = Image.new("RGBA", im.size, (104, 158, 78, 255))
    bg.alpha_composite(im)
    bg = bg.resize((SHEET_W * 4, SHEET_H * 4), Image.NEAREST)
    os.makedirs(os.path.dirname(PREVIEW), exist_ok=True)
    bg.convert("RGB").save(PREVIEW)


if __name__ == "__main__":
    main()
