
using Assets.Scripts;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;

    private Projectile projectile;
    private Rigidbody2D rb;

    private int damageAmount;

    private float travelledDistance;
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

        travelledDistance =
            Vector3.Distance(startPosition, transform.position);

        if (travelledDistance >= projectile.range)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Projectile newProjectile)
    {
        if (newProjectile == null)
        {
            Debug.LogError("Projectile NULL!");
            return;
        }

        projectile = newProjectile;

        CalculateDamage();
    }

    void CalculateDamage()
    {
        if (projectile == null)
        {
            Debug.LogError("Projectile missing!");
            return;
        }

        damageAmount = Mathf.Max(1, projectile.damage);

        Debug.Log(
            $"Projectile initialized. Base damage: {projectile.damage}, Final damage: {damageAmount}"
        );
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            return;

        Debug.Log("Projectile hit: " + other.collider.name);

        IDamageable damageable =
            other.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Damage dealt: " + damageAmount);

            damageable.TakeDamage(damageAmount);
        }
        else
        {
            Debug.Log("No IDamageable found");
        }

        Destroy(gameObject);
    }
}