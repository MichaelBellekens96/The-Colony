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

    public PlayerController playerController;
    public BuildingPlacement buildingManager;

    private Color green = Color.green;
    private Color red = Color.red;

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
        }
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
}
