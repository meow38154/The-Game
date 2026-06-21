using UI.Notification;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class NotificationEvent : AbstractInteractionEvent
    {
        [SerializeField] private string content;


        protected override void InvokeEvent()
        {
            //EventBus.Publish(new NotificationAnimMessage(content));
        }
    }
}