using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    [SerializeField] float steerSpeed = 100f;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float speedUp = 50f;
    [SerializeField] float speedDown = 10f;


    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "SpeedUp"){
            moveSpeed = speedUp;
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        moveSpeed = speedDown;
    }

    // Update is called once per frame
    void Update()
    {
        float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount, 0);
    }
}
