using System.Collections.Generic;
using GGMLib.ObjectPool.Runtime;
using TMPro;
using UI.Announcement;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Quest
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private Transform questsContainerTrm;
        [SerializeField] private Sprite defaultQuestSprite;
        [SerializeField] private Sprite clearQuestSprite;
        
        private readonly Dictionary<QuestListEnum, (string questTitle, bool isClear)> _questDic = new();
        
        private void OnEnable()
        {
            EventBus.Subscribe<QuestAddMessage>(QuestAdd);
            EventBus.Subscribe<QuestRemoveMessage>(QuestRemove);
            EventBus.Subscribe<QuestClearMessage>(QuestClear);
            EventBus.Subscribe<QuestAllRemoveMessage>(QuestAllRemove);

            QuestViewUpdate();
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<QuestAddMessage>(QuestAdd);
            EventBus.Unsubscribe<QuestRemoveMessage>(QuestRemove);
            EventBus.Unsubscribe<QuestClearMessage>(QuestClear);
            EventBus.Unsubscribe<QuestAllRemoveMessage>(QuestAllRemove);
        }

        private void QuestAdd(QuestAddMessage message)
        {
            EventBus.Publish(new AnnouncementMessage("새 할 일 추가됨!"));
            _questDic.TryAdd(message.title, (message.questTitle, false));
            QuestViewUpdate();
        }

        private void QuestRemove(QuestRemoveMessage message)
        {
            EventBus.Publish(new QuestCountRemoveMessage(1));
            _questDic.Remove(message.title);
            QuestViewUpdate();
        }

        private void QuestClear(QuestClearMessage message)
        {
            EventBus.Publish(new AnnouncementMessage("할 일 완료됨!"));
            if (!_questDic.TryGetValue(message.title, out var quest))
                return;

            _questDic[message.title] = (quest.questTitle, true);
            QuestViewUpdate();
        }

        private void QuestAllRemove(QuestAllRemoveMessage questAllRemoveMessage)
        {
            _questDic.Clear();
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
                else
                    image.sprite = defaultQuestSprite;

                questIndex++;
            }
        }
    }
}