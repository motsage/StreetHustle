using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //MovementSpeeds
    public float walkSpeed = 10;
    public float sprintSpeed = 15;
    public float mouseSensitivity = 2f;
    public float runSpeed = 40f;
    //Used to change the speed of the player
    private float currentSpeed;

    //Getting Inputs
    private float horizontalInput;
    private float verticalInput;

    Animator anim;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //   horizontalInput = Input.GetAxis("Horizontal");
        //  verticalInput = Input.GetAxis("Vertical");
        movement();
        //Sprinting with Left Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 Move = new Vector3(horizontalInput, 0, verticalInput);
        rb.linearVelocity = Move * currentSpeed;

        bool isWalking = Move.magnitude > 0.1f;

        //Checking if the player is walking to play the animations.
        if (isWalking)
        {
            anim.Play("Walking");
        }
        else if(!Input.GetKey(KeyCode.Q))
        {
            anim.Play("Charge@Standing Idle");
        }
    }

    void movement() 
    {
        // Look around
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        float currentSpeed = (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
           ? runSpeed
           : walkSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;


        rb.MovePosition(transform.position + move * currentSpeed * Time.fixedDeltaTime);
        // Turn left/right
        transform.Rotate(0, mouseX, 0);
    }
}
