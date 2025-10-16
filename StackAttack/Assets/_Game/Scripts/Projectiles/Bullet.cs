using System.Collections;
using UnityEngine;

public class Bullet : ProjectileBase
{
    [SerializeField] private float speed = 10f;
    private BulletPool bulletPool;
    private Rigidbody rb;
    private float arrivedTime = 0f;
    [SerializeField] private float lifeTime = 1.5f;

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

        arrivedTime = 0f;
    }

    void Update()
    {
        arrivedTime += Time.deltaTime;
        if (arrivedTime >= lifeTime)
        {
            arrivedTime = 0f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bulletPool.ReleaseBullet(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHitable>(out var hitable))
        {
            hitable.Hit(damage);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            bulletPool.ReleaseBullet(this);
        }
    }
}
