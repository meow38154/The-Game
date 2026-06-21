using GGMLib.ObjectPool.Runtime;
using Pool;
using Sound;
using UnityEngine;
using Utility;

namespace UI.Interaction.Events
{
    public class KillEventEvent : AbstractInteractionEvent
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolItemSO poolItem;

        [SerializeField] private SoundClipSO sound;
        
        protected override void InvokeEvent()
        {
            KillParticlePool item = poolManager.Pop<KillParticlePool>(poolItem);
            
            item.ExtraResetEvents();
            
            Transform ownerTrm = InteractionTrigger.Owner;
            
            item.transform.position = ownerTrm.position;
            ownerTrm.gameObject.SetActive(false);
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
        }
    }
}