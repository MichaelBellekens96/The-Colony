using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public BuildUIManager buildUIManager;
    public BuildingData data;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        buildUIManager.LoadBuildingData(data);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        buildUIManager.ResetBuildingData();
    }
}
