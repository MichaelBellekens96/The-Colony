using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour {

    public GameObject[] allPlants;
    public float growRate;
    public int resourceBoxSpawns;
    public int scalePlant;

    public bool plantsFullyGrown;
    public bool mayHarvest;
    public bool plantsGrowing;
    private Vector3 growFactor;
    private float currentScale;

    public PlayerController playerController;

    private void Start()
    {
        for (int i = 0; i < allPlants.Length; i++)
        {
            allPlants[i].transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GrowPlants();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            HarvestPlants(playerController.transform.position, playerController.transform.rotation);
        }
    }

    public void GrowPlants()
    {
        plantsFullyGrown = false;
        currentScale = 0;
        plantsGrowing = true;
        growFactor = new Vector3(growRate, growRate, growRate);
        StartCoroutine(GrowingPlants());
    }

    public void HarvestPlants(Vector3 pos, Quaternion rot)
    {
        plantsFullyGrown = false;
        for (int i = 0; i < allPlants.Length; i++)
        {
            allPlants[i].transform.localScale = Vector3.zero;
        }
        if (resourceBoxSpawns == 1)
        {
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos + Vector3.right, rot);
        }
        else if (resourceBoxSpawns == 2)
        {
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos + Vector3.right, rot);
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos - Vector3.right, rot);
        }
        else if (resourceBoxSpawns == 3)
        {
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos + Vector3.right, rot);
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos - Vector3.right, rot);
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, pos - Vector3.forward, rot);
        }
        mayHarvest = false;
    }

    private IEnumerator GrowingPlants()
    {
        for (int i = 0; i < allPlants.Length; i++)
        {
            allPlants[i].transform.localScale = Vector3.zero;
        }

        while (!plantsFullyGrown)
        {
            for (int i = 0; i < allPlants.Length; i++)
            {
                allPlants[i].transform.localScale += growFactor * Time.deltaTime;
                currentScale = allPlants[i].transform.localScale.y;
            }
            if (currentScale >= scalePlant)
            {
                plantsFullyGrown = true;
            }
            else
            {
                yield return null;
            }
        }
        plantsGrowing = false;
        mayHarvest = true;
    }
}
