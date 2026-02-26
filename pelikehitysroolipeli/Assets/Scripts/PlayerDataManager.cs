using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton joka hallitsee pelaajan kokemuspisteit‰, rahaa ja HP:t‰.
/// UI p‰ivitet‰‰n automaattisesti.
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [Header("Player Stats")]
    [SerializeField] private int experiencePoints = 0;
    [SerializeField] private int money = 100;
    [SerializeField] private int health = 100;

    [Header("UI References (TMP Text)")]
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text hitpointText;

    public int ExperiencePoints => experiencePoints;
    public int Money => money;
    public int Health => health;

    private void Awake()
    {
        // Singleton turvallisuus
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // P‰ivitet‰‰n UI aina kun scene vaihtuu
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIReferences();
        UpdateUI();
    }

    private void FindUIReferences()
    {
        if (experienceText == null)
            experienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        if (coinText == null)
            coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
        if (hitpointText == null)
            hitpointText = GameObject.Find("HitpointText")?.GetComponent<TMP_Text>();
    }

    private void UpdateUI()
    {
        if (experienceText != null)
            experienceText.text = "XP: " + experiencePoints;
        if (coinText != null)
            coinText.text = "Coins: " + money;
        if (hitpointText != null)
            hitpointText.text = "HP: " + health;
    }

    // ================= EXPERIENCE =================
    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        experiencePoints += amount;
        UpdateUI();
    }

    // ================= HEALTH =================
    public void AddHealth(int amount)
    {
        if (amount <= 0) return;
        health += amount;
        UpdateUI();
    }

    public int RemoveHealth(int damageAmount)
    {
        if (damageAmount <= 0) return health;
        health -= damageAmount;
        health = Mathf.Max(0, health);
        UpdateUI();
        return health;
    }

    // ================= MONEY =================
    public int AddMoney(int coinAmount)
    {
        if (coinAmount <= 0) return money;
        money += coinAmount;
        UpdateUI();
        return money;
    }

    public bool TakeMoney(int coinAmount)
    {
        if (coinAmount <= 0) return true;
        if (money < coinAmount) return false;

        money -= coinAmount;
        UpdateUI();
        return true;
    }

    // ================= ONGUI TESTI =================
    private void OnGUI()
    {
        int buttonWidth = 120;
        int buttonHeight = 30;
        int margin = 10;

        int baseX = margin; // vasen reuna
        int yStart = Screen.height - (buttonHeight + margin) * 3; // ensimm‰isen rivin y
        int y = yStart;

        // XP napit
        if (GUI.Button(new Rect(baseX, y, buttonWidth, buttonHeight), "+XP"))
        {
            AddExperience(10);
        }
        if (GUI.Button(new Rect(baseX + buttonWidth + margin, y, buttonWidth, buttonHeight), "-XP"))
        {
            experiencePoints = Mathf.Max(0, experiencePoints - 10);
            UpdateUI();
        }

        y += buttonHeight + margin;

        // Money napit
        if (GUI.Button(new Rect(baseX, y, buttonWidth, buttonHeight), "+Coins"))
        {
            AddMoney(10);
        }
        if (GUI.Button(new Rect(baseX + buttonWidth + margin, y, buttonWidth, buttonHeight), "-Coins"))
        {
            TakeMoney(10);
        }

        y += buttonHeight + margin;

        // Health napit
        if (GUI.Button(new Rect(baseX, y, buttonWidth, buttonHeight), "+HP"))
        {
            AddHealth(10);
        }
        if (GUI.Button(new Rect(baseX + buttonWidth + margin, y, buttonWidth, buttonHeight), "-HP"))
        {
            RemoveHealth(10);
        }
    }
}