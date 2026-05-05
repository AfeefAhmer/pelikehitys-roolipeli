using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class PlayerSaveData
{
    public string playerName;
    public int experiencePoints;
    public int money;
    public int health;
}

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [Header("Player Info")]
    public string playerName = "Player";

    [Header("Player Stats")]
    [SerializeField] private int experiencePoints = 0;
    [SerializeField] private int money = 100;
    [SerializeField] private int health = 100;

    [Header("UI References (TMP Text)")]
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text hitpointText;

    private string savePath;

    public int ExperiencePoints => experiencePoints;
    public int Money => money;
    public int Health => health;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/player_save.json";

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        FindUIReferences();
        UpdateUI();
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

    // ================= FIND UI =================
    private void FindUIReferences()
    {
        experienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
        hitpointText = GameObject.Find("HitpointText")?.GetComponent<TMP_Text>();
    }

    // ================= UI UPDATE =================
    private void UpdateUI()
    {
        if (experienceText != null)
            experienceText.text = "XP: " + experiencePoints;

        if (coinText != null)
            coinText.text = "Coins: " + money;

        if (hitpointText != null)
            hitpointText.text = "HP: " + health;
    }

    // ================= SAVE =================
    public void SaveData()
    {
        PlayerSaveData data = new PlayerSaveData
        {
            playerName = playerName,
            experiencePoints = experiencePoints,
            money = money,
            health = health
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved to: " + savePath);
    }

    // ================= LOAD =================
    public void LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        playerName = data.playerName;
        experiencePoints = data.experiencePoints;
        money = data.money;
        health = data.health;

        UpdateUI();

        Debug.Log("Game Loaded");
    }

    // ================= RESET =================
    public void ResetAll()
    {
        experiencePoints = 0;
        money = 100;
        health = 100;

        UpdateUI();
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

    public void SetHealth(int value)
    {
        health = value;
        UpdateUI();
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
}