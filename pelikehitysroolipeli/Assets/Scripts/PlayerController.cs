using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    DoorController activeDoor;

    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
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

            // 🔊 Soita ääni kun ovi löydetään
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerOpenDoor);
        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");

            // 🔊 Soita ääni kun kauppias löydetään
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerMeetMerchant);
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
            // 🔊 Virheääni
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
}