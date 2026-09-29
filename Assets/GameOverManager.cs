using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject jumpscareImage;

    public void ShowGameOver()
    {
        if (jumpscareImage != null)
            jumpscareImage.SetActive(true);

        Invoke("EnableGameOverScreen", 2.5f);
    }

    void EnableGameOverScreen()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}