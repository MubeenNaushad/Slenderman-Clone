using UnityEngine;

public class RandomTimerSound : MonoBehaviour
{
    public AudioClip soundEffect; // Reference to the sound effect to play
    private AudioSource audioSource;
    public float minInterval = 2f; // Minimum time interval in seconds
    public float maxInterval = 30f; // Maximum time interval in seconds

    private float timer;
    private float nextSoundTime;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the sound effect
        audioSource.clip = soundEffect;

        // Initialize the timer
        nextSoundTime = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to play the sound
        if (timer >= nextSoundTime)
        {
            PlayRandomSound();

            // Reset the timer and set a new random interval
            timer = 0f;
            nextSoundTime = Random.Range(minInterval, maxInterval);
        }
    }

    private void PlayRandomSound()
    {
        // Play the sound effect
        audioSource.Play();
    }
}