using Agents;
using UnityEngine;

namespace FSM.States
{
    public class PlayerMoveState : AbstractPlayerState
    {
        public PlayerMoveState(Agent agent, int clipHash) : base(agent, clipHash)
        {
            
        }

        public override void Update()
        {
            base.Update();
            HandleMovementChange(Player.PlayerInputSo.MoveDir);
        }
        
        private void HandleMovementChange(Vector2 movementKey)
        {
//            Debug.Log(movementKey);
            ControlMovement.SetMovementDirection(movementKey);

            if (movementKey.sqrMagnitude < 0.1f)
            {
                Player.ChangeState(PlayerState.PlayerIdle, 0.1f);
            }
        }
    }
}