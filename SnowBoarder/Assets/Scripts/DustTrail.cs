using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DustTrail : MonoBehaviour
{
    [SerializeField] ParticleSystem dustEffect;

    public void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Ground") {
            dustEffect.Play();
        }
    }

    public void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Ground") {
            dustEffect.Stop();
        }
    }
}
