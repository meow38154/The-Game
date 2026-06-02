using UnityEngine;

namespace UI.Dialogues
{
    [CreateAssetMenu(fileName = "Base Dialogue Data", menuName = "Dialogue/Base Data", order = 0)]
    public class DialogueDataSo : ScriptableObject
    {
        [field: SerializeField] public DialogueBundleData[] DialogueBundleData { get; private set; }
    }
}