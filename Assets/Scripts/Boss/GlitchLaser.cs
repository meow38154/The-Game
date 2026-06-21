using System;
using System.Collections;
using Sound;
using Unity.Cinemachine;
using UnityEngine;
using Utility;

namespace Boss
{
    public class GlitchLaser : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        private bool _coolTime = true;
        [SerializeField] private SoundClipSO soundClip;
        
        private void OnEnable()
        {
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, soundClip));
            _coolTime = true;
        }

        private void Update()
        {
            if (!_coolTime) return;
            StartCoroutine(Shaking());
            impulseSource.GenerateImpulse();
        }

        private IEnumerator Shaking()
        {
            _coolTime = false;
            yield return new WaitForSeconds(0.1f);
            _coolTime = true;
        }
    }
}