using UnityEngine;

public class TaxiCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Assuming any collision counts as a collision penalty
        GameManager.Instance.IncrementCollision();
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
    }
}
