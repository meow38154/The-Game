using System;
using UnityEngine;
using Utility;

namespace Sound
{
    public class StartSound : MonoBehaviour
    {
        [SerializeField] private SoundClipSO  sound;
        
        private void Start()
        {
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
        }
    }
}