using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Quest
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private Transform questsContainerTrm;
        [SerializeField] private Sprite clearQuestSprite;

        private readonly Dictionary<QuestListEnum, (string questTitle, bool isClear)> _questDic = new();
        
        private void OnEnable()
        {
            EventBus.Subscribe<QuestAddMessage>(QuestAdd);
            EventBus.Subscribe<QuestRemoveMessage>(QuestRemove);
            EventBus.Subscribe<QuestClearMessage>(QuestClear);

            QuestViewUpdate();
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<QuestAddMessage>(QuestAdd);
            EventBus.Unsubscribe<QuestRemoveMessage>(QuestRemove);
            EventBus.Unsubscribe<QuestClearMessage>(QuestClear);
        }

        private void QuestAdd(QuestAddMessage message)
        {
            _questDic.TryAdd(message.title, (message.questTitle, false));
            QuestViewUpdate();
        }

        private void QuestRemove(QuestRemoveMessage message)
        {
            _questDic.Remove(message.title);
            QuestViewUpdate();
        }

        private void QuestClear(QuestClearMessage message)
        {
            if (!_questDic.TryGetValue(message.title, out var quest))
                return;

            _questDic[message.title] = (quest.questTitle, true);
            QuestViewUpdate();
        }

        private void QuestViewUpdate()
        {
            int questIndex = 0;

            for (int i = 0; i < questsContainerTrm.childCount; i++)
            {
                questsContainerTrm.GetChild(i).gameObject.SetActive(false);
            }

            foreach (var quest in _questDic)
            {
                if (questIndex >= questsContainerTrm.childCount)
                    break;

                Transform child = questsContainerTrm.GetChild(questIndex);
                child.gameObject.SetActive(true);

                TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                    text.text = quest.Value.questTitle;

                Image image = child.GetComponentInChildren<Image>();
                if (image != null && quest.Value.isClear)
                    image.sprite = clearQuestSprite;

                questIndex++;
            }
        }
    }
}