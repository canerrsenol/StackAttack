using DG.Tweening;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = Vector3.zero;

        DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.one, .75f)
            .OnComplete(() =>
            {
                DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.one * 1.1f, 0.5f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }

    void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
