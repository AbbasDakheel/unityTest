using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ObjectSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Drag your GameObjects here")]
    public GameObject[] targetObjects;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private bool isAnyPlayerActive = false;

    private int currentIndex;
    private Rigidbody currentRb;
    private Collider currentCollider;
    private CameraFollow cam;
    void Start()
    {
        if (Camera.main != null)
        {  
            cam = Camera.main.GetComponent<CameraFollow>();
        }
        //DeactivateAll();
    }

    void SwitchTo(int index)
    {
        currentIndex = index;
        currentRb = targetObjects[currentIndex].GetComponent<Rigidbody>();
        currentCollider = targetObjects[currentIndex].GetComponent<Collider>();
        if (cam != null)
        {
            //Debug.Log("cam is not null in main script ");
            cam.SetTarget(targetObjects[currentIndex].transform);
        }
    }

    // Update is called once per frame


    public void DeactivateAll()
    {
        foreach(GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        isAnyPlayerActive = false;
    }

    public void ActivateObject( int indexToActive)
    {
        if ( indexToActive < 0 || indexToActive > targetObjects.Length)
        {
            //Debug.Log("Index not valid!");
            return;
        }
        if (targetObjects[indexToActive] != null)
        {
            //DeactivateAll();
            targetObjects[indexToActive].SetActive(true);
            currentIndex = indexToActive;
            isAnyPlayerActive = true;
            SwitchTo(currentIndex);
        }
    }

    void Update()
    {

        float moveX = 0f;

        var keyboard = Keyboard.current;
        if(keyboard != null)
        {
            if(keyboard.digit1Key.isPressed) ActivateObject(0);
            if(keyboard.digit2Key.isPressed) ActivateObject(1);
            if(keyboard.digit3Key.isPressed) ActivateObject(2);
            if (isAnyPlayerActive)
            {
                if(keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                {
                    moveX -= 1f;
                }
                
                if(keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                {
                    moveX += 1f;
                }
                Vector3 move = new Vector3(moveX, 0, 0).normalized * moveSpeed;
                currentRb.linearVelocity = new Vector3(move.x, currentRb.linearVelocity.y, move.z);

                // 3. Jump (Spacebar) with ground check
                bool isGrounded = CheckIfGrounded();
                //Debug.Log($"is grounded : {isGrounded}");
                // Vector3 rayStart = currentCollider.bounds.center;
                // float rayDistance = currentCollider.bounds.extents.y + 1f;
                // Debug.DrawRay(rayStart, Vector3.down * rayDistance, isGrounded ? Color.green : Color.red);
                
                
                if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
                {
                    Debug.Log("sapce key pressed");
                    currentRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                if (keyboard.rKey.wasPressedThisFrame || keyboard.backspaceKey.wasPressedThisFrame)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
                    
        }
        // if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateObject(0);
        // if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateObject(1);
        // if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateObject(2);
        // if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     currentActiveGameObject.transform(1,1,1);
        // }
        // if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        // {
            
        // }
        
    }

    private bool CheckIfGrounded()
    {
        Vector3 rayStart = currentCollider.bounds.center;
        float rayDistance = currentCollider.bounds.extents.y + 0.1f;
        bool hit = Physics.Raycast(rayStart,Vector3.down, rayDistance);

        //Debug.DrawRay(rayStart, Vector3.down * rayDistance, hit ? Color.green : Color.red);
        return hit;
        // float distanceToGround = currentCollider.bounds.extents.y + 0.1f;
        // return Physics.Raycast(transform.position, Vector3.down, distanceToGround);
    }

}
