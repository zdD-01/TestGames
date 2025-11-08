using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // For TextMeshPro
public class ObstacleView : MonoBehaviour
{
    [SerializeField] public Image obstacleImage; // Reference to the obstacle image
    [SerializeField] public Image tickImage;
    [SerializeField] public TextMeshProUGUI obstacleCountText; // Reference to the text for the obstacle count
    private string obstacleType;
    
    private int currentCount;
    // Start is called before the first frame update
    void Start()
    {
        //obstacleImage = GetComponentInChildren<Image>("ObstacleImage"); // Get the Image component
        //tickImage = GetComponentInChildren<Image>("TickImage");
        //obstacleCountText = GetComponentInChildren<TextMeshProUGUI>(); // Get the TextMeshPro component
        obstacleType = "";
        obstacleCountText.enabled = true;
        obstacleImage.enabled = true;
        tickImage.enabled = false;
    }

    // Method to update the view
    public void UpdateObstacleView(string type, Sprite obstacleSprite, int obstacleCount)
    {
        obstacleType = type;
        // Set the obstacle image
        obstacleImage.sprite = obstacleSprite;

        // Update the count text
        obstacleCountText.text = obstacleCount.ToString();
        currentCount = obstacleCount;
    }

    public void DecreaseObstacleCount() {
        currentCount--;
        if(currentCount <= 0) {
            obstacleCountText.enabled = false;
            tickImage.enabled = true;
        } else {
            obstacleCountText.text = currentCount.ToString();
        }
    }

    public string getObstacleType() {
        return obstacleType;
    }
}
