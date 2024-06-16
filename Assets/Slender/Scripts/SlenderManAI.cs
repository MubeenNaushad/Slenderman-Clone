using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class SlenderManAI : MonoBehaviour
{
    public Transform player; // Reference to the player's GameObject
    public float baseTeleportDistance = 10f; // Base teleportation distance
    public float baseTeleportCooldown = 15f; // Base time between teleportation attempts
    public float baseChaseProbability = 0.05f; // Base probability of chasing the player
    public float rotationSpeed = 5f; // Rotation speed when looking at the player
    public AudioClip teleportSound; // Reference to the teleport sound effect
    private AudioSource audioSource;

    public GameObject staticObject; // Reference to the "static" GameObject
    public float staticActivationRange = 5f; // Range at which "static" should be activated
    public float deathActivationRange = 2f; // Range at which death should be activated
    public VideoClip staticVideo; // Reference to the static video
    public VideoClip deathVideo; // Reference to the death video
    public Material staticMaterial; // Reference to the static material (Fade)
    public Material deathMaterial; // Reference to the death material (Opaque)

    private Vector3 baseTeleportSpot;
    private float teleportTimer;
    private bool returningToBase;
    private bool isDeathVideoPlaying = false; // Flag to check if death video is playing

    private SlenderPlayerController playerController; // Reference to the player's controller
    private VideoPlayer videoPlayer;
    private Renderer staticRenderer; // Reference to the renderer of the static object
    private GameLogic gameLogic; // Reference to the game logic script

    public GameObject deathUI;

    public GameObject victoryUI;

    private void Start()
    {
        baseTeleportSpot = transform.position;
        teleportTimer = baseTeleportCooldown;

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the teleport sound
        audioSource.clip = teleportSound;

        // Ensure the "static" object is initially turned off
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        // Get reference to the player's controller
        playerController = player.GetComponent<SlenderPlayerController>();

        // Get the VideoPlayer component from the static object
        if (staticObject != null)
        {
            videoPlayer = staticObject.GetComponent<VideoPlayer>();
            staticRenderer = staticObject.GetComponent<Renderer>();
            videoPlayer.clip = staticVideo; // Set the initial video clip to the static video

            // Register for the video end event
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        // Get reference to the game logic script
        gameLogic = GameObject.FindWithTag("GameLogic").GetComponent<GameLogic>();

        //Show the no escape UI
        victoryUI.SetActive(true);
        StartCoroutine(NoEscapeUI());
    }

    private void Update()
    {
        if (player == null || isDeathVideoPlaying)
        {
            return;
        }

        UpdateAggressiveness(gameLogic.pageCount); // Update Slenderman's aggressiveness based on pages collected

        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot();
                teleportTimer = baseTeleportCooldown;
                returningToBase = false;
            }
            else
            {
                DecideTeleportAction();
                teleportTimer = baseTeleportCooldown;
            }
        }

        RotateTowardsPlayer();

        // Check player distance and toggle the "static" object accordingly
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= deathActivationRange)
        {
            // Freeze the player's movement
            playerController.canMove = false;

            // Play the death video and adjust the material and scale
            if (videoPlayer != null && videoPlayer.clip != deathVideo)
            {
                isDeathVideoPlaying = true;
                videoPlayer.clip = deathVideo;
                staticRenderer.material = deathMaterial;
                videoPlayer.SetDirectAudioMute(0, false); // Unmute audio
                videoPlayer.Play();

                // Adjust the scale of the static object for the death video
                staticObject.transform.localScale = new Vector3(1.28f, 0.72f, 1f);
            }
        }
        else if (distanceToPlayer <= staticActivationRange)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true);
            }

            // Play the static video and reset the material and scale
            if (videoPlayer != null && videoPlayer.clip != staticVideo)
            {
                videoPlayer.clip = staticVideo;
                staticRenderer.material = staticMaterial;
                videoPlayer.SetDirectAudioMute(0, true); // Mute audio for static video
                videoPlayer.Play();

                // Reset the scale of the static object for the static video
                staticObject.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust as needed
            }
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false);
            }

            // Allow the player to move
            playerController.canMove = true;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (vp.clip == deathVideo)
        {
            // Show the death UI when the death video ends
            deathUI.SetActive(true);

            // Restart the game
            StartCoroutine(RestartAndResetDeathUI());
        }
    }

    private IEnumerator NoEscapeUI(){
        yield return new WaitForSeconds(5f);
        victoryUI.SetActive(false);
    }

    private IEnumerator RestartAndResetDeathUI()
    {
        // Wait for a few seconds
        yield return new WaitForSeconds(5f); // Adjust the time as needed

        // Restart the game
        RestartGame();

        // Reset the death UI to inactive
        deathUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateAggressiveness(int pageCount)
    {
        if (pageCount >= 2 && pageCount < 7)
        {
            // Gradually increase aggressiveness
            baseTeleportDistance = 15f + (pageCount - 2) * 1f; // Increase teleport distance
            baseTeleportCooldown = 15f - (pageCount - 2) * 0.5f; // Decrease teleport cooldown
            baseChaseProbability = 0.2f + (pageCount - 2) * 0.05f; // Increase catch probability
        }
        else if (pageCount == 7)
        {
            // Significantly increase aggressiveness
            baseTeleportDistance = 15f; // Increase teleport distance
            baseTeleportCooldown = 7f; // Decrease teleport cooldown
            baseChaseProbability = 1f; // Maximum catch probability
        }
        else if (pageCount == 8)
        {
            // Slenderman is defeated
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void DecideTeleportAction()
    {
        float randomValue = Random.value;

        if (randomValue <= baseChaseProbability)
        {
            TeleportNearPlayer();
        }
        else
        {
            TeleportToBaseSpot();
        }
    }

    private void TeleportNearPlayer()
    {
        Vector3 randomPosition = player.position + Random.onUnitSphere * baseTeleportDistance;
        randomPosition.y = transform.position.y; // Keep the same Y position
        transform.position = randomPosition;

        // Play the teleport sound
        audioSource.Play();
    }

    private void TeleportToBaseSpot()
    {
        transform.position = baseTeleportSpot;
        returningToBase = true;

        // Play the teleport sound
        audioSource.Play();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Ignore the vertical component

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
