using UnityEngine;

public class PassengerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("Pickup"))
            {
                Debug.Log($"Player entered Pickup Trigger: {gameObject.name}");
                GameManager.Instance.PassengerPickedUp();
                // Optionally, deactivate the pickup trigger after pickup
                // gameObject.SetActive(false);
            }
            else if (CompareTag("Dropoff"))
            {
                Debug.Log($"Player entered Drop-off Trigger: {gameObject.name}");
                GameManager.Instance.PassengerDropped();
                // Optionally, deactivate the drop-off trigger after drop-off
                // gameObject.SetActive(false);
            }
        }
    }
}
