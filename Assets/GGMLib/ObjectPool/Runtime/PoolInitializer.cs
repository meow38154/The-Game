using System;
using UnityEngine;

namespace GGMLib.ObjectPool.Runtime
{
    public class PoolInitializer : MonoBehaviour
    {
        [field: SerializeField] public PoolManagerSO PoolManagerAsset { get; private set; }

        private void Awake()
        {
            PoolInitializer[] poolInitializer = FindObjectsByType<PoolInitializer>(FindObjectsSortMode.None);
            
            if (poolInitializer.Length > 1) return;
            
            PoolManagerAsset.InitializePool(transform);
            
            DontDestroyOnLoad(gameObject);
        }
    }
}