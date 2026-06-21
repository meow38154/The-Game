using UnityEngine;

namespace UI.Inventory
{
    public readonly struct ItemDescriptionMessage
    {
        public readonly Vector2 pos;
        public readonly string name;
        public readonly string lore;

        public ItemDescriptionMessage(Vector3 pos, string name, string lore)
        {
            this.pos = pos;
            this.name = name;
            this.lore = lore;
        }
    }
    
    public readonly struct ItemDescriptionRemoveMessage
    {
    }
}