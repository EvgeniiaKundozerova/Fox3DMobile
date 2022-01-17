using System;

namespace Code.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public State HeroState;
        public HeroStats HeroStats;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new HeroStats();
        }
    }
}