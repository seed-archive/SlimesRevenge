using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace SlimesRevenge.Tests
{
    public class SlimeTests : SpawnFixture
    {
        private Sprite water;
        private Sprite oil;
        private Sprite poison;
        private Sprite acid;
        private Sprite blood;
        private Sprite lava;
        private Sprite mercury;

        [SetUp]
        public void SetUp()
        {
            SlimeSprites.ResetCacheForTests();
            water = Sheet("Water");
            oil = Sheet("Oil");
            poison = Sheet("Poison");
            acid = Sheet("Acid");
            blood = Sheet("Blood");
            lava = Sheet("Lava");
            mercury = Sheet("Mercury");
        }

        [TearDown]
        public void TearDown()
        {
            SlimeSprites.ResetCacheForTests();
        }

        [Test]
        public void Appearance_ShowsTheSheetOfTheDominantSubstance()
        {
            var cases = new (Substance substance, Sprite sheet)[]
            {
                (new Water(), water),
                (new Oil(), oil),
                (new Poison(), poison),
                (new Acid(), acid),
                (new Blood(), blood),
                (new Lava(), lava),
                (new Mercury(), mercury),
            };
            var slime = SlimeWithRenderer();
            slime.BindSheets(water, oil, poison, acid, blood, lava, mercury);

            foreach (var (substance, sheet) in cases)
            {
                slime.Volume.Fill(substance, substance.Clone(), substance.Clone());
                slime.ApplyAppearance();
                Assert.AreSame(sheet.texture, Shown(slime).texture, substance.GetType().Name);
            }
        }

        [Test]
        public void Appearance_MercuryWithoutItsSheet_FallsBackToWater()
        {
            var slime = SlimeWithRenderer();
            slime.Volume.Fill(new Mercury(), new Mercury());
            slime.BindSheets(water, oil, poison, acid, blood, lava, null);

            Assert.AreSame(water.texture, Shown(slime).texture);
        }

        [Test]
        public void Appearance_PlaysTheWalkRowOfTheFacing()
        {
            var slime = SlimeWithRenderer();
            slime.Volume.Fill(new Water(), new Water());
            slime.BindSheets(water, oil, poison, acid, blood, lava, mercury);

            slime.Face(slime.Cell + Vector2Int.left);
            slime.ApplyAppearance();
            var west = SlimeSprites.Walk(water, WalkFacing.West)[0].rect;
            Assert.AreEqual(west, Shown(slime).rect);

            slime.Face(slime.Cell + Vector2Int.right);
            slime.ApplyAppearance();
            var east = SlimeSprites.Walk(water, WalkFacing.East)[0].rect;
            Assert.AreNotEqual(west, east);
            Assert.AreEqual(east, Shown(slime).rect);
        }

        [Test]
        public void Appearance_SameLook_DoesNotRestartTheWalk()
        {
            var slime = SlimeWithRenderer();
            slime.Volume.Fill(new Water(), new Water());
            slime.BindSheets(water, oil, poison, acid, blood, lava, mercury);
            var animator = slime.GetComponent<CreatureSpriteAnimator>();
            Assert.IsNotNull(animator);

            animator.Tick(CreatureSpriteAnimator.DefaultFrameSeconds);
            var secondFrame = Shown(slime).rect;
            Assert.AreNotEqual(SlimeSprites.Walk(water, WalkFacing.East)[0].rect, secondFrame);

            // LateUpdate calls ApplyAppearance every frame; an unchanged look keeps the frame.
            slime.ApplyAppearance();
            Assert.AreEqual(secondFrame, Shown(slime).rect);
        }

        [Test]
        public void Appearance_WithoutRenderer_ShowsNothing()
        {
            var slime = SpawnSlime(new Vector2Int(2, 2));
            Assert.DoesNotThrow(() =>
                slime.BindSheets(water, oil, poison, acid, blood, lava, mercury)
            );
            Assert.IsNull(slime.GetComponent<CreatureSpriteAnimator>());
        }

        private Slime SlimeWithRenderer()
        {
            var slime = SpawnSlime(new Vector2Int(2, 2));
            slime.gameObject.AddComponent<SpriteRenderer>();
            return slime;
        }

        private static Sprite Shown(Slime slime)
        {
            var sprite = slime.GetComponent<SpriteRenderer>().sprite;
            Assert.IsNotNull(sprite, "The slime shows no sprite.");
            return sprite;
        }

        /// <summary>The sheet's top-left cell (frame 0), found by position, not by sprite name.</summary>
        private static Sprite Sheet(string name)
        {
            Sprite first = null;
            foreach (
                var asset in AssetDatabase.LoadAllAssetsAtPath($"Assets/Art/Slimes/{name}.png")
            )
            {
                if (
                    asset is Sprite sprite
                    && (
                        first == null
                        || sprite.rect.y > first.rect.y
                        || (sprite.rect.y == first.rect.y && sprite.rect.x < first.rect.x)
                    )
                )
                {
                    first = sprite;
                }
            }

            Assert.IsNotNull(first, $"No sprites in Assets/Art/Slimes/{name}.png");
            return first;
        }
    }
}
