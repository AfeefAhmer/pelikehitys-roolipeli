using UnityEngine;

public class Miekka : Tavara
{
    public override string ToString() => "Miekka";

    public override bool Use(PlayerController player)
    {
        player.chosenWeapon = this;
        Debug.Log("Miekka valittu aseeksi");
        return true;
    }
}