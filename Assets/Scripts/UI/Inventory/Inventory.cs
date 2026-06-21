using System;
using UI.Inventory.SO;
using UnityEngine;
using Utility;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform panel;
        private ItemSlot[] _itemSlots =  new ItemSlot[24];

        private void OnEnable()
        {
            for (int i = 0; i < panel.childCount; i++)
            {
                _itemSlots[i] = panel.GetChild(i).GetComponent<ItemSlot>();
            }
            
            EventBus.Subscribe<ItemMessage>(HandleItemAddMessage);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ItemMessage>(HandleItemAddMessage);
        }

        private void HandleItemAddMessage(ItemMessage item)
        {
            switch (item.type)
            {
                case WhatToDo.Add:
                    AddItem(item.itemData);
                    break;
                case WhatToDo.Remove:
                    RemoveItem(item.itemData);
                    break;
                case WhatToDo.Existence:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddItem(ItemDataSo itemDataSo)
        {
            foreach (var t in _itemSlots)
            {
                if (t.ItemData != null) continue;

                t.ItemData = itemDataSo;
                SortInventory();
                return;
            }

            Debug.Log("인벤토리가 가득 찼습니다.");
        }

        private void RemoveItem(ItemDataSo itemDataSo)
        {
            foreach (var t in _itemSlots)
            {
                if (t.ItemData != itemDataSo) continue;

                t.ItemData = null;
                SortInventory();
                return;
            }
        }

        private void SortInventory()
        {
            ItemDataSo[] items = new ItemDataSo[_itemSlots.Length];

            int index = 0;

            foreach (var slot in _itemSlots)
            {
                if (slot.ItemData == null) continue;

                items[index++] = slot.ItemData;
            }

            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].ItemData = items[i];
            }
        }
    }
}