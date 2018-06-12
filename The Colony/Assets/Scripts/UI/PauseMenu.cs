using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public void LoadPreviousGame()
    {
        AudioManager.Instance.Play("Btn_Press");
        MainUIManager.Instance.LoadPreviousSave();
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("Btn_Press");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
