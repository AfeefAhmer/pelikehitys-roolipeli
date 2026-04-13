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
    public GameObject ArrowPrefab;
    public Tavara chosenWeapon;
    public InventoryUI inventoryUI;
    public float arrowSpeed = 20f;
    public float fireCooldown = 0.5f;  // Aika sekunteina nuolien välillä
    private float lastFireTime = -10f;


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

    void Update()
    {

        // Tarkistetaan hiiren vasen nappi ja cooldown
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime >= fireCooldown)
        {
            ShootAtMouse();
            lastFireTime = Time.time;
        }
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
            Debug.Log("Found Door");
        }
        else if (collision.CompareTag("Item"))
        {
            Tavara itemPrefab = collision.GetComponent<Tavara>();
            if (itemPrefab != null)
            {
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

    private Tavara CreateInventoryItem(Tavara prefab)
    {
        Tavara itemInstance = Instantiate(prefab);
        itemInstance.gameObject.SetActive(false);
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

    public void OpenDoor() => activeDoor?.ReceiveAction(DoorController.OvenToiminto.Avaa);
    public void CloseDoor() => activeDoor?.ReceiveAction(DoorController.OvenToiminto.Sulje);
    public void LockDoor() => activeDoor?.ReceiveAction(DoorController.OvenToiminto.Lukitse);
    public void UnlockDoor() => activeDoor?.ReceiveAction(DoorController.OvenToiminto.AvaaLukko);

    // 🔹 NUOLEN AMPUMINEN 2D
    public void ShootArrow(Vector3 target)
    {
        if (chosenArrow == null)
        {
            return;
        }

        GameObject arrowInstance = Instantiate(ArrowPrefab.gameObject, transform.position, Quaternion.identity);

        // 🔥 LISÄTTY: annetaan Nuoli-data controllerille
        ArrowController controller = arrowInstance.GetComponent<ArrowController>();
        if (controller != null)
        {
            controller.Initialize(chosenArrow);
        }

        // 2D: suunta x-y tasossa
        Vector2 direction = ((Vector2)target - rb.position).normalized;

        // Käännetään nuoli oikeaan suuntaan (z-rotation)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();
        if (arrowRb != null)
        {
            arrowRb.linearVelocity = direction * arrowSpeed;
        }
    }

    private void ShootAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        ShootArrow(worldPosition);
    }
}