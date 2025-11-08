
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneManager : MonoBehaviour
{
    public static LevelSceneManager Instance { get; private set; }  // Singleton instance

    [Header("Level Information")]
    private int currentLevel;                       // The current level number.
    private LevelData currentLevelData;             // The data for the current level.                       

    [SerializeField] private string levelDataPath = "Levels/level_";

    [Header("UI Elements")]
    [SerializeField] private GameObject winPopup;                     // UI panel for when the player wins.
    private ParticleSystem winParticles;
    [SerializeField] private GameObject losePopup;                    // UI panel for when the player loses.
    //[SerializeField] private TMPro.TextMeshProUGUI moveCounterText;   // UI element to display remaining moves.

    private bool IsLevelLoaded = false;
    private bool isLevelCompleted = false;
    public int movesRemaining;                      // Remaining moves for the player to complete the level.
    public Dictionary<string, int> obstacleCounts; // To track obstacles


    // Property to safely access currentLevelData from other classes (Read-only)
    public LevelData CurrentLevelData => currentLevelData;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Debug.Log("LevelSceneManager instance created!");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.Log("Another LevelSceneManager instance found, destroying the other one!");
            Destroy(Instance.gameObject);
            Instance = this;
            //Destroy(gameObject);  // Destroy the new instance
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isLevelCompleted = false;
        IsLevelLoaded = false;
        currentLevel = PlayerPrefs.GetInt("levelState");
        Debug.Log("Current level: " + currentLevel);
        if(winPopup != null) {
            winParticles = winPopup.GetComponentInChildren<ParticleSystem>();
        }
        LoadLevel(currentLevel);
    }

    void LoadLevel(int levelNumber)
    {
        string levelString = levelNumber < 10 ? "0" + levelNumber.ToString() : levelNumber.ToString();

        // Load the level JSON data from Resources folder.
        TextAsset levelFile = Resources.Load<TextAsset>(levelDataPath + levelString);
        if (levelFile != null)
        {
            currentLevelData = JsonUtility.FromJson<LevelData>(levelFile.text);
            movesRemaining = currentLevelData.move_count;
            getObstacles();
            Debug.Log("Level " + levelNumber + " loaded!");
            IsLevelLoaded = true;
        }
        else
        {
            Debug.LogError("Level data not found for level " + levelNumber);
        }
    }

    private void getObstacles() {
        obstacleCounts = new Dictionary<string, int>();

        foreach (string cell in currentLevelData.grid)
        {
            if (cell == "bo" || cell == "s" || cell == "v")
            {
                if (!obstacleCounts.ContainsKey(cell))
                {
                    obstacleCounts[cell] = 0;
                }
                obstacleCounts[cell]++;
            }
        }
    }

    public bool IsLevelLoadedSuccessfully()
    {
        return IsLevelLoaded;
    }

    public bool IsLevelCompleted()
    {
        return isLevelCompleted;
    }

    public void onWin()
    {
        Debug.Log("Win!");
        isLevelCompleted = true;
        currentLevel++;
        Debug.Log("Current level: " + currentLevel);
        PlayerPrefs.SetInt("levelState", currentLevel);
        PlayerPrefs.Save();
        if(winPopup != null) {
            Debug.Log("Win popup activated!");
            winPopup.SetActive(true);
            winParticles.Play();
        }
        
        // Load main scene after 5 seconds
        StartCoroutine(LoadMainScene(5f));

    }

    public void onLose()
    {
        Debug.Log("Lose!");
        isLevelCompleted = true;
        if(losePopup != null)
            losePopup.SetActive(true);
        else     
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    IEnumerator LoadMainScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

}

[System.Serializable]
public class LevelData
{
    public int level_number;   // The number of the level.
    public int grid_width;     // Width of the grid.
    public int grid_height;    // Height of the grid.
    public int move_count;     // Maximum number of moves allowed for this level.
    
    // The grid layout represented as a list of strings:
    // "r" - Red cube, "g" - Green cube, "b" - Blue cube, "y" - Yellow cube
    // "bo" - Box obstacle, "s" - Stone obstacle, "v" - Vase obstacle, "t" - TNT
    public List<string> grid;
}
