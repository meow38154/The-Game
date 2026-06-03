using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "Enum State List", menuName = "Etc/Enum State List", order = 0)]
public class EnumSpawnSystem : ScriptableObject
{
        [SerializeField] private string fileName;
        [SerializeField] private string[] stateListEnumName;

        [ContextMenu("Load")]
        public void Load()
        {
                EnumSpawner.EnumSpawn(fileName, stateListEnumName);
        }
}