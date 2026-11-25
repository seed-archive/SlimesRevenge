using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlimesRevenge.Tests.DesignGuard
{
    /// <summary>A slime build: a name and the liquids it starts with.</summary>
    public sealed class Build
    {
        public Build(string name, Func<Substance[]> units)
        {
            Name = name;
            Units = units;
        }

        public string Name { get; }

        /// <summary>Fresh substance instances for one fight.</summary>
        public Func<Substance[]> Units { get; }
    }

    /// <summary>The enemies of one fight: one kind, a count.</summary>
    public readonly struct Encounter
    {
        public Encounter(CreatureKind kind, int count)
        {
            Kind = kind;
            Count = count;
        }

        public CreatureKind Kind { get; }
        public int Count { get; }
        public string Name => Count + "× " + Kind;
    }

    public enum FightResult
    {
        Won,
        Lost,
        Stalemate,
    }

    public readonly struct FightOutcome
    {
        public FightOutcome(
            FightResult result,
            int rounds,
            int unitsLeft,
            int unitsSpent,
            int enemiesKilled,
            string cause
        )
        {
            Result = result;
            Rounds = rounds;
            UnitsLeft = unitsLeft;
            UnitsSpent = unitsSpent;
            EnemiesKilled = enemiesKilled;
            Cause = cause;
        }

        public FightResult Result { get; }
        public int Rounds { get; }
        public int UnitsLeft { get; }
        public int UnitsSpent { get; }
        public int EnemiesKilled { get; }

        /// <summary>Why the slime lost, or why the fight did not end; empty on a win.</summary>
        public string Cause { get; }
    }

    /// <summary>
    /// The face-tank policy: walk to the nearest enemy and strike it with the liquid the slime
    /// has most of, until everyone is dead or the slime is. Plays by the game's own rules
    /// (TurnManager, Combat, statuses, enemy AI); moves only through <see cref="GridPath"/>.
    /// </summary>
    public static class FaceTankFight
    {
        public const int MaxRounds = 200;
        public static readonly Vector2Int SlimeCell = new Vector2Int(1, 5);

        public static FightOutcome Run(Build build, Encounter encounter, int seed)
        {
            var spawned = new List<GameObject>();
            try
            {
                return Play(build, encounter, seed, spawned);
            }
            finally
            {
                foreach (var go in spawned)
                {
                    if (go != null)
                    {
                        UnityEngine.Object.DestroyImmediate(go);
                    }
                }

                ActionLog.Clear();
            }
        }

        private static FightOutcome Play(
            Build build,
            Encounter encounter,
            int seed,
            List<GameObject> spawned
        )
        {
            var world = World.CreateGrass();
            var slime = SpawnSlime(build, spawned);
            var enemies = SpawnEnemies(encounter, spawned);
            var turns = Track(spawned, new GameObject("Turns")).AddComponent<TurnManager>();
            turns.Rng = new SystemRng(seed);
            turns.Bind(world, slime, enemies.ToArray());

            var unitsSpent = 0;
            for (var round = 0; round < MaxRounds; round++)
            {
                if (turns.IsGameOver || !slime.IsAlive)
                {
                    return Outcome(FightResult.Lost, round, slime, enemies, unitsSpent);
                }

                var target = Nearest(slime, enemies);
                if (target == null)
                {
                    return Outcome(FightResult.Won, round, slime, enemies, unitsSpent);
                }

                if (GridStep.IsAdjacent(slime, target))
                {
                    var liquid = MostOf(slime.Volume);
                    if (slime.Volume.CanSpend && turns.TryAttack(target.Cell, liquid))
                    {
                        unitsSpent++;
                        continue;
                    }

                    turns.TryWait();
                    continue;
                }

                if (!StepToward(turns, world, slime, target))
                {
                    turns.TryWait();
                }
            }

            return Outcome(FightResult.Stalemate, MaxRounds, slime, enemies, unitsSpent);
        }

        private static Slime SpawnSlime(Build build, List<GameObject> spawned)
        {
            var slime = Track(spawned, new GameObject("Slime")).AddComponent<Slime>();
            slime.PlaceOn(SlimeCell);
            slime.Volume.Clear();
            foreach (var unit in build.Units())
            {
                slime.Volume.Add(unit);
            }

            slime.SetMaxHitPoints(0);
            slime.RefreshVolumeStatuses();
            return slime;
        }

        /// <summary>Enemies stand in a column five cells east of the slime, inside its vision.</summary>
        private static List<Creature> SpawnEnemies(Encounter encounter, List<GameObject> spawned)
        {
            var type = CreatureCatalog.TypeOf(encounter.Kind);
            if (type == null)
            {
                throw new InvalidOperationException("No creature class for " + encounter.Kind);
            }

            var enemies = new List<Creature>(encounter.Count);
            for (var i = 0; i < encounter.Count; i++)
            {
                var go = Track(spawned, new GameObject(encounter.Kind.ToString()));
                var enemy = (Creature)go.AddComponent(type);
                enemy.PlaceOn(SlimeCell + new Vector2Int(5, i - encounter.Count / 2));
                enemy.SetUpBody();
                enemies.Add(enemy);
            }

            return enemies;
        }

        private static Creature Nearest(Slime slime, List<Creature> enemies)
        {
            Creature best = null;
            var bestDistance = int.MaxValue;
            foreach (var enemy in enemies)
            {
                if (enemy == null || !enemy.IsAlive)
                {
                    continue;
                }

                var distance = GridStep.Chebyshev(slime.Cell, enemy.Cell);
                if (distance < bestDistance)
                {
                    best = enemy;
                    bestDistance = distance;
                }
            }

            return best;
        }

        /// <summary>The liquid the slime has most of; ties go to the one nearer the tip.</summary>
        private static Substance MostOf(Volume volume)
        {
            var counts = new Dictionary<Type, int>();
            Substance best = null;
            var bestCount = 0;
            for (var i = volume.Units.Count - 1; i >= 0; i--)
            {
                var substance = volume.Units[i].Substance;
                var type = substance.GetType();
                counts.TryGetValue(type, out var count);
                counts[type] = ++count;
                if (count > bestCount)
                {
                    best = substance;
                    bestCount = count;
                }
            }

            return best;
        }

        private static bool StepToward(TurnManager turns, World world, Slime slime, Creature target)
        {
            var goals = new List<Vector2Int>();
            foreach (var offset in GridStep.AdjacentOffsets)
            {
                var cell = target.Cell + offset;
                if (world.Contains(cell) && !turns.Session.IsOccupied(cell))
                {
                    goals.Add(cell);
                }
            }

            return GridPath.TryWalk(
                    world,
                    slime.Cell,
                    goals,
                    cell => cell != slime.Cell && turns.Session.IsOccupied(cell),
                    1,
                    out var destination,
                    slime
                ) && turns.TryMoveTo(destination);
        }

        private static FightOutcome Outcome(
            FightResult result,
            int rounds,
            Slime slime,
            List<Creature> enemies,
            int unitsSpent
        )
        {
            var killed = 0;
            foreach (var enemy in enemies)
            {
                if (enemy == null || !enemy.IsAlive)
                {
                    killed++;
                }
            }

            return new FightOutcome(
                result,
                rounds,
                slime.Volume.UnitCount,
                unitsSpent,
                killed,
                result == FightResult.Won ? string.Empty : Cause(result, slime, enemies)
            );
        }

        private static string Cause(FightResult result, Slime slime, List<Creature> enemies)
        {
            if (result == FightResult.Stalemate)
            {
                return "no result in " + MaxRounds + " rounds";
            }

            foreach (var status in slime.Statuses)
            {
                if (status is IDamageOverTime)
                {
                    return "status: " + status.GetType().Name;
                }
            }

            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.IsAlive && GridStep.IsAdjacent(slime, enemy))
                {
                    return "melee: " + enemy.Kind;
                }
            }

            return "unknown";
        }

        private static GameObject Track(List<GameObject> spawned, GameObject go)
        {
            spawned.Add(go);
            return go;
        }
    }
}
