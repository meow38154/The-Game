using System.Collections;
using DG.Tweening;
using Sound;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Utility;

namespace Cutscene
{
    public class ConsoleCutscene : AbstractCutscene
    {
        [SerializeField] private CinemachineCamera cam;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private Transform me;
        [SerializeField] private GameObject console;
        [SerializeField] private VideoPlayer videoPlayer;

        [SerializeField] private GameObject bloom;
        [SerializeField] private GameObject blueScreen;
        
        [SerializeField] private UnityEvent onFinish;
        [SerializeField] private SoundClipSO sound;
        
        
        protected override IEnumerator CutsceneStartCoroutine()
        {
            cam.Follow = me;
            me.DORotate(Vector3.zero, 0.5f);
            yield return new WaitForSeconds(2f);
            console.transform.DOMoveY(-50, 0);
            console.transform.DOMoveY(1, 1);
            console.SetActive(true);
            yield return new WaitForSeconds(3f);
            videoPlayer.Play();
            yield return new WaitForSeconds(6f);
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
            bloom.SetActive(true);
            blueScreen.SetActive(true);
            EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
            impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(2f);
            cam.Follow = Player.transform;
            onFinish?.Invoke();
        }
    }
}