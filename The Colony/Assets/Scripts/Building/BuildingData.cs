using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingData : ScriptableObject {

    public string buildingName;
    public GameObject buildingPrefab;
    public BuildingTypes type;

    public Sprite buildingSprite;
    public string description;

    public int metal;
    public int bioPlastic;
}
