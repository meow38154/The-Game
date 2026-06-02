using System;
using System.Collections.Generic;
using Agents;
using FSM.SO;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        private Dictionary<int, State> _stateDict;
        
        public StateMachine(Agent agent, StateSo[] stateList)
        {
            _stateDict = new Dictionary<int, State>();
            
            foreach (StateSo stateData in stateList)
            {
                Type type = Type.GetType(stateData.stateClassName);
                Debug.Assert(type != null, $"찾고자 하는 타입이 존재하지 않습니다. : {stateData.stateClassName}");

                int paramHash = stateData.AnimationParam != null ? stateData.AnimationParam.ParamHash : 0;
                State agentState = (State)Activator.CreateInstance(type, agent, paramHash);
                
                _stateDict.Add(stateData.AssetIndex, agentState);
            }
        }

        public void ChangeState(int newStateIndex, float transitionDuration = 0.1f)
        {
            CurrentState?.Exit();
            State newState = _stateDict.GetValueOrDefault(newStateIndex);
            Debug.Assert(newState != null, $"실행하려는 상태가 존재하지 않습니다. : {newStateIndex}");
            CurrentState = newState;
            CurrentState.Enter(transitionDuration);
        }
        
        public void UpdateMachine() => CurrentState?.Update();
    }
}