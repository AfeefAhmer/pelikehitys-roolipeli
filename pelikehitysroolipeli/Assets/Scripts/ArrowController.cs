using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Nuolen nopeus
    public float speed = 10f;

    void Update()
    {
        // Liikuta nuolta eteenpäin sen nopeudella
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // Tämä kutsutaan automaattisesti kun nuoli törmää johonkin
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Tarkistetaan, ettei törmätty pelaajaan
        if (other.collider.CompareTag("Player")==false)
        {
            // Tulostetaan Debug-logiin mihin nuoli osui
            Debug.Log("Nuoli osui: " + other.collider.name + " (Tag: " + other.collider.tag + ")");

            // Tuhoa nuoli
            Destroy(gameObject);
        }
        
    }
}