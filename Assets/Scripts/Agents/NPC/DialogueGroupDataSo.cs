using UI.Dialogues;
using UnityEngine;

namespace Agents.NPC
{
    [CreateAssetMenu(fileName = "base Dialogue Group", menuName = "Dialogue/Group", order = 0)]
    public class DialogueGroupDataSo : ScriptableObject
    {
        [field: SerializeField] public DialogueDataSo[] Dialogues { get; private set; }
    }
}