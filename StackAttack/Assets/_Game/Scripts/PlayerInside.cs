using UnityEngine;

public class PlayerInside : MonoBehaviour
{
    private Health health;

    void Awake()
    {
        health = GetComponentInParent<Health>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IObstacle>(out var obstacle))
        {
            health.TakeDamage(obstacle.CrashDamage);
            obstacle.Crashed();
        }
    }
}
