using System;

namespace SlimesRevenge
{
    public sealed class SystemRng : IRng
    {
        private readonly Random random;

        public SystemRng()
            : this(new Random()) { }

        /// <summary>The same seed gives the same sequence (repeatable simulations).</summary>
        public SystemRng(int seed)
            : this(new Random(seed)) { }

        private SystemRng(Random random)
        {
            this.random = random;
        }

        public int Pick(int count)
        {
            if (count <= 1)
            {
                return 0;
            }

            return random.Next(count);
        }

        public bool WanderRatherThanIdle()
        {
            return random.Next(2) == 0;
        }
    }
}
