using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimesRevenge.Tests.DesignGuard
{
    public class DesignGuardTests
    {
        [TearDown]
        public void TearDown()
        {
            // A defeat shows the game-over screen, which creates one EventSystem at the root.
            foreach (var system in Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
            {
                Object.DestroyImmediate(system.gameObject);
            }
        }

        [Test]
        public void FaceTank_SameSeed_GivesTheSameFight()
        {
            var build = DesignGuardReport.Builds()[0];
            var encounter = DesignGuardReport.Encounters().First(e => e.Count == 2);

            var first = FaceTankFight.Run(build, encounter, seed: 7);
            var second = FaceTankFight.Run(build, encounter, seed: 7);

            Assert.AreEqual(first.Result, second.Result);
            Assert.AreEqual(first.Rounds, second.Rounds);
            Assert.AreEqual(first.UnitsLeft, second.UnitsLeft);
            Assert.AreEqual(first.UnitsSpent, second.UnitsSpent);
            Assert.AreEqual(first.EnemiesKilled, second.EnemiesKilled);
            Assert.AreEqual(first.Cause, second.Cause);
        }

        [Test]
        public void FaceTank_EndsWithAResult()
        {
            var build = DesignGuardReport.Builds()[0];
            var encounter = DesignGuardReport.Encounters()[0];

            var outcome = FaceTankFight.Run(build, encounter, seed: 0);

            Assert.Greater(outcome.Rounds, 0, "The fight must play at least one round.");
            if (outcome.Result == FightResult.Won)
            {
                Assert.AreEqual(encounter.Count, outcome.EnemiesKilled);
            }
            else
            {
                Assert.IsNotEmpty(outcome.Cause, "A fight that was not won says why.");
            }
        }

        [Test]
        public void Report_CoversEveryBuildAndEncounter_AndIsWritten()
        {
            var data = DesignGuardReport.Run();
            var folder = DesignGuardReport.Write(data);

            var builds = DesignGuardReport.Builds().Count;
            var encounters = DesignGuardReport.Encounters().Count;
            Assert.AreEqual(builds * encounters, data.cells.Length);
            foreach (var build in data.builds)
            {
                foreach (var encounter in data.encounters)
                {
                    Assert.IsTrue(
                        data.cells.Any(c => c.build == build && c.encounter == encounter),
                        build + " × " + encounter
                    );
                }
            }

            Assert.IsTrue(File.Exists(Path.Combine(folder, "design-guard.json")));
            Assert.IsTrue(File.Exists(Path.Combine(folder, "data.js")));
            Assert.IsTrue(File.Exists(Path.Combine(folder, "index.html")));
            Debug.Log("[DesignGuard] report: " + folder);
        }
    }
}
