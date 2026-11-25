using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace SlimesRevenge.Tests
{
    public class GridStepTests
    {
        [Test]
        public void IsAdjacent_IncludesDiagonals_NotFarther()
        {
            var origin = Vector2Int.zero;
            Assert.IsTrue(GridStep.IsAdjacent(origin, Vector2Int.right));
            Assert.IsTrue(GridStep.IsAdjacent(origin, Vector2Int.up));
            Assert.IsTrue(GridStep.IsAdjacent(origin, new Vector2Int(1, 1)));
            Assert.IsTrue(GridStep.IsAdjacent(origin, new Vector2Int(-1, 1)));
            Assert.IsFalse(GridStep.IsAdjacent(origin, origin));
            Assert.IsFalse(GridStep.IsAdjacent(origin, new Vector2Int(2, 0)));
            Assert.IsFalse(GridStep.IsAdjacent(origin, new Vector2Int(2, 1)));
        }

        [Test]
        public void AdjacentOffsets_AreExactlyChebyshevOne()
        {
            Assert.AreEqual(8, GridStep.AdjacentOffsets.Length);
            var seen = new HashSet<Vector2Int>();
            foreach (var offset in GridStep.AdjacentOffsets)
            {
                Assert.AreEqual(1, GridStep.Chebyshev(Vector2Int.zero, offset));
                Assert.IsTrue(seen.Add(offset));
            }
        }

        [Test]
        public void Ring_DistanceOne_MatchesAdjacentOffsets()
        {
            var origin = new Vector2Int(3, 4);
            var ring = new HashSet<Vector2Int>(GridStep.Ring(origin, 1));
            Assert.AreEqual(8, ring.Count);
            foreach (var offset in GridStep.AdjacentOffsets)
            {
                Assert.IsTrue(ring.Contains(origin + offset));
            }
        }

        [Test]
        public void Ring_DistanceTwo_IsTheSixteenCellBorder()
        {
            var origin = new Vector2Int(3, 4);
            var ring = new HashSet<Vector2Int>(GridStep.Ring(origin, 2));
            Assert.AreEqual(16, ring.Count);
            foreach (var cell in ring)
            {
                Assert.AreEqual(2, GridStep.Chebyshev(origin, cell));
            }
        }

        [Test]
        public void Ring_ZeroOrNegativeDistance_IsEmpty()
        {
            CollectionAssert.IsEmpty(GridStep.Ring(Vector2Int.zero, 0));
            CollectionAssert.IsEmpty(GridStep.Ring(Vector2Int.zero, -1));
        }

        [Test]
        public void InRange_IncludesTheRangeBorder()
        {
            Assert.IsTrue(GridStep.InRange(Vector2Int.zero, new Vector2Int(2, -2), 2));
            Assert.IsTrue(GridStep.InRange(Vector2Int.zero, Vector2Int.zero, 0));
            Assert.IsFalse(GridStep.InRange(Vector2Int.zero, new Vector2Int(3, 1), 2));
        }

        [Test]
        public void FromSwipe_PicksTheNearestOfEightDirections()
        {
            Assert.AreEqual(Vector2Int.right, GridStep.FromSwipe(new Vector2(1f, 0f)));
            Assert.AreEqual(new Vector2Int(1, 1), GridStep.FromSwipe(new Vector2(1f, 1f)));
            Assert.AreEqual(Vector2Int.up, GridStep.FromSwipe(new Vector2(0f, 1f)));
            Assert.AreEqual(new Vector2Int(-1, 1), GridStep.FromSwipe(new Vector2(-1f, 1f)));
            Assert.AreEqual(Vector2Int.left, GridStep.FromSwipe(new Vector2(-1f, 0f)));
            Assert.AreEqual(new Vector2Int(-1, -1), GridStep.FromSwipe(new Vector2(-1f, -1f)));
            Assert.AreEqual(Vector2Int.down, GridStep.FromSwipe(new Vector2(0f, -1f)));
            Assert.AreEqual(new Vector2Int(1, -1), GridStep.FromSwipe(new Vector2(1f, -1f)));
        }

        [Test]
        public void FromSwipe_SectorBordersAre45DegreesWide()
        {
            Assert.AreEqual(Vector2Int.right, GridStep.FromSwipe(Swipe(20f)));
            Assert.AreEqual(new Vector2Int(1, 1), GridStep.FromSwipe(Swipe(25f)));
            Assert.AreEqual(new Vector2Int(1, 1), GridStep.FromSwipe(Swipe(65f)));
            Assert.AreEqual(Vector2Int.up, GridStep.FromSwipe(Swipe(70f)));
            Assert.AreEqual(Vector2Int.up, GridStep.FromSwipe(Swipe(110f)));
            Assert.AreEqual(new Vector2Int(-1, 1), GridStep.FromSwipe(Swipe(115f)));
            Assert.AreEqual(new Vector2Int(-1, 1), GridStep.FromSwipe(Swipe(155f)));
            Assert.AreEqual(Vector2Int.left, GridStep.FromSwipe(Swipe(160f)));
            Assert.AreEqual(Vector2Int.left, GridStep.FromSwipe(Swipe(200f)));
            Assert.AreEqual(new Vector2Int(-1, -1), GridStep.FromSwipe(Swipe(205f)));
            Assert.AreEqual(new Vector2Int(-1, -1), GridStep.FromSwipe(Swipe(245f)));
            Assert.AreEqual(Vector2Int.down, GridStep.FromSwipe(Swipe(250f)));
            Assert.AreEqual(Vector2Int.down, GridStep.FromSwipe(Swipe(290f)));
            Assert.AreEqual(new Vector2Int(1, -1), GridStep.FromSwipe(Swipe(295f)));
            Assert.AreEqual(new Vector2Int(1, -1), GridStep.FromSwipe(Swipe(335f)));
            Assert.AreEqual(Vector2Int.right, GridStep.FromSwipe(Swipe(340f)));
        }

        [Test]
        public void FromSwipe_NoMovement_IsNoDirection()
        {
            Assert.AreEqual(Vector2Int.zero, GridStep.FromSwipe(Vector2.zero));
        }

        [Test]
        public void TryFromKey_MapsWasdArrowsKeypadAndDigits()
        {
            AssertKey(Vector2Int.up, KeyCode.W, KeyCode.UpArrow, KeyCode.Keypad8, KeyCode.Alpha8);
            AssertKey(
                Vector2Int.down,
                KeyCode.S,
                KeyCode.DownArrow,
                KeyCode.Keypad2,
                KeyCode.Alpha2
            );
            AssertKey(
                Vector2Int.left,
                KeyCode.A,
                KeyCode.LeftArrow,
                KeyCode.Keypad4,
                KeyCode.Alpha4
            );
            AssertKey(
                Vector2Int.right,
                KeyCode.D,
                KeyCode.RightArrow,
                KeyCode.Keypad6,
                KeyCode.Alpha6
            );
            AssertKey(new Vector2Int(-1, 1), KeyCode.Keypad7, KeyCode.Alpha7);
            AssertKey(new Vector2Int(1, 1), KeyCode.Keypad9, KeyCode.Alpha9);
            AssertKey(new Vector2Int(-1, -1), KeyCode.Keypad1, KeyCode.Alpha1);
            AssertKey(new Vector2Int(1, -1), KeyCode.Keypad3, KeyCode.Alpha3);
        }

        [Test]
        public void TryFromKey_WaitKeys_WaitWithoutMoving()
        {
            foreach (var key in new[] { KeyCode.Keypad5, KeyCode.Alpha5, KeyCode.Space })
            {
                Assert.IsTrue(
                    GridStep.TryFromKey(key, out var offset, out var wait),
                    key.ToString()
                );
                Assert.IsTrue(wait, key.ToString());
                Assert.AreEqual(Vector2Int.zero, offset, key.ToString());
            }
        }

        [Test]
        public void TryFromKey_OtherKeys_AreIgnored()
        {
            foreach (var key in new[] { KeyCode.Q, KeyCode.Return, KeyCode.Alpha0 })
            {
                Assert.IsFalse(
                    GridStep.TryFromKey(key, out var offset, out var wait),
                    key.ToString()
                );
                Assert.IsFalse(wait, key.ToString());
                Assert.AreEqual(Vector2Int.zero, offset, key.ToString());
            }
        }

        [Test]
        public void IsAdjacent_Creatures_UsesCells()
        {
            var a = TestCreature.Spawn(null, Vector2Int.zero);
            var b = TestCreature.Spawn(null, new Vector2Int(1, 1));
            try
            {
                a.PlaceOn(Vector2Int.zero);
                b.PlaceOn(new Vector2Int(1, 1));
                Assert.IsTrue(GridStep.IsAdjacent(a, b));
                Assert.IsTrue(CreatureMoves.IsAdjacent(a, b));
                b.PlaceOn(new Vector2Int(2, 0));
                Assert.IsFalse(GridStep.IsAdjacent(a, b));
            }
            finally
            {
                Object.DestroyImmediate(a.gameObject);
                Object.DestroyImmediate(b.gameObject);
            }
        }

        private static Vector2 Swipe(float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        private static void AssertKey(Vector2Int expected, params KeyCode[] keys)
        {
            foreach (var key in keys)
            {
                Assert.IsTrue(
                    GridStep.TryFromKey(key, out var offset, out var wait),
                    key.ToString()
                );
                Assert.AreEqual(expected, offset, key.ToString());
                Assert.IsFalse(wait, key.ToString());
            }
        }
    }
}
