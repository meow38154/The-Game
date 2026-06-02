using GGMLib.AnimatorSystem;
using UnityEngine;

namespace FSM.SO
{
    [CreateAssetMenu(fileName = "base State", menuName = "FSM/StateSO", order = 0)]
    public class StateSo : ScriptableObject
    {
        [field: SerializeField] public int AssetIndex { get; private set; }
        [field: SerializeField] public string StateName { get; private set; }
        [field: SerializeField] public AnimParamSO AnimationParam { get; private set; }


        public string stateClassName;
    }
}