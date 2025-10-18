using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiHealth : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private TextMeshProUGUI healthText;
    private int lastHealth = -1;
    private Tween healthTween;

    private void OnEnable()
    {
        globalEventsSO.PlayerEvents.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        globalEventsSO.PlayerEvents.HealthChanged -= OnHealthChanged;
        healthTween?.Kill();
    }

    private void OnHealthChanged(int newHealth)
    {
        if (lastHealth == -1)
        {
            lastHealth = newHealth;
            healthText.text = newHealth.ToString();
            return;
        }

        if (newHealth == lastHealth)
        {
            return;
        }

        healthTween?.Kill();
        healthTween = DOVirtual.Int(lastHealth, newHealth, .5f, value =>
        {
            lastHealth = value;
            healthText.text = value.ToString();
        });
    }
}
