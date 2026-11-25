namespace SlimesRevenge
{
    /// <summary>
    /// Softcore defeat recovery, bound to <see cref="TurnManager.TrySoftcoreContinue"/> by
    /// <see cref="WorldView"/>. The slime revives on the cell where it died: spawn pool
    /// placement is not implemented yet.
    /// </summary>
    public static class SoftcoreRevive
    {
        public const int SpawnPoolWater = 5;

        /// <summary>
        /// Abort any in-progress meal, clear statuses, replace the volume with
        /// <see cref="SpawnPoolWater"/> units of water, and reactivate the player.
        /// Returns true when revive succeeded.
        /// </summary>
        public static bool TryContinue(TurnManager turns, Creature player, World world)
        {
            if (player == null)
            {
                return false;
            }

            // Abort Digesting meal without transfer (reward was never granted).
            player.FindStatus<Digesting>()?.Abort();
            player.ClearAllStatuses();

            // TODO(SpawnPool): place the slime on a cell adjacent to the spawning pool.
            // Binding sketch once SpawnPool exists on the level:
            //   var poolCell = world.SpawnPoolCell;
            //   var near = PickWalkableNeighbor(world, poolCell);
            //   player.PlaceOn(near);
            //   turns.Session.SetControlled(near); // when SpawnPool ships
            _ = turns;
            _ = world;

            player.Volume.Clear();
            for (var i = 0; i < SpawnPoolWater; i++)
            {
                player.Volume.Add(new Water());
            }

            if (!player.gameObject.activeSelf)
            {
                player.gameObject.SetActive(true);
            }

            if (player is Slime)
            {
                player.SetMaxHitPoints(0);
            }

            player.RefreshVolumeStatuses();
            return player.IsAlive;
        }
    }
}
