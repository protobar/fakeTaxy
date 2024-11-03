using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;  // Target for the camera to orbit around (taxi car)
    public float orbitSpeed = 0.5f;  // Speed of orbit movement
    public float minAngle = 60f;  // Minimum angle for orbit on Y-axis
    public float maxAngle = 120f;  // Maximum angle for orbit on Y-axis
    public float distance = 5f;  // Distance from the target

    private float currentAngle;  // Current angle of rotation
    private bool movingForward = true;  // Direction of orbit movement

    void Start()
    {
        // Initialize the camera's position and angle based on starting transform
        currentAngle = minAngle;
        UpdateCameraPosition();
    }

    void Update()
    {
        // Smoothly move the angle back and forth between min and max angle
        if (movingForward)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                movingForward = false;
            }
        }
        else
        {
            currentAngle -= orbitSpeed * Time.deltaTime;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;
                movingForward = true;
            }
        }

        // Update the camera's position based on the current angle
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // Calculate position based on angle and distance
        float radians = currentAngle * Mathf.Deg2Rad;
        float x = target.position.x + distance * Mathf.Sin(radians);
        float z = target.position.z + distance * Mathf.Cos(radians);

        // Set the camera's position and rotation
        transform.position = new Vector3(x, target.position.y + 2.26f, z);  // Adjust Y position as needed
        transform.LookAt(target);
    }
}
