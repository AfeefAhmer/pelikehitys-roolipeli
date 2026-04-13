using System.Text;
using System.Collections.Generic;
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

        // 🔥 Kopioidaan lista ja käännetään se
        List<Tavara> items = new List<Tavara>(player.GetInventory().GetItems());
        items.Reverse();

        foreach (var item in items)
        {
            if (item == null) continue;

            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);

            TextMeshProUGUI text = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = item.ToString();

            Button btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                Tavara currentItem = item;
                btn.onClick.AddListener(() =>
                {
                    player.KaytaTavaraa(currentItem);
                });
            }
        }

        //Tekstimuotoinen lista(valinnainen)
        if (itemListText != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Items:");
            sb.AppendLine("----------------");

            foreach (var item in items)
                sb.AppendLine(item != null ? item.ToString() : "Null Item");

            itemListText.text = sb.ToString();
        }
    }
}