using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Agents.Players
{
    public readonly struct PlayerNavRotateMessage
    {
        public readonly int channelNumber;
        public readonly Transform target;

        public PlayerNavRotateMessage(Transform target, int channelNumber)
        {
            this.channelNumber = channelNumber;
            this.target = target;
        }
    }

    public class PlayerNavRotate : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private readonly Dictionary<int, ArrowData> _arrows = new();

        private sealed class ArrowData
        {
            public Transform target;
            public GameObject visual;
        }

        private void Awake()
        {
            EventBus.Subscribe<PlayerNavRotateMessage>(TargetRegistration);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerNavRotateMessage>(TargetRegistration);
        }

        private void TargetRegistration(PlayerNavRotateMessage message)
        {
            ArrowData arrowData = GetOrCreateArrow(message.channelNumber);

            arrowData.target = message.target;

            arrowData.visual.SetActive(message.target != null);
        }

        private ArrowData GetOrCreateArrow(int channelNumber)
        {
            if (_arrows.TryGetValue(channelNumber, out ArrowData arrowData))
                return arrowData;

            if (prefab == null)
            {
                Debug.LogError($"{nameof(PlayerNavRotate)}: prefab이 비어있음");
                return null;
            }

            GameObject visual = Instantiate(prefab, transform);
            visual.name = $"NavArrow_Channel_{channelNumber}";
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
            visual.SetActive(false);

            arrowData = new ArrowData
            {
                target = null,
                visual = visual
            };

            _arrows.Add(channelNumber, arrowData);
            return arrowData;
        }

        private void Update()
        {
            foreach (ArrowData arrowData in _arrows.Values)
            {
                if (arrowData == null || arrowData.visual == null)
                    continue;

                if (arrowData.target == null)
                {
                    arrowData.visual.SetActive(false);
                    continue;
                }

                Vector3 direction = arrowData.target.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude <= 0.0001f)
                    continue;

                if (!arrowData.visual.activeSelf)
                    arrowData.visual.SetActive(true);

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                arrowData.visual.transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            }
        }
    }
}