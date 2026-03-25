using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 lastMovement;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;

    private Reppu inventory;
    private DoorController activeDoor;

    public Nuoli chosenArrow;
    public Tavara chosenWeapon;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        // 🔹 LUODAAN inventory ennen muita skriptejä
        inventory = new Reppu(10, 20f, 15f);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventoryUI?.RefreshUI();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnMoveAction(InputValue value)
    {
        lastMovement = value.Get<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            activeDoor = collision.GetComponent<DoorController>();
        }
        else if (collision.CompareTag("Item"))
        {
            Tavara itemPrefab = collision.GetComponent<Tavara>();
            if (itemPrefab != null)
            {
                // 🔹 Luo inventoryyn kelpaava instanssi
                Tavara itemData = CreateInventoryItem(itemPrefab);
                
                if (inventory.Lisaa(itemData))
                {
                    Destroy(collision.gameObject); // poista scene-esine
                    inventoryUI?.RefreshUI();
                    Debug.Log("Esine lisätty inventoryyn: " + itemData.ToString());
                }
                else
                {
                    Destroy(itemData); // ei mahdu inventoryyn
                    Debug.Log("Reppu täynnä, esine ei lisätty: " + itemPrefab.ToString());
                }
            }
        }
    }

    // 🔹 Luo inventoryyn kelpaavan esine-instanssin
    private Tavara CreateInventoryItem(Tavara prefab)
    {
        // Instantiate, jotta saadaan oikea aliluokka ja ylikirjoitettu Use-metodi
        Tavara itemInstance = Instantiate(prefab);
        itemInstance.gameObject.SetActive(false); // ei näy scenessä
        return itemInstance;
    }

    // 🔹 INVENTORY METODIT

    public Reppu GetInventory() => inventory;

    public bool OstoLisatty(Tavara tavara)
    {
        if (inventory.Lisaa(tavara))
        {
            inventoryUI?.RefreshUI();
            return true;
        }

        Debug.Log("Reppu täynnä!");
        return false;
    }

    public void KaytaTavaraa(Tavara tavara)
    {
        if (tavara == null)
        {
            Debug.Log("Tavara NULL");
            return;
        }

        bool success = tavara.Use(this);

        if (success)
        {
            PoistaTavara(tavara);
        }
    }

    public void PoistaTavara(Tavara tavara)
    {
        inventory.GetItems().Remove(tavara);
        inventoryUI?.RefreshUI();
    }
}