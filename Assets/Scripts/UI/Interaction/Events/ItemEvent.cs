using UI.Inventory;
using UI.Inventory.SO;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class ItemEvent : AbstractInteractionEvent
    {
        [SerializeField] private ItemDataSo itemDataSo;
        [SerializeField] private WhatToDo whatToDo;

        private bool _end;
        
        protected override void InvokeEvent()
        {
            if (_end) return;
            _end = true;
            EventBus.Publish(new ItemMessage(itemDataSo, whatToDo));
        }
    }
}