using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement setting")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Collider playerCollider;

    private bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        float moveX = 0f;
        float moveZ = 0f;

        var keyboard = Keyboard.current;
        if(keyboard != null)
        {
            if(keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveZ += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveZ -= 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX += 1f;
        }


       
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // 3. Jump (Spacebar) with ground check
        bool isGrounded = CheckIfGrounded();
        
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool CheckIfGrounded()
    {
        float distanceToGround = playerCollider.bounds.extents.y + 0.1f;
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            isGameOver = true;
            //gameObject.SetActive(false);
            Debug.Log("Game Over! you should avoid touching the lava.");
        }
    }

    

}
