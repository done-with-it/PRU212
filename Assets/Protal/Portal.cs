using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextLevelName; // Name of the next level to load
    public AudioClip completionSound; // Audio clip for completion

    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource is not attached, add it
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "character"
            || collision.gameObject.name == "Player")
        {
            // Play the completion sound if available
            if (completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }

            // Wait for a short time before loading the next level
            StartCoroutine(LoadNextLevelAfterDelay(1.5f)); // Adjust the delay as needed
        }
    }

    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextLevelName);
    }
}
