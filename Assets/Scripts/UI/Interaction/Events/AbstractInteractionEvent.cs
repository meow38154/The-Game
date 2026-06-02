using System;
using UI.Interaction.Interface;
using UnityEngine;

namespace UI.Interaction.Events
{
    public abstract class AbstractInteractionEvent : MonoBehaviour, IInteractionEvent
    { 
        [SerializeField] private int requiredInteractionCount;
        public IInteractionTrigger InteractionTrigger { get; set; }
        
        public void Start()
        {
            InteractionTrigger.OnInteractionTrigger += HandleInteraction;
        }

        private void HandleInteraction()
        {
            if (requiredInteractionCount <= 1)
            {
                InvokeEvent();
            }
            else
            {
                requiredInteractionCount--;
            }
        }

        private void OnValidate()
        {
            if (requiredInteractionCount > 0) return;
            requiredInteractionCount = 1;
        }

        protected abstract void InvokeEvent();
    }
}