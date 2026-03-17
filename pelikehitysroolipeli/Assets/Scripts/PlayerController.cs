using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    DoorController activeDoor;
    Reppu Inventory;
    Tavara tavara = null!;

    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        Inventory = new Reppu(10, 20, 15);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            Debug.Log("Found Door");
            activeDoor = collision.GetComponent<DoorController>();

            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerOpenDoor);
        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");

            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerMeetMerchant);
        }
        else if (collision.CompareTag("Item"))
        {
            Debug.Log("Found Item");

            tavara = collision.GetComponent<Tavara>();

            if (Inventory.Lisaa(tavara))
            {
                Debug.Log("Item added in Inventory");

                // 🔥 Poistaa objektin pelistä
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Ei onnistunut");
            }
        }
    }
    public bool OstoLisatty(Tavara Osto)
    {
        if (Inventory.Lisaa(Osto))
        {
            Debug.Log("Item added in Inventory");
            return true;  // onnistui
        }
        else
        {
            Debug.Log("Ei onnistunut, reppu täynnä");
            return false; // ei onnistunut
        }
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }

    public void OpenDoor()
    {
        if (activeDoor != null)
        {
            activeDoor.ReceiveAction(DoorController.OvenToiminto.Avaa);
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerOpenDoor);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
        }
    }

    public void CloseDoor()
    {
        if (activeDoor != null)
        {
            activeDoor.ReceiveAction(DoorController.OvenToiminto.Sulje);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
        }
    }

    public void LockDoor()
    {
        if (activeDoor != null)
        {
            activeDoor.ReceiveAction(DoorController.OvenToiminto.Lukitse);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
        }
    }

    public void UnlockDoor()
    {
        if (activeDoor != null)
        {
            activeDoor.ReceiveAction(DoorController.OvenToiminto.AvaaLukko);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
        }
    }
    public Reppu GetInventory()
    {
        return Inventory;
    }
}