using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMerchantInteraction : MonoBehaviour
{
    private MerchantInfo currentMerchant;

    [Header("Arrow UI")]
    public GameObject arrowPanel;
    public TextMeshProUGUI arrowNameLabel;
    public TMP_Dropdown karkiDropdown;
    public TMP_Dropdown peraDropdown;
    public Slider varsiSlider;
    public TextMeshProUGUI varsiText;
    public TextMeshProUGUI arrowPriceText;
    public Button arrowBuyButton;
    public Button arrowCancelButton;

    [Header("Food UI")]
    public GameObject foodPanel;
    public TextMeshProUGUI foodNameLabel;
    public TMP_Dropdown paaDropdown;
    public TMP_Dropdown lisukeDropdown;
    public TMP_Dropdown kastikeDropdown;
    public TextMeshProUGUI foodPriceText;
    public Button foodBuyButton;
    public Button foodCancelButton;

    [Header("Prefabs")]
    public GameObject ateriaPrefab; // Prefab, jossa Ateria-komponentti ja Inspectorissa asetettu paino/tilavuus
    public GameObject nuoliPrefab;

    private int selectedKarki;
    private int selectedPera;
    private int selectedPaa;

    private int varrenPituus = 70;
    private double currentCost;
    private int currentHeal;

    

    void Start()
    {
        CloseMerchant();

        if (arrowBuyButton != null)
            arrowBuyButton.onClick.AddListener(BuyItem);

        if (arrowCancelButton != null)
            arrowCancelButton.onClick.AddListener(CloseMerchant);

        if (foodBuyButton != null)
            foodBuyButton.onClick.AddListener(BuyItem);

        if (foodCancelButton != null)
            foodCancelButton.onClick.AddListener(CloseMerchant);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MerchantInfo merchant = other.GetComponent<MerchantInfo>();
        if (merchant != null)
        {
            currentMerchant = merchant;
            OpenMerchant();

            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerMeetMerchant);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentMerchant != null &&
            other.GetComponent<MerchantInfo>() == currentMerchant)
        {
            CloseMerchant();
        }
    }

    void OpenMerchant()
    {
        if (currentMerchant == null) return;

        CloseMerchant();

        if (currentMerchant.merchantType == MerchantType.ArrowMerchant)
        {
            if (arrowPanel == null) return;

            arrowPanel.SetActive(true);
            arrowNameLabel.text = currentMerchant.merchantName;

            SetupDropdown(karkiDropdown,
                currentMerchant.GetKarkiNames(),
                val => { selectedKarki = val; UpdateArrowCost(); });

            SetupDropdown(peraDropdown,
                currentMerchant.GetPeraNames(),
                val => { selectedPera = val; UpdateArrowCost(); });

            if (varsiSlider != null)
            {
                varsiSlider.minValue = 60;
                varsiSlider.maxValue = 100;
                varsiSlider.wholeNumbers = true;
                varsiSlider.value = 70;

                varrenPituus = 70;
                UpdateLengthText();

                varsiSlider.onValueChanged.RemoveAllListeners();
                varsiSlider.onValueChanged.AddListener(val =>
                {
                    varrenPituus = (int)val;
                    UpdateLengthText();
                    UpdateArrowCost();
                });
            }

            UpdateArrowCost();
        }
        else if (currentMerchant.merchantType == MerchantType.FoodMerchant)
        {
            if (foodPanel == null) return;

            foodPanel.SetActive(true);
            foodNameLabel.text = currentMerchant.merchantName;

            selectedPaa = 0; // alustetaan valinta

            SetupDropdown(paaDropdown,
                currentMerchant.GetPaaraakaNames(),
                val => { selectedPaa = val; UpdateFoodCost(); });

            SetupDropdown(lisukeDropdown,
                currentMerchant.GetLisukeNames(),
                val => { });

            SetupDropdown(kastikeDropdown,
                currentMerchant.GetKastikeNames(),
                val => { });

            UpdateFoodCost();
        }
    }

    void CloseMerchant()
    {
        if (arrowPanel != null)
            arrowPanel.SetActive(false);

        if (foodPanel != null)
            foodPanel.SetActive(false);

        currentCost = 0;
        currentHeal = 0;
    }

    void SetupDropdown(TMP_Dropdown dropdown,
                       string[] options,
                       System.Action<int> onChange)
    {
        if (dropdown == null) return;

        dropdown.ClearOptions();
        dropdown.AddOptions(
            new System.Collections.Generic.List<string>(options));

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(val => onChange.Invoke(val));

        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    void UpdateLengthText()
    {
        if (varsiText != null)
            varsiText.text = "Pituus: " + varrenPituus + " cm";
    }

    void UpdateArrowCost()
    {
        if (currentMerchant == null) return;

        currentCost = currentMerchant.GetArrowPrice(
            selectedKarki,
            selectedPera,
            varrenPituus);

        currentHeal = 0;

        if (arrowPriceText != null)
            arrowPriceText.text =
                "Hinta: " + currentCost.ToString("0.00") + " kultaa";
    }

    void UpdateFoodCost()
    {
        if (currentMerchant == null) return;

        currentCost =
            currentMerchant.GetFoodPrice(selectedPaa);

        currentHeal =
            currentMerchant.GetFoodHeal(selectedPaa);

        if (foodPriceText != null)
            foodPriceText.text =
                "Hinta: " + currentCost + " kultaa";
    }

    void BuyItem()
    {
        if (currentMerchant == null) return;
        if (currentCost <= 0) return;

        // Tarkista rahat ensin
        if (!PlayerDataManager.Instance.TakeMoney((int)currentCost))
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
            Debug.Log("Ei tarpeeksi rahaa ostoon!");
            return;
        }

        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerBuyItem);

        PlayerController controller = GetComponent<PlayerController>();
        if (controller == null) return;

        // 🍖 FOOD MERCHANT
        if (currentMerchant.merchantType == MerchantType.FoodMerchant)
        {
            if (ateriaPrefab != null)
            {
                GameObject go = Instantiate(ateriaPrefab);
                Tavara tavara = go.GetComponent<Ateria>();

                if (tavara != null)
                {
                    if (controller.OstoLisatty(tavara))
                    {
                        PlayerDataManager.Instance.AddHealth(currentHeal);
                        Debug.Log("Ateria ostettu! Hinta: " + currentCost + " kultaa");
                    }
                    else
                    {
                        // Reppu täynnä → perutaan osto
                        PlayerDataManager.Instance.AddMoney((int)currentCost);
                        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
                        Debug.Log("Reppu on täynnä! Et voi ostaa ateriaa.");
                    }
                }
            }
            else
            {
                Debug.LogError("Ateria prefab puuttuu!");
            }
        }

        // 🏹 ARROW MERCHANT
        else if (currentMerchant.merchantType == MerchantType.ArrowMerchant)
        {
            if (nuoliPrefab != null)
            {
                GameObject go = Instantiate(nuoliPrefab);
                Nuoli nuoli = go.GetComponent<Nuoli>();

                if (nuoli != null)
                {
                    Nuolikärki karkiEnum = (Nuolikärki)selectedKarki;
                    string pera = currentMerchant.GetPeraNames()[selectedPera];

                    nuoli.AsetaTiedot(karkiEnum, pera, varrenPituus);

                    if (controller.OstoLisatty(nuoli))
                    {
                        Debug.Log("Nuoli ostettu! Hinta: " + currentCost + " kultaa");
                    }
                    else
                    {
                        // Reppu täynnä → perutaan osto
                        PlayerDataManager.Instance.AddMoney((int)currentCost);
                        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.PlayerInvalidAction);
                        Debug.Log("Reppu on täynnä! Et voi ostaa nuolta.");
                    }
                }
            }
            else
            {
                Debug.LogError("Nuoli prefab puuttuu!");
            }
        }
    }
}