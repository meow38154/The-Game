using UnityEngine;
using DG.Tweening;

public class SpriteFadeIn : MonoBehaviour
{
    public void FadeIn(SpriteRenderer target)
    {
        target.DOFade(1, 1f);
    }
}