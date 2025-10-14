using UnityEngine;
using UnityEngine.UI;

public class FinishArea : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private Image fillImage;

    void OnEnable()
    {
        globalEventsSO.PlayerEvents.zPositionChanged += OnZPositionChanged;
    }

    void OnDisable()
    {
        globalEventsSO.PlayerEvents.zPositionChanged -= OnZPositionChanged;
    }

    private void OnZPositionChanged(float progress)
    {
        fillImage.fillAmount = Mathf.Clamp01(progress);
    }
}