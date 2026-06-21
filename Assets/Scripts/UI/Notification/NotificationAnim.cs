using System;
using GGMLib.AnimatorSystem;
using TMPro;
using UnityEngine;
using Utility;

namespace UI.Notification
{
    public class NotificationAnim : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private Animator anim;
        [SerializeField] private AnimParamSO animParam;

        private void Awake()
        {
            EventBus.Subscribe<NotificationAnimMessage>(PlayNotification);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<NotificationAnimMessage>(PlayNotification);
        }

        private void PlayNotification(NotificationAnimMessage notification)
        {
            if (content == null) return;
            if (anim == null) return;
            if (animParam == null) return;

            content.text = notification.content;

            anim.Play(animParam.ParamHash, 0, 0f);
            anim.Update(0f);
        }
    }

    public readonly struct NotificationAnimMessage
    {
        public readonly string content;

        public NotificationAnimMessage(string content)
        {
            this.content = content;
        }
    }
}