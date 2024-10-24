using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main UI")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI quotaText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI passengersText;
    public TextMeshProUGUI fareText;
    public TextMeshProUGUI individualFareText;

    [Header("Score Panel")]
    public GameObject scorePanel;
    public TextMeshProUGUI panelTimeText;
    public TextMeshProUGUI panelCollisionsText;
    public TextMeshProUGUI panelFareText;
    public TextMeshProUGUI dayResultText;

    private void Start()
    {
        scorePanel.SetActive(false);
    }

    public void UpdateDayInfo(int dayNumber, float quota)
    {
        dayText.text = $"Day {dayNumber}";
        quotaText.text = $"Earn: ${quota:F2}";
    }

    public void UpdateDayTimer(float timeRemaining)
    {
        timerText.text = $"Time Left: {timeRemaining:F1}s";
    }

    public void UpdatePassengersPicked(int count)
    {
        passengersText.text = $"Passengers: {count}";
    }

    public void ShowFare(float totalFare)
    {
        fareText.text = $"Total Earnings: ${totalFare:F2}";
    }


    public void ShowIndividualFare(float fare)
    {
        individualFareText.text = $"Fare: ${fare:F2}";
    }

    public void ShowScorePanel(float time, int collisions, float fare)
    {
        panelTimeText.text = $"Time: {time:F1}s";
        panelCollisionsText.text = $"Collisions: {collisions}";
        panelFareText.text = $"Fare: ${fare:F2}";
        scorePanel.SetActive(true);
    }

    public void DisplayDaySuccess(int dayNumber, bool success)
    {
        dayResultText.text = success ? $"Day {dayNumber} Complete!" : $"Day {dayNumber} Failed!";
        scorePanel.SetActive(true);
    }
}
