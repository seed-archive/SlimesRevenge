namespace SlimesRevenge
{
    public sealed class Mandragora : Creature
    {
        public override CreatureKind Kind => CreatureKind.Mandragora;

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
