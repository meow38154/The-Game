using System;
using UI.Interaction.Events;

namespace UI.Dialogues
{
    [Serializable]
    public struct DialogueBundleData
    {
        public DialogueProfileDataSo profile;
        public string[] dialogues;
    } 
}