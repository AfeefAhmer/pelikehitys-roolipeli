using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour, IDamageable
{
    public int health = 500;
    public float speed = 5f;

    private SpriteRenderer spriteRenderer;
    private Collider2D colider;

    private Vector3 savedPosition;
    public event UnityAction OnDeath;
    public int damage = 10;
    private Transform player;
    Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colider = GetComponent<Collider2D>();
    }

    void Start()
    {
        FindPlayer();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 🔥 varmistetaan että player löytyy aina
        if (player == null)
        {
            FindPlayer();
            return;
        }

        FollowPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Ghost löysi pelaajan!");
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Liikutaan kohti pelaajaa ilman rotaatiota
        rb.linearVelocity = direction * speed;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        Debug.Log("Ghost otti vahinkoa: " + amount);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            savedPosition = transform.position;
            StartCoroutine(Disappear(3f));
        }
    }

    IEnumerator Disappear(float duration)
    {
        spriteRenderer.enabled = false;
        colider.enabled = false;

        yield return new WaitForSeconds(duration);

        transform.position = savedPosition;

        spriteRenderer.enabled = true;
        colider.enabled = true;
    }

    void Die()
    {
        Debug.Log("Ghost kuoli lopullisesti");

        if (GameManager.instance != null)
        {
            GameManager.instance.WinGame();
        }

        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}