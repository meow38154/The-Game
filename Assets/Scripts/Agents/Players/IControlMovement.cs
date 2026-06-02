using UnityEngine;

namespace Agents.Players
{
    public interface IControlMovement
    {
        bool CanManualMovement { get; set; }
        void SetAutoVelocity(Vector3 velocity);
        void SetMovementDirection(Vector2 movementInput);
        void RotateTo(Vector3 direction);
    }
}