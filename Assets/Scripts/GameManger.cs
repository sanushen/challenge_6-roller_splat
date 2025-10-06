using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour
{
    public static GameManger singleton;
    private GroundPiece[] allGroundPieces;

    public AudioClip levelCompleteSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        SetupNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);  // MOVED INSIDE
            
            // ADD THIS: Get or add Audio Source
            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        } 
        else if (singleton != this) 
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }
    
    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++) {
            if(allGroundPieces[i].isColored == false) {
                isFinished = false;
                break;
            }
        }

        if(isFinished) 
        {
            // PLAY SOUND BEFORE CHANGING LEVEL
            if(audioSource != null && levelCompleteSound != null)
            {
                audioSource.PlayOneShot(levelCompleteSound, 1.0f);
            }
            
            // Wait a moment before changing level so sound can play
            StartCoroutine(DelayedLevelChange());
        }
    }

    private IEnumerator DelayedLevelChange()
    {
        yield return new WaitForSeconds(1f); // Wait 1 second for sound to play
        startNextLevel();
    }

    private void startNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // If we're at the last scene (scene 4), loop back to scene 0
        if(currentSceneIndex >= 4)
        {
            FindObjectOfType<GameOverUI>()?.ShowGameOver();
        }
        else
        {
            // Load the next scene
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Restart from Level 1
    }
}
