using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// PlayerDataManager hallitsee pelaajan kokemuspisteitä, rahaa ja osumapisteitä.
/// MonoBehaviour-singleton, joka säilyy scene-vaihdosten yli.
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [Header("Player Data")]
    [SerializeField] private int experiencePoints = 0;
    [SerializeField] private int money = 0;
    [SerializeField] private int health = 100;

    [Header("UI References")]
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text hitpointText;

    public int ExperiencePoints => experiencePoints;
    public int Money => money;
    public int Health => health;

    private void Awake()
    {
        // Singleton-toteutus
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Haetaan UI-elementit scenestä jos niitä ei ole asetettu editorissa
        experienceText = GameObject.Find("ExperienceText")?.GetComponent<TMP_Text>();
        coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
        hitpointText = GameObject.Find("HitpointText")?.GetComponent<TMP_Text>();


        UpdateUI();
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

    // EXPERIENCE
    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        experiencePoints += amount;
        UpdateUI();
    }

    // HEALTH
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

    // MONEY
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

        if (money < coinAmount)
            return false;

        money -= coinAmount;
        UpdateUI();
        return true;
    }

    // TESTAUSNAPIT
    void OnGUI()
    {
        int w = 160;
        int h = 30;
        int gap = 35;

        // Oikea reuna
        int x = Screen.width - w - 10;
        int y = 10;

        if (GUI.Button(new Rect(x, y, w, h), "Add XP"))
            AddExperience(10);

        y += gap;

        if (GUI.Button(new Rect(x, y, w, h), "Add HP"))
            AddHealth(5);

        y += gap;

        if (GUI.Button(new Rect(x, y, w, h), "Take Damage"))
            RemoveHealth(10);

        y += gap;

        if (GUI.Button(new Rect(x, y, w, h), "Add Money"))
            AddMoney(5);

        y += gap;

        if (GUI.Button(new Rect(x, y, w, h), "Spend Money"))
            TakeMoney(5);
    }

}
