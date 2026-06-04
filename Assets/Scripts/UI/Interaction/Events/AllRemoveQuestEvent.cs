using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
[DefaultExecutionOrder(-5)]
    public class AllRemoveQuestEvent : AbstractInteractionEvent
    {
        private bool _end;
        
        protected override void InvokeEvent()
        {
            if (_end) return;
            _end = true;
            
            EventBus.Publish(new QuestAllRemoveMessage());
        }
    }
}