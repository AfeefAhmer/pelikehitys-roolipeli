using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class CrabController : MonoBehaviour, IDamageable
{
    public int health = 500;
    public float speed = 2f;

    // TÄMÄ PUUTTUI
    public event UnityAction OnDeath;

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

        //  ILMOITA SPAWNERILLE
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}