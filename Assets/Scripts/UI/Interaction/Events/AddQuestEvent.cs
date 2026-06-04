using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class AddQuestEvent : AbstractInteractionEvent
    {
        [Header("Quest Settings")]
        [SerializeField] private string title;
        [SerializeField] private QuestListEnum questTitle;

        private bool _end;
        
        protected override void InvokeEvent()
        {
            if (_end) return;
            _end = true;
            
            EventBus.Publish(new QuestAddMessage(questTitle, title));
        }
    }
}