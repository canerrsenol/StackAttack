using UnityEngine;

public class BulletPool : GenericPoolBase<BulletPool, Bullet>
{
    public Bullet GetBullet() => GetFromPool();

    public void ReleaseBullet(Bullet bullet) => SendToPool(bullet);
}
