using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI itemListText;

    void Update()
    {
        if (player == null) return;

        Reppu inv = player.GetInventory();
        if (inv == null) return;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Items:");
        sb.AppendLine("----------------");

        foreach (var item in inv.GetItems())
        {
            sb.AppendLine(item.ToString());
        }

        sb.AppendLine("----------------");
        sb.AppendLine("Count: " + inv.TavaroidenMaara);
        sb.AppendLine("Paino: " + inv.NykyPaino);
        sb.AppendLine("Tilavuus: " + inv.NykyTilavuus);

        itemListText.text = sb.ToString();
    }
}