using Lean.Touch;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private float shootInterval = 0.2f;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootForce = 20f;
    private BulletPool bulletPool;

    private bool isShooting;
    private float shootCooldown;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
        bulletPool = BulletPool.Instance;
    }

    void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    void OnFingerDown(LeanFinger finger)
    {
        isShooting = true;
        shootCooldown = 0f;
    }

    void OnFingerUp(LeanFinger finger)
    {
        isShooting = false;
        shootCooldown = 0f;
    }

    void Update()
    {
        if (gameManager.GameState != GameState.Started) return;

        if (isShooting)
        {
            shootCooldown -= Time.deltaTime;

            if (shootCooldown <= 0f)
            {
                Shoot();
                shootCooldown = shootInterval > 0f ? shootInterval : 0f;
            }
        }
    }

    void Shoot()
    {
        var bullet = bulletPool?.GetBullet();
        if (bullet == null)
        {
            return;
        }

        bullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
    }
}
