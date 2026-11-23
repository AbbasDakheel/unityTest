
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera should follow (set by your character switcher)
    public Transform target;

    // How smoothly the camera catches up (higher = faster, lower = smoother)
    public float smoothSpeed = 5f;

    // The distance to keep between camera and player
    public Vector3 offset;

    void Start()
    {
        // If you haven't set an offset, calculate the initial distance automatically
        if (target != null && offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    // LateUpdate runs AFTER player movement has finished for the frame
    // This prevents camera jitter/stuttering!
    void LateUpdate()
    {
        if (target == null) return;

        // Desired position where camera wants to be
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate from current position to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // For a 2.5D game (like Thomas Was Alone), you often only want to follow X and Y, keeping Z fixed:
        // smoothedPosition.z = transform.position.z; 

        transform.position = smoothedPosition;
        //Debug.Log("LateUpdate in Camera Follow script");
    }

    // Call this function when you switch characters!
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
