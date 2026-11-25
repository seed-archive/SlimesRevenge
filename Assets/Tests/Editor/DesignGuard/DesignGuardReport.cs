using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SlimesRevenge.Tests.DesignGuard
{
    /// <summary>
    /// Runs every build against every encounter over a few seeds and writes the result as JSON
    /// (and as <c>data.js</c> for the static page in <c>tools/design-guard-site/</c>).
    /// </summary>
    public static class DesignGuardReport
    {
        public static readonly int[] Seeds = { 0, 1, 2 };
        public const int MaxEnemies = 3;

        /// <summary>The starting mix and one pure build of each liquid, full capacity.</summary>
        public static IReadOnlyList<Build> Builds()
        {
            var builds = new List<Build>
            {
                new Build(
                    "Starting mix",
                    () =>
                    {
                        var volume = new Volume();
                        Slime.FillStarting(volume);
                        return volume.Units.Select(unit => unit.Substance).ToArray();
                    }
                ),
            };
            foreach (var liquid in SubstanceCatalog.Samples)
            {
                var sample = liquid;
                builds.Add(
                    new Build(
                        "10 " + sample.GetType().Name,
                        () =>
                            Enumerable
                                .Range(0, Volume.Capacity)
                                .Select(_ => Volume.CloneSubstance(sample))
                                .ToArray()
                    )
                );
            }

            return builds;
        }

        /// <summary>One, two and three of every enemy in the live roster.</summary>
        public static IReadOnlyList<Encounter> Encounters()
        {
            var encounters = new List<Encounter>();
            foreach (var entry in CreatureCatalog.Opponents)
            {
                for (var count = 1; count <= MaxEnemies; count++)
                {
                    encounters.Add(new Encounter(entry.Kind, count));
                }
            }

            return encounters;
        }

        public static ReportData Run()
        {
            var builds = Builds();
            var encounters = Encounters();
            var data = new ReportData
            {
                generatedUtc = DateTime.UtcNow.ToString("u"),
                commit = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "local",
                policy = "face-tank",
                actionsPerRound = 1,
                seeds = Seeds.Length,
                builds = builds.Select(b => b.Name).ToArray(),
                encounters = encounters.Select(e => e.Name).ToArray(),
            };

            var cells = new List<CellData>();
            foreach (var build in builds)
            {
                foreach (var encounter in encounters)
                {
                    var outcomes = Seeds
                        .Select(seed => FaceTankFight.Run(build, encounter, seed))
                        .ToList();
                    cells.Add(Summarise(build, encounter, outcomes));
                }
            }

            data.cells = cells.ToArray();
            return data;
        }

        public static string Write(ReportData data)
        {
            var folder = Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", "Logs", "DesignGuard")
            );
            Directory.CreateDirectory(folder);
            var json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(Path.Combine(folder, "design-guard.json"), json);
            File.WriteAllText(
                Path.Combine(folder, "data.js"),
                "window.DESIGN_GUARD = " + json + ";\n"
            );
            var page = Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", "tools", "design-guard-site", "index.html")
            );
            File.Copy(page, Path.Combine(folder, "index.html"), overwrite: true);
            return folder;
        }

        private static CellData Summarise(
            Build build,
            Encounter encounter,
            List<FightOutcome> outcomes
        )
        {
            var wins = outcomes.Count(o => o.Result == FightResult.Won);
            var cause = outcomes
                .Where(o => o.Result != FightResult.Won)
                .GroupBy(o => o.Cause)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            return new CellData
            {
                build = build.Name,
                encounter = encounter.Name,
                enemy = encounter.Kind.ToString(),
                enemies = encounter.Count,
                winRate = (float)wins / outcomes.Count,
                avgRounds = (float)outcomes.Average(o => o.Rounds),
                avgUnitsLeft = (float)outcomes.Average(o => o.UnitsLeft),
                avgUnitsSpent = (float)outcomes.Average(o => o.UnitsSpent),
                cause = cause ?? string.Empty,
            };
        }

        [Serializable]
        public sealed class ReportData
        {
            public string generatedUtc;
            public string commit;
            public string policy;
            public int actionsPerRound;
            public int seeds;
            public string[] builds;
            public string[] encounters;
            public CellData[] cells;
        }

        [Serializable]
        public sealed class CellData
        {
            public string build;
            public string encounter;
            public string enemy;
            public int enemies;
            public float winRate;
            public float avgRounds;
            public float avgUnitsLeft;
            public float avgUnitsSpent;
            public string cause;
        }
    }
}
