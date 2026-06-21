using Agents.Players;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class NavTargetRegistrationEvent : AbstractInteractionEvent
    {
        [Header("Nav Target Registration")]
        [SerializeField] private Transform target;
        [SerializeField] private int navChannel;
        
        protected override void InvokeEvent()
        {
            Debug.Log($"[NavTargetRegistrationEvent] channel: {navChannel}, target: {target}");

            EventBus.Publish(new PlayerNavRotateMessage(target, navChannel));
        }
    }
}