using Assets.Scripts;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Nuolen nopeus
    public float speed = 5f;

    public Nuolikärki kärki;
    private int damageAmount;

    private int pituus;

    void Start()
    {
        MaterialDamage();
    }

    void Update()
    {
        // Liikuta nuolta eteenpäin
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // Tätä kutsutaan kun nuoli spawnataan
    public void Initialize(Nuoli nuoli)
    {
        if (nuoli != null)
        {
            kärki = nuoli.karki;   // 🔥 suoraan enum
            pituus = nuoli.pituus;
        }

        MaterialDamage();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            return;

        Debug.Log("Nuoli osui: " + other.collider.name + " (Tag: " + other.collider.tag + ")");

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }

    void MaterialDamage()
    {
        int baseDamage = 0;

        switch (kärki)
        {
            case Nuolikärki.Puu:
                baseDamage = 1;
                break;

            case Nuolikärki.Teräs:
                baseDamage = 10;
                break;

            case Nuolikärki.Timantti:
                baseDamage = 30;
                break;
        }

        // pituus lisää pientä bonusta
        damageAmount = baseDamage + Mathf.RoundToInt(pituus / 5f);

        //nopeus vaikuttaa vielä lopulliseen vahinkoon
        damageAmount = Mathf.RoundToInt(damageAmount * speed);
    }
}