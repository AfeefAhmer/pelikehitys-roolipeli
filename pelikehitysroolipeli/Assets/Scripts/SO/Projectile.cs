using UnityEngine;

public class Projectile : Item
{
    public int damage;
    public float range;

    [HideInInspector]
    public ProjectileController controller;

    public override bool Use(PlayerController player)
    {
        return false;
    }
}