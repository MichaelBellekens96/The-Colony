using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airlock : MonoBehaviour {

    GameObject triggerObject;

    private void OnTriggerExit(Collider other)
    {
        triggerObject = other.gameObject;
        if (triggerObject.tag == "Player")
        {
            if (triggerObject.GetComponent<PlayerController>().insideBase)
            {
                StartCoroutine(CheckPlayetStillInBase());
            }
        }
    }

    private IEnumerator CheckPlayetStillInBase()
    {
        yield return new WaitForSeconds(0.3f);
        if (!triggerObject.GetComponent<PlayerController>().CheckStillInBase())
        {
            triggerObject.GetComponent<PlayerStats>().oxygenRate = triggerObject.GetComponent<PlayerTasks>().tempOxygenRate;
            triggerObject.GetComponent<PlayerStats>().Oxygen = 0;
            triggerObject.GetComponent<PlayerController>().insideBase = false;
            MainUIManager.Instance.UpdateStatsPanel();
        }

    }
}
