using UI.Interaction;
using UI.Interaction.Events;
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
        public readonly AddQuestEvent[] addQuestEvent;
        public readonly int channelNum;

        public DialogueStartMessage(DialogueBundleData[] dialogues, IInteractionTrigger interactionObject, UnityEvent unityEvent,  AddQuestEvent[] addQuestEvent,  int channelNum)
        {
            this.channelNum = channelNum;
            this.addQuestEvent = addQuestEvent;
            this.interactionObject = interactionObject;
            this.dialogues = dialogues;
            this.unityEvent = unityEvent;
        }
    }
}