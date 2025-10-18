using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "Scriptable Objects/BulletSettings")]
public class BulletSettings : ScriptableObject
{
    public float speed = 10f;
    public float lifeTime = 1.5f;
}
