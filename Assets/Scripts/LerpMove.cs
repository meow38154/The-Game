using DG.Tweening;
using UnityEngine;

public class LerpMove : MonoBehaviour
{
    [SerializeField] private Transform targetTrm;

    public void Move()
    {
        transform.DOMove(targetTrm.position, 1f).SetEase(Ease.Linear);
    }
}