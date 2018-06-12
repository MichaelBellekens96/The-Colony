using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStatsMenu : MonoBehaviour {

    [Header("References")]
    public Camera statsMenuCamera;
    public Canvas statsMenu;
    public PlayerController playerController;
    public InputManager input;

    [Header("UI Elements")]
    public Text currentHeatMap;

    public Text basePowerProd;
    public Text basePowerCons;
    public Text baseWaterProd;
    public Text baseWaterCons;

    public Text buildingName;
    public Text buildingPower;
    public Text buildingWater;
    public Text buildingOxygen;
    public Text buildingEnabled;

    [Header("Silhouette Boxes")]
    public GameObject[] silhouetteBoxes = new GameObject[5];
    public List<GameObject> allSilhouetteBoxes;
    public List<BuildingController> allBuildingControllers;

    private Vector3 newCameraPos;
    private Vector3 silhouetteNewPos = new Vector3(0, 20, 0);
    private Color redSilhouette = new Color(1, 0, 0, 0.5f);
    private Color greenSilhouette = new Color(0, 1, 0, 0.5f);

    private Vector3 raycastHeight = new Vector3(0, 50, 0);
    private RaycastHit statsBoxHit;
    private string heatMap;
    public GameObject buildingHit;

    private void OnEnable()
    {
        newCameraPos = playerController.transform.position;
        newCameraPos.y = 50f;
        statsMenuCamera.transform.position = newCameraPos;

        currentHeatMap.text = "";
        heatMap = "";

        buildingName.text = "";
        buildingPower.text = "";
        buildingWater.text = "";
        buildingOxygen.text = "";
        buildingEnabled.text = "";

        UpdateBaseUtilitiesUI();
        GenerateSilhouettes();
    }

    private void OnDisable()
    {
        for (int i = 0; i < allSilhouetteBoxes.Count; i++)
        {
            Destroy(allSilhouetteBoxes[i]);
        }
        allSilhouetteBoxes.Clear();
        allBuildingControllers.Clear();
    }

    private void Update()
    {
        MoveCamera();
        GetBuildingStats();
    }

    private void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (statsMenuCamera.orthographicSize < 15)
            {
                statsMenuCamera.orthographicSize += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (statsMenuCamera.orthographicSize > 3)
            {
                statsMenuCamera.orthographicSize -= 1;
            }
        }

        statsMenuCamera.orthographicSize = Mathf.Clamp(statsMenuCamera.orthographicSize + -Input.GetAxis("Mouse ScrollWheel") * 20, 3f, 15f);

        newCameraPos.x = input.h_Axis;
        newCameraPos.y = 0;
        newCameraPos.z = input.v_Axis;

        statsMenuCamera.transform.position += newCameraPos * 12 * Time.deltaTime;
    }

    private void UpdateBaseUtilitiesUI()
    {
        basePowerProd.text = "Total Power Production: " + BaseManager.Instance.totalPowerProduction;
        basePowerCons.text = "Total Power Consumption: " + BaseManager.Instance.totalPowerConsumption;
        baseWaterProd.text = "Total Water Production: " + BaseManager.Instance.totalWaterProduction;
        baseWaterCons.text = "Total Water Consumption: " + BaseManager.Instance.totalWaterConsumption;
    }

    private void GenerateSilhouettes()
    {
        List<GameObject> allBuildings = BaseManager.Instance.BuildingList;
        List<GameObject> allDisabledBuildings = BaseManager.Instance.DisabledBuildings;

        for (int i = 0; i < allBuildings.Count; i++)
        {
            if (allBuildings[i].GetComponent<BuildingController>())
            {
                BuildingTypes type = allBuildings[i].GetComponent<BuildingController>().buildingData.type;

                allSilhouetteBoxes.Add(Instantiate(silhouetteBoxes[(int)type], allBuildings[i].transform.position + silhouetteNewPos, allBuildings[i].transform.rotation, allBuildings[i].transform));
                allBuildingControllers.Add(allBuildings[i].GetComponent<BuildingController>());

            }
        }
        for (int i = 0; i < allDisabledBuildings.Count; i++)
        {
            if (allDisabledBuildings[i].GetComponent<BuildingController>())
            {
                BuildingTypes type = allDisabledBuildings[i].GetComponent<BuildingController>().buildingData.type;

                allSilhouetteBoxes.Add(Instantiate(silhouetteBoxes[(int)type], allDisabledBuildings[i].transform.position + silhouetteNewPos, allDisabledBuildings[i].transform.rotation, allDisabledBuildings[i].transform));
                allBuildingControllers.Add(allDisabledBuildings[i].GetComponent<BuildingController>());

            }
        }
        for (int i = 0; i < allSilhouetteBoxes.Count; i++)
        {
            allSilhouetteBoxes[i].SetActive(false);
        }
    }

    public void ShowHeatmap(string chosenHeatmap)
    {
        for (int i = 0; i < allSilhouetteBoxes.Count; i++)
        {
            allSilhouetteBoxes[i].SetActive(true);
        }

        heatMap = chosenHeatmap;

        switch (chosenHeatmap)
        {
            case "Power":
                // Do something
                currentHeatMap.text = "Power";
                for (int i = 0; i < allSilhouetteBoxes.Count; i++)
                {
                    if (allBuildingControllers[i].hasPower)
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = greenSilhouette;
                    }
                    else
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = redSilhouette;
                    }
                }
                break;
            case "Water":
                // Do Something
                currentHeatMap.text = "Water";
                for (int i = 0; i < allSilhouetteBoxes.Count; i++)
                {
                    if (allBuildingControllers[i].hasWater)
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = greenSilhouette;
                    }
                    else
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = redSilhouette;
                    }
                }
                break;
            case "Oxygen":
                // Do something
                currentHeatMap.text = "Oxygen";
                for (int i = 0; i < allSilhouetteBoxes.Count; i++)
                {
                    if (allBuildingControllers[i].hasOxygen)
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = greenSilhouette;
                    }
                    else
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = redSilhouette;
                    }
                }
                break;
            case "On/Off":
                // Do something
                currentHeatMap.text = "On/Off";
                for (int i = 0; i < allSilhouetteBoxes.Count; i++)
                {
                    if (allBuildingControllers[i].buildingEnabled)
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = greenSilhouette;
                    }
                    else
                    {
                        allSilhouetteBoxes[i].GetComponent<Renderer>().material.color = redSilhouette;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void GetBuildingStats()
    {
        if (input.mouse_0_down)
        {
            //Debug.DrawRay(CurrentCursorLocation() + raycastHeight, -transform.up * 50, Color.yellow);
            if (Physics.Raycast(CurrentCursorLocation() + raycastHeight, -transform.up, out statsBoxHit, 50f, 1 << 14))
            {
                if (statsBoxHit.transform.gameObject.GetComponentInParent<BuildingController>())
                {
                    BuildingController buildingController = statsBoxHit.transform.gameObject.GetComponentInParent<BuildingController>();
                    buildingHit = buildingController.gameObject;

                    buildingName.text = "Name: " + buildingController.buildingData.buildingName;
                    buildingPower.text = "Power: " + buildingController.buildingData.powerCons.ToString();
                    buildingWater.text = "Water: " + buildingController.buildingData.waterCons.ToString();
                    buildingOxygen.text = buildingController.hasOxygen ? "Oxygen: Yes" : "Oxygen: No";
                    buildingEnabled.text = buildingController.buildingEnabled ? "Enabled" : "Disabled";
                }
            }
        }
    }

    private Vector3 CurrentCursorLocation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = statsMenuCamera.transform.position.y;
        return statsMenuCamera.ScreenToWorldPoint(mousePosition);
    }

    public void EnableSelectedBuilding()
    {
        if (buildingHit != null)
        {
            BaseManager.Instance.EnableBuilding(buildingHit);
            StartCoroutine(ReDrawSilhouettes(heatMap));

            AudioManager.Instance.Play("Enable_Building");

            BuildingController buildingController = buildingHit.GetComponent<BuildingController>();

            buildingName.text = "Name: " + buildingController.buildingData.buildingName;
            buildingPower.text = "Power: " + buildingController.buildingData.powerCons.ToString();
            buildingWater.text = "Water: " + buildingController.buildingData.waterCons.ToString();
            buildingOxygen.text = buildingController.hasOxygen ? "Oxygen: Yes" : "Oxygen: No";
            buildingEnabled.text = buildingController.buildingEnabled ? "Enabled" : "Disabled";
        }
    }

    public void DisableSelectedBuilding()
    {
        if (buildingHit != null)
        {
            BaseManager.Instance.DisableBuilding(buildingHit);
            StartCoroutine(ReDrawSilhouettes(heatMap));

            AudioManager.Instance.Play("Disable_Building");

            BuildingController buildingController = buildingHit.GetComponent<BuildingController>();

            buildingName.text = "Name: " + buildingController.buildingData.buildingName;
            buildingPower.text = "Power: " + buildingController.buildingData.powerCons.ToString();
            buildingWater.text = "Water: " + buildingController.buildingData.waterCons.ToString();
            buildingOxygen.text = buildingController.hasOxygen ? "Oxygen: Yes" : "Oxygen: No";
            buildingEnabled.text = buildingController.buildingEnabled ? "Enabled" : "Disabled";
        }
    }

    private IEnumerator ReDrawSilhouettes(string heatMapString)
    {
        yield return new WaitForSeconds(0.1f);
        ShowHeatmap(heatMapString);
        UpdateBaseUtilitiesUI();
    }
}
