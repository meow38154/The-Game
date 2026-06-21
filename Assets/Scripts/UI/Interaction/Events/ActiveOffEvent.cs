using UnityEngine;

namespace UI.Interaction.Events
{
    [DefaultExecutionOrder(-20)]
    public class ActiveOffEvent : AbstractInteractionEvent
    {
        protected override void InvokeEvent()
        {
            InteractionTrigger.Owner.gameObject.SetActive(false);
        }
    }
}