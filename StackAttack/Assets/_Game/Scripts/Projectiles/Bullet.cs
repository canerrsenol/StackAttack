using UnityEngine;

public class Bullet : ProjectileBase
{
    [SerializeField] private float speed = 10f;

    private BulletPool bulletPool;

    void Awake()
    {
        bulletPool = BulletPool.Instance;
    }

    void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IHitable>(out var hitable))
        {
            hitable.Hit(damage);
        }

        if (bulletPool != null)
        {
            bulletPool.ReleaseBullet(this);
        }
    }
}
