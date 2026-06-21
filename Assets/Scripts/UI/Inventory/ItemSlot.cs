using UI.Inventory.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class ItemSlot : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField] private ItemDataSo itemData;
        [SerializeField] private Image image;

        public ItemDataSo ItemData
        {
            get => itemData;
            set
            {
                if (itemData == value) return;

                itemData = value;
                OnItemChanged();
            }
        }

        private void Awake()
        {
            if (image == null)
                image = GetComponent<Image>();

            OnItemChanged();
        }

        private void OnItemChanged()
        {
            image.color = new Color(1, 1, 1, itemData == null ? 0 : 1);

            if (itemData == null)
            {
                image.sprite = null;
                return;
            }

            image.sprite = itemData.Icon;
        }
        

        public void OnPointerExit(PointerEventData eventData)
        {
            if (itemData == null) return;

            EventBus.Publish(new ItemDescriptionRemoveMessage());
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (itemData == null) return;

            EventBus.Publish(
                new ItemDescriptionMessage(
                    eventData.position,
                    itemData.Name,
                    itemData.Lore
                )
            );
        }
    }
}