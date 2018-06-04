using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airlock : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        if (triggerObject.tag == "Player")
        {
            if (triggerObject.GetComponent<PlayerController>().insideBase)
            {
                triggerObject.GetComponent<PlayerStats>().oxygenRate = triggerObject.GetComponent<PlayerTasks>().tempOxygenRate;
                triggerObject.GetComponent<PlayerStats>().Oxygen = 0;
                triggerObject.GetComponent<PlayerController>().insideBase = false;
                MainUIManager.Instance.UpdateStatsPanel();
            }
        }
    }
}
