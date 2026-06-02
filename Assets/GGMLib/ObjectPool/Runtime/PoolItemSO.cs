using UnityEngine;

namespace GGMLib.ObjectPool.Runtime
{
    [CreateAssetMenu(fileName = "Pool Item", menuName = "Lib/ObjectPool/PoolItem", order = 0)]
    public class PoolItemSO : ScriptableObject
    {
        public string poolName;
        public GameObject prefab;
        public int initCount;
    }
}