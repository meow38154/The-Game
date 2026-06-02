using UnityEngine;

namespace GGMLib.ObjectPool.Runtime
{
    public interface IPoolable
    {
        public PoolItemSO PoolItem { get; }
        public GameObject GameObject { get; }

        void ResetItem();
    }
}