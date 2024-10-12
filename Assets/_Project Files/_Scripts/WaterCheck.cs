using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class WaterCheck : MonoBehaviour
{
    public float waterLevel = 2f; // Y-position threshold for water detection
    public AudioClip waterSound; // Sound to play when the car falls in water

    private AudioSource audioSource; // AudioSource to play the sound
    private bool isRespawning = false; // Prevents multiple triggers

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the sound clip
        audioSource.clip = waterSound;
    }

    private void Update()
    {
        // Check if the car is below water level and not already respawning
        if (transform.position.y < waterLevel && !isRespawning)
        {
            StartCoroutine(HandleSubmersion());
        }
    }

    private IEnumerator HandleSubmersion()
    {
        isRespawning = true;

        // Play the water sound
        if (audioSource != null && waterSound != null)
        {
            audioSource.Play();
        }

        // Wait for the sound to finish playing
        if (audioSource != null && waterSound != null)
        {
            yield return new WaitForSeconds(waterSound.length);
        }

        // Restart the current scene
        RestartScene();
    }

    private void RestartScene()
    {
        // Get the currently active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log("Scene has been restarted.");
    }
}
