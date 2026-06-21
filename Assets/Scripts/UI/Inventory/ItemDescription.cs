using TMPro;
using UnityEngine;
using Utility;

namespace UI.Inventory
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI loreText;

        private void Awake()
        {
            EventBus.Subscribe<ItemDescriptionMessage>(Description);
            EventBus.Subscribe<ItemDescriptionRemoveMessage>(RemoveDescription);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ItemDescriptionMessage>(Description);
            EventBus.Unsubscribe<ItemDescriptionRemoveMessage>(RemoveDescription);
        }

        private void RemoveDescription(ItemDescriptionRemoveMessage message)
        {
            rectTransform.gameObject.SetActive(false);
        }

        private void Description(ItemDescriptionMessage msg)
        {
            rectTransform.gameObject.SetActive(true);

            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                msg.pos,
                cam,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint + new Vector2(80, -40);

            nameText.text = msg.name;
            loreText.text = msg.lore;
        }
    }
}