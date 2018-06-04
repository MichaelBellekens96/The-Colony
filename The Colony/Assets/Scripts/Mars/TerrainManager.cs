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

            rndMountain = Random.Range(0, 15);
            mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
            mountain.transform.localScale *= 2f;
            //Destroy(mountain.GetComponent<Collider>());
            //mountain.AddComponent<BoxCollider>();

            for (int j = 0; j < gridHeight; j++)
            {
                rndIndex = Random.Range(0, 4);
                rndRotation = Random.Range(0, 4);
                spawnPosition.z -= 50;
                currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;

                if (i == 0 || i == gridWidth -1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(70, 110))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                    //Destroy(mountain.GetComponent<Collider>());
                    //mountain.AddComponent<BoxCollider>();
                }
                else if (j == gridHeight -1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                    //Destroy(mountain.GetComponent<Collider>());
                    //mountain.AddComponent<BoxCollider>();
                }
                else if (Random.Range(0, 100) < mountainFrequency)
                {
                    rndMountain = Random.Range(0, 15);
                    Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 7.3f, 0), Quaternion.Euler(new Vector3(0, Random.Range(0, 359))), marsTransform);
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

            rndMountain = Random.Range(0, 15);
            mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
            mountain.transform.localScale *= 2f;
            //Destroy(mountain.GetComponent<Collider>());
            //mountain.AddComponent<BoxCollider>();

            for (int j = 0; j < gridHeight; j++)
            {
                rndIndex = Random.Range(0, 4);
                rndRotation = Random.Range(0, 4);
                spawnPosition.z -= 50;
                currentTerrainTile = Instantiate(TerrainTilles[rndIndex], spawnPosition, Quaternion.Euler(rotations[rndRotation]), marsTransform) as GameObject;

                if (i == 0 || i == gridWidth - 1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(70, 110))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                    //Destroy(mountain.GetComponent<Collider>());
                    //mountain.AddComponent<BoxCollider>();
                }
                else if (j == gridHeight - 1)
                {
                    currentTerrainTile.AddComponent<BoxCollider>().size = new Vector3(50, 32, 50);

                    rndMountain = Random.Range(0, 15);
                    mountain = Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 14f, 0), Quaternion.Euler(new Vector3(0, Random.Range(-20, 20))), marsTransform) as GameObject;
                    mountain.transform.localScale *= 2f;
                    //Destroy(mountain.GetComponent<Collider>());
                    //mountain.AddComponent<BoxCollider>();
                }
                else if (Random.Range(0, 100) < mountainFrequency)
                {
                    rndMountain = Random.Range(0, 15);
                    Instantiate(MountainTypes[rndMountain], spawnPosition + new Vector3(0, 7.3f, 0), Quaternion.Euler(new Vector3(0, Random.Range(0, 359))), marsTransform);
                }
            }

            spawnPosition.x += 50;
            spawnPosition.z = startPositionHeight;
        }
    }
}
