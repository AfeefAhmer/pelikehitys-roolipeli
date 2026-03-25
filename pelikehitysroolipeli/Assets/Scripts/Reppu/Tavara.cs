using UnityEngine;

public class Tavara : MonoBehaviour
{
    [SerializeField] private float paino;
    [SerializeField] private float tilavuus;

    public float Paino => paino;
    public float Tilavuus => tilavuus;

    public virtual bool Consumable => false;

    public override string ToString() => "Tavara";

    public virtual bool Use(PlayerController player)
    {
        Debug.Log("T‰t‰ tavaraa ei voi k‰ytt‰‰");
        return false;
    }
}