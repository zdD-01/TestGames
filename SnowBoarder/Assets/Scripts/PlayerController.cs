using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] float torqueAmmount = 1f;
    [SerializeField] float boostSpeed = 50f;

    [SerializeField] float baseSpeed = 30f;
    SurfaceEffector2D surfaceEffector2D;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindAnyObjectByType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove) 
        {
            RotatePlayer();
            RespondToBoost();
        }
    }
            
    private void RotatePlayer() {
        if(Input.GetKey(KeyCode.LeftArrow)) {
            rb2d.AddTorque(torqueAmmount);
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            rb2d.AddTorque(-torqueAmmount);
        }
    }

    private void RespondToBoost() {
        if(Input.GetKey(KeyCode.UpArrow)) {
            surfaceEffector2D.speed = boostSpeed;
        } else
        {
            surfaceEffector2D.speed = baseSpeed;
        }
    }

    public void disableControl() {
        canMove = false;
    } 
}
