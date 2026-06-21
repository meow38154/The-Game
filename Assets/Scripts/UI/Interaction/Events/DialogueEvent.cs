using Agents.NPC;
using GGMLib.ObjectPool.Runtime;
using Pool;
using UI.Dialogues;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace UI.Interaction.Events
{
    public class DialogueEvent : AbstractInteractionEvent
    {
        [Header("Dialogues Settings")]
        [SerializeField] private DialogueGroupDataSo dialogues;
        [SerializeField] private UnityEvent dialogueEndEvent;
        
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
                    InteractionTrigger, dialogueEndEvent, transform.parent.GetComponentsInChildren<AddQuestEvent>(), InteractionTrigger.ChannelNumber));

            if (_indexCount < dialogues.Dialogues.Length - 1)
                _indexCount++;
        }

        protected override void InvokeEvent()
        {
            PlayDialogue();
        }
    }
}