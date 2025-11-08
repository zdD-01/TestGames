using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float delayAmount = 1f;
    [SerializeField] ParticleSystem finishEffect;
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            finishEffect.Play();
            Invoke("ReLoadScene", delayAmount);
        }
    }

    void ReLoadScene() {
        SceneManager.LoadScene(0);
    }
}
