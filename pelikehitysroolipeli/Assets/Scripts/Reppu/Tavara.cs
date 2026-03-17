using UnityEngine;

public class Tavara : MonoBehaviour
{
    [SerializeField] private double paino;
    [SerializeField] private double tilavuus;

    public double Paino => paino;
    public double Tilavuus => tilavuus;

    public override string ToString()
    {
        return "Tavara";
    }
}