using Agents;
using Agents.Players;
using UnityEngine;

namespace FSM.States
{
    public abstract class AbstractPlayerState : State
    {
        protected Player Player;
        protected IControlMovement ControlMovement;
        
        protected AbstractPlayerState(Agent agent, int clipHash) : base(agent, clipHash)
        {
            Player = agent as Player;
            Debug.Assert(Player != null, "플레이어 상태는 반드시 플레이어에게 붙어야 합니다.");
            ControlMovement = Player.GetModule<IControlMovement>();
            Debug.Assert(ControlMovement != null, "플레이어는 ControlMovement를 가져야합니다.");
        }
    }
}