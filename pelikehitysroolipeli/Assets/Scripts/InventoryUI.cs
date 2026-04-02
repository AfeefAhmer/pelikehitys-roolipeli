using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerController player;

    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemListParent;
    public TextMeshProUGUI itemListText;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (player == null || itemSlotPrefab == null || itemListParent == null)
            return;

        // Poista vanhat slotit
        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        foreach (var item in player.GetInventory().GetItems())
        {
            if (item == null) continue;

            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);

            TextMeshProUGUI text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = item.ToString();

            Button btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                Tavara currentItem = item; // viittaa inventory-instanssiin
                //btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    player.KaytaTavaraa(currentItem);
                });
            }
        }

        // Tekstimuotoinen lista (valinnainen)
        if (itemListText != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Items:");
            sb.AppendLine("----------------");
            foreach (var item in player.GetInventory().GetItems())
                sb.AppendLine(item != null ? item.ToString() : "Null Item");

            itemListText.text = sb.ToString();
        }
    }
}