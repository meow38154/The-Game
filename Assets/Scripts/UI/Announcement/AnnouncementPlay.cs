using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utility;

namespace UI.Announcement
{
    public class AnnouncementPlay : MonoBehaviour
    {
        [SerializeField] private float animDuration = 0.5f;
        [SerializeField] private float waitDuration = 3f;
        
        private RectTransform _rectTransform;
        private TextMeshProUGUI _content;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _content = GetComponentInChildren<TextMeshProUGUI>();
            EventBus.Subscribe<AnnouncementMessage>(PlayDialogue);
        }

        private Sequence _sequence;

        public void PlayDialogue(AnnouncementMessage message)
        {
            _content.text = message.content;

            _sequence?.Kill();
            _rectTransform.DOKill();

            _rectTransform.anchoredPosition = new Vector2(-1500f, _rectTransform.anchoredPosition.y);

            _sequence = DOTween.Sequence();

            _sequence.Append(_rectTransform.DOAnchorPosX(-750f, animDuration).SetEase(Ease.OutCubic));
            _sequence.AppendInterval(waitDuration);
            _sequence.Append(_rectTransform.DOAnchorPosX(-1500f, animDuration).SetEase(Ease.InCubic));
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
            EventBus.Unsubscribe<AnnouncementMessage>(PlayDialogue);
        }
    }
}