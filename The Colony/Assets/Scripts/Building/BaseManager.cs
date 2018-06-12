using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {

    public static BaseManager Instance;

    [Header("Utilities")]
    public int totalPowerProduction;
    public int totalPowerConsumption;
    public int totalWaterProduction;
    public int totalWaterConsumption;
    
    [Header("All Buildings")]
    public List<GameObject> BuildingList = new List<GameObject>();
    public List<GameObject> DisabledBuildings = new List<GameObject>();
    public List<GameObject> ConstructionSites = new List<GameObject>();

    [Header("Scriptable Objects")]
    public BuildingData[] buildingDatas;
    public Transform baseTransform;
    public BuildingPlacement buildingPlacement;

    public int buildings;
    public int disabledBuildings;
    public int utility;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9)) Save();
        if (Input.GetKeyDown(KeyCode.F10)) Load();
    }

    public void Save()
    {
        buildings = 0;
        utility = 0;
        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].GetComponent<BuildingController>())
            {
                Debug.Log("controller found");
                buildings++;
            }
            else
            {
                Debug.Log("no controller found");
                utility++;
            }
        }
        disabledBuildings = DisabledBuildings.Count;

        Debug.Log("Saving Base...");
        SaveLoadManager.SaveBase(this);
        Debug.Log("Saving ConstructionSites...");
        SaveLoadManager.SaveConstructionSite(this);
    }

    public void Load()
    {
        Debug.Log("Loading Base...");
        BaseData data = SaveLoadManager.LoadBase();
        if (data != null)
        {
            BuildingData currentBuildingData = null;

            for (int i = 0; i < BuildingList.Count; i++)
            {
                Destroy(BuildingList[i]);
            }
            BuildingList.Clear();

            for (int i = 0; i < DisabledBuildings.Count; i++)
            {
                Destroy(DisabledBuildings[i]);
            }
            DisabledBuildings.Clear();


            totalPowerProduction = 0;
            totalPowerConsumption = 0;
            totalWaterProduction = 0;
            totalWaterConsumption = 0;

            for (int i = 0; i < data.buildingPosX.Length; i++)
            {
                for (int j = 0; j < buildingDatas.Length; j++)
                {
                    if (buildingDatas[j].buildingName == data.buildingName[i])
                    {
                        currentBuildingData = buildingDatas[j];
                    }
                }

                GameObject building = Instantiate(currentBuildingData.buildingPrefab, new Vector3(data.buildingPosX[i], data.buildingPosY[i], data.buildingPosZ[i]), Quaternion.Euler(new Vector3(0, data.buildingRotY[i], 0)), baseTransform) as GameObject;
                building.name = currentBuildingData.buildingName;
                building.GetComponent<BuildingController>().buildingData = currentBuildingData;
                BuildingList.Add(building);
                building.GetComponent<BuildingController>().hasOxygen = true;
                building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = true;
            }

            currentBuildingData = null;

            for (int i = 0; i < data.utilityPosX.Length; i++)
            {
                for (int j = 0; j < buildingDatas.Length; j++)
                {
                    if (buildingDatas[j].buildingName == data.utilityName[i])
                    {
                        currentBuildingData = buildingDatas[j];
                    }
                }

                GameObject building = Instantiate(currentBuildingData.buildingPrefab, new Vector3(data.utilityPosX[i], data.utilityPosY[i], data.utilityPosZ[i]), Quaternion.Euler(new Vector3(0, data.utilityRotY[i], 0)), baseTransform) as GameObject;
                building.name = currentBuildingData.buildingName;
                BuildingList.Add(building);
            }

            currentBuildingData = null;

            for (int i = 0; i < data.disBuildingPosX.Length; i++)
            {
                for (int j = 0; j < buildingDatas.Length; j++)
                {
                    if (buildingDatas[j].buildingName == data.disBuildingName[i])
                    {
                        currentBuildingData = buildingDatas[j];
                    }
                }

                GameObject building = Instantiate(currentBuildingData.buildingPrefab, new Vector3(data.disBuildingPosX[i], data.disBuildingPosY[i], data.disBuildingPosZ[i]), Quaternion.Euler(new Vector3(0, data.disBuildingRotY[i], 0)), baseTransform) as GameObject;
                building.name = currentBuildingData.buildingName;
                building.GetComponent<BuildingController>().buildingData = currentBuildingData;
                DisableBuilding(building);
                building.GetComponent<BuildingController>().hasPower = data.disBuildingHasPower[i];
                building.GetComponent<BuildingController>().hasWater = data.disBuildingHasWater[i];
            }

            RecalculateOxygen();

            totalPowerProduction = data.powerProd;
            totalPowerConsumption = data.powerCons;
            totalWaterProduction = data.waterProd;
            totalWaterConsumption = data.waterCons;
        }

        Debug.Log("Loading Construction Sites...");
        ConstructionData constructionData = SaveLoadManager.LoadConstructionSite();
        if (constructionData != null)
        {
            int dataLength = constructionData.posX.Length;
            GameObject constructionSite = null;
            BuildingData currentBuildingData = null;
            BuildingController buildingController = null;

            for (int i = 0; i < ConstructionSites.Count; i++)
            {
                GameObject site = ConstructionSites[i];
                Destroy(site.GetComponentInChildren<ConstructionSite>().linkedBuilding);
                ConstructionSites.Remove(site);
                Destroy(site);
            }

            ConstructionSites.Clear();

            for (int i = 0; i < dataLength; i++)
            {
                for (int j = 0; j < buildingDatas.Length; j++)
                {
                    if (buildingDatas[j].buildingName == constructionData.buildingName[i])
                    {
                        currentBuildingData = buildingDatas[j];
                    }
                }

                GameObject selectedBuilding = Instantiate(currentBuildingData.buildingPrefab, new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0)), this.transform);
                selectedBuilding.transform.parent = this.transform;
                selectedBuilding.transform.position = new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]) - new Vector3(0, 0.3f, 0);
                selectedBuilding.name = currentBuildingData.buildingName;
                buildingController = selectedBuilding.GetComponent<BuildingController>();

                selectedBuilding.SetActive(false);
                if (buildingController != null)
                {
                    buildingController.buildingData = currentBuildingData;
                }

                string constructionName = constructionData.siteName[i];
                switch (constructionName)
                {
                    case "ConstructionSite_4x4(Clone)":
                        constructionSite = Instantiate(buildingPlacement.ConstructionSites[0], new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0))) as GameObject;
                        break;
                    case "ConstructionSite_5x5(Clone)":
                        constructionSite = Instantiate(buildingPlacement.ConstructionSites[1], new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0))) as GameObject;
                        break;
                    case "ConstructionSite_8x8(Clone)":
                        constructionSite = Instantiate(buildingPlacement.ConstructionSites[2], new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0))) as GameObject;
                        break;
                    case "ConstructionSite_8x3(Clone)":
                        constructionSite = Instantiate(buildingPlacement.ConstructionSites[3], new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0))) as GameObject;
                        break;
                    case "ConstructionSite_10x10(Clone)":
                        constructionSite = Instantiate(buildingPlacement.ConstructionSites[4], new Vector3(constructionData.posX[i], constructionData.posY[i], constructionData.posZ[i]), Quaternion.Euler(new Vector3(0, constructionData.rotY[i], 0))) as GameObject;
                        break;
                }

                constructionSite.GetComponentInChildren<ConstructionSite>().InitializeConstructionSite(currentBuildingData);
                constructionSite.GetComponentInChildren<ConstructionSite>().linkedBuilding = selectedBuilding;
                ConstructionSites.Add(constructionSite);
            }
        }
    }

    public void AddBuildingToList(GameObject building, BuildingData data)
    {
        Debug.Log("AddBuildingToList");
        BuildingList.Add(building);

        totalPowerConsumption += data.powerCons;
        totalPowerProduction += data.powerProd;
        totalWaterConsumption += data.waterCons;
        totalWaterProduction += data.waterProd;

        CheckUtilities(building);
    }

    public void RemoveBuildingFromList(GameObject building)
    {
        Debug.Log("RemoveBuildingFromList");
        BuildingList.Remove(building);
    }

    public void EnableBuilding(GameObject building)
    {
        Debug.Log("EnableBuilding_BaseManager");
        BuildingController buildingController = building.GetComponent<BuildingController>();

        if (buildingController.buildingEnabled == true)
        {
            Debug.Log("Building already enabled");
            return;
        }

        DisabledBuildings.Remove(building);
        BuildingList.Add(building);
        buildingController.hasPower = true;
        buildingController.hasWater = true;
        totalPowerConsumption += buildingController.buildingData.powerCons;
        totalWaterConsumption += buildingController.buildingData.waterCons;

        for (int i = 0; i < buildingController.doors.Length; i++)
        {
            buildingController.doors[i].enabled = true;
        }
        for (int i = 0; i < buildingController.rotatingDoors.Length; i++)
        {
            buildingController.rotatingDoors[i].enabled = true;
        }
        for (int i = 0; i < buildingController.lights.Length; i++)
        {
            buildingController.lights[i].enabled = true;
        }
        for (int i = 0; i < buildingController.emissionMaterial.Length; i++)
        {
            buildingController.emissionMaterial[i].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        }

        if (building.GetComponent<Kitchen>())
        {
            building.GetComponent<Kitchen>().PlaySound();
        }

        buildingController.buildingEnabled = true;
        CheckUtilities(building);
    }

    public void DisableBuilding()
    {
        BuildingController buildingController = null;

        for (int i = 0; i < BuildingList.Count; i++)
        {
            Debug.Log(i.ToString());
            if (BuildingList[i].GetComponent<BuildingController>())
            {
                if (!BuildingList[i].GetComponent<BuildingController>().buildingData.utilityBuilding)
                {
                    buildingController = BuildingList[i].GetComponent<BuildingController>();
                }
            }
        }

        if (buildingController.buildingEnabled == false)
        {
            Debug.Log("Building already disabled");
            return;
        }

        for (int i = 0; i < buildingController.doors.Length; i++)
        {
            buildingController.doors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.rotatingDoors.Length; i++)
        {
            buildingController.rotatingDoors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.lights.Length; i++)
        {
            buildingController.lights[i].enabled = false;
        }
        for (int i = 0; i < buildingController.emissionMaterial.Length; i++)
        {
            buildingController.emissionMaterial[i].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }

        buildingController.buildingEnabled = false;
        DisabledBuildings.Add(buildingController.gameObject);
        RemoveBuildingFromList(buildingController.gameObject);
        buildingController.hasPower = false;
        buildingController.hasWater = false;
        totalPowerConsumption -= buildingController.buildingData.powerCons;
        totalWaterConsumption -= buildingController.buildingData.waterCons;
    }

    public void DisableBuilding(GameObject building)
    {
        BuildingController buildingController = building.GetComponent<BuildingController>();

        if (buildingController.buildingEnabled == false)
        {
            Debug.Log("Building already disabled");
            return;
        }

        for (int i = 0; i < buildingController.doors.Length; i++)
        {
            buildingController.doors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.rotatingDoors.Length; i++)
        {
            buildingController.rotatingDoors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.lights.Length; i++)
        {
            buildingController.lights[i].enabled = false;
        }
        for (int i = 0; i < buildingController.emissionMaterial.Length; i++)
        {
            buildingController.emissionMaterial[i].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }

        buildingController.buildingEnabled = false;
        DisabledBuildings.Add(buildingController.gameObject);
        RemoveBuildingFromList(buildingController.gameObject);
        totalPowerConsumption -= buildingController.buildingData.powerCons;
        totalWaterConsumption -= buildingController.buildingData.waterCons;
        RecalculateOxygen();
        if (building.GetComponent<OxygenGenerator>())
        {
            building.GetComponent<OxygenGenerator>().StopSounds();
            DisableBuildingsWithNoOxygen();
        }
        if (building.GetComponent<Kitchen>())
        {
            building.GetComponent<Kitchen>().StopPlaying();
        }
    }

    public void DisableBuilding(bool producesWater)
    {
        BuildingController buildingController = null;

        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].GetComponent<BuildingController>())
            {
                if (BuildingList[i].GetComponent<BuildingController>().buildingData.waterCons > 0)
                {
                    buildingController = BuildingList[i].GetComponent<BuildingController>();
                }
            }
        }

        if (buildingController == null || buildingController.buildingEnabled == false)
        {
            Debug.Log("buildingController empty OR building already disabled");
            return;
        }

        for (int i = 0; i < buildingController.doors.Length; i++)
        {
            buildingController.doors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.rotatingDoors.Length; i++)
        {
            buildingController.rotatingDoors[i].enabled = false;
        }
        for (int i = 0; i < buildingController.lights.Length; i++)
        {
            buildingController.lights[i].enabled = false;
        }
        for (int i = 0; i < buildingController.emissionMaterial.Length; i++)
        {
            buildingController.emissionMaterial[i].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        }

        buildingController.buildingEnabled = false;
        DisabledBuildings.Add(buildingController.gameObject);
        RemoveBuildingFromList(buildingController.gameObject);
        buildingController.hasPower = true;
        buildingController.hasWater = false;
        totalPowerConsumption -= buildingController.buildingData.powerCons;
        totalWaterConsumption -= buildingController.buildingData.waterCons;
    }

    public void CheckUtilities()
    {
        if (totalPowerConsumption > totalPowerProduction || totalPowerProduction == 0)
        {
            DisableBuilding();
            Debug.Log("CheckUtilities powercons > powerprod");
        }
        else if (totalWaterConsumption > totalWaterProduction || totalWaterProduction == 0)
        {
            DisableBuilding(true);
            Debug.Log("CheckUtilities watercons > waterprod");
        }
    }

    public void CheckUtilities(GameObject building)
    {
        /*if (totalPowerConsumption > totalPowerProduction || totalPowerProduction == 0 && building.GetComponent<BuildingController>())
        {
            Debug.Log("CheckUtilities(building) powercons > powerprod");
            building.GetComponent<BuildingController>().hasPower = false;
            //building.GetComponent<BuildingController>().hasWater = false;
            DisableBuilding(building);
        }
        if (totalWaterConsumption > totalWaterProduction || totalWaterProduction == 0 && building.GetComponent<BuildingController>())
        {
            if (building.GetComponent<BuildingController>().buildingData.waterCons == 0)
            {
                Debug.Log("You may build!");
                //building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = true;
                building.GetComponent<BuildingController>().buildingEnabled = true;
                RecalculateOxygen();
                return;
            }

            Debug.Log("CheckUtilities(building) watercons > waterprod");
            //building.GetComponent<BuildingController>().hasPower = true;
            building.GetComponent<BuildingController>().hasWater = false;
            DisableBuilding(building);
        }
        else
        {
            if (building.GetComponent<BuildingController>())
            {
                Debug.Log("You may build! - else");
                building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = true;
                building.GetComponent<BuildingController>().buildingEnabled = true;
            }
            
            RecalculateOxygen();
        }*/

        if (totalPowerConsumption > totalPowerProduction || totalPowerProduction == 0 && building.GetComponent<BuildingController>())
        {
            Debug.Log("CheckUtilities(building) powercons > powerprod");
            building.GetComponent<BuildingController>().hasPower = false;
            //building.GetComponent<BuildingController>().hasWater = false;
            //DisableBuilding(building);
        }
        else if (building.GetComponent<BuildingController>())
        {
            building.GetComponent<BuildingController>().hasPower = true;
        }

        if (totalWaterConsumption > totalWaterProduction || totalWaterProduction == 0 && building.GetComponent<BuildingController>())
        {
            if (building.GetComponent<BuildingController>().buildingData.waterCons == 0)
            {
                Debug.Log("You may build!");
                //building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = true;
                //building.GetComponent<BuildingController>().buildingEnabled = true;
                //RecalculateOxygen();
                //return;
            }
            else
            {
                Debug.Log("CheckUtilities(building) watercons > waterprod");
                //building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = false;
                //DisableBuilding(building);
            }
        }
        else if (building.GetComponent<BuildingController>())
        {
            building.GetComponent<BuildingController>().hasWater = true;
        }

        RecalculateOxygen();

        if (building.GetComponent<BuildingController>())
        {
            if (building.GetComponent<BuildingController>().hasPower && building.GetComponent<BuildingController>().hasWater && building.GetComponent<BuildingController>().hasOxygen)
            {
                Debug.Log("You may build! - hasPower = true, hasWater = true, hasOxygen = true");
                building.GetComponent<BuildingController>().hasPower = true;
                building.GetComponent<BuildingController>().hasWater = true;
                building.GetComponent<BuildingController>().buildingEnabled = true;

                if (building.GetComponent<OxygenGenerator>())
                {
                    building.GetComponent<OxygenGenerator>().PlaySounds();
                }
                //RecalculateOxygen();
            }
            else
            {
                DisableBuilding(building);
            }
        }
    }

    public void DisableAllLights()
    {
        BuildingController buildingController = null;
        for (int i = 0; i < BuildingList.Count; i++)
        {
            buildingController = BuildingList[i].GetComponent<BuildingController>();
            if (buildingController != null)
            {
                for (int j = 0; j < buildingController.lights.Length; j++)
                {
                    buildingController.lights[j].enabled = false;
                }
                for (int j = 0; j < buildingController.emissionMaterial.Length; j++)
                {
                    buildingController.emissionMaterial[j].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                }
            }
        }
    }

    public void EnableAllLights()
    {
        BuildingController buildingController = null;
        for (int i = 0; i < BuildingList.Count; i++)
        {
            buildingController = BuildingList[i].GetComponent<BuildingController>();
            if (buildingController != null)
            {
                for (int j = 0; j < buildingController.lights.Length; j++)
                {
                    buildingController.lights[j].enabled = true;
                }
                for (int j = 0; j < buildingController.emissionMaterial.Length; j++)
                {
                    buildingController.emissionMaterial[j].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                }
            }
        }
    }

    public void RecalculateOxygen()
    {
        Debug.Log("Recalculating oxygen");
        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].GetComponent<BuildingController>())
            {
                BuildingList[i].GetComponent<BuildingController>().hasOxygen = false;
            }
        }
        for (int i = 0; i < DisabledBuildings.Count; i++)
        {
            if (DisabledBuildings[i].GetComponent<BuildingController>())
            {
                DisabledBuildings[i].GetComponent<BuildingController>().hasOxygen = false;
            }
        }
        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].GetComponent<OxygenGenerator>())
            {
                BuildingList[i].GetComponent<OxygenGenerator>().SpreadOxygen();
            }
        }
    }

    public void DisableBuildingsWithNoOxygen()
    {
        List<BuildingController> buildingControllers = new List<BuildingController>();
        for (int i = 0; i < BaseManager.Instance.BuildingList.Count; i++)
        {
            if (BaseManager.Instance.BuildingList[i].GetComponent<BuildingController>())
            {
                buildingControllers.Add(BaseManager.Instance.BuildingList[i].GetComponent<BuildingController>());
            }
        }

        for (int i = 0; i < buildingControllers.Count; i++)
        {
            if (!buildingControllers[i].hasOxygen)
            {
                DisableBuilding(buildingControllers[i].gameObject);
            }
        }
    }
}
