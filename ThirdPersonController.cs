using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    //PlayerControls controls;

    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float jumpForce;
   
    private Vector3 Velocity;
    [SerializeField] private float Gravity = -9.81f;

    public float turnSmoothtime = 0.1f, rotationSpeed = 8;
    float turnSmoothvelocity;
    float targetAngle;
    float angle;
   
    Animator animator;
    public bool iswalk;

    private void Start()
    {
        animator = GetComponent<Animator>();
       
        iswalk = true;
        
    }

    // Update is called once per frame
    void Update()
    {
    
        if (controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("jump");

            }


        }
        else
        {
            Velocity.y -= Gravity * -2f * Time.deltaTime;
            
        }
        controller.Move(Velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (direction.magnitude >= 0.1f && iswalk)
        {
            iswalk = true;
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothtime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            


            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            
        }

        if (horizontal < 0 || horizontal > 0 || vertical < 0 || vertical > 0)
        {
            animator.SetBool("walking", iswalk);
        }
        else
        {
            animator.SetBool("walking", false);

        }

    }

   
    public void jump()
    {
        Velocity.y = jumpForce;
    }
  
}
