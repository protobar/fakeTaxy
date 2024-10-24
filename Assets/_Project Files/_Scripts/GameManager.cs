using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Points")]
    public Transform[] pickupPoints;
    public Transform[] dropPoints;

    [Header("Taxi")]
    public GameObject taxi;
    public ArrowIndicator arrowIndicator;

    [Header("UI Elements")]
    public UIManager uiManager;

    [Header("NPCs")]
    public GameObject[] npcPrefabs;

    [Header("Passenger Info")]
    private Transform currentPickupPoint;
    private Transform currentDropPoint;
    private bool hasPassenger = false;
    private float tripStartTime;
    private int collisionCount = 0;
    private int passengersCompleted = 0;

    private GameObject currentPassengerNPC;

    // Fare settings
    public float baseFare = 2.0f;
    public float costPerSecond = 0.1f;
    public float costPerDistance = 0.5f;
    public float totalFare = 0.0f;

    // Day series settings
    public int currentDay = 1;
    public float dayDuration = 180.0f;  // 3 minutes
    private float dayTimer;
    public float earningsQuota = 100.0f;
    public float earningsIncreaseRate = 1.5f;  // Increase quota each day
    public float timeMultiplier = 0.9f;  // Reduce available time each day

    private enum GameState { SelectingPassenger, GoingToPickup, GoingToDropoff, DayOver }
    private GameState currentState = GameState.SelectingPassenger;

    private void Awake()
    {
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
        Time.timeScale = 1.0f;

        StartNewDay();
    }

    private void Update()
    {
        if (currentState != GameState.DayOver)
        {
            dayTimer -= Time.deltaTime;
            uiManager.UpdateDayTimer(dayTimer);

            if (dayTimer <= 0)
            {
                EndDay(false);  // Time's up
            }

            if (totalFare >= earningsQuota)
            {
                EndDay(true);  // Quota reached
            }
        }
    }

    public void StartNewDay()
    {
        dayTimer = dayDuration;
        totalFare = 0.0f;
        uiManager.UpdateDayInfo(currentDay, earningsQuota);

        SelectNewPassenger();
    }

    private void EndDay(bool success)
    {
        currentState = GameState.DayOver;
        if (success)
        {
            currentDay++;
            earningsQuota *= earningsIncreaseRate;
            dayDuration *= timeMultiplier;
            uiManager.DisplayDaySuccess(currentDay, true);
        }
        else
        {
            uiManager.DisplayDaySuccess(currentDay, false);
        }

        Invoke("StartNewDay", 5f);  // Start the next day after 5 seconds
    }

    public void SelectNewPassenger()
    {
        currentState = GameState.SelectingPassenger;

        foreach (Transform pickup in pickupPoints)
        {
            pickup.gameObject.SetActive(false);
        }

        foreach (Transform drop in dropPoints)
        {
            drop.gameObject.SetActive(false);
        }

        currentPickupPoint = pickupPoints[Random.Range(0, pickupPoints.Length)];
        currentDropPoint = dropPoints[Random.Range(0, dropPoints.Length)];

        currentPickupPoint.gameObject.SetActive(true);
        arrowIndicator.SetTarget(currentPickupPoint.position);
        uiManager.UpdatePassengersPicked(passengersCompleted);

        SpawnPassengerAtPickup();
    }

    private void SpawnPassengerAtPickup()
    {
        if (npcPrefabs.Length == 0)
        {
            Debug.LogError("No NPC prefabs assigned in GameManager.");
            return;
        }

        int npcIndex = Random.Range(0, npcPrefabs.Length);
        GameObject npcPrefab = npcPrefabs[npcIndex];

        currentPassengerNPC = Instantiate(npcPrefab, currentPickupPoint.position, Quaternion.identity);
        currentPassengerNPC.transform.LookAt(taxi.transform.position);
        currentPassengerNPC.transform.Rotate(0, 180f, 0);
    }

    public void PassengerPickedUp()
    {
        hasPassenger = true;
        tripStartTime = Time.time;
        collisionCount = 0;
        currentState = GameState.GoingToDropoff;

        currentPickupPoint.gameObject.SetActive(false);
        currentDropPoint.gameObject.SetActive(true);
        arrowIndicator.SetTarget(currentDropPoint.position);

        //uiManager.ShowArrow(true);

        if (currentPassengerNPC != null)
        {
            currentPassengerNPC.SetActive(false);
        }
    }

    public void PassengerDropped()
    {
        hasPassenger = false;
        float tripTime = Time.time - tripStartTime;
        float distance = Vector3.Distance(currentPickupPoint.position, currentDropPoint.position);
        float fare = CalculateFare(tripTime, distance);

        totalFare += fare;
        passengersCompleted++;

        uiManager.ShowFare(totalFare);
        uiManager.ShowIndividualFare(fare);

        CalculateScore(tripTime, collisionCount);
        //uiManager.ShowArrow(false);
        uiManager.ShowScorePanel(tripTime, collisionCount, totalFare);
        SpawnPassengerAtDropoff();
        SelectNewPassenger();
    }

    private float CalculateFare(float time, float distance)
    {
        float fare = baseFare + (costPerSecond * time) + (costPerDistance * distance);
        return fare;
    }

    private void CalculateScore(float time, int collisions)
    {
        int score = Mathf.Max(1000 - Mathf.RoundToInt(time * 10) - collisions * 100, 0);
        Debug.Log($"Score calculated: {score}");
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1.0f;
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;

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

    


}

