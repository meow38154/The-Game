using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class AddQuestEvent : AbstractInteractionEvent
    {
        [SerializeField] private string title;
        [SerializeField] private QuestListEnum questTitle;
        protected override void InvokeEvent()
        {
            EventBus.Publish(new QuestAddMessage(questTitle, title));
        }
    }
}