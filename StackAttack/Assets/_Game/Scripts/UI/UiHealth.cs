using TMPro;
using UnityEngine;

public class UiHealth : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        globalEventsSO.PlayerEvents.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        globalEventsSO.PlayerEvents.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }
}
