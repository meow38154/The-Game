using Agents;
using UnityEngine;

namespace FSM
{
    public abstract class State
    {
        protected readonly Agent _agent;
        protected readonly int _stateClipHash;
        protected readonly IRenderer _renderer;

        public State(Agent agent, int clipHash)
        {
            _agent = agent;
            _stateClipHash = clipHash;
            _renderer = agent.GetModule<IRenderer>();
        }

        public virtual void Enter(float transitionDuration, int layerIndex = 0)
        {
            _renderer.PlayClip(_stateClipHash, 0, transitionDuration, layerIndex);
        }
        
        public virtual void Update(){}
        public virtual void Exit(){}
    }
}