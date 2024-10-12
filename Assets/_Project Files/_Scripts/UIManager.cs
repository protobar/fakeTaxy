using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI passengersText;
    public TextMeshProUGUI collisionsText;
    public TextMeshProUGUI scoreText;

    [Header("Score Panel")]
    public GameObject scorePanel;
    public TextMeshProUGUI panelTimeText;
    public TextMeshProUGUI panelCollisionsText;
    public TextMeshProUGUI panelScoreText;
    public TextMeshProUGUI fareText;
    public TextMeshProUGUI individualFareText; // New field for individual fare
    public Button closeButton;

    private void Start()
    {
        scorePanel.SetActive(false);
        closeButton.onClick.AddListener(HideScorePanel);
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {time:F1}s";
    }

    public void UpdatePassengersPicked(int count)
    {
        passengersText.text = $"Passengers: {count}";
    }

    public void UpdateCollisions(int count)
    {
        collisionsText.text = $"Collisions: {count}";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowScorePanel(float time, int collisions, int score)
    {
        panelTimeText.text = $"Time: {time:F1}s";
        panelCollisionsText.text = $"Collisions: {collisions}";
        panelScoreText.text = $"Score: {score}";
        scorePanel.SetActive(true);
    }

    public void HideScorePanel()
    {
        scorePanel.SetActive(false);
        UpdateScore(GameManager.Instance.totalScore);
    }

    public void ShowArrow(bool show)
    {
        // Implement if needed
    }

    public void ResetUI()
    {
        timerText.text = "Time: 0s";
        passengersText.text = "Passengers: 0";
        collisionsText.text = "Collisions: 0";
        scoreText.text = "Score: 0";
        HideScorePanel();
    }

    public void ResetTimer()
    {
        timerText.text = "Time: 0s";
    }

    public void ShowFare(float fare)
    {
        fareText.text = $"Earnings: ${fare:F2}";
    }

    public void ShowIndividualFare(float fare)
    {
        individualFareText.text = $"Fare: ${fare:F2}"; // Display individual fare
    }
}
