using Assets.Scripts;
using UnityEngine;

public class CrabController : MonoBehaviour, IDamageable
{
    public int health = 500;
    public float speed = 2f;

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
        // Esim. rapu suuttuu ja nopeutuu
        speed += 1f;
        Debug.Log("Crab suuttui ja nopeutui!");
    }

    void Die()
    {
        Debug.Log("Crab kuoli");
        Destroy(gameObject);
    }
}