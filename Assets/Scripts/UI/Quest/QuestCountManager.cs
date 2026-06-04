using System;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace UI.Quest
{
    [Serializable]
    public struct QuestRemoveCountEvents
    {
        public int count;
        public UnityEvent events;
    }    
    
    [Serializable]
    public struct QuestAddCountEvents
    {
        public int count;
        public UnityEvent events;
    }    
    
    [Serializable]
    public struct QuestClearCountEvents
    {
        public int count;
        public UnityEvent events;
    }
    
    public class QuestCountManager : MonoBehaviour
    {
        [SerializeField] private QuestRemoveCountEvents[]  questCountRemoveEvent;
        [SerializeField] private QuestAddCountEvents[]  questCountAddEvent;
        [SerializeField] private QuestClearCountEvents[]  questCountClearEvent;
        private int _questAddCount;
        private int _questRemoveCount;
        private int _questClearCount;

        private void Awake()
        {
            EventBus.Subscribe<QuestCountRemoveMessage>(QuestRemoveCountSum);
            EventBus.Subscribe<QuestCountAddMessage>(QuestAddCountSum);
            EventBus.Subscribe<QuestCountClearMessage>(QuestClearCountSum);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<QuestCountRemoveMessage>(QuestRemoveCountSum);
            EventBus.Unsubscribe<QuestCountAddMessage>(QuestAddCountSum);
            EventBus.Unsubscribe<QuestCountClearMessage>(QuestClearCountSum);
        }

        private void QuestRemoveCountSum(QuestCountRemoveMessage removeMessage)
        {
            _questRemoveCount++;
            foreach (var events in questCountRemoveEvent)
            {
                if (events.count == _questRemoveCount)
                {
                    events.events.Invoke();
                }
            }
        }        
        
        private void QuestAddCountSum(QuestCountAddMessage addMessage)
        {
            _questAddCount++;
            foreach (var events in questCountAddEvent)
            {
                if (events.count == _questAddCount)
                {
                    events.events.Invoke();
                }
            }
        }        
        
        private void QuestClearCountSum(QuestCountClearMessage removeMessage)
        {
            _questClearCount++;
            foreach (var events in questCountClearEvent)
            {
                if (events.count == _questClearCount)
                {
                    events.events.Invoke();
                }
            }
        }
    }
}