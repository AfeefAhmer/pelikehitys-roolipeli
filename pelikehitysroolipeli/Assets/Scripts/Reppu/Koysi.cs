using UnityEngine;

public class Koysi : Tavara
{
    public override string ToString() => "Koysi";

    public override bool Use(PlayerController player)
    {
        player.chosenWeapon = this;
        Debug.Log("Köysi valittu aseeksi");
        return false;
    }
}
