using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public List<GameObject> TerrainTilles = new List<GameObject>();
    public List<GameObject> MountainTypes = new List<GameObject>();
    private GameObject mountain;
    private GameObject currentTerrainTile;
    public Vector3[] rotations = new Vector3[4];

    public int gridWidth;
    public int gridHeight;
    public float mountainFrequency;
    public Transform marsTransform;

    
    public List<GameObject> spawnedMountains = new List<GameObject>();
    public List<int> spawnedMountainType = new List<int>();

    public List<GameObject> spawnedTerrainTilles = new List<GameObject>();
    public List<int> spawnedTerrainType = new List<int>();
    public List<int> spawnedTerrainRot = new List<int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Load();
        }
    }

    private void Awake()
    {
        int rndIndex;
        int rndRotation;
        int rndMountain;
        float startPositionWidth = -(gridWidth * 50 - 50) / 2;
        float startPositionHeight = (gridHeight * 50 - 50) / 2;
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = startPositionWidth;
        spawnPosition.z = startPositionHeight;

        for (int i = 0; i < gridWidth; i++)
        {
            rndIndex = Random.Range(0, 4);
            rndRotation = Random.Range(0, 4);
            currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;
            currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

            spawnedTerrainTilles.Add(currentTerrainTile);
            spawnedTerrainType.Add(rndIndex);
            spawnedTerrainRot.Add(rndRotation);

            rndMountain = Random.Range(0, 15);
            mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
            mountain.transform.localScale *= 2f;

            for (int j = 0; j < gridHeight; j++)
            {
                rndIndex = Random.Range(0, 4);
                rndRotation = Random.Range(0, 4);
                spawnPosition.z -= 50;
                currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;

                spawnedTerrainTilles.Add(currentTerrainTile);
                spawnedTerrainType.Add(rndIndex);
                spawnedTerrainRot.Add(rndRotation);

                if (i == 0 || i == gridWidth -1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(70, 110))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                }
                else if (j == gridHeight -1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                }
                else if (Random.Range(0, 100) < mountainFrequency)
                {
                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 7.3f, 0), Quaternion.Euler(new Vector3(0, Random.Range(0, 359))), marsTransform);
                    spawnedMountains.Add(mountain);
                    spawnedMountainType.Add(rndMountain);
                }
            }

            spawnPosition.x += 50;
            spawnPosition.z = startPositionHeight;
        }
    }

    public void GenerateNewTerrain()
    {
        Transform[] allChildren = marsTransform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i] != marsTransform)
            {
                Destroy(allChildren[i].gameObject);
            }
        }

        spawnedTerrainTilles.Clear();
        spawnedTerrainType.Clear();
        spawnedTerrainRot.Clear();

        spawnedMountains.Clear();
        spawnedMountainType.Clear();

        int rndIndex;
        int rndRotation;
        int rndMountain;
        float startPositionWidth = -(gridWidth * 50 - 50) / 2;
        float startPositionHeight = (gridHeight * 50 - 50) / 2;
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = startPositionWidth;
        spawnPosition.z = startPositionHeight;

        for (int i = 0; i < gridWidth; i++)
        {
            rndIndex = Random.Range(0, 4);
            rndRotation = Random.Range(0, 4);
            currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;
            currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

            spawnedTerrainTilles.Add(currentTerrainTile);
            spawnedTerrainType.Add(rndIndex);
            spawnedTerrainRot.Add(rndRotation);

            rndMountain = Random.Range(0, 15);
            mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
            mountain.transform.localScale *= 2f;

            for (int j = 0; j < gridHeight; j++)
            {
                rndIndex = Random.Range(0, 4);
                rndRotation = Random.Range(0, 4);
                spawnPosition.z -= 50;
                currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;

                spawnedTerrainTilles.Add(currentTerrainTile);
                spawnedTerrainType.Add(rndIndex);
                spawnedTerrainRot.Add(rndRotation);

                if (i == 0 || i == gridWidth - 1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(70, 110))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                }
                else if (j == gridHeight - 1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                }
                else if (Random.Range(0, 100) < mountainFrequency)
                {
                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 7.3f, 0), Quaternion.Euler(new Vector3(0, Random.Range(0, 359))), marsTransform);

                    spawnedMountains.Add(mountain);
                    spawnedMountainType.Add(rndMountain);
                }
            }

            spawnPosition.x += 50;
            spawnPosition.z = startPositionHeight;
        }
    }

    public void Save()
    {
        Debug.Log("Saving terrain...");
        SaveLoadManager.SaveTerrain(this);
    }

    public void Load()
    {
        Debug.Log("Loading terrain...");
        TerrainData data = SaveLoadManager.LoadTerrain();
        if (data != null)
        {
            Transform[] allChildren = marsTransform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allChildren.Length; i++)
            {
                if (allChildren[i] != marsTransform)
                {
                    Destroy(allChildren[i].gameObject);
                }
            }

            spawnedTerrainTilles.Clear();
            spawnedTerrainType.Clear();
            spawnedTerrainRot.Clear();

            spawnedMountains.Clear();
            spawnedMountainType.Clear();

            int rndMountain;
            float startPositionWidth = -(data.gridWidth * 50 - 50) / 2;
            float startPositionHeight = (data.gridHeight * 50 - 50) / 2;
            Vector3 spawnPosition = Vector3.zero;
            spawnPosition.x = startPositionWidth;
            spawnPosition.z = startPositionHeight;

            for (int i = 0; i < gridWidth; i++)
            {
                rndMountain = Random.Range(0, 15);
                mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                mountain.transform.localScale *= 2f;

                for (int j = 0; j < gridHeight; j++)
                {
                    spawnPosition.z -= 50;
                    if (i == 0 || i == gridWidth - 1)
                    { 
                        rndMountain = Random.Range(0, 15);
                        mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(70, 110))), marsTransform) as GameObject;
                        mountain.transform.localScale *= 2f;
                    }
                    else if (j == gridHeight - 1)
                    {
                        rndMountain = Random.Range(0, 15);
                        mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                        mountain.transform.localScale *= 2f;
                    }
                }

                spawnPosition.x += 50;
                spawnPosition.z = startPositionHeight;
            }

            for (int i = 0; i < data.numTerrainTilles; i++)
            {
                GameObject terrainTille = Instantiate(TerrainTilles[data.terrainTypes[i]], new Vector3(data.terrainPosX[i], data.terrainPosY[i], data.terrainPosZ[i]), Quaternion.Euler(rotations[data.terrainRotIndex[i]]), marsTransform);
                spawnedTerrainTilles.Add(terrainTille);
                spawnedTerrainRot.Add(data.terrainRotIndex[i]);
                spawnedTerrainType.Add(data.terrainRotIndex[i]);
            }

            for (int i = 0; i < data.numMountains; i++)
            {
                GameObject mountainObject = Instantiate(MountainTypes[data.mountainType[i]], new Vector3(data.mountainPosX[i], data.mountainPosY[i], data.mountainPosZ[i]), Quaternion.Euler(new Vector3(0, data.mountainRotY[i], 0)), marsTransform);
                spawnedMountains.Add(mountainObject);
                spawnedMountainType.Add(data.mountainType[i]);
            }
        }
    }
}
