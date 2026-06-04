using System;
using UI.Interaction.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Interaction.Events
{
    public class InteractionUnityEvent : AbstractInteractionEvent
    {        
        [Header("Event Settings")]
        [SerializeField] private UnityEvent onInteractionTrigger;

        protected override void InvokeEvent()
        {
            onInteractionTrigger?.Invoke();
        }
    }
}