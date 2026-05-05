using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("RESTART BUTTON PRESSED");

        Time.timeScale = 1f;

        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.ResetAll();
        }
        SceneManager.LoadScene(0);
    }
}