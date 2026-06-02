using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogues
{
    [CreateAssetMenu(fileName = "Base Dialogue Profile Data", menuName = "Dialogue/Profile", order = 0)]
    public class DialogueProfileDataSo : ScriptableObject
    {
        [field: SerializeField] public Sprite ProfileIcon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
    }
}