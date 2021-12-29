using UnityEngine;

namespace Code.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string MeleeAttackButton = "Fire1";
        private const string RangeAttackButton = "Fire3";

        public abstract Vector2 Axis { get; }

        public bool IsMeleeAttackButtonUp() => 
            SimpleInput.GetButtonUp(MeleeAttackButton);

        public bool IsRangeAttackButtonUp() => 
            SimpleInput.GetButtonUp(RangeAttackButton);

        protected static Vector2 SimpleInputAxis() => 
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}