using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private Button levelButton;


    int currentLevel;
    
    void Awake()
    {
         if (!PlayerPrefs.HasKey("levelState"))
        {
            PlayerPrefs.SetInt("levelState", 1);
            PlayerPrefs.Save();
        }

    }

    // Start is called before the first frame update
    void Start() {
        currentLevel = PlayerPrefs.GetInt("levelState");
        Debug.Log("Maiin Current level: " + currentLevel);
        UpdateLevelButtonText();
        levelButton.onClick.AddListener(OnLevelButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateLevelButtonText()
    {
        if (currentLevel > 10)
        {
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finished";  // Display "Finished" when all levels are completed.
        }
        else
        {
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + currentLevel.ToString();  // Display the current level.
        }
    }

     // Handles what happens when the level button is clicked.
    public void OnLevelButtonClicked()
    {
        if (currentLevel <= 10)
        {
            // Load the LevelScene with the current level.
            SceneManager.LoadScene("LevelScene");
        }
        else
        {
            // Optionally, show a message or play some animation indicating that the game is completed.
            Debug.Log("All levels completed!");
        }
    }
}
