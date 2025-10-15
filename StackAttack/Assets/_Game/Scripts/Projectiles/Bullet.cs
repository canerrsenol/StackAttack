using System.Collections;
using UnityEngine;

public class Bullet : ProjectileBase
{
    [SerializeField] private float speed = 10f;
    private BulletPool bulletPool;
    private Rigidbody rb;

    void Awake()
    {
        bulletPool = BulletPool.Instance;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

    }

    void OnDisable()
    {
        StopAllCoroutines();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHitable>(out var hitable))
        {
            hitable.Hit(damage);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            StopAllCoroutines();
            bulletPool.ReleaseBullet(this);
        }
    }
}
