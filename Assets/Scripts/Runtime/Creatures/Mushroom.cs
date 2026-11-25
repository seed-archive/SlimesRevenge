namespace SlimesRevenge
{
    public sealed class Mushroom : Creature
    {
        public override CreatureKind Kind => CreatureKind.Mushroom;

        public static void FillStarting(Volume volume)
        {
            volume.Fill(new Water());
        }

        private void Awake() => SetUpBody();

        public override void SetUpBody()
        {
            SetMaxHitPoints(5);
            if (Volume.UnitCount == 0)
            {
                FillStarting(Volume);
            }
        }
    }
}
