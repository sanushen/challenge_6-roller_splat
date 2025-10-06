using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour
{
    public static GameManger singleton;
    private GroundPiece[] allGroundPieces;
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
        } else if (singleton != this) {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
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

        if(isFinished) {
            //Call next level
            startNextLevel();
        }
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
