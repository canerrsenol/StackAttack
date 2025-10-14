using DG.Tweening;
using UnityEngine;

public class UiScaleAnimation : MonoBehaviour
{
    void OnEnable()
    {
        transform.DOScale(Vector3.one * 1.1f, .35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    void OnDisable()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
}
