using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class CrabController : MonoBehaviour, IDamageable
{
    public int health = 500;
    public float speed = 2f;

    public event UnityAction OnDeath;

    private Transform player;
    public int damage = 10;
    void Start()
    {
        // Etsit‰‰n pelaaja tagilla (muista asettaa Player-tag Unityss‰)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Liikutaan kohti pelaajaa ilman rotaatiota
        transform.position += direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        Debug.Log("Crab otti vahinkoa: " + amount);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            ReactToDamage();
        }
    }

    void ReactToDamage()
    {
        speed += 1f;
        Debug.Log("Crab suuttui ja nopeutui!");
    }

    void Die()
    {
        Debug.Log("Crab kuoli");

        OnDeath?.Invoke();

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