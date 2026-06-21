using UI.Inventory.SO;

namespace UI.Inventory
{
    public readonly struct ItemMessage
    {
        public readonly ItemDataSo itemData;
        public readonly WhatToDo type;
        
        public ItemMessage(ItemDataSo itemData,  WhatToDo type)
        {
            this.type = type;
            this.itemData = itemData;
        }
    }
}