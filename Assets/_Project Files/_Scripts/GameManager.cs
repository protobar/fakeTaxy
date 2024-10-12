using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Points")]
    public Transform[] pickupPoints;
    public Transform[] dropPoints;

    [Header("Taxi")]
    public GameObject taxi;
    public ArrowIndicator arrowIndicator; // Reference to the ArrowIndicator script

    [Header("UI Elements")]
    public UIManager uiManager;

    [Header("NPCs")]
    public GameObject[] npcPrefabs; // Array of NPC prefabs with idle and wave animations

    [Header("Screen Fade")]
    public ScreenFade screenFade; // Reference to ScreenFade script

    [Header("Passenger Info")]
    private Transform currentPickupPoint;
    private Transform currentDropPoint;
    private bool hasPassenger = false;
    private float tripStartTime;
    private int collisionCount = 0;
    public int totalScore = 0;
    private int passengersCompleted = 0;

    private GameObject currentPassengerNPC; // Reference to the current passenger NPC

    // Fare settings
    public float baseFare = 2.0f; // Base fare
    public float costPerSecond = 0.1f; // Cost per second
    public float costPerDistance = 0.5f; // Cost per unit distance
    public float totalFare = 0.0f; // Total accumulated fare

    // Define game states
    private enum GameState { SelectingPassenger, GoingToPickup, GoingToDropoff }
    private GameState currentState = GameState.SelectingPassenger;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SelectNewPassenger();
    }

    private void Update()
    {
        if (currentState == GameState.GoingToDropoff && hasPassenger)
        {
            float elapsedTime = Time.time - tripStartTime;
            uiManager.UpdateTimer(elapsedTime);
        }
    }

    public void SelectNewPassenger()
    {
        currentState = GameState.SelectingPassenger;

        // Disable all pickup and drop-off points first
        foreach (Transform pickup in pickupPoints)
        {
            pickup.gameObject.SetActive(false);
        }

        foreach (Transform drop in dropPoints)
        {
            drop.gameObject.SetActive(false);
        }

        // Select random pickup and drop-off points
        currentPickupPoint = pickupPoints[Random.Range(0, pickupPoints.Length)];
        currentDropPoint = dropPoints[Random.Range(0, dropPoints.Length)];

        // Enable only the current pickup point
        currentPickupPoint.gameObject.SetActive(true);

        // Debug statements
        Debug.Log($"Selected Pickup Point: {currentPickupPoint.name} at position {currentPickupPoint.position}");
        Debug.Log($"Selected Drop-off Point: {currentDropPoint.name} at position {currentDropPoint.position}");

        // Update arrow indicator to point to the pickup location
        arrowIndicator.SetTarget(currentPickupPoint.position);

        // Notify UIManager to update passengers picked
        uiManager.UpdatePassengersPicked(passengersCompleted);

        // Spawn NPC at pickup point
        SpawnPassengerAtPickup();
    }

    private void SpawnPassengerAtPickup()
    {
        if (npcPrefabs.Length == 0)
        {
            Debug.LogError("No NPC prefabs assigned in GameManager.");
            return;
        }

        // Select a random NPC prefab
        int npcIndex = Random.Range(0, npcPrefabs.Length);
        GameObject npcPrefab = npcPrefabs[npcIndex];

        // Instantiate NPC at pickup point's position and rotation
        currentPassengerNPC = Instantiate(npcPrefab, currentPickupPoint.position, Quaternion.identity);

        // Ensure NPC is facing the taxi
        currentPassengerNPC.transform.LookAt(taxi.transform.position);
        currentPassengerNPC.transform.Rotate(0, 180f, 0);
    }

    public void PassengerPickedUp()
    {
        hasPassenger = true;
        tripStartTime = Time.time;
        collisionCount = 0;
        currentState = GameState.GoingToDropoff;

        // Deactivate pickup point and activate drop-off point
        currentPickupPoint.gameObject.SetActive(false);
        currentDropPoint.gameObject.SetActive(true);

        // Update arrow to point to drop-off location
        arrowIndicator.SetTarget(currentDropPoint.position);

        uiManager.ShowArrow(true);
        Debug.Log("Passenger picked up. Now heading to drop-off point.");

        // Handle passenger NPC disappearance and screen fade
        if (currentPassengerNPC != null)
        {
            // Make NPC disappear (e.g., disable the GameObject)
            currentPassengerNPC.SetActive(false);
            Debug.Log("Passenger NPC has entered the taxi and disappeared.");
        }
    }

    public void PassengerDropped()
    {
        hasPassenger = false;
        float tripTime = Time.time - tripStartTime;
        float distance = Vector3.Distance(currentPickupPoint.position, currentDropPoint.position);
        float fare = CalculateFare(tripTime, distance);

        totalFare += fare; // Add the current fare to the total fare
        passengersCompleted++;

        uiManager.ShowFare(totalFare); // Display total fare on UI
        uiManager.ShowIndividualFare(fare); // Display fare for the current NPC

        CalculateScore(tripTime, collisionCount);
        uiManager.ShowArrow(false);
        uiManager.ShowScorePanel(tripTime, collisionCount, totalScore);

        Debug.Log($"Passenger dropped off. Time: {tripTime:F1}s, Collisions: {collisionCount}, Total Score: {totalScore}, Total Fare: {totalFare:F2}, Individual Fare: {fare:F2}");

        SpawnPassengerAtDropoff();

        // Reset the timer
        uiManager.ResetTimer();

        SelectNewPassenger();
    }


    private void SpawnPassengerAtDropoff()
    {
        if (npcPrefabs.Length == 0)
        {
            Debug.LogError("No NPC prefabs assigned in GameManager.");
            return;
        }

        // Select a random NPC prefab
        int npcIndex = Random.Range(0, npcPrefabs.Length);
        GameObject npcPrefab = npcPrefabs[npcIndex];

        // Instantiate NPC at drop-off point's position and rotation
        GameObject npc = Instantiate(npcPrefab, currentDropPoint.position, Quaternion.identity);

        // Ensure NPC is facing the taxi
        npc.transform.LookAt(taxi.transform.position);

        // Play wave animation
        Animator animator = npc.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Wave");
            Debug.Log("Passenger NPC at drop-off is waving.");
        }
        else
        {
            Debug.LogWarning($"NPC prefab {npcPrefab.name} does not have an Animator component.");
        }

        Destroy(npc, 2f);
    }

    private float CalculateFare(float time, float distance)
    {
        float fare = baseFare + (costPerSecond * time) + (costPerDistance * distance);
        return fare;
    }

    private void CalculateScore(float time, int collisions)
    {
        // Example scoring: base score minus penalties
        int score = Mathf.Max(1000 - Mathf.RoundToInt(time * 10) - collisions * 100, 0);
        totalScore += score;
        Debug.Log($"Score calculated: {score}, Total Score: {totalScore}");
    }

    public void IncrementCollision()
    {
        collisionCount++;
        uiManager.UpdateCollisions(collisionCount);
        Debug.Log($"Collision detected. Total Collisions this trip: {collisionCount}");
    }

    // Optional: Reset game
    public void ResetGame()
    {
        totalScore = 0;
        passengersCompleted = 0;
        totalFare = 0; // Reset total fare if needed
        uiManager.ResetUI();
        SelectNewPassenger();
        Debug.Log("Game reset.");
    }
}
