using UnityEngine;

public class Jousi : Tavara
{
    public override string ToString() => "Jousi";

    public override bool Use(PlayerController player)
    {
        player.chosenWeapon = this;
        Debug.Log("Jousi valittu aseeksi");
        return false;
    }
}

