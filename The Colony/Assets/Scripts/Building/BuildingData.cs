using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingData : ScriptableObject {

    public string buildingName;
    public GameObject buildingPrefab;
    public BuildingTypes type;
    public bool utilityBuilding;

    public Sprite buildingSprite;
    public string description;

    public int metal;
    public int bioPlastic;

    [Header("Consumption")]
    public int powerCons;
    public int waterCons;


    [Header("Production")]
    public int powerProd;
    public int waterProd;
}
