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
        if (totalPowerConsumption > totalPowerProduction || totalPowerProduction == 0 && building.GetComponent<BuildingController>())
        {
            Debug.Log("CheckUtilities(building) powercons > powerprod");
            building.GetComponent<BuildingController>().hasPower = false;
            building.GetComponent<BuildingController>().hasWater = false;
            DisableBuilding(building);
        }
        else if (totalWaterConsumption > totalWaterProduction || totalWaterProduction == 0 && building.GetComponent<BuildingController>())
        {
            if (building.GetComponent<BuildingController>())
            {
                if (building.GetComponent<BuildingController>().buildingData.waterCons == 0)
                {
                    Debug.Log("You may build!");
                    building.GetComponent<BuildingController>().hasPower = true;
                    building.GetComponent<BuildingController>().hasWater = true;
                    building.GetComponent<BuildingController>().buildingEnabled = true;
                    RecalculateOxygen();
                    return;
                }
            }
            Debug.Log("CheckUtilities(building) watercons > waterprod");
            building.GetComponent<BuildingController>().hasPower = true;
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
        for (int i = 0; i < BuildingList.Count; i++)
        {
            if (BuildingList[i].GetComponent<OxygenGenerator>())
            {
                BuildingList[i].GetComponent<OxygenGenerator>().SpreadOxygen();
            }
        }
    }
}
