using UnityEngine;

namespace UI.Inventory.SO
{
    [CreateAssetMenu(fileName = "base Item Data", menuName = "UI/Item", order = 0)]
    public class ItemDataSo : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemListEnum ItemEnum { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Lore { get; private set; }
    }
}