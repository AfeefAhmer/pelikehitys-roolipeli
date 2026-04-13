using UnityEngine;

public class Nuoli : Tavara
{
    public Nuolikärki karki;   // 🔥 muutettu string → enum
    public string pera;
    public int pituus;
    public GameObject projectilePrefab;

    public void AsetaTiedot(Nuolikärki k, string p, int v)
    {
        karki = k;
        pera = p;
        pituus = v;
    }

    public override string ToString() => $"Nuoli ({karki}, {pera}, {pituus}cm)";

    public override bool Use(PlayerController player)
    {
        player.chosenArrow = this;
        player.ArrowPrefab = projectilePrefab;
        Debug.Log("Nuoli valittu käyttöön");
        return false;
    }
}