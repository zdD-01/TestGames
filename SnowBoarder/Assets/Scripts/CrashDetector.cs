using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float delayAmount = 0.1f;
    [SerializeField] ParticleSystem crashEffect;
    private bool hasCrashed = false;

    void Start() {
        hasCrashed = false;
    }
    public void OnTrigerEnter2D(Collision2D other) {

        Debug.Log("Triger detected charsh");
        if(other.gameObject.tag == "Ground") 
        {
            Debug.Log("Ground collision detected");
            if(!hasCrashed) {
                FindObjectOfType<PlayerController>().disableControl();
                crashEffect.Play();
                hasCrashed = true;
            }
            Debug.Log("Reloading scene");
            Invoke("ReLoadScene", delayAmount);
        }
    }

    void ReLoadScene() {
        SceneManager.LoadScene(0);
    }
}
