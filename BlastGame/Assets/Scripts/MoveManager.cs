using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [Header("ObstacleViewPrefabs")]
    [SerializeField] private GameObject obstacleViewPrefab;
    [SerializeField] private Sprite vaseSprite;
    [SerializeField] private Sprite boxSprite;
    [SerializeField] private Sprite stoneSprite;

    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI moveCounterText;
    [SerializeField] private Transform goalWindow;
    private LevelSceneManager levelManager;
    private int movesRemaining;
    public Dictionary<string, int> obstacleCounts;
    public Dictionary<string, ObstacleView> obstacles;
    // Start is called before the first frame update

    void Start()
    {
        levelManager = LevelSceneManager.Instance;
        if (levelManager != null)
        {
            Debug.Log("LevelSceneManager found!");
            if(levelManager.IsLevelLoadedSuccessfully())
            {
                obstacleCounts = new Dictionary<string, int>(levelManager.obstacleCounts);
                obstacles = new Dictionary<string, ObstacleView>();
                createObstacles();
                movesRemaining = levelManager.movesRemaining;
                moveCounterText.text = levelManager.movesRemaining.ToString();
            }
            else
            {
                Debug.Log("Level not loaded successfully! yet for move view");
            }
        }
        
    }

    public void createObstacles() {
        foreach (KeyValuePair<string, int> entry in obstacleCounts)
        {
            string type = entry.Key;
            int count = entry.Value;
            GameObject obstacleView = Instantiate(obstacleViewPrefab);
            obstacleView.transform.localPosition = new Vector3(goalWindow.transform.localPosition.x, goalWindow.transform.localPosition.y, 0);
            obstacleView.transform.SetParent(goalWindow);
            obstacleView.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            ObstacleView obstacleViewScript = obstacleView.GetComponent<ObstacleView>();
            Sprite sprite = null;
            if(type == "v") {
                sprite = vaseSprite;
            } else if(type == "bo") {
                sprite = boxSprite;
            } else if(type == "s") {
                sprite = stoneSprite;
            }
            obstacleViewScript.UpdateObstacleView(type, sprite, count);
            obstacles.Add(type, obstacleViewScript); 
        }
    }

    public void DecreaseMoveCount() {
        if(movesRemaining > 0) {
            movesRemaining--;
            moveCounterText.text = movesRemaining.ToString();
            if(movesRemaining == 0) {
                if(IsObstaclesCleared()) {
                    levelManager.onWin();
                } else {
                    levelManager.onLose();
                }
            }
        }
    }

    public void DecreaseObstacleCount(string type) {
        if(!obstacleCounts.ContainsKey(type)) {
            return;
        }
        ObstacleView obstacleView = obstacles[type];
        obstacleCounts[type]--;
        obstacleView.DecreaseObstacleCount();
        if(obstacleCounts[type] == 0) {
            obstacles.Remove(type); 
        }
        if(IsObstaclesCleared() && movesRemaining > 1) {
            levelManager.onWin();
        }
    }

    public bool IsObstaclesCleared() {
        if(obstacles.Count == 0) 
            return true;
        else
            return false;
    }
    // Update is called once per frame
    void Update()
    {
        // Check if all obstacles are destroyed call levelManager onWin
    }
}