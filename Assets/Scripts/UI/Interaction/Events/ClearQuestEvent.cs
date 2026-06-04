using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class ClearQuestEvent : AbstractInteractionEvent
    {
        [Header("Quest Settings")]
        [SerializeField] private QuestListEnum quest;
        [SerializeField] private bool counting;

        private bool _end;

        protected override void InvokeEvent()
        {
            if (_end) return;
            _end = true;
            
            EventBus.Publish(new QuestClearMessage(quest));
            
            if (!counting) return;
            EventBus.Publish(new QuestCountClearMessage());
        }
    }
}