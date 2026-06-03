using UI.Interaction;
using UI.Interaction.Interface;
using UnityEngine;

namespace UI.Dialogues
{
    public readonly struct DialogueStartMessage
    {
        public readonly DialogueBundleData[] dialogues;
        public readonly IInteractionTrigger interactionObject;

        public DialogueStartMessage(DialogueBundleData[] dialogues, IInteractionTrigger interactionObject)
        {
            this.interactionObject = interactionObject;
            this.dialogues = dialogues;
        }
    }
}