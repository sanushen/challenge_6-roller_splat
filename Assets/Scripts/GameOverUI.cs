using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}