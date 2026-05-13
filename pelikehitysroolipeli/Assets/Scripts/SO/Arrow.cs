
using UnityEngine;

public class Arrow : Projectile
{
    public Nuolikärki kärki;
    public int pituus;

    public void Initialize(Nuoli data)
    {
        kärki = data.karki;
        pituus = data.pituus;

        range = pituus;

        switch (kärki)
        {
            case Nuolikärki.Puu:
                damage = 1;
                break;

            case Nuolikärki.Teräs:
                damage = 10;
                break;

            case Nuolikärki.Timantti:
                damage = 50;
                break;
        }
    }

    public override bool Use(PlayerController player)
    {
        ProjectileController projectileController =
            GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            projectileController.Initialize(this);
        }

        return true;
    }
}