using System;
using UnityEngine;
using System.Collections;
using Agents.Players;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine.Events;
using Utility;

namespace Cutscene
{

    public class LeverCutscene : AbstractCutscene
    {
        [SerializeField] private CinemachineCamera cam;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private CinemachineImpulseSource impulseSource2;
        [SerializeField] private UnityEvent unityEvent;
        
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Transform console;
        

        protected override IEnumerator CutsceneStartCoroutine()
        {
            cam.Follow = console;
            yield return new WaitForSeconds(2f);
            console.transform.DOMoveY(console.position.y + 8f, 0.5f);
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < 30; i++)
            {
                impulseSource2.GenerateImpulse();
                yield return new WaitForSeconds(0.1f);
            }

            impulseSource.GenerateImpulse();
            particles.Play();
            unityEvent?.Invoke();
            yield return new WaitForSeconds(3f);
            cam.Follow = Player.transform;
        }
    }
}