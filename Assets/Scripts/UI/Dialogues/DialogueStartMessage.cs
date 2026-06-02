using UI.Interaction;
using UI.Interaction.Interface;
using UnityEngine;

namespace UI.Dialogues
{
    public readonly struct DialogueStartMessage
    {
        public readonly DialogueBundleData[] Dialogues;
        public readonly IInteractionTrigger InteractionObject;

        public DialogueStartMessage(DialogueBundleData[] dialogues, IInteractionTrigger interactionObject)
        {
            InteractionObject = interactionObject;
            Dialogues = dialogues;
        }
    }
}