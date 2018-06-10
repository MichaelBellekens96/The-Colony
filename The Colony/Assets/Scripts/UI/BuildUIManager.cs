using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIManager : MonoBehaviour {

    private BuildingPlacement buildManager;

    [Header("Build Menu")]
    public Canvas buildMenu;
    public Transform buttonHolder;
    public Button buildingBtnPrefab;

    public List<BuildingData> buildingList = new List<BuildingData>();

    [Header("Building Info")]
    public GameObject buildingPanel;

    public Image buildingSprite;
    public Text description;

    public Text resource1;
    public Image resourceSprite1;
    public Text resource2;
    public Image resourceSprite2;

    public Text powerConsProd;
    public Text waterConsProd;

    private void Awake()
    {
        buildManager = GetComponent<BuildingPlacement>();
    }

    public void CreateBuildingMenu()
    {
        Button currentButton;
        Vector3 newPosition = Vector3.zero;
        Button[] allButtons = new Button[buildingList.Count];

        for (int i = 0; i < buildingList.Count; i++)
        {
            int n = i;
            currentButton = Instantiate(buildingBtnPrefab, buttonHolder);
            newPosition.x = 50 + i * 250;
            currentButton.transform.localPosition = newPosition;
            currentButton.onClick.AddListener(() => buildManager.CreateBuilding(buildingList[n]));
            currentButton.gameObject.AddComponent<BuildingButtonEvents>();
            allButtons[n] = currentButton;
        }

        for (int i = 0; i < buildingList.Count; i++)
        {
            allButtons[i].GetComponent<BuildingButtonEvents>().data = buildingList[i];
            allButtons[i].GetComponent<BuildingButtonEvents>().buildUIManager = this;

            allButtons[i].GetComponentInChildren<Text>().text = buildingList[i].buildingName;
            allButtons[i].GetComponentInChildren<Image>().sprite = buildingList[i].buildingSprite;
        }

        buildingPanel.gameObject.SetActive(false);
    }

    public void LoadBuildingData(BuildingData data)
    {
        buildingPanel.SetActive(true);

        buildingSprite.sprite = data.buildingSprite;
        description.text = data.description;

        if (data.metal > 0)
        {
            resource1.transform.parent.gameObject.SetActive(true);
            resource1.text = data.metal.ToString();
        }
        if (data.bioPlastic > 0)
        {
            resource2.transform.parent.gameObject.SetActive(true);
            resource2.text = data.bioPlastic.ToString();
        }
        if (data.utilityBuilding)
        {
            powerConsProd.text = "Power Production: " + data.powerProd.ToString();
            waterConsProd.text = "Water Prodcution: " + data.waterProd.ToString();
        }
        else
        {
            powerConsProd.text = "Power Consumption: " + data.powerCons.ToString();
            waterConsProd.text = "Water Consumption: " + data.waterCons.ToString();
        }
    }

    public void ResetBuildingData()
    {
        buildingSprite.sprite = null;
        description.text = "";

        resource1.transform.parent.gameObject.SetActive(false);
        resource1.text = "0";
        resource2.transform.parent.gameObject.SetActive(false);
        resource2.text = "0";

        waterConsProd.text = "";
        powerConsProd.text = "";

        buildingPanel.SetActive(false);
    }
}
