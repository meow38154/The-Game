using System;
using System.Collections.Generic;
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
        public PoolItemSO displayPool;
    }

    public class ClearDisplayEvent : AbstractInteractionEvent
    {
        [Header("Clear Display")]
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private bool startDisplay;
        [SerializeField] private PoolItemSO displayPool;
        [SerializeField] protected ClearDisplay[] targets;

        private bool _end;

        private readonly Dictionary<(Transform target, PoolItemSO poolItem), ClearDisplayPool> _spawnedDisplays = new();

        protected override void Start()
        {
            base.Start();

            if (!startDisplay) return;

            ClearDisplayActiveOn(InteractionTrigger.Owner, displayPool);
        }

        protected override void InvokeEvent()
        {
            if (_end) return;

            _end = true;

            // InvokeEvent 실행 시 Owner 밑의 ClearDisplayPool 전부 제거
            ClearAllOwnerDisplays();

            foreach (ClearDisplay item in targets)
            {
                if (item.active)
                {
                    ClearDisplayActiveOn(item.target, item.displayPool);
                }
                else
                {
                    ClearDisplayActiveOff(item.target, item.displayPool);
                }
            }
        }

        private void ClearAllOwnerDisplays()
        {
            if (poolManager == null) return;
            if (InteractionTrigger.Owner == null) return;

            Transform owner = InteractionTrigger.Owner.transform;

            ClearDisplayPool[] displays = owner.GetComponentsInChildren<ClearDisplayPool>(true);

            foreach (ClearDisplayPool display in displays)
            {
                if (display == null) continue;

                RemoveDisplayFromDictionary(display);
                poolManager.Push(display, true);
            }
        }

        private void RemoveDisplayFromDictionary(ClearDisplayPool display)
        {
            var removeKeys = _spawnedDisplays
                .Where(pair => pair.Value == display)
                .Select(pair => pair.Key)
                .ToArray();

            foreach (var key in removeKeys)
            {
                _spawnedDisplays.Remove(key);
            }
        }

        private Transform GetTarget(Transform trm)
        {
            if (trm != null) return trm;

            return InteractionTrigger.Owner != null
                ? InteractionTrigger.Owner.transform
                : null;
        }

        protected void ClearDisplayActiveOn(Transform trm, PoolItemSO poolItem)
        {
            Transform target = GetTarget(trm);

            if (target == null) return;
            if (poolItem == null) return;
            if (poolManager == null) return;

            var key = (target, poolItem);

            if (_spawnedDisplays.ContainsKey(key)) return;

            ClearDisplayPool display = poolManager.Pop<ClearDisplayPool>(poolItem);

            if (display == null)
            {
                Debug.LogError($"[ClearDisplayEvent] Pop 실패: {poolItem.name}", this);
                return;
            }

            display.transform.SetParent(target, false);
            display.transform.localPosition = Vector3.up * 1.5f;
            display.transform.localRotation = Quaternion.identity;
            display.transform.localScale = Vector3.one;

            _spawnedDisplays.Add(key, display);
        }

        protected void ClearDisplayActiveOff(Transform trm, PoolItemSO poolItem)
        {
            Transform target = GetTarget(trm);

            if (target == null) return;
            if (poolItem == null) return;
            if (poolManager == null) return;

            var key = (target, poolItem);

            if (!_spawnedDisplays.TryGetValue(key, out ClearDisplayPool display)) return;

            _spawnedDisplays.Remove(key);

            if (display == null) return;

            poolManager.Push(display, true);
        }
    }
}