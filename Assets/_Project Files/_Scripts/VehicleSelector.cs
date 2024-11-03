using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleSelector : MonoBehaviour
{
    [System.Serializable]
    public class Vehicle
    {
        public GameObject vehicleModel;
        public bool isUnlocked;
        public int price;
        public int power;      // Power stat (0-100)
        public int handling;   // Handling stat (0-100)
        public int braking;    // Braking stat (0-100)
    }

    public Vehicle[] vehicles;
    private int currentIndex = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI priceText;
    public Image lockIcon;  // Updated to use an Image component for the lock icon
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI handlingText;
    public TextMeshProUGUI brakingText;

    public Button leftButton;
    public Button rightButton;

    private void Start()
    {
        UpdateVehicleDisplay();
    }

    public void CycleLeft()
    {
        currentIndex = (currentIndex - 1 + vehicles.Length) % vehicles.Length;
        UpdateVehicleDisplay();
    }

    public void CycleRight()
    {
        currentIndex = (currentIndex + 1) % vehicles.Length;
        UpdateVehicleDisplay();
    }

    public void DisplayUnlockedVehicle()
    {
        // Find the first unlocked vehicle in the array
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i].isUnlocked)
            {
                currentIndex = i;
                break;
            }
        }

        // Update the display to show the unlocked vehicle
        UpdateVehicleDisplay();
    }

    private void UpdateVehicleDisplay()
    {
        // Activate the selected vehicle model and deactivate others
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].vehicleModel.SetActive(i == currentIndex);
        }

        // Update the UI based on whether the current vehicle is unlocked
        Vehicle currentVehicle = vehicles[currentIndex];

        if (currentVehicle.isUnlocked)
        {
            priceText.gameObject.SetActive(false);
            lockIcon.gameObject.SetActive(false);  // Hide lock icon if unlocked
        }
        else
        {
            priceText.text = $"Price: ${currentVehicle.price}";
            lockIcon.gameObject.SetActive(true);   // Show lock icon if locked
            priceText.gameObject.SetActive(true);
        }

        powerText.text = $"Power: {currentVehicle.power}/100";
        handlingText.text = $"Handling: {currentVehicle.handling}/100";
        brakingText.text = $"Braking: {currentVehicle.braking}/100";
    }
}
