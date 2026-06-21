using UnityEngine;
using Utility;

namespace Sound
{
    public class SoundPlayAh : MonoBehaviour
    {
        public void SoundPlay(SoundClipSO sound)
        {
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
        }
    }
}