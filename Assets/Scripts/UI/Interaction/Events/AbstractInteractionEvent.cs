using System;
using UI.Interaction.Interface;
using UnityEngine;

namespace UI.Interaction.Events
{
    public abstract class AbstractInteractionEvent : MonoBehaviour, IInteractionEvent
    { 
        [Header("Start Setting")]
        [SerializeField] private int requiredInteractionCount;
        [Header("Quest Setting")]
        [SerializeField] private QuestListEnum questListEnum;
        [SerializeField] private bool completion;

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