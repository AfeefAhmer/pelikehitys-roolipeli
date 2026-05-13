using UnityEngine;

public class MeleeStrike : Projectile
{
    protected virtual void Awake()
    {
        weight = 0f;
        size = 0f;
    }

    public override bool Use(PlayerController player)
    {
        return false;
    }
}