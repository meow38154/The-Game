using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Cutscene
{
    public class BossFightCutscene : AbstractCutscene
    {
        [SerializeField] private UnityEvent onStart;
        [SerializeField] private UnityEvent onEnd;
        [SerializeField] private SpriteRenderer console;
        [SerializeField] private Transform map;
        
        protected override IEnumerator CutsceneStartCoroutine()
        {
            onStart?.Invoke();
            map.DOMoveY(0, 0.2f);
            yield return new WaitForSeconds(2f);
            console.DOFade(1, 1f);
            onEnd?.Invoke();
        }
    }
}