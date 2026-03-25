using UnityEngine;

public class Vesi : Tavara
{
    public int healAmount = 20;

    public override string ToString() => "Vesi";

    public override bool Use(PlayerController player)
    {
        PlayerDataManager.Instance.AddHealth(healAmount);
        Debug.Log("Vesi käytetty! + " + healAmount + " HP");
        return true;
    }
}