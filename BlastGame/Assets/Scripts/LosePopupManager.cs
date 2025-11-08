using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePopupManager : MonoBehaviour
{
    public void OnCloseButtonClicked()
    {
        // Close the popup
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void OnReplayButtonClicked()
    {
        // Close the popup
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
    }
}
