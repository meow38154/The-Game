using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class ClearQuestEvent : AbstractInteractionEvent
    {
        [SerializeField] private QuestListEnum quest;
        
        protected override void InvokeEvent()
        {
            EventBus.Publish(new QuestClearMessage(quest));
        }
    }
}