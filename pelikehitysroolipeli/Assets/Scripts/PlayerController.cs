using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Vector2 lastMovement;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private Reppu inventory;
    private DoorController activeDoor;

    public Nuoli chosenArrow;
    public GameObject ArrowPrefab;
    public Tavara chosenWeapon;
    public InventoryUI inventoryUI;

    public float arrowSpeed = 20f;

    public float fireCooldown = 0.5f;
    private float lastFireTime = -10f;
    public Weapon currentWeapon;
    public PlayerWeaponManager weaponManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (inventory == null)
        {
            inventory = new Reppu(10, 20f, 15f);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform spawn = GameObject.FindWithTag("SpawnPoint").transform;

        transform.position = spawn.position;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        inventoryUI?.RefreshUI();

        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.SetHealth(currentHealth);
        }

        currentHealth = maxHealth;

        if (weaponManager == null)
            weaponManager = GetComponent<PlayerWeaponManager>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position +
            lastMovement * moveSpeed * Time.fixedDeltaTime
        );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;

            Vector3 worldPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction =
                ((Vector2)worldPosition - rb.position).normalized;

            weaponManager?.Attack(direction);
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
                    Destroy(collision.gameObject);

                    inventoryUI?.RefreshUI();

                    Debug.Log(
                        "Esine lisätty inventoryyn: " +
                        itemData.ToString()
                    );
                }
                else
                {
                    Destroy(itemData);

                    Debug.Log(
                        "Reppu täynnä, esine ei lisätty: " +
                        itemPrefab.ToString()
                    );
                }
            }
        }
    }

    private Tavara CreateInventoryItem(Tavara prefab)
    {
        Tavara itemInstance = Instantiate(prefab);

        Tavara tavara =
            (Tavara)prefab.GetComponent<Tavara>().TeeKopio();

        itemInstance.gameObject.SetActive(false);

        return tavara;
    }

    // INVENTORY

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

    // DOORS

    public void OpenDoor()
        => activeDoor?.ReceiveAction(
            DoorController.OvenToiminto.Avaa
        );

    public void CloseDoor()
        => activeDoor?.ReceiveAction(
            DoorController.OvenToiminto.Sulje
        );

    public void LockDoor()
        => activeDoor?.ReceiveAction(
            DoorController.OvenToiminto.Lukitse
        );

    public void UnlockDoor()
        => activeDoor?.ReceiveAction(
            DoorController.OvenToiminto.AvaaLukko
        );

    // SHOOTING

    public void ShootArrow(Vector3 target)
    {
        if (chosenArrow == null)
        {
            Debug.Log("No arrow selected");

            return;
        }

        GameObject arrowInstance =
            Instantiate(
                ArrowPrefab,
                transform.position,
                Quaternion.identity
            );

        // Arrow component
        Arrow arrow =
            arrowInstance.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.Initialize(chosenArrow);
        }
        else
        {
            Debug.LogError("Arrow component missing!");
        }

        // Projectile controller
        ProjectileController projectileController =
            arrowInstance.GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            projectileController.Initialize(arrow);
        }
        else
        {
            Debug.LogError(
                "ProjectileController missing!"
            );
        }

        // Direction
        Vector2 direction =
            ((Vector2)target - rb.position).normalized;

        // Rotation
        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        arrowInstance.transform.rotation =
            Quaternion.Euler(0, 0, angle);

        // Rigidbody velocity
        Rigidbody2D arrowRb =
            arrowInstance.GetComponent<Rigidbody2D>();

        if (arrowRb != null)
        {
            arrowRb.linearVelocity =
                direction * arrowSpeed;
        }
        else
        {
            Debug.LogError(
                "Arrow Rigidbody2D missing!"
            );
        }
    }

    private void ShootAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;

        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction =
            ((Vector2)worldPosition - rb.position).normalized;

        if (currentWeapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        currentWeapon.Attack(direction);
    }

    // HEALTH

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        currentHealth =
            Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log(
            "Player otti damagea: " + amount
        );

        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.SetHealth(
                currentHealth
            );
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player kuoli");
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        Debug.Log("Weapon equipped: " + weapon.name);
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeapon.Use(this);
        }
    }
}