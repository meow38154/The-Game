using FSM;
using FSM.SO;
using GGMLib.ModuleSystem;
using UnityEngine;
using Utility;

namespace Agents.Players
{
    [DefaultExecutionOrder(-1)]
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInputSo PlayerInputSo { get; private set; }   
        [SerializeField] private StateListSo playerStates;

        private IControlMovement _movement;
        public IControlMovement Movement => _movement;
        private StateMachine _stateMachine;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            _stateMachine = new StateMachine(this, playerStates.States);
            _movement = GetModule<IControlMovement>(); 
        }
        
        private void Start()
        {
            ChangeState(PlayerState.PlayerIdle, transitionDuration: 0);
            EventBus.Publish(new PlayerGameObjectMessage(gameObject));
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        
        
        public void ChangeState(PlayerState newState, float transitionDuration)
            => _stateMachine.ChangeState((int)newState, transitionDuration);
    }
}