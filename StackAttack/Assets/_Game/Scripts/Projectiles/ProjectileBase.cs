using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected int damage = 1;
    public int Damage { get => damage; set => damage = value; }

    public virtual void Initialize(int damage)
    {
        this.damage = damage;
    }
}
