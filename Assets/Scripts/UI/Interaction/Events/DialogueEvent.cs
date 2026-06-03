using Agents.NPC;
using UI.Dialogues;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class DialogueEvent : AbstractInteractionEvent
    {
        [SerializeField] private DialogueGroupDataSo dialogues;
        private int _indexCount;


        public void PlayDialogue()
        {
            if (dialogues == null || dialogues.Dialogues == null || dialogues.Dialogues.Length == 0)
                return;

            if (InteractionTrigger == null)
            {
                Debug.LogError($"Interaction is null");
                return;
            }
            
            EventBus.Publish(
                new DialogueStartMessage(
                    dialogues.Dialogues[_indexCount].DialogueBundleData,
                    InteractionTrigger));

            if (_indexCount < dialogues.Dialogues.Length - 1)
                _indexCount++;
        }

        protected override void InvokeEvent()
        {
            PlayDialogue();
        }
    }
}