using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform arrowTransform; // The visual arrow object
    public float arrowHeight = 2f;

    private Vector3 targetPosition;

    private void Start()
    {
        if (arrowTransform == null)
        {
            arrowTransform = transform;
        }
    }

    private void Update()
    {
        if (targetPosition == Vector3.zero)
            return;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Keep the arrow horizontal

        if (direction != Vector3.zero)
        {
            // Rotate the arrow to face the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            arrowTransform.rotation = Quaternion.Slerp(arrowTransform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        // Optionally, position the arrow above the taxi
        transform.position = transform.position + Vector3.up * arrowHeight;
        Debug.Log($"Arrow indicator set to target position: {targetPosition}");
    }
}
