using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour {

    public static MainUIManager Instance;

    public Canvas HUD;
    [Header("StatsPanel")]
    public GameObject statsPanel;
    public Slider healthSlider;
    public Image healthColor;
    public Slider oxygenSlider;
    public Image oxygenColor;
    public Slider hungerSlider;
    public Image hungerColor;
    public Slider thirstSlider;
    public Image thirstColor;
    public Slider sleepSlider;
    public Image sleepColor;

    [Header("ResourcePanel")]
    public GameObject resourcesPanel;
    public Text metalText;
    public Text metalOreText;
    public Text bioPlasticText;
    public Text rawFoodText;
    public Text mealText;
    public Text drinkText;

    [Header("ToolsPanel")]
    public GameObject toolsPanel;
    public Text activeTool;

    [Header("Contruction Site")]
    public GameObject constructionPanel;
    public Text buildingName;
    public Text numNeededMetal;
    public Text numNeededBioPlastic;
    public Text buildProgression;

    [Header("Interaction")]
    public Text interactionText;
    public Image sleepPanel;

    public PlayerController playerController;
    public PlayerStats playerStats;
    public BuildingPlacement buildingManager;
    public BaseStatsMenu baseStatsMenu;
    public SunMoonRotation sunRotation;
    public ResourceManager resourceManager;
    public TerrainManager terrainManager;
    public BaseManager baseManager;

    private Color green = Color.green;
    private Color red = Color.red;
    public Color sleeping = new Color(0, 0, 0, 1);
    public Color awake = new Color(0, 0, 0, 0);

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        playerController.gameObject.SetActive(true);
        buildingManager.gameObject.SetActive(false);
        constructionPanel.SetActive(false);
        ToggleInteractionText(false);
    }

    public void ToggleBuildMenu()
    {
        bool active = buildingManager.gameObject.activeSelf;
        Camera playerCamera = playerController.GetComponentInChildren<Camera>();
        AudioListener playerAudioListener = playerController.GetComponentInChildren<AudioListener>();
        Camera buildCamera = buildingManager.GetComponentInChildren<Camera>();

        // If buildingmenu is already active, turn it off
        if (active)
        {
            buildCamera.enabled = false;
            buildingManager.gameObject.SetActive(false);
            playerController.enabled = true;
            playerCamera.enabled = true;
            playerAudioListener.enabled = true;
            toolsPanel.SetActive(true);
            statsPanel.SetActive(true);
            resourcesPanel.SetActive(true);
            baseStatsMenu.enabled = false;
            baseStatsMenu.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // If buildingmenu is not active, turn it on
        else
        {
            buildingManager.gameObject.SetActive(true);
            buildCamera.enabled = true;
            playerController.enabled = false;
            playerCamera.enabled = false;
            playerAudioListener.enabled = false;
            toolsPanel.SetActive(false);
            statsPanel.SetActive(false);
            resourcesPanel.SetActive(true);
            baseStatsMenu.enabled = false;
            baseStatsMenu.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ToggleStatsMenu()
    {
        Debug.Log("Toggling StatsMenu...");
        Camera playerCamera = playerController.GetComponentInChildren<Camera>();
        AudioListener playerAudioListener = playerController.GetComponentInChildren<AudioListener>();
        Camera buildCamera = buildingManager.GetComponentInChildren<Camera>();
        Camera statsCamera = baseStatsMenu.statsMenuCamera;

        // If Statsmenu is active disable it
        if (baseStatsMenu.gameObject.activeSelf)
        {
            Debug.Log("Disabling Statmenu");
            baseStatsMenu.enabled = false;
            baseStatsMenu.gameObject.SetActive(false);
            statsCamera.gameObject.SetActive(false);

            playerCamera.enabled = true;
            playerController.enabled = true;
            playerAudioListener.enabled = true;
            toolsPanel.SetActive(true);
            statsPanel.SetActive(true);
            resourcesPanel.SetActive(true);

            buildCamera.enabled = false;
            buildingManager.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // If Statsmenu is not active enable it
        else
        {
            Debug.Log("Enabling Statmenu");
            baseStatsMenu.enabled = true;
            baseStatsMenu.gameObject.SetActive(true);
            statsCamera.gameObject.SetActive(true);

            playerCamera.enabled = false;
            playerController.enabled = false;
            playerAudioListener.enabled = false;
            toolsPanel.SetActive(false);
            statsPanel.SetActive(false);
            resourcesPanel.SetActive(false);

            buildCamera.enabled = false;
            buildingManager.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateStatsPanel()
    {
        healthSlider.value = playerStats.Health;
        if (playerStats.Health > 50) healthColor.color = Color.Lerp(Color.yellow, green, (playerStats.Health - 50) / 50);
        if (playerStats.Health < 50) healthColor.color = Color.Lerp(red, Color.yellow, playerStats.Health / 50);

        oxygenSlider.value = playerStats.Oxygen;
        if (playerStats.Oxygen > 50) oxygenColor.color = Color.Lerp(Color.yellow, green, (playerStats.Oxygen - 50) / 50);
        if (playerStats.Oxygen < 50) oxygenColor.color = Color.Lerp(red, Color.yellow, playerStats.Oxygen / 50);

        hungerSlider.value = playerStats.Hunger;
        if (playerStats.Hunger > 50) hungerColor.color = Color.Lerp(Color.yellow, green, (playerStats.Hunger - 50) / 50);
        if (playerStats.Hunger < 50) hungerColor.color = Color.Lerp(red, Color.yellow, playerStats.Hunger / 50);

        thirstSlider.value = playerStats.Thirst;
        if (playerStats.Thirst > 50) thirstColor.color = Color.Lerp(Color.yellow, green, (playerStats.Thirst - 50) / 50);
        if (playerStats.Thirst < 50) thirstColor.color = Color.Lerp(red, Color.yellow, playerStats.Thirst / 50);

        sleepSlider.value = playerStats.Sleep;
        if (playerStats.Sleep > 50) sleepColor.color = Color.Lerp(Color.yellow, green, (playerStats.Sleep - 50) / 50);
        if (playerStats.Sleep < 50) sleepColor.color = Color.Lerp(red, Color.yellow, playerStats.Sleep / 50);

    }

    public void UpdateStatsPanel(float health, float oxygen, float hunger, float thirst, float sleep)
    {
        healthSlider.value = health;
        if (health > 50) healthColor.color = Color.Lerp(Color.yellow, green, (health - 50) / 50);
        if (health < 50) healthColor.color = Color.Lerp(red, Color.yellow, health / 50);

        oxygenSlider.value = oxygen;
        if (oxygen > 50) oxygenColor.color = Color.Lerp(Color.yellow, green, (oxygen -50) /50);
        if (oxygen < 50) oxygenColor.color = Color.Lerp(red, Color.yellow, oxygen / 50);

        hungerSlider.value = hunger;
        if (hunger > 50) hungerColor.color = Color.Lerp(Color.yellow, green, (hunger - 50) / 50);
        if (hunger < 50) hungerColor.color = Color.Lerp(red, Color.yellow, hunger / 50);

        thirstSlider.value = thirst;
        if (thirst > 50) thirstColor.color = Color.Lerp(Color.yellow, green, (thirst - 50) / 50);
        if (thirst < 50) thirstColor.color = Color.Lerp(red, Color.yellow, thirst / 50);

        sleepSlider.value = sleep;
        if (sleep > 50) sleepColor.color = Color.Lerp(Color.yellow, green, (sleep - 50) / 50);
        if (sleep < 50) sleepColor.color = Color.Lerp(red, Color.yellow, sleep / 50);

    }

    public void UpdateToolPanel(int index)
    {
        switch (index)
        {
            case (int)Tools.Hands:
                activeTool.text = "Hands";
                break;
            case (int)Tools.Drill:
                activeTool.text = "Drill";
                break;
            case (int)Tools.Welder:
                activeTool.text = "Welder";
                break;
            case (int)Tools.Shovel:
                activeTool.text = "Shovel";
                break;
            default:
                break;
        }
    }

    public void UpdateResourcePanel(int metal, int metalOre, int bioPlastic, int rawFood, int meals, int drinks)
    {
        metalText.text = metal.ToString();
        metalOreText.text = metalOre.ToString();
        bioPlasticText.text = bioPlastic.ToString();
        rawFoodText.text = rawFood.ToString();
        mealText.text = meals.ToString();
        drinkText.text = drinks.ToString();
    }

    public void UpdateConstructionPanel(int metal, int bioPlastic, string name)
    {
        buildingName.text = name;
        numNeededMetal.text = metal.ToString();
        numNeededBioPlastic.text = bioPlastic.ToString();
    }

    public void ToggleConstructionPanel(bool value)
    {
        constructionPanel.SetActive(value);
    }

    public void UpdateBuildPercentage(float percentage)
    {
        buildProgression.text = Mathf.RoundToInt(percentage * 10).ToString() + "%";
    }

    public void SetInteractionText(string message)
    {
        interactionText.text = message;
    }

    public void ToggleInteractionText(bool value)
    {
        interactionText.gameObject.SetActive(value);
    }

    public void LoadPreviousSave()
    {
        playerStats.Load();
        resourceManager.Load();
        baseManager.Load();
        terrainManager.Load();
        sunRotation.Load();
    }

    public void GoToSleep()
    {
        StartCoroutine(SleepTransition());
    }

    private IEnumerator SleepTransition()
    {
        float transitionTime = 3f;
        float elapsedTime = 0;
        playerController.enabled = false;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            sleepPanel.color = Color.Lerp(awake, sleeping, Mathf.Clamp(elapsedTime / transitionTime, 0, 1));
            
            yield return null;
        }

        playerStats.Sleep = 100;
        sunRotation.timeScale = 40;

        yield return new WaitForSeconds(5f);

        // Save all data
        playerStats.Save();
        resourceManager.Save();
        baseManager.Save();
        terrainManager.Save();
        sunRotation.Save();

        sunRotation.timeScale = 0.5f;
        elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            sleepPanel.color = Color.Lerp(sleeping, awake, Mathf.Clamp(elapsedTime / transitionTime, 0, 1));
            
            yield return null;
        }

        playerController.enabled = true;
    }
}