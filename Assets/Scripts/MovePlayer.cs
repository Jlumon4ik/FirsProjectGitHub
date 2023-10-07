using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;

    Vector3 velocity;

    bool isGrounded;

    public float gravity = -9.8f;

    public float jumpHeight = 1f;

    public Transform groundCheck;

    public LayerMask groundMask;

    public float groundDistant = 0.4f;




    void Update()
    {


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistant, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
