using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private Image fillImage;
    private Tween tween;

    void OnEnable()
    {
        globalEventsSO.UIEvents.ProgressBarChanged += OnProgressBarChanged;
    }

    void OnDisable()
    {
        globalEventsSO.UIEvents.ProgressBarChanged -= OnProgressBarChanged;
    }

    private void OnProgressBarChanged(float progress)
    {
        // Buradan 0 ile 1 arasında bir değer alırsınız ve fillImage'ın fillAmount özelliğini güncellersiniz eğer tween zaten varsa onu durdurun
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
        tween = fillImage.DOFillAmount(progress, 0.2f).SetEase(Ease.InOutSine);
    }
}
