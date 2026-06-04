using System;
using System.Linq;
using GGMLib.ObjectPool.Runtime;
using Pool;
using UnityEngine;

namespace UI.Interaction.Events
{
    [Serializable]
    public struct ClearDisplay
    {
        public Transform target;
        public bool active;
    }
    
    public class ClearDisplayEvent : AbstractInteractionEvent
    {
        [Header("Clear Display")]
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolItemSO clearDisplayPool;
        [SerializeField] private PoolItemSO qaDisplayPool;
        [SerializeField] private bool startDisplay;
        [SerializeField] protected ClearDisplay[] targets;

        private bool _end;

        protected override void Start()
        {
            base.Start();
            if (!startDisplay) return;

            if (targets.Any(item => item.active))
            {
                ClearDisplayActiveOn(InteractionTrigger.Owner.transform, qaDisplayPool);
            }
        }

        protected override void InvokeEvent()
        {
            if (_end) return;
            
            _end = true;
            
            foreach (ClearDisplay item in targets)
            {
                if (item.active)
                {
                    ClearDisplayActiveOn(item.target);
                }
                else
                {
                    ClearDisplayActiveOff(item.target);
                }
            }
        }
        

        protected void ClearDisplayActiveOn(Transform trm, PoolItemSO item = null)
        {
            if (trm == null) return;
            if (item == null) item = clearDisplayPool;
            Transform target = trm;
            
            ClearDisplayPool cdp = poolManager.Pop<ClearDisplayPool>(item);
            cdp.transform.SetParent(target);
            cdp.transform.localPosition = Vector3.up * 1.5f;
        }        
        
        protected void ClearDisplayActiveOff(Transform trm)
        {            
            Transform target = trm != null ? trm : InteractionTrigger.Owner;
            
            ClearDisplayPool a = target.GetComponentInChildren<ClearDisplayPool>();
            
            if (a == null) return;
            
            poolManager.Push(a, true);
        }
    }
}