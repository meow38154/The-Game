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

        public void PlayDialogue(AnnouncementMessage message)
        {
            _content.text = message.content;
            _rectTransform.DOKill();
            
            Sequence seq = DOTween.Sequence();
            
            seq.Append(_rectTransform.DOAnchorPosX(-750f, animDuration).SetEase(Ease.OutCubic));
            
            seq.AppendInterval(waitDuration);
            
            seq.Append(_rectTransform.DOAnchorPosX(-1500f, animDuration).SetEase(Ease.InCubic));
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<AnnouncementMessage>(PlayDialogue);
        }
    }
}