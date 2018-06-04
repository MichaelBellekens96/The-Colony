using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

    public OpenDoor[] doors;
    public OpenTurningDoor[] rotatingDoors;
    public Light[] lights;

    public GameObject[] emissionMaterial;

    public BuildingData buildingData;

    public bool hasOxygen = false;
    public bool hasWater = false;
    public bool hasPower = false;
    public bool buildingEnabled = true;
}
