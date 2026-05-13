using Assets.Scripts;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;

    private Projectile projectile;
    private Rigidbody2D rb;

    private int damageAmount;

    private Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (projectile == null)
            return;

        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= projectile.range)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Projectile newProjectile)
    {
        if (newProjectile == null)
            return;

        projectile = newProjectile;

        damageAmount = Mathf.Max(1, projectile.damage);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            return;

        IDamageable damageable =
            other.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }
}