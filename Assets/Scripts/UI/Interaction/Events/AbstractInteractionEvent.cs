using System;
using System.Text.RegularExpressions;
using UI.Interaction.Interface;
using UnityEngine;

namespace UI.Interaction.Events
{
    public abstract class AbstractInteractionEvent : MonoBehaviour, IInteractionEvent
    { 
        [Header("Start Setting")]
        [SerializeField] private int requiredInteractionCount;

        [Header("Channel Setting")] 
        [SerializeField] private int channelNumber;
        public  int ChannelNumber => channelNumber;

        public IInteractionTrigger InteractionTrigger { get; set; }
        
        protected virtual void Start()
        {
            InteractionTrigger.OnInteractionTrigger += HandleInteraction;
        }

        private void HandleInteraction(int num)
        {
            if (num != channelNumber) return;
            
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
            string baseName = gameObject.name;
            for (int i = 0; i < 10; i++)
                baseName = Regex.Replace(baseName, @"\s*\(\d+\)$", "");
            
            baseName = Regex.Replace(baseName, @"\s*\(c\s*=\s*-?\d+\)$", "");

            gameObject.name = $"{baseName} (c = {channelNumber})";
            
            if (requiredInteractionCount > 0) return;
            requiredInteractionCount = 1;
        }

        protected abstract void InvokeEvent();
    }
}