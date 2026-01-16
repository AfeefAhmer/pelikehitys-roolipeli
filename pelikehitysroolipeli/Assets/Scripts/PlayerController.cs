using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;
    DoorController activeDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Huomaa mitä pelaaja löytää
        if (collision.CompareTag("Door"))
        {
            Debug.Log("Found Door");
            activeDoor = collision.GetComponent<DoorController>();
        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");
        }
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }
    public void OpenDoor()
    {
        activeDoor.ReceiveAction(DoorController.OvenToiminto.Avaa);
    }
    public void CloseDoor()
    {
        activeDoor.ReceiveAction(DoorController.OvenToiminto.Sulje);
    }
    public void LockDoor()
    {
        activeDoor.ReceiveAction(DoorController.OvenToiminto.Lukitse);
    }
    public void UnlockDoor() 
    {
        activeDoor.ReceiveAction(DoorController.OvenToiminto.AvaaLukko);
    }
}
