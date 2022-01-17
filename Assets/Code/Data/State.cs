using System;

namespace Code.Data
{
    [Serializable]
    public class State
    {
        public float CurrentHP;
        public float MaxHP;

        public void ResetHP() => CurrentHP = MaxHP;
    }
}