using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Utility;

namespace FSM.SO
{
    [CreateAssetMenu(fileName = "state List", menuName = "FSM/StateListSo", order = 1)]
    public class StateListSo : ScriptableObject
    {
        [SerializeField] private string stateListEnumName = "StateType";
        [field: SerializeField] public StateSo[] States { get; private set; }

        [ContextMenu("Enum export")]
        public void EnumExport()
        {
            string[] sv = new string[States.Length];
            for (int i = 0; i < States.Length; i++)
            {
                sv[i] = States[i].StateName;
            }
            
            Utility.EnumSpawner.EnumSpawn(stateListEnumName, sv);
        }
    }
}