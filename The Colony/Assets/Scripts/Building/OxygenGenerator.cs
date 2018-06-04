using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenGenerator : MonoBehaviour {

    public float oxygenRadius;
    public BuildingController buildingController;

    public List<BuildingController> surroundingBuildings = new List<BuildingController>();
    private int baseLength;
    private List<GameObject> allBuildings;

	// Use this for initialization
	void Start () {
        buildingController = GetComponent<BuildingController>();
        if (buildingController.buildingEnabled)
        {
            allBuildings = BaseManager.Instance.BuildingList;
            baseLength = BaseManager.Instance.BuildingList.Count;
            for (int i = 0; i < baseLength; i++)
            {
                if (allBuildings[i].GetComponent<BuildingController>() && Vector3.Distance(transform.position, allBuildings[i].transform.position) < oxygenRadius)
                {
                    surroundingBuildings.Add(allBuildings[i].GetComponent<BuildingController>());
                }
            }
            for (int i = 0; i < surroundingBuildings.Count; i++)
            {
                surroundingBuildings[i].hasOxygen = true;
            }
        }
	}

    public void SpreadOxygen()
    {
        buildingController = GetComponent<BuildingController>();
        if (buildingController.buildingEnabled)
        {
            allBuildings = BaseManager.Instance.BuildingList;
            baseLength = BaseManager.Instance.BuildingList.Count;
            for (int i = 0; i < baseLength; i++)
            {
                if (allBuildings[i].GetComponent<BuildingController>() && Vector3.Distance(transform.position, allBuildings[i].transform.position) < oxygenRadius)
                {
                    surroundingBuildings.Add(allBuildings[i].GetComponent<BuildingController>());
                }
            }
            for (int i = 0; i < surroundingBuildings.Count; i++)
            {
                surroundingBuildings[i].hasOxygen = true;
            }
        }
    }
}