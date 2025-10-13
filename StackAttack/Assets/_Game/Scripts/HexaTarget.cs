using DG.Tweening;
using TMPro;
using UnityEngine;

public class HexaTarget : MonoBehaviour, IHitable
{
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private int health = 3;

    void OnValidate()
    {
        healthText.text = health.ToString();
    }

    public void Hit(float damage)
    {
        Debug.Log($"HexaTarget was hit! Damage: {damage}");

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1, 0.5f));
        sequence.AppendCallback(() =>
        {
            health -= (int)damage;
            healthText.text = health.ToString();

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        });
    }
}
