using System.Collections;
using DG.Tweening;
using Sound;
using Unity.Cinemachine;
using UnityEngine;
using Utility;

namespace Boss
{
    public class ErrorTile : MonoBehaviour
    {
        [SerializeField] private float noticeTime = 2f;
        [SerializeField] private SpriteRenderer redSprite;
        [SerializeField] private GameObject attack;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private CinemachineImpulseSource impulseSource2;
        [SerializeField] private ParticleSystem onEnableParticle;
        [SerializeField] private SoundClipSO sound;
        
        private void OnEnable()
        {
            StartCoroutine(Pattern());
        }

        private IEnumerator Pattern()
        {
            redSprite.DOFade(0.5f, noticeTime / 5);
            yield return new WaitForSeconds(noticeTime);
            onEnableParticle.Play();
            redSprite.DOFade(0, 0);
            attack.SetActive(true);
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
            impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(1f);
            impulseSource2.GenerateImpulse();
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
            onEnableParticle.Play();
            attack.SetActive(false);
            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        }
    }
}