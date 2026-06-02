using System;

namespace UI.Interaction.Interface
{
    public interface IInteractionTrigger
    {
        public event Action OnInteractionTrigger; 
        void InteractionActive(bool value);
    }
}