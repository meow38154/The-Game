using UI.Interaction;
using UI.Interaction.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Dialogues
{
    public readonly struct DialogueStartMessage
    {
        public readonly DialogueBundleData[] dialogues;
        public readonly IInteractionTrigger interactionObject;
        public readonly UnityEvent unityEvent;

        public DialogueStartMessage(DialogueBundleData[] dialogues, IInteractionTrigger interactionObject, UnityEvent unityEvent)
        {
            this.interactionObject = interactionObject;
            this.dialogues = dialogues;
            this.unityEvent = unityEvent;
        }
    }
}