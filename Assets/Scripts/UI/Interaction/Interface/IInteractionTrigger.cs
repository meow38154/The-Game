using System;
using UnityEngine;

namespace UI.Interaction.Interface
{
    public interface IInteractionTrigger
    {
        public int ChannelNumber { get; }
        public Transform Owner { get; }
        public event Action<int> OnInteractionTrigger; 
        void InteractionActive(bool value);
    }
}