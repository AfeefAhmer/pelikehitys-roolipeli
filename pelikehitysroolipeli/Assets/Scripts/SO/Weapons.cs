using UnityEngine;

public class Weapon : Item
{
    public WeaponDataSO data;

    private SpriteRenderer sr;

    public Sprite Sprite => data != null ? data.sprite : null;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && data != null)
        {
            sr.sprite = data.sprite;
        }
    }

    public override bool Use(PlayerController player)
    {
        if (player == null)
            return false;

        player.EquipWeapon(this);
        return true;
    }

    public bool Attack(Vector2 direction)
    {
        if (data == null || data.projectilePrefab == null)
            return false;

        GameObject projObj =
            Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);

        Projectile proj = projObj.GetComponent<Projectile>();
        ProjectileController controller = projObj.GetComponent<ProjectileController>();

        if (proj == null || controller == null)
            return false;

        controller.Initialize(proj);

        Rigidbody2D rb = projObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * data.projectileSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        return true;
    }
}