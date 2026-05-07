using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    // ================= STATES =================
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        PauseMenu
    }

    public GameState currentState;

    [Header("UI")]
    public GameObject winMenu;
    public TMP_Text winText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        ChangeState(GameState.MainMenu);
        HideWinMenu();
    }

    private void Start()
    {
        HideWinMenu();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ================= SCENE LOAD =================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        winMenu = GameObject.Find("WinMenu");

        if (winMenu != null)
        {
            winMenu.SetActive(false);
            winText = winMenu.GetComponentInChildren<TMP_Text>();
        }

        Time.timeScale = 1f;
        CloseAllUI();
    }

    // ================= STATE SYSTEM =================
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game State changed to: " + currentState);

        switch (currentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.PauseMenu:
                Time.timeScale = 0f;
                break;
        }
    }

    // ================= TEST UI (OnGUI) =================
    private void OnGUI()
    {
        // ================= STATE DEBUG =================
        GUI.Box(new Rect(10, 10, 180, 200), "Game States");

        if (GUI.Button(new Rect(20, 40, 160, 30), "Main Menu"))
        {
            ChangeState(GameState.MainMenu);
            SceneManager.LoadScene(0);
        }

        if (GUI.Button(new Rect(20, 80, 160, 30), "Playing"))
        {
            ChangeState(GameState.Playing);
        }

        if (GUI.Button(new Rect(20, 120, 160, 30), "Paused"))
        {
            ChangeState(GameState.Paused);
        }

        if (GUI.Button(new Rect(20, 160, 160, 30), "Pause Menu"))
        {
            ChangeState(GameState.PauseMenu);
        }

        if (GUI.Button(new Rect(20, 200, 160, 30), "Quit"))
        {
            Application.Quit();
        }

        // ================= PAUSE MENU UI =================
        if (currentState == GameState.PauseMenu)
        {
            GUI.Box(new Rect(220, 10, 200, 160), "Pause Menu");

            if (GUI.Button(new Rect(230, 40, 180, 30), "Save"))
            {
                if (PlayerDataManager.Instance != null)
                    PlayerDataManager.Instance.SaveData();
            }

            if (GUI.Button(new Rect(230, 80, 180, 30), "Load"))
            {
                if (PlayerDataManager.Instance != null)
                    PlayerDataManager.Instance.LoadData();
            }

            if (GUI.Button(new Rect(230, 120, 180, 30), "Resume"))
            {
                ChangeState(GameState.Playing);
            }
        }
    }

    // ================= UI =================
    private void HideWinMenu()
    {
        Time.timeScale = 1f;

        if (winMenu != null)
            winMenu.SetActive(false);
    }

    public void WinGame()
    {
        if (winMenu != null)
            winMenu.SetActive(true);

        Time.timeScale = 1f;
    }

    private void CloseAllUI()
    {
        GameObject shop = GameObject.Find("ShopPanel");
        if (shop != null)
            shop.SetActive(false);

        GameObject merchant = GameObject.Find("MerchantPanel");
        if (merchant != null)
            merchant.SetActive(false);
    }
}