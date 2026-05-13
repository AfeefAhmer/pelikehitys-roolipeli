using UnityEngine;

public class Item : MonoBehaviour
{
    public float weight;
    public float size;

    public virtual bool Use(PlayerController player)
    {
        return false;
    }
}