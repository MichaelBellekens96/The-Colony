using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager {

	public static void SavePlayer(PlayerStats player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/player.cly", FileMode.Create);

        PlayerData data = new PlayerData(player);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/player.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/player.cly", FileMode.Open);

            PlayerData data = bf.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Player savefile doesn't exist.");
            return null;
        }
    }

    public static void SaveTerrain(TerrainManager terrain)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/terrain.cly", FileMode.Create);

        TerrainData data = new TerrainData(terrain);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static TerrainData LoadTerrain()
    {
        if (File.Exists(Application.persistentDataPath + "/terrain.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/terrain.cly", FileMode.Open);

            TerrainData data = bf.Deserialize(stream) as TerrainData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Terrain save file doesn't exist.");
            return null;
        }
    }

    public static void SaveSunMoon(SunMoonRotation sky)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/sky.cly", FileMode.Create);

        SunMoonData data = new SunMoonData(sky);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static SunMoonData LoadSunMoon()
    {
        if (File.Exists(Application.persistentDataPath + "/sky.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/sky.cly", FileMode.Open);

            SunMoonData data = bf.Deserialize(stream) as SunMoonData;
            stream.Close();

            return data;
        }
        else
	    {
            Debug.LogWarning("Sky save doesn't exist.");
            return null;
        }
    }

    public static void SaveResources(ResourceManager resources)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/resources.cly", FileMode.Create);

        ResourceBoxData data = new ResourceBoxData(resources);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static ResourceBoxData LoadResources()
    {
        if (File.Exists(Application.persistentDataPath + "/resources.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/resources.cly", FileMode.Open);

            ResourceBoxData data = bf.Deserialize(stream) as ResourceBoxData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Resources save doesn't exist.");
            return null;
        }
    }

    public static void SaveBase(BaseManager marsBase)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Base.cly", FileMode.Create);

        BaseData data = new BaseData(marsBase);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static BaseData LoadBase()
    {
        if (File.Exists(Application.persistentDataPath + "/Base.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Base.cly", FileMode.Open);

            BaseData data = bf.Deserialize(stream) as BaseData;

            stream.Close();
            return data;
        }
        else
	    {
            Debug.LogWarning("Basedata doesn't exist.");
            return null;
        }
    }

    public static void SaveConstructionSite(BaseManager constructionSite)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/constructionSites.cly", FileMode.Create);

        ConstructionData data = new ConstructionData(constructionSite);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static ConstructionData LoadConstructionSite()
    {
        if (File.Exists(Application.persistentDataPath + "/constructionSites.cly"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/constructionSites.cly", FileMode.Open);

            ConstructionData data = bf.Deserialize(stream) as ConstructionData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Player savefile doesn't exist.");
            return null;
        }
    }
}

[Serializable]
public class PlayerData
{
    public float health;
    public float hunger;
    public float oxygen;
    public float sleep;

    public float hungerRate;
    public float oxygenRate;
    public float sleepRate;

    public bool inBase;

    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;

    public PlayerData(PlayerStats playerStats)
    {
        health = playerStats.Health;
        hunger = playerStats.Hunger;
        oxygen = playerStats.Oxygen;
        sleep = playerStats.Sleep;

        hungerRate = playerStats.hungerRate;
        oxygenRate = playerStats.oxygenRate;
        sleepRate = playerStats.sleepRate;

        inBase = playerStats.controller.insideBase;

        posX = playerStats.gameObject.transform.position.x;
        posY = playerStats.gameObject.transform.position.y;
        posZ = playerStats.gameObject.transform.position.z;

        rotX = playerStats.gameObject.transform.rotation.eulerAngles.x;
        rotY = playerStats.gameObject.transform.rotation.eulerAngles.y;
        rotZ = playerStats.gameObject.transform.rotation.eulerAngles.z;
    }
}

[Serializable]
public class TerrainData
{
    public int currentIndex;

    public int gridWidth;
    public int gridHeight;
    public int numTerrainTilles;
    public int numMountains;

    public int[] terrainTypes;
    public float[] terrainPosX;
    public float[] terrainPosY;
    public float[] terrainPosZ;
    public int[] terrainRotIndex;

    public int[] mountainType;
    public float[] mountainPosX;
    public float[] mountainPosY;
    public float[] mountainPosZ;
    public float[] mountainRotY;

    public TerrainData(TerrainManager terrain)
    {
        gridWidth = terrain.gridWidth;
        gridHeight = terrain.gridHeight;
        numTerrainTilles = terrain.spawnedTerrainTilles.Count;
        numMountains = terrain.spawnedMountains.Count;

        terrainTypes = new int[numTerrainTilles];
        terrainPosX = new float[numTerrainTilles];
        terrainPosY = new float[numTerrainTilles];
        terrainPosZ = new float[numTerrainTilles];
        terrainRotIndex = new int[numTerrainTilles];

        mountainType = new int[numMountains];
        mountainPosX = new float[numMountains];
        mountainPosY = new float[numMountains];
        mountainPosZ = new float[numMountains];
        mountainRotY = new float[numMountains];

        for (int i = 0; i < numMountains; i++)
        {
            mountainType[i] = terrain.spawnedMountainType[i];

            mountainPosX[i] = terrain.spawnedMountains[i].transform.position.x;
            mountainPosY[i] = terrain.spawnedMountains[i].transform.position.y;
            mountainPosZ[i] = terrain.spawnedMountains[i].transform.position.z;

            mountainRotY[i] = terrain.spawnedMountains[i].transform.eulerAngles.y;
        }

        for (int i = 0; i < numTerrainTilles; i++)
        {
            terrainTypes[i] = terrain.spawnedTerrainType[i];
            terrainRotIndex[i] = terrain.spawnedTerrainRot[i];

            terrainPosX[i] = terrain.spawnedTerrainTilles[i].transform.position.x;
            terrainPosY[i] = terrain.spawnedTerrainTilles[i].transform.position.y;
            terrainPosZ[i] = terrain.spawnedTerrainTilles[i].transform.position.z;
        }
    }
}

[Serializable]
public class SunMoonData
{
    public float sunPosY;
    public float sunPosZ;
    public float moonPosY;
    public float moonPosZ;

    public SunMoonData(SunMoonRotation data)
    {
        sunPosY = data.Sun.transform.position.y;
        sunPosZ = data.Sun.transform.position.z;
        moonPosY = data.Moon.transform.position.y;
        moonPosZ = data.Moon.transform.position.z;
    }
}

[Serializable]
public class ResourceBoxData
{
    public int numMetal;
    public int numMetalOre;
    public int numBioplastic;
    public int numRawFood;
    public int numMeals;

    public int numStoredMetal;
    public int numStoredMetalOre;
    public int numStoredBioplastic;
    public int numStoredRawFood;

    public float[] metalPosX;
    public float[] metalPosY;
    public float[] metalPosZ;

    public float[] metalOrePosX;
    public float[] metalOrePosY;
    public float[] metalOrePosZ;

    public float[] BioplasticPosX;
    public float[] BioplasticPosY;
    public float[] BioplasticPosZ;

    public float[] rawFoodPosX;
    public float[] rawFoodPosY;
    public float[] rawFoodPosZ;

    public ResourceBoxData(ResourceManager data)
    {
        numMetal = data.numMetalBoxes;
        numMetalOre = data.numMetalOreBoxes;
        numBioplastic = data.numBioplasticBoxes;
        numRawFood = data.numRawFoodBoxes;
        numMeals = data.numMeals;

        numStoredMetal = data.numStoredMetalBoxes;
        numStoredMetalOre = data.numStoredMetalOreBoxes;
        numStoredBioplastic = data.numStoredBioplasticBoxes;
        numStoredRawFood = data.numStoredRawFoodBoxes;

        metalPosX = new float[data.MetalBox.Count];
        metalPosY = new float[data.MetalBox.Count];
        metalPosZ = new float[data.MetalBox.Count];

        metalOrePosX = new float[data.MetalOreBox.Count];
        metalOrePosY = new float[data.MetalOreBox.Count];
        metalOrePosZ = new float[data.MetalOreBox.Count];

        BioplasticPosX = new float[data.BioPlasticBox.Count];
        BioplasticPosY = new float[data.BioPlasticBox.Count];
        BioplasticPosZ = new float[data.BioPlasticBox.Count];

        rawFoodPosX = new float[data.RawFoodBox.Count];
        rawFoodPosY = new float[data.RawFoodBox.Count];
        rawFoodPosZ = new float[data.RawFoodBox.Count];

        for (int i = 0; i < data.MetalBox.Count; i++)
        {
            metalPosX[i] = data.MetalBox[i].position.x;
            metalPosY[i] = data.MetalBox[i].position.y;
            metalPosZ[i] = data.MetalBox[i].position.z;
        }
        for (int i = 0; i < data.MetalOreBox.Count; i++)
        {
            metalOrePosX[i] = data.MetalOreBox[i].position.x;
            metalOrePosY[i] = data.MetalOreBox[i].position.y;
            metalOrePosZ[i] = data.MetalOreBox[i].position.z;
        }
        for (int i = 0; i < data.BioPlasticBox.Count; i++)
        {
            BioplasticPosX[i] = data.BioPlasticBox[i].position.x;
            BioplasticPosY[i] = data.BioPlasticBox[i].position.y;
            BioplasticPosZ[i] = data.BioPlasticBox[i].position.z;
        }
        for (int i = 0; i < data.RawFoodBox.Count; i++)
        {
            rawFoodPosX[i] = data.RawFoodBox[i].position.x;
            rawFoodPosY[i] = data.RawFoodBox[i].position.y;
            rawFoodPosZ[i] = data.RawFoodBox[i].position.z;
        }
    }
}

[Serializable]
public class BaseData
{
    public int powerProd;
    public int powerCons;
    public int waterProd;
    public int waterCons;

    public int numBuildings;
    public int numDisabledBuildings;
    public int numUtilities;

    public float[] buildingPosX;
    public float[] buildingPosY;
    public float[] buildingPosZ;
    public float[] buildingRotY;
    public string[] buildingName;

    public float[] disBuildingPosX;
    public float[] disBuildingPosY;
    public float[] disBuildingPosZ;
    public float[] disBuildingRotY;
    public string[] disBuildingName;
    public bool[] disBuildingHasPower;
    public bool[] disBuildingHasWater;

    public float[] utilityPosX;
    public float[] utilityPosY;
    public float[] utilityPosZ;
    public float[] utilityRotY;
    public string[] utilityName;

    private int currentBuilding;
    private int currentUtility;

    public BaseData(BaseManager data)
    {
        powerProd = data.totalPowerProduction;
        powerCons = data.totalPowerConsumption;
        waterProd = data.totalWaterProduction;
        waterCons = data.totalWaterConsumption;

        numBuildings = data.buildings;
        numDisabledBuildings = data.disabledBuildings;
        numUtilities = data.utility;

        buildingPosX = new float[numBuildings];
        buildingPosY = new float[numBuildings];
        buildingPosZ = new float[numBuildings];
        buildingRotY = new float[numBuildings];
        buildingName = new string[numBuildings];

        disBuildingPosX = new float[numDisabledBuildings];
        disBuildingPosY = new float[numDisabledBuildings];
        disBuildingPosZ = new float[numDisabledBuildings];
        disBuildingRotY = new float[numDisabledBuildings];
        disBuildingName = new string[numDisabledBuildings];
        disBuildingHasPower = new bool[numDisabledBuildings];
        disBuildingHasWater = new bool[numDisabledBuildings];

        utilityPosX = new float[numUtilities];
        utilityPosY = new float[numUtilities];
        utilityPosZ = new float[numUtilities];
        utilityRotY = new float[numUtilities];
        utilityName = new string[numUtilities];

        currentBuilding = 0;
        currentUtility = 0;

        for (int i = 0; i < data.BuildingList.Count; i++)
        {
            if (data.BuildingList[i].GetComponent<BuildingController>())
            {
                Debug.Log("BuildingController found!");
                buildingName[currentBuilding] = data.BuildingList[i].GetComponent<BuildingController>().buildingData.buildingName;
                buildingPosX[currentBuilding] = data.BuildingList[i].transform.position.x;
                buildingPosY[currentBuilding] = data.BuildingList[i].transform.position.y;
                buildingPosZ[currentBuilding] = data.BuildingList[i].transform.position.z;
                buildingRotY[currentBuilding] = data.BuildingList[i].transform.rotation.eulerAngles.y;
                currentBuilding++;
            }
            else
            {
                Debug.Log("NO BuildingController found!");
                utilityName[currentUtility] = data.BuildingList[i].name;
                utilityPosX[currentUtility] = data.BuildingList[i].transform.position.x;
                utilityPosY[currentUtility] = data.BuildingList[i].transform.position.y;
                utilityPosZ[currentUtility] = data.BuildingList[i].transform.position.z;
                utilityRotY[currentUtility] = data.BuildingList[i].transform.rotation.eulerAngles.y;
                currentUtility++;
            }
        }

        for (int i = 0; i < data.DisabledBuildings.Count; i++)
        {
            disBuildingName[i] = data.DisabledBuildings[i].GetComponent<BuildingController>().buildingData.buildingName;
            disBuildingPosX[i] = data.DisabledBuildings[i].transform.position.x;
            disBuildingPosY[i] = data.DisabledBuildings[i].transform.position.y;
            disBuildingPosZ[i] = data.DisabledBuildings[i].transform.position.z;
            disBuildingRotY[i] = data.DisabledBuildings[i].transform.rotation.eulerAngles.y;
            disBuildingHasPower[i] = data.DisabledBuildings[i].GetComponent<BuildingController>().hasPower;
            disBuildingHasWater[i] = data.DisabledBuildings[i].GetComponent<BuildingController>().hasWater;
        }
    }
}

[Serializable]
public class ConstructionData
{
    public float[] posX;
    public float[] posY;
    public float[] posZ;
    public float[] rotY;
    public string[] buildingName;
    public string[] siteName;

    public ConstructionData(BaseManager data)
    {
        posX = new float[data.ConstructionSites.Count];
        posY = new float[data.ConstructionSites.Count];
        posZ = new float[data.ConstructionSites.Count];
        rotY = new float[data.ConstructionSites.Count];
        buildingName = new string[data.ConstructionSites.Count];
        siteName = new string[data.ConstructionSites.Count];

        for (int i = 0; i < data.ConstructionSites.Count; i++)
        {
            posX[i] = data.ConstructionSites[i].transform.position.x;
            posY[i] = data.ConstructionSites[i].transform.position.y;
            posZ[i] = data.ConstructionSites[i].transform.position.z;
            rotY[i] = data.ConstructionSites[i].transform.rotation.eulerAngles.y;

            buildingName[i] = data.ConstructionSites[i].GetComponentInChildren<ConstructionSite>().buildingName;
            siteName[i] = data.ConstructionSites[i].name;
        }
    }
}
