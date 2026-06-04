using Agents;
using UnityEngine;

namespace FSM.States
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public PlayerIdleState(Agent agent, int clipHash) : base(agent, clipHash)
        {
            
        }

        public override void Enter(float transitionDuration, int layerIndex = 0)
        {
            base.Enter(transitionDuration, layerIndex);
            ControlMovement.SetMovementDirection(Vector2.zero);
        }

        public override void Update()
        {
            base.Update();
            HandleMovementChange(Player.Movement.CanManualMovement ? Player.PlayerInputSo.MoveDir : Vector2.zero);
        }
        
        private void HandleMovementChange(Vector2 movementKey)
        {
            if(movementKey.sqrMagnitude > 0.1f)
                Player.ChangeState(PlayerState.PlayerMove, 0.1f); 
        }
    }
}