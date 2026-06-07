using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace UI.Quest
{
    public enum QuestConditionType
    {
        Added,
        Removed,
        Cleared
    }

    [Serializable]
    public struct QuestConditionEvent
    {
        public QuestConditionType conditionType;
        public QuestListEnum[] requiredQuests;
        public UnityEvent events;

        [HideInInspector] public bool alreadyInvoked;
    }

    public class QuestCountManager : MonoBehaviour, IQuestCountManager
    {
        [SerializeField] private QuestConditionEvent[] questConditionEvents;

        public List<QuestListEnum> QuestAddEnumLs { get; set; } = new();
        public List<QuestListEnum> QuestRemoveEnumLs { get; set; } = new();
        public List<QuestListEnum> QuestClearEnumLs { get; set; } = new();

        private void Awake()
        {
            EventBus.Subscribe<QuestCountRemoveMessage>(OnQuestRemoved);
            EventBus.Subscribe<QuestCountAddMessage>(OnQuestAdded);
            EventBus.Subscribe<QuestCountClearMessage>(OnQuestCleared);
        }

        private void Start()
        {
            EventBus.Publish(new QuestCountListMessage(this));
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<QuestCountRemoveMessage>(OnQuestRemoved);
            EventBus.Unsubscribe<QuestCountAddMessage>(OnQuestAdded);
            EventBus.Unsubscribe<QuestCountClearMessage>(OnQuestCleared);
        }

        private void OnQuestAdded(QuestCountAddMessage message)
        {
            AddUnique(QuestAddEnumLs, message.quest);
            CheckEvents();
        }

        private void OnQuestRemoved(QuestCountRemoveMessage message)
        {
            AddUnique(QuestRemoveEnumLs, message.quest);
            CheckEvents();
        }

        private void OnQuestCleared(QuestCountClearMessage message)
        {
            Debug.Log($"실제로 카운트 매니저에 들어온 Clear: {message.quest}");

            AddUnique(QuestClearEnumLs, message.quest);
            CheckEvents();
        }

        private void AddUnique(List<QuestListEnum> list, QuestListEnum quest)
        {
            if (!list.Contains(quest))
                list.Add(quest);
        }

        private void CheckEvents()
        {
            for (int i = 0; i < questConditionEvents.Length; i++)
            {
                if (questConditionEvents[i].alreadyInvoked)
                    continue;

                if (!IsConditionComplete(questConditionEvents[i]))
                    continue;

                questConditionEvents[i].alreadyInvoked = true;
                questConditionEvents[i].events?.Invoke();
            }
        }

        private bool IsConditionComplete(QuestConditionEvent conditionEvent)
        {
            List<QuestListEnum> targetList = conditionEvent.conditionType switch
            {
                QuestConditionType.Added => QuestAddEnumLs,
                QuestConditionType.Removed => QuestRemoveEnumLs,
                QuestConditionType.Cleared => QuestClearEnumLs,
                _ => null
            };

            foreach (QuestListEnum quest in conditionEvent.requiredQuests)
            {
                if (!targetList.Contains(quest))
                {
                    return false;
                }
            }

            return true;
        }
    }
}