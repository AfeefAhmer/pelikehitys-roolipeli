using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class GhostController : MonoBehaviour, IDamageable
{
    public int health = 500;

    private SpriteRenderer spriteRenderer;
    private Collider2D colider;

    private Vector3 savedPosition;

    // 🔥 LISÄÄ TÄMÄ
    public event UnityAction OnDeath;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colider = GetComponent<Collider2D>();
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

        Debug.Log("Ghost katosi " + duration + " sekunniksi");

        yield return new WaitForSeconds(duration);

        transform.position = savedPosition;

        spriteRenderer.enabled = true;
        colider.enabled = true;

        Debug.Log("Ghost palasi takaisin");
    }

    void Die()
    {
        Debug.Log("Ghost kuoli lopullisesti");

        // 🔥 TÄRKEIN RIVI
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}