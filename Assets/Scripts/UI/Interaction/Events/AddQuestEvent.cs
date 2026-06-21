using Sound;
using UI.Notification;
using UI.Quest;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class AddQuestEvent : AbstractInteractionEvent
    {
        [Header("Quest Settings")]
        [SerializeField] private string title;
        [SerializeField] private string notificationTitle;
        [SerializeField] private QuestListEnum questTitle;
        [SerializeField] private bool noTitle;
        [SerializeField] private SoundClipSO sound;
        public bool End { get; set; }
        
        protected override void InvokeEvent()
        {
            
//            AddQuest();
        }

        public void AddQuest()
        {
            if (End) return;
            End = true;
            EventBus.Publish(new QuestAddMessage(questTitle, title));
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
            if (noTitle) return;
            EventBus.Publish(new NotificationAnimMessage(notificationTitle == "" ?  title : notificationTitle));
        }
    }
}