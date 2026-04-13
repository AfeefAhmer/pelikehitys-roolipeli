using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class GhostController : MonoBehaviour, IDamageable
{
    public int health = 100;

    private SpriteRenderer spriteRenderer;
    private Collider2D colider;

    private Vector3 savedPosition;

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
            // Talletetaan sijainti juuri ennen katoamista
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

        // Palautetaan samaan paikkaan
        transform.position = savedPosition;

        spriteRenderer.enabled = true;
        colider.enabled = true;

        Debug.Log("Ghost palasi takaisin");
    }

    void Die()
    {
        Debug.Log("Ghost kuoli lopullisesti");
        Destroy(gameObject);
    }
}