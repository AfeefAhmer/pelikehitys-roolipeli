using UnityEngine;

public class Ateria : Tavara
{
    public int healAmount = 20;

    public override string ToString() => "Ateria";

    public override bool Use(PlayerController player)
    {
        PlayerDataManager.Instance.AddHealth(healAmount);
        Debug.Log("Ateria käytetty! + " + healAmount + " HP");
        return true;
    }
}