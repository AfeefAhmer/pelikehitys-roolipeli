using UnityEngine;

public class Nuoli : Tavara
{
    public string karki;
    public string pera;
    public int pituus;

    public void AsetaTiedot(string k, string p, int v)
    {
        karki = k;
        pera = p;
        pituus = v;
    }

    public override string ToString() => $"Nuoli ({karki}, {pera}, {pituus}cm)";

    public override bool Use(PlayerController player)
    {
        player.chosenArrow = this;
        Debug.Log("Nuoli valittu käyttöön");
        return true;
    }
}